using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Documents Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DocumentsForm : Form, IDocumentsFormContract
    {
        private readonly DocumentsFormPresenter presenter;

        /// <summary>
        /// Creates a new DocumentsForm view.
        /// </summary>
        public DocumentsForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DocumentsFormPresenter(this, session);
            presenter.Initialize();
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
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            insertButton.Enabled = rights.CanInsert;
            deleteButton.Enabled = rights.CanDelete;
        }
        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.EnterAsync();
        }
        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            presenter.Leave();
        }
        /// <summary>
        /// Handles the click event for insert Button and updates the related state.
        /// </summary>
        private async void insertButton_Click(object sender, EventArgs e)
        {
            await presenter.InsertAsync();
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            await presenter.DeleteAsync();
        }
        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await presenter.ChangeAsync();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
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
            presenter.OpenDocument(e.ColumnIndex, e.RowIndex);
        }
        /// <summary>
        /// Handles the selected Index Changed event for active Clients Box and updates the related state.
        /// </summary>
        private async void activeClientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            await presenter.ActiveClientsChangedAsync();
        }
        /// <summary>
        /// Handles the selected Index Changed event for client Box and updates the related state.
        /// </summary>
        private async void clientBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            await presenter.ClientChangedAsync();
        }
        /// <summary>
        /// Handles the checked Changed event for date Check Box and updates the related state.
        /// </summary>
        private async void dateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await presenter.DateFilterChangedAsync();
        }
        /// <summary>
        /// Handles the value Changed event for date Box and updates the related state.
        /// </summary>
        private async void dateBox_ValueChanged()
        {
            if (Program.DesignMode)
                return;

            await presenter.DateChangedAsync();
        }
        /// <summary>
        /// Handles the drop Down Closed event for client Box and updates the related state.
        /// </summary>
        private void clientBox_DropDownClosed(object sender, EventArgs e)
        {
            presenter.ClientDropDownClosed();
        }

        /// <summary>
        /// Provides the active clients index value for the presenter.
        /// </summary>
        int IDocumentsFormContract.ActiveClientsIndex
        {
            get { return activeClientsBox.SelectedIndex; }
            set { activeClientsBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the active clients focused value for the presenter.
        /// </summary>
        bool IDocumentsFormContract.ActiveClientsFocused
        {
            get { return activeClientsBox.Focused; }
        }

        /// <summary>
        /// Provides the date filter checked value for the presenter.
        /// </summary>
        bool IDocumentsFormContract.DateFilterChecked
        {
            get { return dateCheckBox.Checked; }
        }

        /// <summary>
        /// Provides the date filter focused value for the presenter.
        /// </summary>
        bool IDocumentsFormContract.DateFilterFocused
        {
            get { return dateCheckBox.Focused; }
        }

        /// <summary>
        /// Provides the date box contains focus value for the presenter.
        /// </summary>
        bool IDocumentsFormContract.DateBoxContainsFocus
        {
            get { return dateBox.ContainsFocus; }
        }

        /// <summary>
        /// Provides the document date value for the presenter.
        /// </summary>
        DateTime IDocumentsFormContract.DocumentDate
        {
            get { return dateBox.Date; }
        }

        /// <summary>
        /// Provides the selected client id for the presenter.
        /// </summary>
        int IDocumentsFormContract.SelectedClientId
        {
            get
            {
                DataRowView rowView = clientBox.SelectedItem as DataRowView;
                if (rowView == null || rowView.Row[Columns.Id] == DBNull.Value)
                    return 0;

                return Convert.ToInt32(rowView.Row[Columns.Id]);
            }
        }

        /// <summary>
        /// Runs the add active client filter item view action for the presenter.
        /// </summary>
        void IDocumentsFormContract.AddActiveClientFilterItem(string item)
        {
            activeClientsBox.Items.Add(item);
        }

        /// <summary>
        /// Binds the client list to the view.
        /// </summary>
        void IDocumentsFormContract.BindClients(DataView clients)
        {
            clientBox.DataSource = clients;
            clientBox.DisplayMember = Columns.Name;
        }

        /// <summary>
        /// Clears the client list binding.
        /// </summary>
        void IDocumentsFormContract.ClearClients()
        {
            clientBox.DataSource = null;
        }

        /// <summary>
        /// Binds the documents list to the view.
        /// </summary>
        void IDocumentsFormContract.BindDocuments(DataTable documents)
        {
            view.DataSource = documents;
        }

        /// <summary>
        /// Provides selected document rows for the presenter.
        /// </summary>
        DataRow[] IDocumentsFormContract.GetSelectedDocuments()
        {
            return view.SelectedRows
                .OfType<DataGridViewRow>()
                .Select(selectedRow => selectedRow.DataBoundItem as DataRowView)
                .Where(rowView => rowView != null)
                .Select(rowView => rowView.Row)
                .ToArray();
        }

        /// <summary>
        /// Runs the show open document dialog view action for the presenter.
        /// </summary>
        bool IDocumentsFormContract.ShowOpenDocumentDialog(out string fileName)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    fileName = string.Empty;
                    return false;
                }

                fileName = openFileDialog.FileName;
                return true;
            }
        }

        /// <summary>
        /// Runs the show create document dialog view action for the presenter.
        /// </summary>
        bool IDocumentsFormContract.ShowCreateDocumentDialog(SqlSession session, int clientID, string fileName, DataTable clients, out CreateDocumentData document)
        {
            using (CreateDocumentDialog documentDialog = new CreateDocumentDialog(session, clientID, fileName, clients))
            {
                if (documentDialog.ShowDialog(this) != DialogResult.OK)
                {
                    document = null;
                    return false;
                }

                document = new CreateDocumentData
                {
                    SelectedClientID = documentDialog.SelectedClientID,
                    FilePath = documentDialog.FilePath,
                    DocumentDate = documentDialog.DocumentDate,
                    Description = documentDialog.Description,
                    DocumentFileName = documentDialog.DocumentFileName
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the show select client first view action for the presenter.
        /// </summary>
        void IDocumentsFormContract.ShowSelectClientFirst()
        {
            MessageBox.ShowDialog(this, Messages.clients_select_first);
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IDocumentsFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        /// <summary>
        /// Runs the focus documents view action for the presenter.
        /// </summary>
        void IDocumentsFormContract.FocusDocumentsView()
        {
            view.Focus();
        }
    }
}
