using Microsoft.Win32;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the About Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AboutForm : Pflegehaushaltsbuch.FormControls.Form, IAboutFormContract
    {
        private readonly AboutFormPresenter presenter;


        /// <summary>
        /// Creates a new About Form instance and initializes the required state.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            presenter = new AboutFormPresenter(this);

            richTextBox.Text = string.Format(Messages.about_license, Application.ProductVersion);
        }
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms selectForm, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Administration, sql);
        }
    }
}
