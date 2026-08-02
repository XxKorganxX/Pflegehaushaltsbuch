using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the User Login Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class UserLoginForm : Pflegehaushaltsbuch.FormControls.Form, IUserLoginFormContract
    {
        private readonly UserLoginFormPresenter presenter;


        private bool login = false;
        /// <summary>
        /// Creates a new User Login Form instance and initializes the required state.
        /// </summary>
        public UserLoginForm(SQLBase sql)
        {
            InitializeComponent();
            presenter = new UserLoginFormPresenter(this);
            this.sql = sql;
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            //userNameBox.Focus();
        }
        /*
        /// <summary>
        /// Handles the closing lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!login)
            {
                if (Owner != null && !Owner.IsDisposed)
                {
                    e.Cancel = true;
                    Owner.Close();
                    return;
                }
            }
            base.OnClosing(e);
        }
        */
        /// <summary>
        /// Runs the login operation and updates the related application state.
        /// </summary>
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(userNameBox.Text))
                throw new Exception(Messages.login_insert_username);
            await UserAuthenticator.LoginAsync(sql, userNameBox.Text.Trim(), passwordBox.Text.Trim());
            if (string.IsNullOrWhiteSpace(passwordBox.Text.Trim()))
            {
                DataTable users = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Users, users);

                using (ChangeUserForm createPasswordForm = new ChangeUserForm(sql, userNameBox.Text.Trim(), passwordBox.Text.Trim(), users))
                {
                   if (createPasswordForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                       throw new Exception(Messages.login_keyword_unchanged);
                }
            }
        }
        /// <summary>
        /// Runs the reset User operation and updates the related application state.
        /// </summary>
        private async Task ResetUser()
        {
            string user = userNameBox.Text.Trim();
            string keyword = passwordBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(user))
                throw new Exception(Messages.login_insert_username);
            if (user.ToLower().StartsWith(Messages.login_guest))
                throw new Exception(Messages.login_guest_access_proteced);
            await UserAuthenticator.LoginAsync(sql, user, keyword);

            DataTable users = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, users);

            using (ChangeUserForm createPasswordForm = new ChangeUserForm(sql, user, keyword, users))
            {
                if (createPasswordForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    throw new Exception(Messages.login_keyword_unchanged);
            }
            MessageBox.ShowDialog(this, Messages.login_userdata_changed);
        }
        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            await Login();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        /// <summary>
        /// Handles the click event for cancel Button and updates the related state.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Handles the click event for reset User Button and updates the related state.
        /// </summary>
        private async void resetUserButton_Click(object sender, EventArgs e)
        {
            await ResetUser();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        /// <summary>
        /// Handles the enter event for password Box and updates the related state.
        /// </summary>
        private void passwordBox_Enter(object sender, EventArgs e)
        {
            passwordBox.Text = string.Empty;
        }
        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
