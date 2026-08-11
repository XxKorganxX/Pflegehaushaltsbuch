using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Deadline Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateDeadlineForm : Form, ICreateDeadlineFormContract
    {
        private readonly CreateDeadlineFormPresenter presenter;

        /// <summary>
        /// Creates a new CreateDeadlineForm view.
        /// </summary>
        public CreateDeadlineForm(SqlSession session, DateTime dateTime, string description = "")
        {
            InitializeComponent();
            Session = session;
            presenter = new CreateDeadlineFormPresenter(this, session);
            presenter.Initialize(dateTime, description);
            dateTimeBox.Refresh();
        }

        /// <summary>
        /// Provides the for all months value.
        /// </summary>
        public bool ForAllMonths
        {
            get { return ((ICreateDeadlineFormContract)this).ForAllMonths; }
        }

        /// <summary>
        /// Provides the description value.
        /// </summary>
        public string Description
        {
            get { return ((ICreateDeadlineFormContract)this).Description; }
            set { ((ICreateDeadlineFormContract)this).Description = value; }
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Ok();
        }

        /// <summary>
        /// Provides the deadline date value for the presenter.
        /// </summary>
        DateTime ICreateDeadlineFormContract.DeadlineDate
        {
            get { return dateTimeBox.Date; }
            set { dateTimeBox.Date = value; }
        }

        /// <summary>
        /// Provides the show year value for the presenter.
        /// </summary>
        bool ICreateDeadlineFormContract.ShowYear
        {
            set { dateTimeBox.ShowYear = value; }
        }

        /// <summary>
        /// Provides the for all months value for the presenter.
        /// </summary>
        bool ICreateDeadlineFormContract.ForAllMonths
        {
            get { return allMonthBox.Checked; }
        }

        /// <summary>
        /// Provides the description value for the presenter.
        /// </summary>
        string ICreateDeadlineFormContract.Description
        {
            get { return noteBox.Text.Trim(); }
            set { noteBox.Text = value; }
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void ICreateDeadlineFormContract.AcceptDialog()
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Runs the ok view action for the presenter.
        /// </summary>
        void ICreateDeadlineFormContract.Ok()
        {
        }
    }
}
