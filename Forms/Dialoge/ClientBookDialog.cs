using System;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Client Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ClientBookDialog : Form, IClientBookDialogContract
    {
        private readonly ClientBookDialogPresenter presenter;

        private string bookText = "";
        /// <summary>
        /// Provides the book text value.
        /// </summary>
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
        /// <summary>
        /// Provides the client name value.
        /// </summary>
        public string ClientName
        {
            get { return clientBox.Text; }
        }

        /// <summary>
        /// Provides the client id value.
        /// </summary>
        public int ClientID
        {
            get
            {
                int clientID;
                Int32.TryParse(clientIdBox.Text, out clientID);
                return clientID;
            }
        }

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
        /// Creates a new ClientBookDialog view.
        /// </summary>
        public ClientBookDialog(SqlSession session, string clientName, string clientID)
        {
            InitializeComponent();
            Session = session;
            presenter = new ClientBookDialogPresenter(this, session);
            if (Program.DesignMode)
                return;

            presenter.Initialize(clientName, clientID);
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
            presenter.ValidateOk();
        }

        /// <summary>
        /// Provides the book text value for the presenter.
        /// </summary>
        string IClientBookDialogContract.BookText
        {
            get { return BookText; }
            set { BookText = value; }
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal IClientBookDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Provides the book to value for the presenter.
        /// </summary>
        int IClientBookDialogContract.BookTo
        {
            get { return bookingToBox.SelectedIndex; }
            set { bookingToBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the book category value for the presenter.
        /// </summary>
        int IClientBookDialogContract.BookCategory
        {
            get { return bookingCategoryBox.SelectedIndex; }
            set { bookingCategoryBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the booking date value for the presenter.
        /// </summary>
        DateTime IClientBookDialogContract.BookingDate
        {
            get { return BookingDate; }
        }

        /// <summary>
        /// Runs the add booking category view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.AddBookingCategory(string text)
        {
            bookingCategoryBox.Items.Add(text);
            if (bookingCategoryBox.SelectedIndex < 0)
                bookingCategoryBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs the add booking target view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.AddBookingTarget(string text)
        {
            bookingToBox.Items.Add(text);
            if (bookingToBox.SelectedIndex < 0)
                bookingToBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs the add client view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.AddClient(string clientName, string clientID)
        {
            clientBox.Items.Add(clientName);
            clientIdBox.Items.Add(clientID);
        }

        /// <summary>
        /// Runs the select client view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.SelectClient(string clientName, string clientID)
        {
            clientBox.SelectedItem = clientName;
            clientIdBox.SelectedItem = clientID;
        }

        /// <summary>
        /// Runs the bind fields view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.BindFields()
        {
            bookTextBox.DataBindings.Add("Text", this, "BookText");
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, string.Empty, "C", Session.Company.Currencies);
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void IClientBookDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
