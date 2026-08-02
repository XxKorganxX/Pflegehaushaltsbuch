using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.WPFControls;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Cash Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashBookDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private DataTable clientsTable = new DataTable();
        public string BookText { get; set; }
        public decimal Amount { get; set; }
        public int BookTo { get; set; }
        public int BookCategory { get; set; }
        public int ClientActive { get; set; }
        public DateTime BookingDate
        {
            get { return payInDate.Date.Date; }
        }

        public SQLBase.BookingTo BookingTarget
        {
            get { return (SQLBase.BookingTo)BookTo; }
        }

        public SQLBase.BookCategory BookingCategory
        {
            get { return (SQLBase.BookCategory)BookCategory; }
        }

        public bool PrintQuittance
        {
            get { return quittanceButton.Checked; }
        }

        public IEnumerable<ID_Client_Data> SelectedClients
        {
            get { return clientList.CheckedItems.Cast<ID_Client_Data>().ToArray(); }
        }

        public string ClientName
        {
            get
            {
                ID_Client_Data client = SelectedClients.FirstOrDefault();
                return client == null ? string.Empty : client.Name;
            }
        }

        public int ClientID
        {
            get
            {
                ID_Client_Data client = SelectedClients.FirstOrDefault();
                return client == null ? 0 : client.ID;
            }
        }
        /// <summary>
        /// Creates a new Cash Book Form instance and initializes the required state.
        /// </summary>
        public CashBookDialog(SQLBase sql)
        {
            InitializeComponent();
            this.sql = sql;
            bookingCategoryBox.Items.Add(SQLBase.BookCategory.Einzahlung.GetDisplayName());
            bookingCategoryBox.Items.Add(SQLBase.BookCategory.Auszahlung.GetDisplayName());
            bookingToBox.Items.Add(SQLBase.BookingTo.Barbestand.GetDisplayName());
            bookingToBox.Items.Add(SQLBase.BookingTo.Bankbestand.GetDisplayName());
            var bookTextBox = new UserTextBox();
            bookTextBox.Bind(System.Windows.Controls.TextBox.TextProperty, this, "BookText");
            bookTextHost.Child = bookTextBox;
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, string.Empty, "C");
            bookingToBox.DataBindings.Add("SelectedIndex", this, "BookTo");
            bookingCategoryBox.DataBindings.Add("SelectedIndex", this, "BookCategory");
        }
        /// <summary>
        /// Handles the load event for booking Form and updates the related state.
        /// </summary>
        private async void BookingForm_Load(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            await ConnectTableToDataBase();
        }
        private Dictionary<string, ID_Client_Data> clientData = new Dictionary<string, ID_Client_Data>();
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            clientList.DataSource = null;
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientsTable, string.Empty);
            clientsTable.PrimaryKey = new DataColumn[]
            {
                    clientsTable.Columns[SQLBase.Names(SQLBase.ColumnNames.id)]
            };
            DataRow[] rows = clientsTable.Select();

            clientList.Items.Clear();
            clientLookUpBox.Clear();
            foreach (DataRow row in rows)
            {
                var data = new ID_Client_Data()
                {
                    Name = row[SQLBase.Names(SQLBase.ColumnNames.name)].ToString(),
                    ID = Int32.Parse(row[SQLBase.Names(SQLBase.ColumnNames.id)].ToString())
                };
                clientData[data.Name] = data;
                clientList.Items.Add(data);
                clientLookUpBox.AutoCompleteCustomSource.Add(data.Name);
            }
        }
        /// <summary>
        /// Handles the key Press event for amount Box and updates the related state.
        /// </summary>
        private void amountBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != ',')
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if (e.KeyChar == ','
                && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Amount == 0)
                    throw new Exception(Messages.missing_amount);
                if (string.IsNullOrWhiteSpace(BookText))
                    throw new Exception(Messages.missing_bookingtext);
                if (payInDate.Date == DateTime.MinValue || payInDate.Date > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
                if (BookingTarget == SQLBase.BookingTo.Barbestand && !SelectedClients.Any())
                    throw new Exception(Messages.clients_select_first);
            }
            catch 
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the selected Index Changed event for booking Box and updates the related state.
        /// </summary>
        private void bookingBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            if (bookingToBox.SelectedIndex == 1)
            {
                quittanceButton.Hide();
                clientLookUpBox.Hide();
                clientList.Hide();
                ChangeButtonState();
                quittanceButton.Visible = false;
            }
            else
            {
                quittanceButton.Show();
                clientList.Show();
                clientLookUpBox.Show();
                ChangeButtonState();
                quittanceButton.Visible = true;
            }
            
            ResumeLayout();
            Refresh();
        }
        /// <summary>
        /// Handles the text Changed event for client Active Box and updates the related state.
        /// </summary>
        private void clientActiveBox_TextChanged(object sender, EventArgs e)
        {
            ChangeButtonState();
        }
        /// <summary>
        /// Runs the change Button State operation and updates the related application state.
        /// </summary>
        private void ChangeButtonState()
        {
            if (bookingToBox.SelectedIndex != 1)
            {
                okButton.Enabled = true;
                return;
            }
        }
        /// <summary>
        /// Handles the validating event for client Look Up Box and updates the related state.
        /// </summary>
        private void clientLookUpBox_Validating(object sender, CancelEventArgs e)
        {
            ID_Client_Data data;
            if (!clientData.TryGetValue(clientLookUpBox.Text, out data))
                return;
            clientList.SelectedItem = data;
        }
        /// <summary>
        /// Handles the text Changed event for client Look Up Box and updates the related state.
        /// </summary>
        private void clientLookUpBox_TextChanged(object sender, EventArgs e)
        {
            ID_Client_Data data;
            if (!clientData.TryGetValue(clientLookUpBox.Text, out data))
                return;
            clientList.SelectedItem = data;
            clientList.SetItemChecked(clientList.SelectedIndex, !clientList.GetItemChecked(clientList.SelectedIndex));
        }
    }
}
