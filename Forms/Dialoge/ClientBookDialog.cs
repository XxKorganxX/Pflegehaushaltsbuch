using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.WPFControls;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Client Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ClientBookDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private DataTable clientsTable = new DataTable();
        private string bookText = "";
        public string BookText
        {
            get
            {
                return bookText;
            }
            set
            {
                bookText = value;
            }
        }
        public decimal Amount { get; set; }
        public int BookTo { get; set; }
        public int BookCategory { get; set; }
        public string ClientName
        {
            get { return clientBox.Text; }
        }

        public int ClientID
        {
            get
            {
                int clientID;
                Int32.TryParse(clientIdBox.Text, out clientID);
                return clientID;
            }
        }

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
        /// <summary>
        /// Creates a new Client Book Form instance and initializes the required state.
        /// </summary>
        public ClientBookDialog(SQLBase sql, string clientName, string clientID)
        {
            InitializeComponent();
            if (Program.DesignMode)
                return;
            var bookTextBox = new UserTextBox();
            bookTextBox.Bind(System.Windows.Controls.TextBox.TextProperty, this, "BookText");
            bookTextHost.Child = bookTextBox;
            bookTextHost.Invalidate();
            bookingCategoryBox.Items.Add(SQLBase.BookCategory.Einzahlung.GetDisplayName());
            bookingCategoryBox.Items.Add(SQLBase.BookCategory.Auszahlung.GetDisplayName());
            bookingToBox.Items.Add(SQLBase.BookingTo.Barbestand.GetDisplayName());
            bookingToBox.Items.Add(SQLBase.BookingTo.Bankbestand.GetDisplayName());
            clientBox.Items.Add(clientName);
            clientIdBox.Items.Add(clientID);
            clientBox.SelectedItem = clientName;
            clientIdBox.SelectedItem = clientID;
            this.sql = sql;
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, string.Empty, "C");
            bookingToBox.DataBindings.Add("SelectedIndex", this, "BookTo");
            bookingCategoryBox.DataBindings.Add("SelectedIndex", this, "BookCategory");
        }
        /// <summary>
        /// Handles the load event for booking Form and updates the related state.
        /// </summary>
        private void BookingForm_Load(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
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
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
    }
}
