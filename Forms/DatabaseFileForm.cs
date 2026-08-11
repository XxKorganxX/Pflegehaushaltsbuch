using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database File Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseFileForm : Form, IDatabaseFileFormContract
    {
        private readonly DatabaseFileFormPresenter presenter;

        /// <summary>
        /// Creates a new DatabaseFileForm view.
        /// </summary>
        public DatabaseFileForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DatabaseFileFormPresenter(this, session);
        }

        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateAsync();
        }

        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            await presenter.ConnectAsync();
        }

        /// <summary>
        /// Provides the password value for the presenter.
        /// </summary>
        string IDatabaseFileFormContract.Password
        {
            get { return passwordBox.Text; }
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void IDatabaseFileFormContract.AcceptDialog()
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Runs the show default login message view action for the presenter.
        /// </summary>
        void IDatabaseFileFormContract.ShowDefaultLoginMessage()
        {
            MessageBox.ShowDialog(this, Messages.database_default_login, Messages.database_default_login_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
