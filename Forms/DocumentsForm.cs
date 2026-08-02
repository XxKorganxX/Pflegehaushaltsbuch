using Microsoft.Data.SqlClient;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Documents Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DocumentsForm : Pflegehaushaltsbuch.FormControls.Form, IDocumentsFormContract
    {
        private readonly DocumentsFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private bool isBindingClients;
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private DataTable table = new DataTable(), clientTable = new DataTable();
        private int clientID = 0;
        /// <summary>
        /// Creates a new Documents Form instance and initializes the required state.
        /// </summary>
        public DocumentsForm()
        {
            InitializeComponent();
            presenter = new DocumentsFormPresenter(this);
            foreach (SQLBase.ClientActive enumval in Enum.GetValues(typeof(SQLBase.ClientActive)))
                activeClientsBox.Items.Add(enumval.GetDisplayName());
            dateBox.DataBindings.Add("Enabled", dateCheckBox, "Checked");
            if (Program.DesignMode)
                return;
            view.AutoGenerateColumns = false;
            this.Enter += clientPanel_Enter;
            this.Leave += clientPanel_Leave;
            activeClientsBox.SelectedIndex = 1;
        }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnUserRights(SQLBase sql)
        {
            base.OnUserRights(sql);
            insertButton.Enabled = sql.User.CanInsert;
            deleteButton.Enabled = sql.User.CanDelete;
        }
        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await ConnectToClients();
                UpdateSelectedClientId();
                await ConnectTableToDataBase();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            table.Clear();
        }
        /// <summary>
        /// Connects the to Clients data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectToClients()
        {
            isBindingClients = true;
            try
            {
                clientBox.DataSource = null;
                clientTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns["id"] };

                string activeColumnName = "active";
                bool activeOnly = activeClientsBox.SelectedIndex == 1;
                clientTable.DefaultView.RowFilter = clientTable.Columns[activeColumnName].DataType == typeof(bool)
                    ? string.Format("{0} = {1}", activeColumnName, activeOnly)
                    : string.Format("{0} = {1}", activeColumnName, activeClientsBox.SelectedIndex);

                clientBox.DataSource = clientTable.DefaultView;
                clientBox.DisplayMember = "name";
            }
            finally
            {
                isBindingClients = false;
            }
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            if (!dateCheckBox.Checked)
                await sql.FillAdapterAsync(SQLBase.SELECT.RecordsByClient, table, clientID);
            else
                await sql.FillAdapterAsync(SQLBase.SELECT.RecordsByClientAndDate, table, clientID, dateBox.Date.Month, dateBox.Date.Year);
            view.DataSource = table;
        }
        private void UpdateSelectedClientId()
        {
            DataRowView rowView = clientBox.SelectedItem as DataRowView;
            clientID = 0;
            if (rowView != null)
                clientID = Int32.Parse(rowView.Row["id"].ToString());
        }
        private async Task RefreshDocumentsAsync(bool refreshClients)
        {
            await databaseOperationLock.WaitAsync();

            try
            {
                if (refreshClients)
                    await ConnectToClients();
                UpdateSelectedClientId();
                await ConnectTableToDataBase();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for insert Button and updates the related state.
        /// </summary>
        private async void insertButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (clientID == 0)
                {
                    MessageBox.ShowDialog(this, Messages.clients_select_first);
                    return;
                }
                string filename = "";
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = true;
                    if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                        return;
                    filename = openFileDialog.FileName;
                }

                DataTable table = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Clients, table);
                table.PrimaryKey = new DataColumn[] { table.Columns["id"] };
                if (table.Rows.Count == 0)
                    throw new Exception(Messages.create_clients_first);

                using (CreateDocumentDialog documentDialog = new CreateDocumentDialog(sql, clientID, filename, table))
                {
                    if (documentDialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    DataTable recordsTable = new DataTable();
                    int client = documentDialog.SelectedClientID;
                    await sql.FillAdapterAsync(SQLBase.SELECT.RecordsByClient, recordsTable, client);
                    byte[] stream = File.ReadAllBytes(documentDialog.FilePath);
                    DataRow row = recordsTable.NewRow();
                    row["client_id"] = client;
                    row["date"] = documentDialog.DocumentDate;
                    row["note"] = documentDialog.Description;
                    row["filename"] = documentDialog.DocumentFileName;
                    row["file"] = stream;
                    row["handsign"] = sql.User.Name;
                    recordsTable.Rows.Add(row);
                    int index = 1;
                    foreach (DataRow currentRow in recordsTable.Rows)
                        currentRow["index"] = index++;
                    if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.RecordsByClient, recordsTable))
                        throw new Exception(Messages.datatable_update_failed);

                    await ConnectTableToDataBase();
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (clientID == 0)
                {
                    MessageBox.ShowDialog(this, Messages.clients_select_first);
                    return;
                }

                foreach (DataGridViewRow selectedRow in view.SelectedRows)
                {
                    (selectedRow.DataBoundItem as DataRowView).Row.Delete();
                }

                var result = await sql.UpdateAdapterAsync(SQLBase.SELECT.Records, table);

                if (!result)
                    throw new Exception(Messages.delete_failed);

                //ConnectTableToDataBase();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await RefreshDocumentsAsync(false);
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        /// <summary>
        /// Handles the cell Double Click event for view and updates the related state.
        /// </summary>
        private void view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            DataRow row = table.Rows[e.RowIndex];
            string filename = row["filename"].ToString();
            byte[] file = row["file"] as byte[];
            filename = Path.Combine(Path.GetTempPath(), filename);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
                fs.Write(file, 0, file.Length);
            Process.Start(filename);
        }
        /// <summary>
        /// Handles the selected Index Changed event for active Clients Box and updates the related state.
        /// </summary>
        private async void activeClientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!activeClientsBox.Focused)
                return;
            await RefreshDocumentsAsync(true);
        }
        /// <summary>
        /// Handles the selected Index Changed event for client Box and updates the related state.
        /// </summary>
        private async void clientBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isBindingClients)
                return;

            await RefreshDocumentsAsync(false);
        }
        /// <summary>
        /// Handles the checked Changed event for date Check Box and updates the related state.
        /// </summary>
        private async void dateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!dateCheckBox.Focused)
                return;
            await RefreshDocumentsAsync(false);
        }
        /// <summary>
        /// Handles the value Changed event for date Box and updates the related state.
        /// </summary>
        private async void dateBox_ValueChanged()
        {
            if (Program.DesignMode)
                return;
            if (!dateBox.ContainsFocus)
                return;
            await RefreshDocumentsAsync(false);
        }
        /// <summary>
        /// Handles the drop Down Closed event for client Box and updates the related state.
        /// </summary>
        private void clientBox_DropDownClosed(object sender, EventArgs e)
        {
            view.Focus();
        }
    }
}
