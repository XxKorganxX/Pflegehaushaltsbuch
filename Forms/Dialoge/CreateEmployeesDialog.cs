using System;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Employees Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateEmployeesDialog : Form, ICreateEmployeesDialogContract
    {
        private readonly CreateEmployeesDialogPresenter presenter;

        public int ID { get; set; }
        public string AssistantName { get; set; }
        public DateTime Date {get; set;}
        public decimal Amount { get; set; }
        /// <summary>
        /// Creates a new CreateAssistantsDialog view.
        /// </summary>
        public CreateEmployeesDialog(int id)
        {
            InitializeComponent();
            presenter = new CreateEmployeesDialogPresenter(this);
            presenter.Initialize(id);
        }
        /// <summary>
        /// Creates a new CreateAssistantsDialog view.
        /// </summary>
        public CreateEmployeesDialog(int id, string name, DateTime date, decimal amount)
        {
            InitializeComponent();
            presenter = new CreateEmployeesDialogPresenter(this);
            presenter.Initialize(id, name, date, amount);
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Ok();
        }

        /// <summary>
        /// Provides the id value for the presenter.
        /// </summary>
        int ICreateEmployeesDialogContract.ID
        {
            get { return ID; }
            set { ID = value; }
        }

        /// <summary>
        /// Provides the assistant name value for the presenter.
        /// </summary>
        string ICreateEmployeesDialogContract.AssistantName
        {
            get { return AssistantName; }
            set { AssistantName = value; }
        }

        /// <summary>
        /// Provides the date value for the presenter.
        /// </summary>
        DateTime ICreateEmployeesDialogContract.Date
        {
            get { return Date; }
            set { Date = value; }
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal ICreateEmployeesDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Runs the add book account view action for the presenter.
        /// </summary>
        void ICreateEmployeesDialogContract.AddBookAccount(string account)
        {
            bookAccountBox.Items.Add(account);
        }

        /// <summary>
        /// Runs the set book account index view action for the presenter.
        /// </summary>
        void ICreateEmployeesDialogContract.SetBookAccountIndex(int index)
        {
            bookAccountBox.SelectedIndex = index;
        }

        /// <summary>
        /// Runs the bind fields view action for the presenter.
        /// </summary>
        void ICreateEmployeesDialogContract.BindFields()
        {
            idBox.DataBindings.Add("Text", this, "ID");
            nameBox.DataBindings.Add("Text", this, "AssistantName");
            dateBox.DataBindings.Add("Date", this, "Date");
            amountBox.DataBindings.Add("Value", this, "Amount");
        }

        /// <summary>
        /// Runs the set amount enabled view action for the presenter.
        /// </summary>
        void ICreateEmployeesDialogContract.SetAmountEnabled(bool enabled)
        {
            amountBox.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICreateEmployeesDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
