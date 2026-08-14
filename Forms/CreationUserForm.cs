using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Creation User Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreationUserForm : Form, ICreationUserFormContract
    {
        private readonly CreationUserFormPresenter presenter;
        private bool requireSuccessfulCreation;

        /// <summary>
        /// Creates a new CreationUserForm view.
        /// </summary>
        public CreationUserForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreationUserFormPresenter(this, session);
            if (Program.DesignMode)
                return;

            presenter.InitializeNew();
        }

        public CreationUserForm(SqlSession session, bool requireSuccessfulCreation)
            : this(session)
        {
            this.requireSuccessfulCreation = requireSuccessfulCreation;
            if (!requireSuccessfulCreation)
                return;

            handsignBox.Text = "🛡️";
            loginBox.Text = "Admin";
            adminCheckBox.Checked = true;
            adminCheckBox.Enabled = false;
            cancelButton.Visible = false;
            CancelButton = null;
            ControlBox = false;
            FormClosing += CreationUserForm_FormClosing;
        }

        /// <summary>
        /// Creates a new CreationUserForm view.
        /// </summary>
        public CreationUserForm(SqlSession session, DataRow row)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreationUserFormPresenter(this, session);
            if (Program.DesignMode)
                return;

            presenter.InitializeExisting(row);
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            await presenter.OkAsync();
        }

        private void CreationUserForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (requireSuccessfulCreation && DialogResult != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }

        /// <summary>
        /// Provides the user name value for the presenter.
        /// </summary>
        string ICreationUserFormContract.Handsign
        {
            get { return handsignBox.Text.Trim(); }
            set { handsignBox.Text = value; }
        }

        /// <summary>
        /// Provides the login value for the presenter.
        /// </summary>
        string ICreationUserFormContract.Login
        {
            get { return loginBox.Text.Trim(); }
            set { loginBox.Text = value; }
        }

        /// <summary>
        /// Provides the insert allowed value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.InsertAllowed
        {
            get { return createCheckBox.Checked; }
            set { createCheckBox.Checked = value; }
        }

        /// <summary>
        /// Provides the change allowed value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.ChangeAllowed
        {
            get { return changeCheckBox.Checked; }
            set { changeCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.BookAllowed
        {
            get { return bookCheckBox.Checked; }
            set { bookCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.CancelBookingAllowed
        {
            get { return cancelBookingCheckBox.Checked; }
            set { cancelBookingCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.CashBalanceAllowed
        {
            get { return cashBalanceCheckBox.Checked; }
            set { cashBalanceCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.BankBalanceAllowed
        {
            get { return bankBalanceCheckBox.Checked; }
            set { bankBalanceCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.PettyCashAllowed
        {
            get { return pettyCashCheckBox.Checked; }
            set { pettyCashCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.ClientsAllowed
        {
            get { return clientsCheckBox.Checked; }
            set { clientsCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.RepresentativesAllowed
        {
            get { return representativesCheckBox.Checked; }
            set { representativesCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.EmployeesAllowed
        {
            get { return employeesCheckBox.Checked; }
            set { employeesCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.DocumentsAllowed
        {
            get { return documentsCheckBox.Checked; }
            set { documentsCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.CashAuditAllowed
        {
            get { return cashAuditCheckBox.Checked; }
            set { cashAuditCheckBox.Checked = value; }
        }

        bool ICreationUserFormContract.StatisticsAllowed
        {
            get { return statisticsCheckBox.Checked; }
            set { statisticsCheckBox.Checked = value; }
        }

        /// <summary>
        /// Provides the admin value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.Admin
        {
            get { return adminCheckBox.Checked; }
            set { adminCheckBox.Checked = value; }
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void ICreationUserFormContract.AcceptDialog()
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Runs the bind data view action for the presenter.
        /// </summary>
        void ICreationUserFormContract.BindData()
        {
        }

        /// <summary>
        /// Runs the show user changed view action for the presenter.
        /// </summary>
        void ICreationUserFormContract.ShowUserChanged()
        {
            MessageBox.ShowDialog(this, Messages.usermanagement_user_changed);
        }

        /// <summary>
        /// Runs the show user created view action for the presenter.
        /// </summary>
        void ICreationUserFormContract.ShowUserCreated()
        {
            MessageBox.ShowDialog(this, Messages.usermanagement_user_created);
        }
    }
}
