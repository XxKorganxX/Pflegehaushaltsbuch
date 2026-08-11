using System;
using System.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Change User Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ChangeUserForm : Form, IChangeUserFormContract
    {
        private readonly ChangeUserFormPresenter presenter;

        /// <summary>
        /// Creates a new ChangeUserForm view.
        /// </summary>
        public ChangeUserForm(SqlSession session, string username, string keyword, DataTable users)
        {
            InitializeComponent();
            Session = session;
            presenter = new ChangeUserFormPresenter(this, session);
            presenter.Initialize(username, keyword, users);
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            await presenter.OkAsync();
        }

        /// <summary>
        /// Provides the user name value for the presenter.
        /// </summary>
        string IChangeUserFormContract.UserName
        {
            get { return usernameBox.Text.Trim(); }
            set { usernameBox.Text = value; }
        }

        /// <summary>
        /// Provides the login value for the presenter.
        /// </summary>
        string IChangeUserFormContract.Login
        {
            get { return loginBox.Text.Trim(); }
            set { loginBox.Text = value; }
        }

        /// <summary>
        /// Provides the keyword value for the presenter.
        /// </summary>
        string IChangeUserFormContract.Keyword
        {
            get { return pw0Box.Text.Trim(); }
        }

        /// <summary>
        /// Provides the keyword again value for the presenter.
        /// </summary>
        string IChangeUserFormContract.KeywordAgain
        {
            get { return pw1Box.Text.Trim(); }
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void IChangeUserFormContract.AcceptDialog()
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Runs the keep dialog open view action for the presenter.
        /// </summary>
        void IChangeUserFormContract.KeepDialogOpen()
        {
            DialogResult = System.Windows.Forms.DialogResult.None;
        }
    }
}
