using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Pflegehaushaltsbuch.FormControls;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Month Calendar Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class MonthCalendarDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        public DateTime DateTime { get; set; }
        /// <summary>
        /// Creates a new Month Calendar Form instance and initializes the required state.
        /// </summary>
        public MonthCalendarDialog(DateTime date)
        {
            InitializeComponent();
            monthCalendar.SetDate(DateTime = date);
        }
        /// <summary>
        /// Handles the date Selected event for month Calendar and updates the related state.
        /// </summary>
        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime = e.Start;
            Close();
        }
    }
}
