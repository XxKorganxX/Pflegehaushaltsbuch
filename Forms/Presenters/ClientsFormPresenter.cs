using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ClientsFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private readonly DataTable deadLinesTable = new DataTable();
        private DataTable table;
        private string client;
        private int clientID;

        public ClientsFormPresenter(IClientsFormContract view, SqlSession session)
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

        protected IClientsFormContract View { get; private set; }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, table);
            table.PrimaryKey = new DataColumn[] { table.Columns[Columns.Id] };

            DateTime today = DateTime.Now;
            deadLinesTable.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.DeadlineByDay, deadLinesTable, today.Day);
            foreach (DataRow deadlineRow in deadLinesTable.Rows)
            {
                DataRow clientRow = table.Rows.Find(deadlineRow[Columns.Id]);
                if (clientRow != null)
                    clientRow[Columns.Info] = 1;
            }

            string activeColumnName = Columns.Active;
            bool activeOnly = View.ActiveClientsFilterIndex == 1;
            table.DefaultView.RowFilter = table.Columns[activeColumnName].DataType == typeof(bool)
                ? string.Format("{0} = {1}", activeColumnName, activeOnly)
                : string.Format("{0} = {1}", activeColumnName, View.ActiveClientsFilterIndex);

            UpdateTotalAmount();
            if (!string.IsNullOrWhiteSpace(View.CurrentSortColumn))
                table.DefaultView.Sort = View.CurrentSortColumn;
            else
                table.DefaultView.Sort = View.DefaultSortColumn;

            View.BindClients(table.DefaultView);
            View.BindClientDates(table.DefaultView);
            View.SetTotalClients(table.DefaultView.Count);

            RestoreSelectedClient();
            SelectionChanged();
        }

        public virtual void Leave()
        {
            DataRow row = GetSelectedRow();
            if (row != null)
            {
                client = row[Columns.Name].ToString();
                clientID = Int32.Parse(row[Columns.Id].ToString());
                View.NotifyClientIdChanged(clientID);
            }

            View.ClearClients();
            table?.Clear();
        }

        public virtual void SelectionChanged()
        {
            View.SetDeadlineText(string.Empty);
            DataRow row = GetSelectedRow();
            if (row == null)
                return;

            object value = row[Columns.Info];
            string selectedClientID = row[Columns.Id].ToString();
            if (value == DBNull.Value)
                return;

            foreach (DataRow deadlineRow in deadLinesTable.Rows)
            {
                if (selectedClientID.Equals(deadlineRow[Columns.Id].ToString()))
                {
                    View.SetDeadlineText(deadlineRow[Columns.Note].ToString());
                    break;
                }
            }
        }

        public virtual void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            if (table != null)
            {
                foreach (DataRowView rowView in table.DefaultView)
                {
                    decimal amount = 0;
                    DataRow row = rowView.Row;
                    if (decimal.TryParse(row[Columns.Amount].ToString(), out amount))
                        totalAmount += amount;
                }
            }

            View.SetTotalAmount(totalAmount.ToString("C"));
        }

        public virtual async Task CreateAccountAsync()
        {
            if (table == null)
                return;

            ClientAccountInput clientData;
            if (View.ShowCreateClientDialog(out clientData))
                await SaveAccountAsync(clientData, false);
        }

        public virtual async Task ChangeAsync()
        {
            DataRow row = GetSelectedRow();
            if (row == null)
                return;

            int selectedClientID = Int32.Parse(row[Columns.Id].ToString());
            ClientAccountInput clientData;
            if (View.ShowChangeClientDialog(selectedClientID, out clientData))
                await SaveAccountAsync(clientData, true);
        }

        public virtual async Task DeleteAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                DataRow rowOfView = GetSelectedRow();
                if (rowOfView == null)
                    return;

                int selectedClientID = Int32.Parse(rowOfView[Columns.Id].ToString());
                if (!View.ConfirmMessage(string.Format(Messages.clients_delete, View.SelectedClientName)))
                    return;

                DataTable bookTable = new DataTable(), bankTable = new DataTable(), bargeTable = new DataTable(), clientTable = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Books, bookTable);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, bankTable);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, bargeTable);
                string clientIdNumber = string.Format("K{0:000}", selectedClientID);

                foreach (DataRow row in bargeTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row[Columns.Account] == DBNull.Value || string.IsNullOrWhiteSpace(row[Columns.Account].ToString()))
                        throw new Exception(Messages.clients_delete_cash_not_assignable);
                    if (row[Columns.Account].ToString().Equals(clientIdNumber))
                        row.Delete();
                }

                foreach (DataRow row in bookTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row[Columns.Id] == DBNull.Value)
                        throw new Exception(Messages.clients_delete_books_not_assignable);
                    if (Int32.Parse(row[Columns.Id].ToString()) == selectedClientID)
                        row.Delete();
                }

                foreach (DataRow row in bankTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row[Columns.Account] == DBNull.Value || string.IsNullOrWhiteSpace(row[Columns.Account].ToString()))
                        throw new Exception(Messages.clients_delete_bank_not_assignable);
                    if (row[Columns.Account].ToString().Equals(clientIdNumber))
                        row.Delete();
                }

                using (var transaction = session.SQL.BeginTransaction())
                {
                    try
                    {
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Books, bookTable))
                            throw new Exception(Messages.clients_changed_failed);
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Cash, bargeTable))
                            throw new Exception(Messages.clients_changed_failed);
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Bank, bankTable))
                            throw new Exception(Messages.clients_changed_failed);
                        await session.SQL.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, selectedClientID);
                        if (clientTable.Rows.Count == 0)
                            throw new Exception(Messages.client_not_found);
                        clientTable.Rows[0].Delete();
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable))
                            throw new Exception(Messages.clients_changed_failed);
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

        public virtual void DeadLines()
        {
            if (GetSelectedRow() == null)
                return;

            View.ShowCalendarForm();
        }

        public virtual void SelectAccount()
        {
            if (GetSelectedRow() == null)
                return;

            View.ShowBookForm();
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual async Task PrintAsync()
        {
            if (table == null)
                return;

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients, View.TotalAmountText);
            DataTable printTable = table.Clone();
            foreach (DataRowView rowView in table.DefaultView)
                printTable.ImportRow(rowView.Row);
            printTable.Columns.Add(Columns.Credit, typeof(decimal));
            printTable.Columns.Add(Columns.Debit, typeof(decimal));
            foreach (DataRow row in printTable.Rows)
            {
                DateTime date = DateTime.Now;
                DataTable books = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Book, books, row[Columns.Id], date.Month, date.Year);
                decimal credit = 0, debit = 0;
                foreach (DataRow bookRow in books.Rows)
                {
                    decimal value = (decimal)bookRow[Columns.Amount];
                    if (value > 0)
                        credit += value;
                    else
                        debit += Math.Abs(value);
                }
                row[Columns.Credit] = credit;
                row[Columns.Debit] = debit;
            }
            View.PrintClients(printTable.Rows.OfType<DataRow>().ToArray());
        }

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, table);
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

        public virtual void ClientBooks()
        {
            View.ShowPrintClientsBooksDialog();
        }

        private async Task SaveAccountAsync(ClientAccountInput clientData, bool updateClient)
        {
            bool transactionCommitted = false;
            try
            {
                DataRow row = updateClient ? table.Rows.Find(clientData.ClientID) : table.NewRow();
                if (row == null)
                    throw new Exception(Messages.client_not_found);

                row[Columns.Id] = clientData.ClientID;
                row[Columns.Title] = clientData.Title;
                row[Columns.Name] = clientData.Name;
                row[Columns.Street] = clientData.Street;
                row[Columns.Zipcode] = clientData.Zipcode;
                row[Columns.City] = clientData.City;
                row[Columns.Born] = clientData.BornDate;
                row[Columns.AdvisorId] = clientData.AdvisorId.HasValue ? (object)clientData.AdvisorId.Value : DBNull.Value;
                row[Columns.HandSign] = session.SQL.User.Name;

                if (!updateClient)
                {
                    row[Columns.Date] = DateTime.Now.Date;
                    row[Columns.Amount] = clientData.Amount;
                    row[Columns.AccountTransfer] = clientData.Amount;
                    row[Columns.Active] = (int)SQLBase.ClientActive.Active;
                    row[Columns.Info] = 0;
                    table.Rows.Add(row);
                }

                using (var transaction = session.SQL.BeginTransaction())
                {
                    try
                    {
                        if (!updateClient && table.Columns.Contains(Columns.AccountId))
                            row[Columns.AccountId] = await session.SQL.CreateAccountIdAsync("Client");

                        bool value = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, table);
                        if (!value)
                            throw new Exception(Messages.clients_changed_failed);

                        if (!updateClient && clientData.Amount != 0)
                        {
                            bool openingBalanceBooked = await session.SQL.ToBankAsync(
                                DateTime.Now.Date,
                                string.Format(Messages.clients_previous_amount, clientData.Name),
                                clientData.Amount,
                                string.Format("K{0:000}", clientData.ClientID),
                                SQLBase.BookCategory.Einzahlung,
                                SQLBase.BookingTo.Altbestand);
                            if (!openingBalanceBooked)
                                throw new Exception(Messages.clients_changed_failed);
                        }

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
                View.ShowMessage(Messages.clients_changed);
            }
            catch
            {
                if (!transactionCommitted)
                    table.RejectChanges();
                throw;
            }
        }

        private async Task ImportClientsAsync(ClientImportInput importData)
        {
            DataTable importTable = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, importTable);
            importTable.PrimaryKey = new DataColumn[] { importTable.Columns[Columns.Id] };

            foreach (ClientImportRecord clientData in importData.Clients)
            {
                DataRow row = importTable.NewRow();
                row[Columns.Id] = clientData.Id;
                row[Columns.Title] = clientData.Title;
                row[Columns.Name] = clientData.Name;
                row[Columns.Street] = clientData.Street;
                row[Columns.Zipcode] = clientData.Zipcode;
                row[Columns.City] = clientData.City;
                row[Columns.Born] = clientData.BornDate;
                row[Columns.Date] = clientData.CreatedDate;
                row[Columns.Amount] = clientData.OpeningBalance;
                row[Columns.AccountTransfer] = clientData.OpeningBalance;
                row[Columns.Active] = (int)SQLBase.ClientActive.Active;
                row[Columns.AdvisorId] = clientData.AdvisorId.HasValue ? (object)clientData.AdvisorId.Value : DBNull.Value;
                row[Columns.HandSign] = session.SQL.User.Name;
                importTable.Rows.Add(row);
            }

            using (var transaction = session.SQL.BeginTransaction())
            {
                try
                {
                    foreach (DataRow row in importTable.Rows)
                    {
                        if (row.RowState == DataRowState.Added && importTable.Columns.Contains(Columns.AccountId))
                            row[Columns.AccountId] = await session.SQL.CreateAccountIdAsync("Client");
                    }

                    bool saved = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Clients, importTable);
                    if (!saved)
                        throw new Exception(Messages.clients_changed_failed);

                    foreach (ClientImportRecord clientData in importData.Clients)
                    {
                        if (clientData.OpeningBalance == 0)
                            continue;

                        bool booked = await session.SQL.ToBankAsync(
                            clientData.CreatedDate,
                            string.Format(Messages.clients_previous_amount, clientData.Name),
                            clientData.OpeningBalance,
                            string.Format("K{0:000}", clientData.Id),
                            SQLBase.BookCategory.Einzahlung,
                            SQLBase.BookingTo.Altbestand);
                        if (!booked)
                            throw new Exception(Messages.clients_changed_failed);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            await ConnectTableToDataBaseAsync();
            View.ShowMessage(Messages.clients_imported);
        }

        private DataTable CreateClientExportTable(DataTable advisorTable)
        {
            DataTable exportTable = new DataTable();
            foreach (string columnName in ImportClientsDialogPresenter.EnglishImportColumns)
            {
                exportTable.Columns.Add(columnName);
            }
            exportTable.Columns.Add(Columns.HandSign);

            foreach (DataRowView rowView in table.DefaultView)
            {
                DataRow sourceRow = rowView.Row;
                DataRow exportRow = exportTable.NewRow();
                exportRow[Columns.ExportDebitorNumber] = sourceRow[Columns.Id];
                exportRow[Columns.ExportTitle] = sourceRow[Columns.Title];
                exportRow[Columns.ExportName] = sourceRow[Columns.Name];
                exportRow[Columns.ExportBorn] = sourceRow[Columns.Born];
                exportRow[Columns.ExportStreet] = sourceRow[Columns.Street];
                exportRow[Columns.ExportZip] = sourceRow[Columns.Zipcode];
                exportRow[Columns.ExportCity] = sourceRow[Columns.City];
                exportRow[Columns.ExportAdvisor] = GetAdvisorName(advisorTable, sourceRow);
                exportRow[Columns.ExportPreviousBalance] = sourceRow[Columns.AccountTransfer];
                exportRow[Columns.HandSign] = sourceRow.Table.Columns.Contains(Columns.HandSign) ? sourceRow[Columns.HandSign] : string.Empty;
                exportTable.Rows.Add(exportRow);
            }

            return exportTable;
        }

        private string GetAdvisorName(DataTable advisorTable, DataRow clientRow)
        {
            if (clientRow[Columns.AdvisorId] == DBNull.Value)
                return string.Empty;

            DataRow advisorRow = advisorTable.Rows.Find(clientRow[Columns.AdvisorId]);
            return advisorRow == null ? string.Empty : advisorRow[Columns.Name].ToString();
        }

        private void RestoreSelectedClient()
        {
            if (string.IsNullOrWhiteSpace(client))
                return;

            View.SelectClientByName(client);
        }

        private DataRow GetSelectedRow()
        {
            int? selectedClientId = View.SelectedClientId;
            if (!selectedClientId.HasValue || table == null)
                return null;

            return table.Rows.Find(selectedClientId.Value);
        }
    }
}
