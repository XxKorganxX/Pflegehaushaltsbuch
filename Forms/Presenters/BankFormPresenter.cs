using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class BankFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private DataTable table;

        public BankFormPresenter(IBankFormContract view, SqlSession session)
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

        protected IBankFormContract View { get; private set; }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            table = new DataTable();
            if (!View.PeriodChecked)
            {
                DateTime date = View.FromDate;
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.BankByDate, table, date.Month, date.Year);
            }
            else
            {
                DateTime fromDate = View.FromDate;
                DateTime toDate = View.ToDate.AddMonths(1);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.BankByPeriod, table, fromDate, toDate);
            }


            if (View.CurrentSortColumn != null)
                table.DefaultView.Sort = View.CurrentSortColumn;
            else
                table.DefaultView.Sort = View.DefaultSortColumn;

            View.BindBank(table);

            object bankTotalAmount = await session.SQL.GetViewAsync("bank_total_amount");
            View.SetTotalAmount(decimal.Parse(bankTotalAmount.ToString()).ToString("C"));
        }

        public virtual async Task BookAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                BankBookingInput input;
                if (!View.ShowBankBookDialog(out input))
                    return;

                    decimal amount = input.Amount;
                    string bookText = input.BookText;
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
                                if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                {
                                    if (valid = await session.SQL.ToBargeAsync(payInDate, bookText, -amount, SQLBase.BookingTo.Bankbestand.GetDisplayName(), SQLBase.BookCategory.Auszahlung, bookTo))
                                    {
                                        valid = await session.SQL.ToBankAsync(payInDate, bookText, amount, SQLBase.BookingTo.Barbestand.GetDisplayName(), SQLBase.BookCategory.Einzahlung, bookTo);
                                    }
                                }
                                else
                                {
                                    if (valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, amount, SQLBase.BookingTo.Bankbestand.GetDisplayName(), SQLBase.BookCategory.Einzahlung, bookTo))
                                    {
                                        valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, -amount, SQLBase.BookingTo.Barbestand.GetDisplayName(), SQLBase.BookCategory.Auszahlung, bookTo);
                                    }
                                }
                                if (valid)
                                {
                                    View.ShowMessage(Messages.booking_sucess);
                                }
                                else
                                {
                                    throw new Exception(Messages.booking_failed);
                                }
                            }
                            else
                            {
                                foreach (ID_Client_Data clientData in clients)
                                {
                                    int clientID = clientData.ID;
                                    string clientName = clientData.Name;
                                    DataRow currentBook = null;
                                    if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                    {
                                        var result = await session.SQL.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, amount, SQLBase.BookCategory.Einzahlung, bookTo);
                                        currentBook = result.Item2;

                                        if (valid = result.Item1)
                                        {
                                            valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Einzahlung, bookTo);
                                        }
                                    }
                                    else
                                    {
                                        var result = await session.SQL.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, -amount, SQLBase.BookCategory.Auszahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                        {
                                            valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Auszahlung, bookTo);
                                        }
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
                                    View.ShowMessage(Messages.bookings_success);
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

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Bank, table);
                if (!valid)
                {
                    table.RejectChanges();
                    View.ShowError(Messages.datatable_update_failed);
                }
                else
                {
                    await ConnectTableToDataBaseAsync();
                    View.ShowMessage(Messages.datatable_updated);
                }
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
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, prevoisMonthTable);
            decimal prevoiusAmount = 0;
            DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            foreach (DataRow row in prevoisMonthTable.Rows.OfType<DataRow>()
                .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
                prevoiusAmount += decimal.Parse(row["amount"].ToString());

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());
            string ouputDate;
            if (dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
                ouputDate = dateBegin.ToString("MMMM yyyy");
            else
                ouputDate = dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, ouputDate);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, prevoiusAmount.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, (prevoiusAmount - ausgaben + einnahmen).ToString("C"));

            View.PrintBank(rows);
        }

        public virtual async Task DateChangedAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task PeriodCheckAsync()
        {
            View.SetPeriodControlsVisible(View.PeriodChecked);
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Export()
        {
            if (table == null)
                return;

            string fileName;
            if (!View.ShowSaveFileDialog(Messages.bank_export_filename, "Excel|*.xlsx", string.Empty, out fileName))
                return;

            Excel.ExportToExcel(table.DefaultView.ToTable(), fileName);
            View.ShowMessage(string.Format(Messages.export_success, fileName));
        }
    }
}
