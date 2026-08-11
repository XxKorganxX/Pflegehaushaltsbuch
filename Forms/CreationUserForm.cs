using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.ComponentModel;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Creation User Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreationUserForm : Form, ICreationUserFormContract
    {
        private readonly CreationUserFormPresenter presenter;

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

        /// <summary>
        /// Handles the checked Changed event for access and updates the related state.
        /// </summary>
        private void access_CheckedChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the validating event for email Box and updates the related state.
        /// </summary>
        private void emailBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !presenter.IsEmailValid();
        }

        /// <summary>
        /// Provides the user name value for the presenter.
        /// </summary>
        string ICreationUserFormContract.UserName
        {
            get { return nameBox.Text.Trim(); }
            set { nameBox.Text = value; }
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
        /// Provides the phone value for the presenter.
        /// </summary>
        string ICreationUserFormContract.Phone
        {
            get { return phoneBox.Text.Trim(); }
            set { phoneBox.Text = value; }
        }

        /// <summary>
        /// Provides the fax value for the presenter.
        /// </summary>
        string ICreationUserFormContract.Fax
        {
            get { return faxBox.Text.Trim(); }
            set { faxBox.Text = value; }
        }

        /// <summary>
        /// Provides the email value for the presenter.
        /// </summary>
        string ICreationUserFormContract.Email
        {
            get { return emailBox.Text.Trim(); }
            set { emailBox.Text = value; }
        }

        /// <summary>
        /// Provides the insert allowed value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.InsertAllowed
        {
            get { return insertBox.Checked; }
            set { insertBox.Checked = value; }
        }

        /// <summary>
        /// Provides the change allowed value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.ChangeAllowed
        {
            get { return changeBox.Checked; }
            set { changeBox.Checked = value; }
        }

        /// <summary>
        /// Provides the delete allowed value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.DeleteAllowed
        {
            get { return deleteBox.Checked; }
            set { deleteBox.Checked = value; }
        }

        /// <summary>
        /// Provides the admin value for the presenter.
        /// </summary>
        bool ICreationUserFormContract.Admin
        {
            get { return adminBox.Checked; }
            set { adminBox.Checked = value; }
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
