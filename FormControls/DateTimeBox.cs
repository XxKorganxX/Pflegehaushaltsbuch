using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Dialoge;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom date time Box control used by the application user interface.
    /// </summary>
    public partial class DateTimeBox : UserControl, INotifyPropertyChanged
    {
        private DateTime dateTime = DateTime.Now;
        /// <summary>
        /// Updates the distance Delegate data and refreshes the related application state.
        /// </summary>
        public delegate void UpdateDistanceDelegate();
        [Category("Aktion")]
        public event UpdateDistanceDelegate ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool days = true;
        public bool Days 
        { 
            get
            { 
                return days; 
            } 
            set 
            { 
                if(days == value)
                    return;
                days = value; 
                dayBox.Visible = value;
                if(value)
                    tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
                else
                    tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.AutoSize;
            } 
        }
        public bool ShowYear
        {
            get { return yearBox.Visible; }
            set { yearBox.Visible = value; }
        }
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Date time Box instance and initializes the required state.
        /// </summary>
        public DateTimeBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            for (int i = 1900; i <= 2100; i++)
                yearBox.Items.Insert(0, i);
            //dateTime = DateTime.Now;
            UpdateDays();
            yearBox.SelectedItem = dateTime.Year;
            monthBox.SelectedIndex = dateTime.Month-1;
            dayBox.SelectedItem = dateTime.Day;           
        }
        //[Browsable(false)]
        //[Bindable(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get 
            { 
                if(!days)
                    return new DateTime( dateTime.Year, dateTime.Month, 1); 
                else
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            }
            set 
            {
                if (Program.DesignMode)
                    return;
                if (dateTime == value)
                    return;
                dateTime = value;
                int currentDay = UpdateDays();
                yearBox.SelectedItem = value.Year;
                monthBox.SelectedIndex = value.Month-1;
                //dayBox.SelectedItem = currentDay;
                dayBox.SelectedIndex = currentDay - 1;
                dayBox.Invalidate();
                monthBox.Invalidate();
                yearBox.Invalidate();
                if (ValueChanged != null)
                    ValueChanged();
                NotifyPropertyChanged("Date");
            }
        }
        /// <summary>
        /// Updates the days data and refreshes the related application state.
        /// </summary>
        private int UpdateDays()
        {
            dayBox.Items.Clear();
            int maxDays = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            for (int i = 0; i < maxDays; i++)
            {
                dayBox.Items.Add((i + 1));
            }
            return Math.Min(maxDays, dateTime.Day);
        }
        /// <summary>
        /// Updates the date data and refreshes the related application state.
        /// </summary>
        private void UpdateDate()
        {
            int year, month = monthBox.SelectedIndex + 1;
            if (!Int32.TryParse(yearBox.SelectedItem.ToString(), out year))
                return;
            int days = DateTime.DaysInMonth(year, month);
            int day = Math.Min(days, dayBox.SelectedIndex + 1);
            Date = new DateTime(year, month, day);
        }
        /// <summary>
        /// Runs the notify Property Changed operation and updates the related application state.
        /// </summary>
        protected void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// Handles the selection Change Committed event for update and updates the related state.
        /// </summary>
        private void update_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            UpdateDate();
        }
        /// <summary>
        /// Handles the click event for calendar Button and updates the related state.
        /// </summary>
        private void calendarButton_Click(object sender, EventArgs e)
        {
            MonthCalendarDialog monthCaleandar = new MonthCalendarDialog(Date);
            monthCaleandar.Font = calendarButton.Font;
            Rectangle rect = calendarButton.RectangleToScreen(calendarButton.ClientRectangle);
            monthCaleandar.Location = new Point(rect.Right - monthCaleandar.ClientRectangle.Width, rect.Bottom);
            monthCaleandar.ShowDialog(this);
            Date = monthCaleandar.DateTime.Date;
        }
        //{
        //    MonthCalendarForm monthCaleandar = new MonthCalendarForm(Date);
        //    monthCaleandar.Font = calendarButton.Font;
        //    Rectangle rect = calendarButton.RectangleToScreen(calendarButton.ClientRectangle);
        //    monthCaleandar.Location = new Point(rect.Right - monthCaleandar.ClientRectangle.Width, rect.Bottom);
        //    monthCaleandar.ShowDialog(this);
        //    Date = monthCaleandar.DateTime.Date;
        //}
    }
}
