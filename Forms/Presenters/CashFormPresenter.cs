using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CashFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private readonly DataTable hardCashTable = new DataTable();
        private DataTable table;
        private bool periodDateRangeInitialized;

        public CashFormPresenter(ICashFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            View = view;
            this.session = session;
        }

        protected ICashFormContract View { get; private set; }

        public virtual async Task EnterAsync()
        {
            hardCashTable.Clear();
            
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
            View.SetTable(hardCashTable);
            UpdateHardCashAmount();
            await LoadAccountLookupAsync();
            await ConnectTableToDataBaseAsync();
            bool periodDateRangeChanged = await InitializePeriodDateRangeAsync();
            View.SetPeriodControlsVisible(View.PeriodChecked);
            if (periodDateRangeChanged)
                await ConnectTableToDataBaseAsync();
        }

        private async Task LoadAccountLookupAsync()
        {
            Dictionary<int, string> accountLookup = new Dictionary<int, string>
            {
                { 0, SQLBase.BookingTo.Barbestand.GetDisplayName() },
                { 1, SQLBase.BookingTo.Bankbestand.GetDisplayName() }
            };

            DataTable clients = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clients, string.Empty);
            AddAccountNames(accountLookup, clients);

            DataTable employees = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, employees);
            AddAccountNames(accountLookup, employees);

            View.SetAccountLookup(accountLookup);
        }

        private static void AddAccountNames(Dictionary<int, string> accountLookup, DataTable table)
        {
            if (table == null || !table.Columns.Contains(Columns.AccountId) || !table.Columns.Contains(Columns.Name))
                return;

            foreach (DataRow row in table.Rows.OfType<DataRow>())
            {
                if (row.RowState == DataRowState.Deleted || row[Columns.AccountId] == DBNull.Value)
                    continue;

                accountLookup[Convert.ToInt32(row[Columns.AccountId])] = row[Columns.Name].ToString();
            }
        }

        private async Task<bool> InitializePeriodDateRangeAsync()
        {
            if (periodDateRangeInitialized)
                return false;

            DataTable allBookings = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, allBookings);
            DateTime? fromDate = null;
            DateTime? toDate = null;
            foreach (DataRow row in allBookings.Rows)
            {
                if (row[Columns.Date] == DBNull.Value)
                    continue;

                DateTime date = Convert.ToDateTime(row[Columns.Date]);
                if (!fromDate.HasValue || date < fromDate.Value)
                    fromDate = date;
                if (!toDate.HasValue || date > toDate.Value)
                    toDate = date;
            }

            if (!fromDate.HasValue || !toDate.HasValue)
                return false;

            View.SetPeriodDateRange(fromDate.Value, toDate.Value);
            periodDateRangeInitialized = true;
            return true;
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            table = new DataTable();
            if (!View.PeriodChecked)
            {
                DateTime date = View.FromDate;
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.BargeFromMonth, table, date.Month, date.Year);
            }
            else
            {
                DateTime fromDate = View.FromDate;
                DateTime toDate = View.ToDate.AddMonths(1);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.BargeByPeriod, table, fromDate, toDate);
            }

            if (View.CurrentSortColumn != null)
                table.DefaultView.Sort = View.CurrentSortColumn;
            else
                table.DefaultView.Sort = View.DefaultSortColumn;

            View.SetCashViewTable(table);

            object bargeTotalAmount = await session.SQL.GetViewAsync("cash_total_amount");
            View.TotalAmountText = Convert.ToDecimal(bargeTotalAmount).ToString("C", session.Company.Currencies);
            UpdateHardCashAmount();
        }

        public virtual void UpdateHardCashAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in hardCashTable.Rows)
            {
                totalAmount += Int32.Parse(row["001"].ToString()) * 0.01m;
                totalAmount += Int32.Parse(row["002"].ToString()) * 0.02m;
                totalAmount += Int32.Parse(row["005"].ToString()) * 0.05m;
                totalAmount += Int32.Parse(row["010"].ToString()) * 0.1m;
                totalAmount += Int32.Parse(row["020"].ToString()) * 0.2m;
                totalAmount += Int32.Parse(row["050"].ToString()) * 0.5m;
                totalAmount += Int32.Parse(row["1"].ToString()) * 1.0m;
                totalAmount += Int32.Parse(row["2"].ToString()) * 2.0m;
                totalAmount += Int32.Parse(row["5"].ToString()) * 5.0m;
                totalAmount += Int32.Parse(row["10"].ToString()) * 10.0m;
                totalAmount += Int32.Parse(row["20"].ToString()) * 20.0m;
                totalAmount += Int32.Parse(row["50"].ToString()) * 50.0m;
                totalAmount += Int32.Parse(row["100"].ToString()) * 100.0m;
                totalAmount += Int32.Parse(row["200"].ToString()) * 200.0m;
                totalAmount += Int32.Parse(row["500"].ToString()) * 500.0m;
            }

            View.HardCashAmountText = totalAmount.ToString("C", session.Company.Currencies);
            View.SetHardCashAmountWarning(!View.TotalAmountText.Equals(View.HardCashAmountText));
        }

        public virtual void EndHardCashEdit()
        {
            if (hardCashTable.Rows.Count > 0)
                hardCashTable.Rows[0].EndEdit();
        }

        public virtual void EndHardCashBindingEdit()
        {
            View.EndEditHardCash();
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual async Task SaveAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool value = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
                if (value)
                {
                    await ConnectTableToDataBaseAsync();
                    View.ShowMessage(Messages.hardmoney_changed);
                }
                else
                {
                    throw new Exception(Messages.hardmoney_changed_failed);
                }
            }
            catch
            {
                table?.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task PrintAsync()
        {
            if (table == null)
                return;

            DateTime dateBegin, dateEnd;
            dateBegin = dateEnd = View.FromDate;
            if (View.PeriodChecked)
                dateEnd = View.ToDate;
            dateEnd = dateEnd.AddMonths(1).AddHours(-1);
            if (dateEnd > DateTime.Now)
                dateEnd = DateTime.Now;

            DataRow[] rows = table.Select("", "date");
            decimal einnahmen = 0;
            decimal ausgaben = 0;
            foreach (DataRow row in rows)
            {
                decimal value = (decimal)(row["amount"]);
                if (value < 0)
                    ausgaben += Math.Abs(value);
                else
                    einnahmen += Math.Abs(value);
            }

            DataTable prevoisMonthTable = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, prevoisMonthTable);
            decimal prevoiusAmount = 0;
            DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            IEnumerable<DataRow> books = prevoisMonthTable.Rows.OfType<DataRow>()
                .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit);
            foreach (DataRow row in books)
                prevoiusAmount += (decimal)(row["amount"]);

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());
            string ouputDate;
            if (dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
                ouputDate = dateBegin.ToString("MMMM yyyy");
            else
                ouputDate = dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, ouputDate);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, prevoiusAmount.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, (prevoiusAmount - ausgaben + einnahmen).ToString("C", session.Company.Currencies));

            View.Print(rows);
        }

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Cash, table);
                if (!valid)
                    throw new Exception(Messages.datatable_update_failed);

                await ConnectTableToDataBaseAsync();
                View.ShowMessage(Messages.datatable_updated);
            }
            catch
            {
                table.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Undo()
        {
            hardCashTable.RejectChanges();
            UpdateHardCashAmount();
        }

        public virtual async Task BookAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                CashBookingInput input;
                if (!View.ShowCashBookDialog(out input))
                    return;

                    string bookText = input.BookText;
                    decimal amount = input.Amount;
                    DateTime payInDate = input.BookingDate;
                    SQLBase.BookingTo bookTo = input.BookingTarget;
                    SQLBase.BookCategory bookCategory = input.BookingCategory;
                    bool printQuittance = input.PrintQuittance;
                    IEnumerable<ID_Client_Data> clients = input.SelectedClients;

                    bool valid = false;

                    using (var transaction = session.SQL.BeginTransaction())
                    {
                        try
                        {
                            if (bookTo == SQLBase.BookingTo.Barbestand)
                            {
                                foreach (ID_Client_Data clientData in clients)
                                {
                                    int clientID = clientData.ID;
                                    string clientName = clientData.Name;
                                    int accountId = await session.SQL.GetClientAccountIdAsync(clientID);
                                    DataRow currentBook = null;
                                    if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                    {
                                        var result = await session.SQL.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, amount, SQLBase.BookCategory.Einzahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                            valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, amount, accountId, SQLBase.BookCategory.Einzahlung, bookTo);
                                    }
                                    else
                                    {
                                        var result = await session.SQL.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, -amount, SQLBase.BookCategory.Auszahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                            valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, -amount, accountId, SQLBase.BookCategory.Auszahlung, bookTo);
                                    }

                                    if (!valid)
                                        throw new Exception(string.Format(Messages.booking_for_client_failed, clientName));

                                    if (printQuittance)
                                    {
                                        List<DataRow> currentBooks = new List<DataRow>();
                                        currentBooks.Add(currentBook);

                                        View.PrintQuittance(clientName, currentBooks);
                                    }
                                }

                                if (valid)
                                    View.ShowMessage(Messages.booking_sucess);
                            }
                            else
                            {
                                if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                {
                                    if (valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, amount, 1, SQLBase.BookCategory.Einzahlung, bookTo))
                                        valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, -amount, 0, SQLBase.BookCategory.Auszahlung, bookTo);
                                }
                                else
                                {
                                    if (valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, -amount, 1, SQLBase.BookCategory.Auszahlung, bookTo))
                                        valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, amount, 0, SQLBase.BookCategory.Einzahlung, bookTo);
                                }
                                if (valid)
                                    View.ShowMessage(Messages.booking_sucess);
                                else
                                    throw new Exception(Messages.booking_failed);
                            }

                            if (!valid)
                                throw new Exception(Messages.booking_failed);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task PeriodCheckAsync()
        {
            View.SetPeriodControlsVisible(View.PeriodChecked);
            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task DateChangedAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Export()
        {
            if (table == null)
                return;

            string fileName;
            if (!View.ShowSaveFileDialog(Messages.cash_export_filename, "Excel|*.xlsx", string.Empty, out fileName))
                return;

            Excel.ExportToExcel(table.DefaultView.ToTable(), fileName, session.Company.CurrencyCode);
            View.ShowMessage(string.Format(Messages.export_success, fileName));
        }

        public virtual void Automatic()
        {
            if (hardCashTable.Rows.Count == 0)
                return;

            View.SuspendBindingHardCash();
            decimal amount = Math.Abs(decimal.Parse(View.TotalAmountText.Replace("â‚¬", "").Trim()));
            int value100 = (int)(amount / 100.0m);
            amount -= value100 * 100;
            int value50 = (int)(amount / 50.0m);
            amount -= value50 * 50;
            int value20 = (int)(amount / 20.0m);
            amount -= value20 * 20;
            int value10 = (int)(amount / 10.0m);
            amount -= value10 * 10;
            int value5 = (int)(amount / 5.0m);
            amount -= value5 * 5;
            int value2 = (int)(amount / 2.0m);
            amount -= value2 * 2;
            int value1 = (int)(amount / 1.0m);
            amount -= value1;
            int value050 = (int)(amount / 0.5m);
            amount -= value050 * 0.5m;
            int value020 = (int)(amount / 0.2m);
            amount -= value020 * 0.2m;
            int value010 = (int)(amount / 0.1m);
            amount -= value010 * 0.1m;
            int value005 = (int)(amount / 0.05m);
            amount -= value005 * 0.05m;
            int value002 = (int)(amount / 0.02m);
            amount -= value002 * 0.02m;
            int value001 = (int)(amount / 0.01m);

            foreach (DataRow row in hardCashTable.Rows)
            {
                row["001"] = value001;
                row["002"] = value002;
                row["005"] = value005;
                row["010"] = value010;
                row["020"] = value020;
                row["050"] = value050;
                row["1"] = value1;
                row["2"] = value2;
                row["5"] = value5;
                row["10"] = value10;
                row["20"] = value20;
                row["50"] = value50;
                row["100"] = value100;
                row["200"] = 0;
                row["500"] = 0;
            }
            View.ResumeBindingHardCash();
            UpdateHardCashAmount();
        }
    }
}
