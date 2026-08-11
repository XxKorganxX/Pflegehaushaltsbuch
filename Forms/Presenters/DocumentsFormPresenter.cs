using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DocumentsFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private bool isBindingClients;
        private DataTable table = new DataTable();
        private DataTable clientTable = new DataTable();
        private int clientID;

        public DocumentsFormPresenter(IDocumentsFormContract view, SqlSession session)
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

        protected IDocumentsFormContract View { get; private set; }

        public virtual void Initialize()
        {
            foreach (SQLBase.ClientActive enumval in Enum.GetValues(typeof(SQLBase.ClientActive)))
                View.AddActiveClientFilterItem(enumval.GetDisplayName());

            View.ActiveClientsIndex = 1;
        }

        public virtual async Task EnterAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await ConnectToClientsAsync();
                UpdateSelectedClientId();
                await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Leave()
        {
            table.Clear();
        }

        public virtual async Task ConnectToClientsAsync()
        {
            isBindingClients = true;
            try
            {
                View.ClearClients();
                clientTable = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns[Columns.Id] };

                string activeColumnName = Columns.Active;
                bool activeOnly = View.ActiveClientsIndex == 1;
                clientTable.DefaultView.RowFilter = clientTable.Columns[activeColumnName].DataType == typeof(bool)
                    ? string.Format("{0} = {1}", activeColumnName, activeOnly)
                    : string.Format("{0} = {1}", activeColumnName, View.ActiveClientsIndex);

                View.BindClients(clientTable.DefaultView);
            }
            finally
            {
                isBindingClients = false;
            }
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            table = new DataTable();
            if (!View.DateFilterChecked)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.RecordsByClient, table, clientID);
            else
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.RecordsByClientAndDate, table, clientID, View.DocumentDate.Month, View.DocumentDate.Year);

            View.BindDocuments(table);
        }

        public virtual async Task RefreshDocumentsAsync(bool refreshClients)
        {
            await databaseOperationLock.WaitAsync();

            try
            {
                if (refreshClients)
                    await ConnectToClientsAsync();

                UpdateSelectedClientId();
                await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task InsertAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (clientID == 0)
                {
                    View.ShowSelectClientFirst();
                    return;
                }

                string filename;
                if (!View.ShowOpenDocumentDialog(out filename))
                    return;

                DataTable clients = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clients);
                clients.PrimaryKey = new DataColumn[] { clients.Columns["id"] };
                if (clients.Rows.Count == 0)
                    throw new Exception(Messages.create_clients_first);

                CreateDocumentData document;
                if (!View.ShowCreateDocumentDialog(session, clientID, filename, clients, out document))
                    return;

                DataTable recordsTable = new DataTable();
                int client = document.SelectedClientID;
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.RecordsByClient, recordsTable, client);
                byte[] stream = File.ReadAllBytes(document.FilePath);
                DataRow row = recordsTable.NewRow();
                row["client_id"] = client;
                row["date"] = document.DocumentDate;
                row["note"] = document.Description;
                row["filename"] = document.DocumentFileName;
                row["file"] = stream;
                row["handsign"] = session.SQL.User.Name;
                recordsTable.Rows.Add(row);

                int index = 1;
                foreach (DataRow currentRow in recordsTable.Rows)
                    currentRow["index"] = index++;

                if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.RecordsByClient, recordsTable))
                    throw new Exception(Messages.datatable_update_failed);

                await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task DeleteAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (clientID == 0)
                {
                    View.ShowSelectClientFirst();
                    return;
                }

                foreach (DataRow row in View.GetSelectedDocuments())
                {
                    row.Delete();
                }

                bool result = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Records, table);

                if (!result)
                    throw new Exception(Messages.delete_failed);
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task ChangeAsync()
        {
            await RefreshDocumentsAsync(false);
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual void OpenDocument(int columnIndex, int rowIndex)
        {
            if (columnIndex == -1 || rowIndex == -1)
                return;

            DataRow row = table.Rows[rowIndex];
            string filename = row["filename"].ToString();
            byte[] file = row["file"] as byte[];
            filename = Path.Combine(Path.GetTempPath(), filename);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
                fs.Write(file, 0, file.Length);

            Process.Start(filename);
        }

        public virtual async Task ActiveClientsChangedAsync()
        {
            if (!View.ActiveClientsFocused)
                return;

            await RefreshDocumentsAsync(true);
        }

        public virtual async Task ClientChangedAsync()
        {
            if (isBindingClients)
                return;

            await RefreshDocumentsAsync(false);
        }

        public virtual async Task DateFilterChangedAsync()
        {
            if (!View.DateFilterFocused)
                return;

            await RefreshDocumentsAsync(false);
        }

        public virtual async Task DateChangedAsync()
        {
            if (!View.DateBoxContainsFocus)
                return;

            await RefreshDocumentsAsync(false);
        }

        public virtual void ClientDropDownClosed()
        {
            View.FocusDocumentsView();
        }

        private void UpdateSelectedClientId()
        {
            clientID = View.SelectedClientId;
        }
    }
}
