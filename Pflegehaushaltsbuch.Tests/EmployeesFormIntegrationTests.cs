using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class EmployeesFormIntegrationTests
    {
        [TestMethod]
        public async Task EmployeesBookingsAndPaybacksUseEmployeeAccounts()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_employees") + ".db");
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

                    EmployeesFormView employeesView = new EmployeesFormView();
                    EmployeesFormPresenter employeesPresenter = new EmployeesFormPresenter(employeesView, session);
                    await employeesPresenter.ConnectTableToDataBaseAsync();

                    employeesView.NextCreateAssistant = CreateAssistantInput(200, "Employee Without Balance", 0m, new DateTime(2026, 8, 1));
                    await employeesPresenter.CreateAsync();
                    employeesView.NextCreateAssistant = CreateAssistantInput(201, "Employee With Balance", 120m, new DateTime(2026, 8, 2));
                    await employeesPresenter.CreateAsync();

                    DataRow employeeWithoutBalance = await LoadEmployeeAsync(session.SQL, 200);
                    DataRow employeeWithBalance = await LoadEmployeeAsync(session.SQL, 201);
                    int accountWithoutBalance = Convert.ToInt32(employeeWithoutBalance["account_id"], CultureInfo.InvariantCulture);
                    int accountWithBalance = Convert.ToInt32(employeeWithBalance["account_id"], CultureInfo.InvariantCulture);

                    Assert.AreNotEqual(accountWithoutBalance, accountWithBalance);
                    AssertAccount(session.SQL, accountWithoutBalance, "Employee", 1);
                    AssertAccount(session.SQL, accountWithBalance, "Employee", 1);
                    AssertDecimal(0m, employeeWithoutBalance["amount_payout"]);
                    AssertDecimal(120m, employeeWithBalance["amount_payout"]);
                    Assert.AreEqual(0, FindRows(session.SQL, SQLBase.SELECT.Cash, "account_id = " + accountWithoutBalance).Length);
                    AssertCashBooking(session.SQL, accountWithBalance, -120m, SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand, "Expected initial payout booking.");

                    employeesView.SelectedAssistantIdValue = 200;
                    employeesView.NextChangeAssistant = CreateAssistantInput(200, "Changed Employee", 60m, new DateTime(2026, 8, 3));
                    await employeesPresenter.ChangeAssistantAsync();

                    employeeWithoutBalance = await LoadEmployeeAsync(session.SQL, 200);
                    Assert.AreEqual("Changed Employee", employeeWithoutBalance["name"].ToString());
                    Assert.AreEqual(accountWithoutBalance, Convert.ToInt32(employeeWithoutBalance["account_id"], CultureInfo.InvariantCulture));
                    AssertDecimal(60m, employeeWithoutBalance["amount_payout"]);
                    AssertCashBooking(session.SQL, accountWithoutBalance, -60m, SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand, "Expected payout booking after changing zero-balance employee.");

                    employeesView.NextPayback = new AssistantPaybackInput
                    {
                        AssistantId = 200,
                        AssistantName = "Changed Employee",
                        PaybackDate = new DateTime(2026, 8, 4),
                        Amount = 25m,
                        Repayment = SQLBase.Repayment.Payout,
                        RepaymentIndex = (int)SQLBase.Repayment.Payout
                    };
                    await employeesPresenter.PayOutAsync();

                    AssertCashBookings(session.SQL, accountWithoutBalance, new[] { -60m, 25m });
                    employeeWithoutBalance = await LoadEmployeeAsync(session.SQL, 200);
                    AssertDecimal(35m, employeeWithoutBalance["amount_payout"]);
                    AssertDecimal(25m, employeeWithoutBalance["amount_payback"]);
                    Assert.AreEqual((int)SQLBase.Repayment.Payout, Convert.ToInt32(employeeWithoutBalance["amount_payback_type"], CultureInfo.InvariantCulture));

                    employeesView.SelectedAssistantIdValue = 201;
                    employeesView.NextPayback = new AssistantPaybackInput
                    {
                        AssistantId = 201,
                        AssistantName = "Employee With Balance",
                        PaybackDate = new DateTime(2026, 8, 5),
                        Amount = 120m,
                        Repayment = SQLBase.Repayment.Transfered,
                        RepaymentIndex = (int)SQLBase.Repayment.Transfered
                    };
                    await employeesPresenter.PayOutAsync();

                    AssertBankBooking(session.SQL, accountWithBalance, 120m, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);
                    employeeWithBalance = await LoadEmployeeAsync(session.SQL, 201);
                    AssertDecimal(0m, employeeWithBalance["amount_payout"]);
                    AssertDecimal(120m, employeeWithBalance["amount_payback"]);
                    AssertDecimal(0m, employeeWithBalance["account_transfer"]);
                    Assert.AreEqual((int)SQLBase.Repayment.Transfered, Convert.ToInt32(employeeWithBalance["amount_payback_type"], CultureInfo.InvariantCulture));
                    Assert.IsFalse(Convert.ToBoolean(employeeWithBalance["active"], CultureInfo.InvariantCulture));
                }
            }
            finally
            {
                sql?.Dispose();
                await DropDatabaseWithRetryAsync(databaseFile, password);
            }
        }

        private static AssistantInput CreateAssistantInput(int id, string name, decimal amount, DateTime date)
        {
            return new AssistantInput
            {
                ID = id,
                AssistantName = name,
                Amount = amount,
                Date = date
            };
        }

        private static async Task<DataRow> LoadEmployeeAsync(SQLBase sql, int employeeId)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Assistant, table, employeeId);
            Assert.AreEqual(1, table.Rows.Count, "Expected exactly one employee row for id " + employeeId + ".");
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

        private static void AssertCashBooking(SQLBase sql, int accountId, decimal amount, SQLBase.BookCategory bookCategory, SQLBase.BookingTo bookingTo, string message)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Cash, "account_id = " + accountId + " AND amount = " + amount.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(1, rows.Length, message);
            Assert.AreEqual((int)bookCategory, Convert.ToInt32(rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)bookingTo, Convert.ToInt32(rows[0]["book_to"], CultureInfo.InvariantCulture));
        }

        private static void AssertCashBookings(SQLBase sql, int accountId, decimal[] amounts)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Cash, "account_id = " + accountId);
            CollectionAssert.AreEqual(
                amounts.OrderBy(amount => amount).ToArray(),
                rows.Select(row => Convert.ToDecimal(row["amount"], CultureInfo.InvariantCulture)).OrderBy(amount => amount).ToArray());
        }

        private static void AssertBankBooking(SQLBase sql, int accountId, decimal amount, SQLBase.BookCategory bookCategory, SQLBase.BookingTo bookingTo)
        {
            DataRow[] rows = FindRows(sql, SQLBase.SELECT.Bank, "account_id = " + accountId);
            Assert.AreEqual(1, rows.Length, "Expected one bank booking for account id " + accountId + ".");
            AssertDecimal(amount, rows[0]["amount"]);
            Assert.AreEqual((int)bookCategory, Convert.ToInt32(rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)bookingTo, Convert.ToInt32(rows[0]["book_to"], CultureInfo.InvariantCulture));
        }

        private static void AssertDecimal(decimal expected, object actual)
        {
            Assert.AreEqual(expected, Convert.ToDecimal(actual, CultureInfo.InvariantCulture));
        }

        private static async Task DropDatabaseWithRetryAsync(string fileName, string password)
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

        private sealed class EmployeesFormView : IEmployeesFormContract
        {
            public AssistantInput NextCreateAssistant { get; set; }
            public AssistantInput NextChangeAssistant { get; set; }
            public AssistantPaybackInput NextPayback { get; set; }
            public int? SelectedAssistantIdValue { get; set; }

            public string DefaultSortColumn => "id";
            public string CurrentSortColumn => null;
            public bool ChangeButtonEnabled => true;
            public int? SelectedAssistantId => SelectedAssistantIdValue;
            public string SelectedAssistantName => string.Empty;

            public void BindEmployees(DataView employees)
            {
                if (!SelectedAssistantIdValue.HasValue && employees.Count > 0)
                    SelectedAssistantIdValue = Convert.ToInt32(employees[0]["id"], CultureInfo.InvariantCulture);
            }

            public void ClearEmployees()
            {
            }

            public void BindEmployeeDate(DataView employees)
            {
            }

            public void SetTotalAmount(string totalAmount)
            {
            }

            public void PrintEmployees(DataRow[] employees)
            {
            }

            public bool ShowCreateAssistantDialog(int id, out AssistantInput input)
            {
                input = NextCreateAssistant;
                return input != null;
            }

            public bool ShowChangeAssistantDialog(int id, string name, DateTime date, decimal amount, out AssistantInput input)
            {
                input = NextChangeAssistant;
                return input != null;
            }

            public bool ShowIoanPaybackDialog(string assistantName, int assistantId, decimal amount, out AssistantPaybackInput input)
            {
                input = NextPayback;
                return input != null;
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

            public bool ShowOpenFileDialog(string fileName, string filter, out string selectedFileName)
            {
                selectedFileName = null;
                return false;
            }
        }
    }
}
