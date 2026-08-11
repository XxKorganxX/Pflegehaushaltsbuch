using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class BookFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private readonly DataTable clientTable = new DataTable();
        private DataTable table;
        private int clientID;

        public BookFormPresenter(IBookFormContract view, SqlSession session)
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
            View.SetClientTable(clientTable);            
        }

        protected IBookFormContract View { get; private set; }

        public virtual void SetClientID(int clientID)
        {
            this.clientID = clientID;
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            table = new DataTable();
            if (!View.PeriodChecked)
            {
                DateTime date = View.FromDate;
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Book, table, clientID.ToString(), date.Month, date.Year);
            }
            else
            {
                DateTime fromDate = View.FromDate;
                DateTime toDate = View.ToDate.AddMonths(1);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.BooksByPeriod, table, clientID.ToString(), fromDate, toDate);
            }

            UpdateDocumentNumbers(table);

            View.SetBookTable(table);
        }

        public virtual void Back()
        {
            View.ShowClientsForm();
        }

        public virtual async Task GetClientInfoAsync()
        {
            clientTable.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, clientID.ToString());
        }

        public virtual async Task<DataTable> EnterAsync()
        {
            await ConnectTableToDataBaseAsync();
            await GetClientInfoAsync();

            return clientTable;
        }

        public virtual void ApplyCommentText(string comment)
        {
            if (clientTable == null || clientTable.Rows.Count == 0)
                return;

            clientTable.Rows[0]["note"] = comment;
        }

        public virtual void UpdateDocumentNumbers(DataTable datatable, DataRow ignore = null)
        {
            DateTime date = DateTime.MinValue;
            int belegNr = 1;
            DataRow[] rows = datatable.Select("", Columns.Date);
            foreach (DataRow row in rows)
            {
                if (row == ignore)
                    continue;

                DateTime currentDate = (DateTime)row[Columns.Date];
                if (date.Month != currentDate.Month || date.Year != currentDate.Year)
                {
                    date = currentDate;
                    belegNr = 1;
                }
                row[Columns.DocumentId] = belegNr++;
            }
        }

        public virtual async Task StornoAsync(DataRow row)
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            bool transactionCommitted = false;
            try
            {
                if (Int32.Parse(row[Columns.BookCategory].ToString()) == (int)SQLBase.BookCategory.Storno)
                    throw new Exception(Messages.booking_already_canceled);
                if (!View.ConfirmMessage(Messages.booking_canceling))
                    return;

                row[Columns.BookCategory] = (int)SQLBase.BookCategory.Storno;
                decimal amount = decimal.Parse(row[Columns.Amount].ToString());
                row[Columns.Amount] = 0;

                using (var transaction = session.SQL.BeginTransaction())
                {
                    try
                    {
                        bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Book, table);
                        if (!valid)
                            throw new Exception(Messages.booking_canceled_failed);

                        bool stornoBooked = true;
                        SQLBase.BookingTo bookingTo = (SQLBase.BookingTo)Int32.Parse(row["book_to"].ToString());
                        if (bookingTo == SQLBase.BookingTo.Bankbestand)
                            stornoBooked = await session.SQL.ToBankAsync(DateTime.Parse(row["date"].ToString()), row["note"].ToString(), -amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Storno, SQLBase.BookingTo.Bankbestand);
                        else if (bookingTo == SQLBase.BookingTo.Barbestand)
                            stornoBooked = await session.SQL.ToBargeAsync(DateTime.Parse(row["date"].ToString()), row["note"].ToString(), -amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Storno, SQLBase.BookingTo.Barbestand);

                        if (!stornoBooked)
                            throw new Exception(Messages.booking_canceled_failed);

                        transaction.Commit();
                        transactionCommitted = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                await ConnectTableToDataBaseAsync();
                await GetClientInfoAsync();
                View.ShowMessage(Messages.booking_canceled_success);
            }
            catch
            {
                if (!transactionCommitted)
                    table.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task UpdateClientNoteAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                if (!valid)
                    View.ShowError(Messages.book_entry_not_changed);
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task UpdateAccountStatusAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                View.EndEditAccount();

                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                if (!valid)
                {
                    clientTable.RejectChanges();
                    View.ShowError(Messages.datatable_update_failed);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void PrintAccount(string totalAmount)
        {
            if (table == null)
                return;

            DateTime from, to;
            from = to = View.FromDate;
            if (View.PeriodChecked)
                to = View.ToDate;
            to = to.AddMonths(1).AddHours(-1);
            if (to > DateTime.Now)
                to = DateTime.Now;
            View.ShowPrintBooksDialog(table, clientID, totalAmount, from, to);
        }

        public virtual async Task BookAsync(string clientName)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                ClientBookingInput input;
                if (!View.ShowClientBookDialog(clientName, clientID.ToString(), out input))
                    return;

                    decimal amount = input.Amount;
                    string bookText = input.BookText;
                    DateTime payInDate = input.BookingDate;
                    clientName = input.ClientName;
                    int clientId = input.ClientID;
                    SQLBase.BookingTo bookTo = input.BookingTarget;
                    SQLBase.BookCategory bookCategory = input.BookingCategory;
                    bool printQuittance = input.PrintQuittance;
                    bool valid = false;

                    DataRow currentBook = null;
                    using (var transaction = session.SQL.BeginTransaction())
                    {
                        try
                        {
                            if (bookTo == SQLBase.BookingTo.Barbestand)
                            {
                                if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                {
                                    var result = await session.SQL.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, amount, SQLBase.BookCategory.Einzahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                        valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientId), SQLBase.BookCategory.Einzahlung, bookTo);
                                }
                                else
                                {
                                    var result = await session.SQL.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, -amount, SQLBase.BookCategory.Auszahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                        valid = await session.SQL.ToBargeAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientId), SQLBase.BookCategory.Auszahlung, bookTo);
                                }
                            }
                            else if (bookTo == SQLBase.BookingTo.Bankbestand)
                            {
                                if (bookCategory == SQLBase.BookCategory.Einzahlung)
                                {
                                    var result = await session.SQL.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, amount, SQLBase.BookCategory.Einzahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                        valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientId), SQLBase.BookCategory.Einzahlung, bookTo);
                                }
                                else
                                {
                                    var result = await session.SQL.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, -amount, SQLBase.BookCategory.Auszahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                        valid = await session.SQL.ToBankAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientId), SQLBase.BookCategory.Auszahlung, bookTo);
                                }
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

                    if (valid)
                    {
                        if (printQuittance)
                        {
                            View.PrintQuittance(clientName, new DataRow[] { currentBook });
                        }
                        View.ShowMessage(Messages.booking_sucess);
                    }
                    else
                    {
                        throw new Exception(Messages.booking_failed);
                    }
                await ConnectTableToDataBaseAsync();
                await GetClientInfoAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
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
            if (!View.ShowSaveFileDialog(Messages.books_export_filename, "Excel|*.xlsx", string.Empty, out fileName))
                return;

            Excel.ExportToExcel(table.DefaultView.ToTable(), fileName);
            View.ShowMessage(string.Format(Messages.export_success, fileName));
        }

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Book, table);
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
    }
}
