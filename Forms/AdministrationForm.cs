using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Administration Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AdministrationForm : Form, IAdministrationFormContract
    {
        private readonly AdministrationFormPresenter presenter;

        /// <summary>
        /// Creates a new AdministrationForm view.
        /// </summary>
        public AdministrationForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new AdministrationFormPresenter(this, session);
        }

        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            ApplyCurrentUserRights();
            presenter.Enter();
        }

        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        /// <summary>
        /// Runs the user Rights operation and updates the related application state.
        /// </summary>
        public void UserRights(int access, bool admin, bool supervisor)
        {
            presenter.UserRights();
        }

        /// <summary>
        /// Handles the click event for client Management Button and updates the related state.
        /// </summary>
        private void clientManagementButton_Click(object sender, EventArgs e)
        {
            presenter.ClientManagement();
        }

        /// <summary>
        /// Handles the click event for cash Button and updates the related state.
        /// </summary>
        private void cashButton_Click(object sender, EventArgs e)
        {
            presenter.Cash();
        }

        /// <summary>
        /// Handles the click event for credit Button and updates the related state.
        /// </summary>
        private void creditButton_Click(object sender, EventArgs e)
        {
            presenter.Credit();
        }

        /// <summary>
        /// Handles the click event for account Holdings Button and updates the related state.
        /// </summary>
        private void accountHoldingsButton_Click(object sender, EventArgs e)
        {
            presenter.AccountHoldings();
        }

        /// <summary>
        /// Handles the click event for user Rights Button and updates the related state.
        /// </summary>
        private void userRightsButton_Click(object sender, EventArgs e)
        {
            presenter.UserRights();
        }

        /// <summary>
        /// Handles the click event for advisor Button and updates the related state.
        /// </summary>
        private void advisorButton_Click(object sender, EventArgs e)
        {
            presenter.Advisor();
        }

        /// <summary>
        /// Handles the click event for cash Office Controlbutton and updates the related state.
        /// </summary>
        private void cashOfficeControlbutton_Click(object sender, EventArgs e)
        {
            presenter.CashOfficeControl();
        }

        /// <summary>
        /// Handles the click event for banking Button and updates the related state.
        /// </summary>
        private void bankingButton_Click(object sender, EventArgs e)
        {
            presenter.Banking();
        }

        /// <summary>
        /// Handles the click event for record Button and updates the related state.
        /// </summary>
        private void recordButton_Click(object sender, EventArgs e)
        {
            presenter.Record();
        }

        /// <summary>
        /// Handles the click event for license Button and updates the related state.
        /// </summary>
        private void licenseButton_Click(object sender, EventArgs e)
        {
            presenter.License();
        }

        /// <summary>
        /// Handles the 1 event for exit Button Click and updates the related state.
        /// </summary>
        private void exitButton_Click_1(object sender, EventArgs e)
        {
            presenter.Main();
        }

        /// <summary>
        /// Handles the click event for database Backup Button and updates the related state.
        /// </summary>
        private async void databaseBackupButton_Click(object sender, EventArgs e)
        {
            await presenter.DatabaseBackupAsync();
        }

        /// <summary>
        /// Handles the click event for layout Button and updates the related state.
        /// </summary>
        private void layoutButton_Click(object sender, EventArgs e)
        {
            presenter.Layout();
        }

        /// <summary>
        /// Handles the click event for restore Button and updates the related state.
        /// </summary>
        private async void restoreButton_Click(object sender, EventArgs e)
        {
            await presenter.RestoreAsync();
        }

        /// <summary>
        /// Handles the click event for reset Database and updates the related state.
        /// </summary>
        private void resetDatabase_Click(object sender, EventArgs e)
        {
            presenter.ResetDatabase();
        }

        /// <summary>
        /// Handles the click event for improved Button and updates the related state.
        /// </summary>
        private void improvedButton_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the click event for db Connect Button and updates the related state.
        /// </summary>
        private async void dbConnectButton_Click(object sender, EventArgs e)
        {
            await presenter.DbConnectAsync();
        }

        /// <summary>
        /// Handles the click event for connect Embedded Database Button and updates the related state.
        /// </summary>
        private void connectEmbeddedDatabaseButton_Click(object sender, EventArgs e)
        {
            presenter.ConnectEmbeddedDatabase();
        }

        /// <summary>
        /// Handles the click event for company Button and updates the related state.
        /// </summary>
        private void companyButton_Click(object sender, EventArgs e)
        {
            presenter.Company();
        }

        /// <summary>
        /// Runs the data exchange button_click action.
        /// </summary>
        private void dataExchangeButton_Click(object sender, EventArgs e)
        {
            presenter.DataExchange();
        }

        /// <summary>
        /// Provides the is database connected value for the presenter.
        /// </summary>
        bool IAdministrationFormContract.IsDatabaseConnected
        {
            get { return Session.SQL != null; }
        }

        /// <summary>
        /// Provides the can administrate database value for the presenter.
        /// </summary>
        bool IAdministrationFormContract.CanAdministrateDatabase
        {
            get { return Session.SQL != null && Session.SQL.User.Admin; }
        }

        /// <summary>
        /// Runs the set administration buttons enabled view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.SetAdministrationButtonsEnabled(bool enabled)
        {
            layoutButton.Enabled =
            userRightsButton.Enabled =
            companyButton.Enabled =
            databaseBackupButton.Enabled =
            restoreButton.Enabled =
            disconnectDatabaseButton.Enabled =
            dataExchangeButton.Enabled =
                enabled;
        }

        /// <summary>
        /// Runs the set improved enabled view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.SetImprovedEnabled(bool enabled)
        {
        }

        /// <summary>
        /// Runs the set connect database enabled view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.SetConnectDatabaseEnabled(bool enabled)
        {
            connectDatabaseButton.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set view enabled view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.SetViewEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// Runs the show form view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.ShowForm(Enums.Forms form)
        {
            ShowFormEvent(form);
        }

        /// <summary>
        /// Runs the show backup file dialog view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ShowBackupFileDialog(out string fileName)
        {
            if (backupFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                fileName = string.Empty;
                return false;
            }

            fileName = backupFileDialog.FileName;
            return true;
        }

        /// <summary>
        /// Runs the show restore file dialog view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ShowRestoreFileDialog(out string fileName)
        {
            if (openBackupFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                fileName = string.Empty;
                return false;
            }

            fileName = openBackupFileDialog.FileName;
            return true;
        }

        /// <summary>
        /// Runs the show progress dialog view action for the presenter.
        /// </summary>
        IAdministrationProgress IAdministrationFormContract.ShowProgressDialog(string text)
        {
            ProgressDialog progressDialog = new ProgressDialog(text);
            progressDialog.Show(this);
            return progressDialog;
        }

        /// <summary>
        /// Runs the show database backup success view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.ShowDatabaseBackupSuccess()
        {
            MessageBox.ShowDialog(this, Messages.database_backup);
        }

        /// <summary>
        /// Runs the show database restore success view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.ShowDatabaseRestoreSuccess()
        {
            MessageBox.ShowDialog(this, Messages.database_restore_staging_success);
        }

        /// <summary>
        /// Runs the confirm database reset view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ConfirmDatabaseReset()
        {
            return MessageBox.ShowDialog(this, Messages.database_reset, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Runs the refresh access state view action for the presenter.
        /// </summary>
        void IAdministrationFormContract.RefreshAccessState()
        {
            OnEnter(null);
        }

        /// <summary>
        /// Runs the show database server connect dialog view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ShowDatabaseServerConnectDialog(SqlSession session, XmlConfig config)
        {
            using (DatabaseConnectionForm connectForm = new DatabaseConnectionForm(session, config))
            {
                return connectForm.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show database manager dialog view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ShowDatabaseManagerDialog(SqlSession session, XmlConfig config, out SQLBase sql)
        {
            using (DatabaseManagerForm connectDBForm = new DatabaseManagerForm(session, config))
            {
                if (connectDBForm.ShowDialog(this) != DialogResult.OK)
                {
                    sql = null;
                    return false;
                }

                sql = connectDBForm.DetachSql();
                return true;
            }
        }

        /// <summary>
        /// Runs the show user login dialog view action for the presenter.
        /// </summary>
        bool IAdministrationFormContract.ShowUserLoginDialog(SqlSession loginSession, out SQLBase authenticatedSql)
        {
            using (UserLoginForm userLoginForm = new UserLoginForm(loginSession))
            {
                if (userLoginForm.ShowDialog(this) != DialogResult.OK)
                {
                    authenticatedSql = null;
                    return false;
                }

                authenticatedSql = loginSession.Detach();
                return true;
            }
        }

    }
}
