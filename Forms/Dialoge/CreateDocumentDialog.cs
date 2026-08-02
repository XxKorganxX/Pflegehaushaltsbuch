using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
using System.CodeDom;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Create Document Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateDocumentDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private string filename = "";
        private int clientID;

        public string FilePath
        {
            get { return filename; }
            private set
            {
                filename = value ?? string.Empty;
                fileBox.Text = filename;
            }
        }

        public string DocumentFileName
        {
            get { return Path.GetFileName(FilePath); }
        }

        public int SelectedClientID
        {
            get
            {
                if (clientBox.SelectedValue == null)
                    return clientID;
                return Int32.Parse(clientBox.SelectedValue.ToString());
            }
        }

        public DateTime DocumentDate
        {
            get { return dateBox.Date.Date; }
        }

        public string Description
        {
            get { return richTextBox.Text; }
        }

        /// <summary>
        /// Creates a new Create Document Form instance and initializes the required state.
        /// </summary>
        public CreateDocumentDialog(SQLBase sql, int clientID, string filename, DataTable table)
        {
            InitializeComponent();
            this.sql = sql;
            this.clientID = clientID;
            FilePath = filename;

            clientBox.DisplayMember = "name";
            clientBox.ValueMember = "id";
            clientBox.DataSource = table;
            clientBox.SelectedValue = clientID;
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                {
                    throw new Exception(Messages.document_missing_filename);
                }
                if (string.IsNullOrWhiteSpace(Description))
                {
                    throw new Exception(Messages.document_missing_description);
                }
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private void importButton_Click(object sender, EventArgs e)
        {
        }
    }
}
