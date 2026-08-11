using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Main Menu Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class MainMenuForm : Form, IMainMenuFormContract
    {
        private readonly MainMenuFormPresenter presenter;

        /// <summary>
        /// Creates a new MainMenuForm view.
        /// </summary>
        public MainMenuForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new MainMenuFormPresenter(this, session);

            presenter.Initialize();
        }

        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override async void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            await presenter.EnterAsync();
        }

        /// <summary>
        /// Runs the user Rights operation and updates the related application state.
        /// </summary>
        public void UserRights(int access, bool admin, bool supervisor)
        {
            presenter.UserRights(access, admin, supervisor);
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
        /// Handles the click event for layout Button and updates the related state.
        /// </summary>
        private void layoutButton_Click(object sender, EventArgs e)
        {
            presenter.Layout();
        }

        /// <summary>
        /// Handles the click event for office Cash Button and updates the related state.
        /// </summary>
        private void officeCashButton_Click(object sender, EventArgs e)
        {
            presenter.OfficeCash();
        }

        /// <summary>
        /// Runs the set work panels enabled view action for the presenter.
        /// </summary>
        void IMainMenuFormContract.SetWorkPanelsEnabled(bool enabled)
        {
            cashPanel.Enabled =
            bankingPanel.Enabled =
            clientsPanel.Enabled =
            advisorPanel.Enabled =
            employeesPanel.Enabled =
            cashCheckPanel.Enabled =
            statisticsPanel.Enabled =
            OfficeCashPanel.Enabled =
            recordPanel.Enabled =
            enabled;
        }

        /// <summary>
        /// Runs the set admin visible view action for the presenter.
        /// </summary>
        void IMainMenuFormContract.SetAdminVisible(bool visible)
        {
            adminPanel.Visible = visible;
        }

        /// <summary>
        /// Runs the show error view action for the presenter.
        /// </summary>
        void IMainMenuFormContract.ShowError(Exception exception)
        {
            MessageBox.ShowError(this, exception);
        }

        /// <summary>
        /// Runs the show user login dialog view action for the presenter.
        /// </summary>
        bool IMainMenuFormContract.ShowUserLoginDialog(SqlSession session)
        {
            using (UserLoginForm dialog = new UserLoginForm(session))
            {
                return dialog.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show form view action for the presenter.
        /// </summary>
        void IMainMenuFormContract.ShowForm(Enums.Forms form)
        {
            ShowFormEvent(form);
        }

        private void languagePictureBox_Click(object sender, EventArgs e)
        {
            languagePictureBox.ContextMenuStrip.Show(languagePictureBox, new System.Drawing.Point(languagePictureBox.Width/2, languagePictureBox.Height/2));
        }

        private void englischToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en");
        }

        private void germanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("de");
        }

        private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("zh-Hans");
        }

        private void spainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("es-ES");
        }

        private void ChangeLanguage(string cultureName)
        {
            Settings.Default.language = cultureName;
            Settings.Default.Save();

            Application.Restart();
        }

        private void aboutPictureBox_Click(object sender, EventArgs e)
        {
            ShowFormEvent(Enums.Forms.AboutUs);
        }
    }
}
