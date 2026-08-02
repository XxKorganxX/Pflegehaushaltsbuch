using System;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Improved Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ImprovedForm : Pflegehaushaltsbuch.FormControls.Form, IImprovedFormContract
    {
        private readonly ImprovedFormPresenter presenter;


        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms selectForm, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnUserRights(int access, bool admin, bool supervisor)
        {
        }
        /// <summary>
        /// Creates a new Improved Form instance and initializes the required state.
        /// </summary>
        public ImprovedForm()
        {
            InitializeComponent();
            presenter = new ImprovedFormPresenter(this);
        }
        /// <summary>
        /// Handles the click event for send Button and updates the related state.
        /// </summary>
        private void sendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                throw new Exception(Messages.improved_missing_text);
            MessageBox.ShowDialog(this, Messages.improved_removed_license_server);
            ShowForm(Enums.Forms.Administration, sql);
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Administration, sql);
        }
    }
}
