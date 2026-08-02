using Pflegehaushaltsbuch.WPFControls;
using Pflegehaushaltsbuch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Deadline Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateDeadlineForm : Pflegehaushaltsbuch.FormControls.Form, ICreateDeadlineFormContract
    {
        private readonly CreateDeadlineFormPresenter presenter;


        UserTextBox noteBox = null;
        /// <summary>
        /// Creates a new Create Deadline Form instance and initializes the required state.
        /// </summary>
        public CreateDeadlineForm(DateTime dateTime, string description = "")
        {
            InitializeComponent();
            presenter = new CreateDeadlineFormPresenter(this);
            dateTimeBox.Date = dateTime;
            dateTimeBox.ShowYear = false;
            noteBox = new UserTextBox();
            noteHost.Child = noteBox;
            Description = description;
            dateTimeBox.Refresh();
        }
        public bool ForAllMonths
        {
            get { return allMonthBox.Checked; }
        }
        public string Description
        {
            get { return noteBox.Text.Trim(); }
            set { noteBox.Text = value; }
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
