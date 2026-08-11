using Pflegehaushaltsbuch.Databases;
using System;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the About Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AboutForm : Form, IAboutFormContract
    {
        private readonly AboutFormPresenter presenter;


        /// <summary>
        /// Creates a new AboutForm view.
        /// </summary>
        public AboutForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new AboutFormPresenter(this);

            richTextBox.Text = string.Format(Messages.about_license, Application.ProductVersion);
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowFormEvent(Enums.Forms.Main);
        }
    }
}
