using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Database Update Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseUpdateDialog : Form, IDatabaseUpdateDialogContract
    {
        private readonly DatabaseUpdateDialogPresenter presenter;

        /// <summary>
        /// Creates a new DatabaseUpdateDialog view.
        /// </summary>
        public DatabaseUpdateDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DatabaseUpdateDialogPresenter(this, session);
            presenter.Initialize();
        }
        /// <summary>
        /// Handles the shown event for database Update Form and updates the related state.
        /// </summary>
        private async void DatabaseUpdateForm_Shown(object sender, EventArgs e)
        {
            await presenter.ShownAsync();
        }

        /// <summary>
        /// Runs the set version view action for the presenter.
        /// </summary>
        void IDatabaseUpdateDialogContract.SetVersion(Version version)
        {
            if (versionBox.InvokeRequired)
            {
                versionBox.Invoke((MethodInvoker)delegate
                {
                    ((IDatabaseUpdateDialogContract)this).SetVersion(version);
                });
                return;
            }

            versionBox.Text = version.ToString();
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IDatabaseUpdateDialogContract.CloseView()
        {
            Close();
        }

        /// <summary>
        /// Runs the close owner view action for the presenter.
        /// </summary>
        void IDatabaseUpdateDialogContract.CloseOwner()
        {
            Owner?.Close();
        }

        /// <summary>
        /// Runs the show error view action for the presenter.
        /// </summary>
        void IDatabaseUpdateDialogContract.ShowError(Exception exception)
        {
            MessageBox.ShowError(this, exception);
        }

        /// <summary>
        /// Runs the exit application view action for the presenter.
        /// </summary>
        void IDatabaseUpdateDialogContract.ExitApplication(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
