using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Improved Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ImprovedForm : Form, IImprovedFormContract
    {
        private readonly ImprovedFormPresenter presenter;

        /// <summary>
        /// Creates a new ImprovedForm view.
        /// </summary>
        public ImprovedForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new ImprovedFormPresenter(this, session);
        }

        /// <summary>
        /// Handles the click event for send Button and updates the related state.
        /// </summary>
        private void sendButton_Click(object sender, EventArgs e)
        {
            presenter.Send();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Provides the text input value for the presenter.
        /// </summary>
        string IImprovedFormContract.TextInput
        {
            get { return textBox.Text; }
        }

        /// <summary>
        /// Runs the show removed license server view action for the presenter.
        /// </summary>
        void IImprovedFormContract.ShowRemovedLicenseServer()
        {
            MessageBox.ShowDialog(this, Messages.improved_removed_license_server);
        }

        /// <summary>
        /// Runs the show form view action for the presenter.
        /// </summary>
        void IImprovedFormContract.ShowForm(Enums.Forms form)
        {
            ShowFormEvent(form);
        }
    }
}
