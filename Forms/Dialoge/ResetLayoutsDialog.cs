using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Reset Layouts window and coordinates its user interface behavior.
    /// </summary>
    public partial class ResetLayoutsDialog : Form, IResetLayoutsDialogContract
    {
        private readonly ResetLayoutsDialogPresenter presenter;

        /// <summary>
        /// Creates a new ResetLayoutsDialog view.
        /// </summary>
        public ResetLayoutsDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new ResetLayoutsDialogPresenter(this, session);
        }

        /// <summary>
        /// Handles the checked Changed event for all Box and updates the related state.
        /// </summary>
        private void allBox_CheckedChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.ResetSelectedLayouts();
        }

        /// <summary>
        /// Handles the click event for cash Box and updates the related state.
        /// </summary>
        private void cashBox_Click(object sender, EventArgs e)
        {
            presenter.UpdateAllSelectionFromLayouts();
        }

        /// <summary>
        /// Handles the click event for all Box and updates the related state.
        /// </summary>
        private void allBox_Click(object sender, EventArgs e)
        {
            presenter.ApplyAllSelection();
        }

        /// <summary>
        /// Provides the all checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.AllChecked
        {
            get { return allBox.Checked; }
            set { allBox.Checked = value; }
        }

        /// <summary>
        /// Provides the clients checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.ClientsChecked
        {
            get { return clientsBox.Checked; }
            set { clientsBox.Checked = value; }
        }

        /// <summary>
        /// Provides the advisors checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.AdvisorsChecked
        {
            get { return advisorsBox.Checked; }
            set { advisorsBox.Checked = value; }
        }

        /// <summary>
        /// Provides the employee checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.EmployeeChecked
        {
            get { return employeeBox.Checked; }
            set { employeeBox.Checked = value; }
        }

        /// <summary>
        /// Provides the cash checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.CashChecked
        {
            get { return cashBox.Checked; }
            set { cashBox.Checked = value; }
        }

        /// <summary>
        /// Provides the bank checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.BankChecked
        {
            get { return bankBox.Checked; }
            set { bankBox.Checked = value; }
        }

        /// <summary>
        /// Provides the bill checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.BillChecked
        {
            get { return billBox.Checked; }
            set { billBox.Checked = value; }
        }

        /// <summary>
        /// Provides the cash check checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.CashCheckChecked
        {
            get { return cashCheckBox.Checked; }
            set { cashCheckBox.Checked = value; }
        }

        /// <summary>
        /// Provides the quittance checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.QuittanceChecked
        {
            get { return quittanceBox.Checked; }
            set { quittanceBox.Checked = value; }
        }

        /// <summary>
        /// Provides the office cash checked value for the presenter.
        /// </summary>
        bool IResetLayoutsDialogContract.OfficeCashChecked
        {
            get { return officeCashBox.Checked; }
            set { officeCashBox.Checked = value; }
        }
    }
}
