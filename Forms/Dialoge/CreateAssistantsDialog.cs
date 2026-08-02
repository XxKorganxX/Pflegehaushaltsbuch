using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Assistants Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateAssistantsDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        public int ID { get; set; }
        public string AssistantName { get; set; }
        public DateTime Date {get; set;}
        public decimal Amount { get; set; }
        /// <summary>
        /// Creates a new Create Assistants Form instance and initializes the required state.
        /// </summary>
        public CreateAssistantsDialog(int id)
        {
            InitializeComponent();
            bookAccountBox.Items.Add(SQLBase.BookingTo.Barbestand.GetDisplayName());
            bookAccountBox.SelectedIndex = 0;
            Date = DateTime.Now;
            ID = id;
            idBox.DataBindings.Add("Text", this, "ID");
            nameBox.DataBindings.Add("Text", this, "AssistantName");
            dateBox.DataBindings.Add("Date", this, "Date");
            amountBox.DataBindings.Add("Value", this, "Amount");
            //bookAccountBox.DataBindings.Add("SelectedIndex", this, "bookAccount");
        }
        /// <summary>
        /// Creates a new Create Assistants Form instance and initializes the required state.
        /// </summary>
        public CreateAssistantsDialog(int id, string name, DateTime date, decimal amount)
        {
            InitializeComponent();
            bookAccountBox.Items.Add(SQLBase.BookingTo.Barbestand.GetDisplayName());
            bookAccountBox.SelectedIndex = 0;
            ID = id;
            AssistantName = name;
            Date = date;
            Amount = amount;
            idBox.DataBindings.Add("Text", this, "ID");
            nameBox.DataBindings.Add("Text", this, "AssistantName");
            dateBox.DataBindings.Add("Date", this, "Date");
            amountBox.DataBindings.Add("Value", this, "Amount");
            //bookAccountBox.DataBindings.Add("SelectedIndex", this, "bookAccount");
            if (amount != 0)
                amountBox.Enabled = false;
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AssistantName))
                    throw new Exception(Messages.assistants_name_missing);
                if (Date > DateTime.Now)
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
