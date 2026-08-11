using System;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Month Calendar Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class MonthCalendarDialog : Form, IMonthCalendarDialogContract
    {
        private readonly MonthCalendarDialogPresenter presenter;

        public DateTime DateTime { get; set; }

        /// <summary>
        /// Creates a new MonthCalendarDialog view.
        /// </summary>
        public MonthCalendarDialog(DateTime date)
        {
            InitializeComponent();

            presenter = new MonthCalendarDialogPresenter(this);
            presenter.Initialize(date);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            ClientSize = monthCalendar.GetPreferredSize(System.Drawing.Size.Empty);
        }

        /// <summary>
        /// Handles the date Selected event for month Calendar and updates the related state.
        /// </summary>
        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            presenter.SelectDate(e.Start);
        }

        /// <summary>
        /// Provides the selected date value for the presenter.
        /// </summary>
        DateTime IMonthCalendarDialogContract.SelectedDate
        {
            get { return DateTime; }
            set { DateTime = value; }
        }

        /// <summary>
        /// Runs the set calendar date view action for the presenter.
        /// </summary>
        void IMonthCalendarDialogContract.SetCalendarDate(DateTime date)
        {
            monthCalendar.SetDate(date);
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IMonthCalendarDialogContract.CloseView()
        {
            Close();
        }
    }
}
