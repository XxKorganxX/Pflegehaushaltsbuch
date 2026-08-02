using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.WPFControls;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Cash Office Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashOfficeBookDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        public decimal Amount { get; set; }
        public int Account { get; set; }
        public string BookText { get; set; }
        public DateTime BookingDate
        {
            get { return dateBox.Date.Date; }
        }

        public SQLBase.BookCategory BookingCategory
        {
            get { return (SQLBase.BookCategory)bookingKindBox.SelectedIndex; }
        }
        /// <summary>
        /// Creates a new Cash Office Book Form instance and initializes the required state.
        /// </summary>
        public CashOfficeBookDialog(SQLBase sql)
        {
            InitializeComponent();
            Amount = 0;
            Account = 0;
            this.sql = sql;
            var bookTextBox = new UserTextBox();
            bookTextBox.Bind(System.Windows.Controls.TextBox.TextProperty, this, "BookText");
            bookTextHost.Child = bookTextBox;
            bookingKindBox.SelectedIndex = 0;
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0, "C");
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Amount == 0)
                    throw new Exception(Messages.missing_amount);
                if (string.IsNullOrWhiteSpace(BookText))
                    throw new Exception(Messages.missing_bookingtext);
                if (dateBox.Date == DateTime.MinValue || dateBox.Date > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
            }
            catch
            { 
                DialogResult = DialogResult.None;
                throw;
            }
        }
    }
}
