using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the User Login Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class UserLoginForm : Form, IUserLoginFormContract
    {
        private readonly UserLoginFormPresenter presenter;

        /// <summary>
        /// Creates a new UserLoginForm view.
        /// </summary>
        public UserLoginForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new UserLoginFormPresenter(this, session);
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            ApplyCurrentUserRights();
            //userNameBox.Focus();
        }
        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            await presenter.ConnectAsync();
        }
        /// <summary>
        /// Handles the click event for cancel Button and updates the related state.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            presenter.Cancel();
        }
        /// <summary>
        /// Handles the click event for reset User Button and updates the related state.
        /// </summary>
        private async void resetUserButton_Click(object sender, EventArgs e)
        {
            await presenter.ResetAndAcceptAsync();
        }
        /// <summary>
        /// Handles the enter event for password Box and updates the related state.
        /// </summary>
        private void passwordBox_Enter(object sender, EventArgs e)
        {
            presenter.PasswordEnter();
        }
        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            presenter.Close();
        }

        /// <summary>
        /// Provides the user name value for the presenter.
        /// </summary>
        string IUserLoginFormContract.UserName
        {
            get { return userNameBox.Text.Trim(); }
        }

        /// <summary>
        /// Provides the password value for the presenter.
        /// </summary>
        string IUserLoginFormContract.Password
        {
            get { return passwordBox.Text.Trim(); }
        }

        /// <summary>
        /// Runs the clear password view action for the presenter.
        /// </summary>
        void IUserLoginFormContract.ClearPassword()
        {
            passwordBox.Text = string.Empty;
        }

        /// <summary>
        /// Runs the show change user dialog view action for the presenter.
        /// </summary>
        bool IUserLoginFormContract.ShowChangePasswordDialog(out string keyword)
        {
            using (ChangeUserForm createPasswordForm = new ChangeUserForm())
            {
                bool accepted = createPasswordForm.ShowDialog(this) == DialogResult.OK;
                keyword = accepted ? ((IChangeUserFormContract)createPasswordForm).Keyword : string.Empty;
                return accepted;
            }
        }

        /// <summary>
        /// Runs the show user data changed view action for the presenter.
        /// </summary>
        void IUserLoginFormContract.ShowUserDataChanged()
        {
            MessageBox.ShowDialog(this, Messages.login_userdata_changed);
        }

        /// <summary>
        /// Runs the set accepted view action for the presenter.
        /// </summary>
        void IUserLoginFormContract.SetAccepted()
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IUserLoginFormContract.CloseView()
        {
            Close();
        }
    }
}
