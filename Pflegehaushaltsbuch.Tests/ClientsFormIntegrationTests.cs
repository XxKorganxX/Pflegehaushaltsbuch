using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class ClientsFormIntegrationTests
    {
        [TestMethod]
        public async Task ClientsDeadlinesAndCashBookingsUseClientAccounts()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_clients") + ".db");
            string password = "integration-test";
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, password, databaseFile);
                sql.Dispose();

                sql = new SQLITE();
                await sql.ConnectAsync(databaseFile, string.Empty, password, databaseFile);
                DatabaseIntegrationTestSupport.SetTestUser(sql);

                using (SqlSession session = new SqlSession())
                {
                    session.Replace(sql);
                    sql = null;

                    ClientsFormView clientsView = new ClientsFormView();
                    ClientsFormPresenter clientsPresenter = new ClientsFormPresenter(clientsView, session);
                    await clientsPresenter.ConnectTableToDataBaseAsync();

                    clientsView.NextCreateClient = CreateClientInput(100, "Client Without Balance", 0m);
                    await clientsPresenter.CreateAccountAsync();
                    clientsView.NextCreateClient = CreateClientInput(101, "Client With Balance", 150.25m);
                    await clientsPresenter.CreateAccountAsync();

                    DataRow clientWithoutBalance = await LoadClientAsync(session.SQL, 100);
                    DataRow clientWithBalance = await LoadClientAsync(session.SQL, 101);
                    int accountWithoutBalance = Convert.ToInt32(clientWithoutBalance["account_id"], CultureInfo.InvariantCulture);
                    int accountWithBalance = Convert.ToInt32(clientWithBalance["account_id"], CultureInfo.InvariantCulture);

                    Assert.AreNotEqual(accountWithoutBalance, accountWithBalance);
                    AssertDecimal(0m, clientWithoutBalance["amount"]);
                    AssertDecimal(150.25m, clientWithBalance["amount"]);
                    AssertAccount(session.SQL, accountWithoutBalance, "Client", 1);
                    AssertAccount(session.SQL, accountWithBalance, "Client", 1);
                    AssertBankBooking(session.SQL, accountWithBalance, 150.25m, SQLBase.BookingTo.Altbestand);
                    Assert.AreEqual(0, FindRows(session.SQL, SQLBase.SELECT.Bank, "account_id = " + accountWithoutBalance).Length);

                    clientsView.SelectedClientIdValue = 100;
                    clientsView.NextChangeClient = CreateClientInput(100, "Changed Client", 0m);
                    clientsView.NextChangeClient.City = "Changed City";
                    await clientsPresenter.ChangeAsync();

                    DataRow changedClient = await LoadClientAsync(session.SQL, 100);
                    Assert.AreEqual("Changed Client", changedClient["name"].ToString());
                    Assert.AreEqual("Changed City", changedClient["city"].ToString());
                    Assert.AreEqual(accountWithoutBalance, Convert.ToInt32(changedClient["account_id"], CultureInfo.InvariantCulture));

                    DeadLinesFormView deadlineView = new DeadLinesFormView
                    {
                        ClientIDValue = 100,
                        CurrentMonthValue = new DateTime(2026, 8, 1),
                        NextDeadline = new DeadlineInput { Description = "Pflegebesuch", ForAllMonths = false }
                    };
                    DeadLinesFormPresenter deadlinePresenter = new DeadLinesFormPresenter(deadlineView, session);
                    await deadlinePresenter.ConnectTableToDataBaseAsync();
                    await deadlinePresenter.CellClickAsync(2, 2);

                    DataTable deadlineTable = new DataTable();
                    await session.SQL.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, deadlineTable, 100);
                    DataRow[] deadlines = deadlineTable.Select("id = 100");
                    Assert.AreEqual(1, deadlines.Length, "Expected one deadline for client 100.");
                    AssertDate(new DateTime(2000, 8, 12), deadlines[0]["date"]);
                    Assert.AreEqual("Pflegebesuch", deadlines[0]["note"].ToString());

                    DateTime today = DateTime.Today;
                    deadlineView.CurrentMonthValue = new DateTime(today.Year, today.Month, 1);
                    deadlineView.NextDeadline = new DeadlineInput { Description = "Termin heute", ForAllMonths = false };
                    await deadlinePresenter.ConnectTableToDataBaseAsync();
                    int todayCell = GetDeadlineCalendarCell(today);
                    await deadlinePresenter.CellClickAsync(todayCell / 7, todayCell % 7);

                    clientsView.SelectedClientIdValue = 100;
                    await clientsPresenter.ConnectTableToDataBaseAsync();
                    Assert.AreEqual("Termin heute", clientsView.DeadlineText);

                    CashFormView cashView = new CashFormView
                    {
                        FromDateValue = new DateTime(2026, 8, 1),
                        ToDateValue = new DateTime(2026, 8, 1),
                        NextBooking = new CashBookingInput
                        {
                            BookText = "Client cash payment",
                            Amount = 40m,
                            BookingDate = new DateTime(2026, 8, 20),
                            BookingCategory = SQLBase.BookCategory.Einzahlung,
                            BookingTarget = SQLBase.BookingTo.Barbestand,
                            PrintQuittance = false,
                            SelectedClients = new[] { new ID_Client_Data { ID = 100, Name = "Changed Client" } }
                        }
                    };
                    CashFormPresenter cashPresenter = new CashFormPresenter(cashView, session);
                    await cashPresenter.EnterAsync();
                    await cashPresenter.BookAsync();

                    AssertCashBooking(session.SQL, accountWithoutBalance, 40m, SQLBase.BookingTo.Barbestand);
                    AssertClientBook(session.SQL, 100, 40m, SQLBase.BookingTo.Barbestand);

                    changedClient = await LoadClientAsync(session.SQL, 100);
                    AssertDecimal(40m, changedClient["amount"]);
                    AssertDate(new DateTime(2026, 8, 20), changedClient["lastbook"]);

                    clientWithBalance = await LoadClientAsync(session.SQL, 101);
                    AssertDecimal(150.25m, clientWithBalance["amount"]);
                }
            }
            finally
            {
                sql?.Dispose();
                await SQLiteTestDropDatabaseWithRetryAsync(databaseFile, password);
            }
        }

        private static ClientAccountInput CreateClientInput(int id, string name, decimal amount)
        {
            return new ClientAccountInput
            {
                ClientID = id,
                Title = "Herr",
                Name = name,
                Street = "Teststrasse 1",
                Zipcode = "12345",
                City = "Teststadt",
                BornDate = new DateTime(1980, 1, 1),
                Amount = amount,
                AdvisorId = null
            };
        }

        private static async Task<DataRow> LoadClientAsync(SQLBase sql, int clientId)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, table, clientId);
            Assert.AreEqual(1, table.Rows.Count, "Expected exactly one client row for id " + clientId + ".");
            return table.Rows[0];
        }

        private static DataRow[] FindRows(SQLBase sql, SQLBase.SELECT select, string filter)
        {
            DataTable table = new DataTable();
            sql.FillAdapterAsync(select, table).GetAwaiter().GetResult();
            return table.Select(filter);
        }

        private static void AssertAccount(SQLBase sql, int accountId, string type, int active)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Accounts, "id = " + accountId);
            Assert.AreEqual(1, rows.Length, "Expected one account row for account id " + accountId + ".");
            Assert.AreEqual(type, rows[0]["type"].ToString());
            Assert.AreEqual(active, Convert.ToInt32(rows[0]["active"], CultureInfo.InvariantCulture));
        }

        private static void AssertBankBooking(SQLBase sql, int accountId, decimal amount, SQLBase.BookingTo bookingTo)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Bank, "account_id = " + accountId);
            Assert.AreEqual(1, rows.Length, "Expected one bank booking for account id " + accountId + ".");
            AssertDecimal(amount, rows[0]["amount"]);
            Assert.AreEqual((int)SQLBase.BookCategory.Einzahlung, Convert.ToInt32(rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)bookingTo, Convert.ToInt32(rows[0]["book_to"], CultureInfo.InvariantCulture));
        }

        private static void AssertCashBooking(SQLBase sql, int accountId, decimal amount, SQLBase.BookingTo bookingTo)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Cash, "account_id = " + accountId);
            Assert.AreEqual(1, rows.Length, "Expected one cash booking for account id " + accountId + ".");
            AssertDecimal(amount, rows[0]["amount"]);
            Assert.AreEqual((int)SQLBase.BookCategory.Einzahlung, Convert.ToInt32(rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)bookingTo, Convert.ToInt32(rows[0]["book_to"], CultureInfo.InvariantCulture));
        }

        private static void AssertClientBook(SQLBase sql, int clientId, decimal amount, SQLBase.BookingTo bookingTo)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Books, "id = " + clientId);
            Assert.AreEqual(1, rows.Length, "Expected one client book row for client id " + clientId + ".");
            AssertDecimal(amount, rows[0]["amount"]);
            Assert.AreEqual(1, Convert.ToInt32(rows[0]["document_id"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)SQLBase.BookCategory.Einzahlung, Convert.ToInt32(rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)bookingTo, Convert.ToInt32(rows[0]["book_to"], CultureInfo.InvariantCulture));
        }

        private static void AssertDecimal(decimal expected, object actual)
        {
            Assert.AreEqual(expected, Convert.ToDecimal(actual, CultureInfo.InvariantCulture));
        }

        private static void AssertDate(DateTime expected, object actual)
        {
            Assert.AreEqual(expected.Date, Convert.ToDateTime(actual, CultureInfo.InvariantCulture).Date);
        }

        private static int GetDeadlineCalendarCell(DateTime date)
        {
            int startCell = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek - 1;
            if (startCell < 0)
                startCell += 7;

            return startCell + date.Day - 1;
        }

        private static async Task SQLiteTestDropDatabaseWithRetryAsync(string fileName, string password)
        {
            for (int attempt = 0; attempt < 10; attempt++)
            {
                try
                {
                    await new SQLITE().DropDatabaseAsync(fileName, string.Empty, password, fileName);
                    return;
                }
                catch (IOException) when (attempt < 9)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Thread.Sleep(100);
                }
            }
        }

        private sealed class ClientsFormView : IClientsFormContract
        {
            public ClientAccountInput NextCreateClient { get; set; }
            public ClientAccountInput NextChangeClient { get; set; }
            public int? SelectedClientIdValue { get; set; }

            public string DefaultSortColumn => "id";
            public string CurrentSortColumn => null;
            public int ActiveClientsFilterIndex => 1;
            public int? SelectedClientId => SelectedClientIdValue;
            public string SelectedClientName => string.Empty;
            public string TotalAmountText { get; private set; }
            public string DeadlineText { get; private set; }

            public void BindClients(DataView clients)
            {
                if (!SelectedClientIdValue.HasValue && clients.Count > 0)
                    SelectedClientIdValue = Convert.ToInt32(clients[0]["id"], CultureInfo.InvariantCulture);
            }

            public void ClearClients()
            {
            }

            public void BindClientDates(DataView clients)
            {
            }

            public void SetTotalClients(int totalClients)
            {
            }

            public void SetTotalAmount(string totalAmount)
            {
                TotalAmountText = totalAmount;
            }

            public void SetDeadlineText(string text)
            {
                DeadlineText = text;
            }

            public void SelectClientById(int clientId)
            {
                SelectedClientIdValue = clientId;
            }

            public void NotifyClientIdChanged(int clientID)
            {
            }

            public bool ShowCreateClientDialog(out ClientAccountInput clientData)
            {
                clientData = NextCreateClient;
                return clientData != null;
            }

            public bool ShowChangeClientDialog(int clientID, out ClientAccountInput clientData)
            {
                clientData = NextChangeClient;
                return clientData != null;
            }

            public void ShowPrintClientsBooksDialog()
            {
            }

            public void PrintClients(DataRow[] clients)
            {
            }

            public void ShowMainForm()
            {
            }

            public void ShowBookForm()
            {
            }

            public void ShowCalendarForm()
            {
            }

            public void ShowMessage(string msg)
            {
            }

            public void ShowError(string msg)
            {
            }

            public bool ConfirmMessage(string msg)
            {
                return true;
            }

            public bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName)
            {
                selectedFileName = null;
                return false;
            }

            public Task ShowMainFormAsync(CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }

        private sealed class DeadLinesFormView : IDeadLinesFormContract
        {
            public DateTime CurrentMonthValue { get; set; }
            public int ClientIDValue { get; set; }
            public DeadlineInput NextDeadline { get; set; }

            public DateTime CurrentMonth => CurrentMonthValue;
            public int ClientID => ClientIDValue;

            public void ShowCalendar(DeadlineCalendar calendar)
            {
            }

            public void ShowClientName(string clientName)
            {
            }

            public void ClearClientName()
            {
            }

            public bool ShowExportDialog(string fileName, out string selectedFileName)
            {
                selectedFileName = null;
                return false;
            }

            public bool ShowCreateDeadlineDialog(DateTime date, string description, out DeadlineInput input)
            {
                input = NextDeadline;
                return input != null;
            }

            public void ShowDatabaseChanged()
            {
            }

            public void ShowClientsForm()
            {
            }

            public void ShowMessage(string msg)
            {
            }

            public void ShowError(string msg)
            {
            }

            public bool ConfirmMessage(string msg)
            {
                return true;
            }
        }

        private sealed class CashFormView : ICashFormContract
        {
            public DateTime FromDateValue { get; set; }
            public DateTime ToDateValue { get; set; }
            public CashBookingInput NextBooking { get; set; }

            public string DefaultSortColumn => "date";
            public string CurrentSortColumn => null;
            public DateTime FromDate => FromDateValue;
            public DateTime ToDate => ToDateValue;
            public bool PeriodChecked => false;
            public string TotalAmountText { get; set; } = 0m.ToString("C", CultureInfo.CurrentCulture);
            public string HardCashAmountText { get; set; }

            public bool ShowCashBookDialog(out CashBookingInput input)
            {
                input = NextBooking;
                return input != null;
            }

            public void SetPeriodControlsVisible(bool visible)
            {
            }

            public void SetPeriodDateRange(DateTime fromDate, DateTime toDate)
            {
                FromDateValue = fromDate;
                ToDateValue = toDate;
            }

            public void SetAccountLookup(Dictionary<int, string> accountLookup)
            {
            }

            public void SetHardCashAmountWarning(bool warning)
            {
            }

            public void ShowMainForm()
            {
            }

            public void ShowMessage(string msg)
            {
            }

            public void ShowError(string msg)
            {
            }

            public bool ConfirmMessage(string msg)
            {
                return true;
            }

            public bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName)
            {
                selectedFileName = null;
                return false;
            }

            public void SetTable(DataTable hardCashTable)
            {
            }

            public void SetCashViewTable(DataTable table)
            {
            }

            public void EndEditHardCash()
            {
            }

            public void SuspendBindingHardCash()
            {
            }

            public void ResumeBindingHardCash()
            {
            }

            public void Print(DataRow[] rows)
            {
            }

            public void PrintQuittance(string clientName, List<DataRow> currentBooks)
            {
            }
        }
    }
}
