using System;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Cash Office Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashOfficeBookDialog : Form, ICashOfficeBookDialogContract
    {
        private readonly CashOfficeBookDialogPresenter presenter;

        public decimal Amount { get; set; }
        public int Account { get; set; }
        public string BookText { get; set; }
        /// <summary>
        /// Provides the booking date value.
        /// </summary>
        public DateTime BookingDate
        {
            get { return dateBox.Date.Date; }
        }

        /// <summary>
        /// Provides the booking category value.
        /// </summary>
        public SQLBase.BookCategory BookingCategory
        {
            get { return (SQLBase.BookCategory)bookingKindBox.SelectedIndex; }
        }
        /// <summary>
        /// Creates a new CashOfficeBookDialog view.
        /// </summary>
        public CashOfficeBookDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CashOfficeBookDialogPresenter(this, session);
            presenter.Initialize();
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.ValidateOk();
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal ICashOfficeBookDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Provides the account value for the presenter.
        /// </summary>
        int ICashOfficeBookDialogContract.Account
        {
            get { return Account; }
            set { Account = value; }
        }

        /// <summary>
        /// Provides the book text value for the presenter.
        /// </summary>
        string ICashOfficeBookDialogContract.BookText
        {
            get { return BookText; }
            set { BookText = value; }
        }

        /// <summary>
        /// Provides the booking date value for the presenter.
        /// </summary>
        DateTime ICashOfficeBookDialogContract.BookingDate
        {
            get { return BookingDate; }
        }

        /// <summary>
        /// Runs the bind fields view action for the presenter.
        /// </summary>
        void ICashOfficeBookDialogContract.BindFields()
        {
            bookTextBox.DataBindings.Add("Text", this, "BookText");
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0, "C", Session.Company.Currencies);
        }

        /// <summary>
        /// Runs the set booking category index view action for the presenter.
        /// </summary>
        void ICashOfficeBookDialogContract.SetBookingCategoryIndex(int index)
        {
            bookingKindBox.SelectedIndex = index;
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICashOfficeBookDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
