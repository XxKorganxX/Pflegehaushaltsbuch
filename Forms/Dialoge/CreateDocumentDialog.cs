using System;
using System.Data;
using System.IO;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Create Document Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateDocumentDialog : Form, ICreateDocumentDialogContract
    {
        private readonly CreateDocumentDialogPresenter presenter;

        private string filename = "";
        private int clientID;

        /// <summary>
        /// Provides the file path value.
        /// </summary>
        public string FilePath
        {
            get { return filename; }
            private set
            {
                filename = value ?? string.Empty;
                fileBox.Text = filename;
            }
        }

        /// <summary>
        /// Provides the document file name value.
        /// </summary>
        public string DocumentFileName
        {
            get { return Path.GetFileName(FilePath); }
        }

        /// <summary>
        /// Provides the selected client id value.
        /// </summary>
        public int SelectedClientID
        {
            get
            {
                if (clientBox.SelectedValue == null)
                    return clientID;
                return Int32.Parse(clientBox.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Provides the document date value.
        /// </summary>
        public DateTime DocumentDate
        {
            get { return dateBox.Date.Date; }
        }

        /// <summary>
        /// Provides the description value.
        /// </summary>
        public string Description
        {
            get { return richTextBox.Text; }
        }

        /// <summary>
        /// Creates a new CreateDocumentDialog view.
        /// </summary>
        public CreateDocumentDialog(SqlSession session, int clientID, string filename, DataTable table)
        {
            InitializeComponent();
            Session = session;
            this.clientID = clientID;
            presenter = new CreateDocumentDialogPresenter(this, session, clientID, filename, table);
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            presenter.Ok();
        }
        /// <summary>
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private void importButton_Click(object sender, EventArgs e)
        {
            presenter.Import();
        }

        /// <summary>
        /// Provides the file path value for the presenter.
        /// </summary>
        string ICreateDocumentDialogContract.FilePath
        {
            get { return FilePath; }
            set { FilePath = value; }
        }

        /// <summary>
        /// Provides the description value for the presenter.
        /// </summary>
        string ICreateDocumentDialogContract.Description
        {
            get { return Description; }
        }

        /// <summary>
        /// Provides the selected client id value for the presenter.
        /// </summary>
        int ICreateDocumentDialogContract.SelectedClientID
        {
            get { return SelectedClientID; }
        }

        /// <summary>
        /// Runs the bind clients view action for the presenter.
        /// </summary>
        void ICreateDocumentDialogContract.BindClients(DataTable table, int clientID)
        {
            clientBox.DisplayMember = Columns.Name;
            clientBox.ValueMember = Columns.Id;
            clientBox.DataSource = table;
            clientBox.SelectedValue = clientID;
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICreateDocumentDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
