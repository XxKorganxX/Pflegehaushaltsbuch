using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Cash Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashBookDialog : Form, ICashBookDialogContract
    {
        private readonly CashBookDialogPresenter presenter;

        public string BookText { get; set; }
        public decimal Amount { get; set; }
        public int ClientActive { get; set; }
        /// <summary>
        /// Provides the booking date value.
        /// </summary>
        public DateTime BookingDate
        {
            get { return payInDate.Date.Date; }
        }

        /// <summary>
        /// Provides the booking target value.
        /// </summary>
        public SQLBase.BookingTo BookingTarget
        {
            get { return (SQLBase.BookingTo)bookingToBox.SelectedIndex; }
        }

        /// <summary>
        /// Provides the booking category value.
        /// </summary>
        public SQLBase.BookCategory BookingCategory
        {
            get { return (SQLBase.BookCategory)bookingCategoryBox.SelectedIndex; }
        }

        /// <summary>
        /// Provides the print quittance value.
        /// </summary>
        public bool PrintQuittance
        {
            get { return quittanceButton.Checked; }
        }

        /// <summary>
        /// Provides the selected clients value.
        /// </summary>
        public IEnumerable<ID_Client_Data> SelectedClients
        {
            get { return clientList.CheckedItems.Cast<ID_Client_Data>().ToArray(); }
        }

        /// <summary>
        /// Provides the client name value.
        /// </summary>
        public string ClientName
        {
            get
            {
                ID_Client_Data client = SelectedClients.FirstOrDefault();
                return client == null ? string.Empty : client.Name;
            }
        }

        /// <summary>
        /// Provides the client id value.
        /// </summary>
        public int ClientID
        {
            get
            {
                ID_Client_Data client = SelectedClients.FirstOrDefault();
                return client == null ? 0 : client.ID;
            }
        }
        /// <summary>
        /// Creates a new CashBookDialog view.
        /// </summary>
        public CashBookDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CashBookDialogPresenter(this, session);
            presenter.Initialize();
        }
        /// <summary>
        /// Handles the load event for booking Form and updates the related state.
        /// </summary>
        private async void BookingForm_Load(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            await presenter.LoadAsync();
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
            presenter.ValidateOk();
        }
        /// <summary>
        /// Handles the selected Index Changed event for booking Box and updates the related state.
        /// </summary>
        private void bookingBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            presenter.BookingTargetChanged();
            
            ResumeLayout();
            Refresh();
        }
        /// <summary>
        /// Handles the text Changed event for client Active Box and updates the related state.
        /// </summary>
        private void clientActiveBox_TextChanged(object sender, EventArgs e)
        {
            presenter.ClientActiveChanged();
        }
        /// <summary>
        /// Handles the validating event for client Look Up Box and updates the related state.
        /// </summary>
        private void clientLookUpBox_Validating(object sender, CancelEventArgs e)
        {
            presenter.ClientLookupValidated();
        }
        /// <summary>
        /// Handles the text Changed event for client Look Up Box and updates the related state.
        /// </summary>
        private void clientLookUpBox_TextChanged(object sender, EventArgs e)
        {
            presenter.ClientLookupTextChanged();
        }

        /// <summary>
        /// Provides the book text value for the presenter.
        /// </summary>
        string ICashBookDialogContract.BookText
        {
            get { return BookText; }
            set { BookText = value; }
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal ICashBookDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Provides the book to value for the presenter.
        /// </summary>
        int ICashBookDialogContract.BookTo
        {
            get { return bookingToBox.SelectedIndex; }
            set { bookingToBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the book category value for the presenter.
        /// </summary>
        int ICashBookDialogContract.BookCategory
        {
            get { return bookingCategoryBox.SelectedIndex; }
            set { bookingCategoryBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the booking date value for the presenter.
        /// </summary>
        DateTime ICashBookDialogContract.BookingDate
        {
            get { return BookingDate; }
        }

        /// <summary>
        /// Provides the booking target value for the presenter.
        /// </summary>
        SQLBase.BookingTo ICashBookDialogContract.BookingTarget
        {
            get { return BookingTarget; }
        }

        /// <summary>
        /// Provides the selected clients value for the presenter.
        /// </summary>
        IEnumerable<ID_Client_Data> ICashBookDialogContract.SelectedClients
        {
            get { return SelectedClients; }
        }

        /// <summary>
        /// Provides the client lookup text value for the presenter.
        /// </summary>
        string ICashBookDialogContract.ClientLookupText
        {
            get { return clientLookUpBox.Text; }
        }

        /// <summary>
        /// Runs the add booking category view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.AddBookingCategory(string text)
        {
            bookingCategoryBox.Items.Add(text);
            if (bookingCategoryBox.SelectedIndex < 0)
                bookingCategoryBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs the add booking target view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.AddBookingTarget(string text)
        {
            bookingToBox.Items.Add(text);
            if (bookingToBox.SelectedIndex < 0)
                bookingToBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs the bind fields view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.BindFields()
        {
            bookTextBox.DataBindings.Add("Text", this, "BookText");
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, string.Empty, "C");
        }

        /// <summary>
        /// Runs the clear clients view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.ClearClients()
        {
            clientList.DataSource = null;
            clientList.Items.Clear();
            clientLookUpBox.Clear();
        }

        /// <summary>
        /// Runs the add client view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.AddClient(ID_Client_Data client)
        {
            clientList.Items.Add(client);
        }

        /// <summary>
        /// Runs the add client lookup name view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.AddClientLookupName(string name)
        {
            clientLookUpBox.AutoCompleteCustomSource.Add(name);
        }

        /// <summary>
        /// Runs the set client selection view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.SetClientSelection(ID_Client_Data client)
        {
            clientList.SelectedItem = client;
        }

        /// <summary>
        /// Runs the toggle selected client checked view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.ToggleSelectedClientChecked()
        {
            if (clientList.SelectedIndex < 0)
                return;

            clientList.SetItemChecked(clientList.SelectedIndex, !clientList.GetItemChecked(clientList.SelectedIndex));
        }

        /// <summary>
        /// Runs the set client selection visible view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.SetClientSelectionVisible(bool visible)
        {
            quittanceButton.Visible = visible;
            clientList.Visible = visible;
            clientLookUpBox.Visible = visible;
        }

        /// <summary>
        /// Runs the set ok enabled view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.SetOkEnabled(bool enabled)
        {
            okButton.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICashBookDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
