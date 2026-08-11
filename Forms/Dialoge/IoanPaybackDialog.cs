using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Ioan Payback window and coordinates its user interface behavior.
    /// </summary>
    public partial class IoanPaybackDialog : Form, IIoanPaybackDialogContract
    {
        private readonly IoanPaybackDialogPresenter presenter;

        /// <summary>
        /// Creates a new IoanPaybackDialog view.
        /// </summary>
        public IoanPaybackDialog(SqlSession session, string name, int id, decimal amount)
        {
            InitializeComponent();
            Session = session;
            presenter = new IoanPaybackDialogPresenter(this);
            presenter.Initialize(name, id, amount);
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.Accept();
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }

        private int id;

        public decimal Amount { get; set; }

        public decimal MaximumAmount { get; private set; }

        /// <summary>
        /// Provides the assistant id value.
        /// </summary>
        public int AssistantId
        {
            get { return id; }
        }

        /// <summary>
        /// Provides the assistant name value.
        /// </summary>
        public string AssistantName
        {
            get { return nameBox.Text; }
        }

        /// <summary>
        /// Provides the payback date value.
        /// </summary>
        public DateTime PaybackDate
        {
            get { return date.Date.Date; }
        }

        /// <summary>
        /// Provides the repayment index value.
        /// </summary>
        public int RepaymentIndex
        {
            get { return repaymentBox.SelectedIndex; }
        }

        /// <summary>
        /// Provides the repayment value.
        /// </summary>
        public SQLBase.Repayment Repayment
        {
            get { return (SQLBase.Repayment)RepaymentIndex; }
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal IIoanPaybackDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Provides the maximum amount value for the presenter.
        /// </summary>
        decimal IIoanPaybackDialogContract.MaximumAmount
        {
            get { return MaximumAmount; }
            set { MaximumAmount = value; }
        }

        /// <summary>
        /// Provides the assistant id value for the presenter.
        /// </summary>
        int IIoanPaybackDialogContract.AssistantId
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Provides the assistant name value for the presenter.
        /// </summary>
        string IIoanPaybackDialogContract.AssistantName
        {
            get { return nameBox.Text; }
            set { nameBox.Text = value; }
        }

        /// <summary>
        /// Provides the payback date value for the presenter.
        /// </summary>
        DateTime IIoanPaybackDialogContract.PaybackDate
        {
            get { return PaybackDate; }
        }

        /// <summary>
        /// Provides the repayment index value for the presenter.
        /// </summary>
        int IIoanPaybackDialogContract.RepaymentIndex
        {
            get { return RepaymentIndex; }
        }

        /// <summary>
        /// Provides the repayment value for the presenter.
        /// </summary>
        SQLBase.Repayment IIoanPaybackDialogContract.Repayment
        {
            get { return Repayment; }
        }

        /// <summary>
        /// Runs the add repayment view action for the presenter.
        /// </summary>
        void IIoanPaybackDialogContract.AddRepayment(string repayment)
        {
            repaymentBox.Items.Add(repayment);
        }

        /// <summary>
        /// Runs the bind amount view action for the presenter.
        /// </summary>
        void IIoanPaybackDialogContract.BindAmount()
        {
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, 0, "C");
        }
    }
}
