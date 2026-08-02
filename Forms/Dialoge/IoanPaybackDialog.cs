using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Ioan Payback window and coordinates its user interface behavior.
    /// </summary>
    public partial class IoanPaybackDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        /// <summary>
        /// Creates a new Ioan Payback instance and initializes the required state.
        /// </summary>
        public IoanPaybackDialog(SQLBase sql, string name, int id, decimal amount)
        {
            this.sql = sql;
            InitializeComponent();
            if (Program.DesignMode)
                return;
            foreach (SQLBase.Repayment enumval in Enum.GetValues(typeof(SQLBase.Repayment)))
                repaymentBox.Items.Add(enumval.GetDisplayName());
            this.id = id;
            MaximumAmount = amount;
            Amount = amount;
            nameBox.Text = name;
            amountBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnValidation, 0, "C");
        }
        public decimal Amount { get; set; }
        public decimal MaximumAmount { get; private set; }
        public int AssistantId
        {
            get { return id; }
        }
        public string AssistantName
        {
            get { return nameBox.Text; }
        }
        public DateTime PaybackDate
        {
            get { return date.Date.Date; }
        }
        public int RepaymentIndex
        {
            get { return repaymentBox.SelectedIndex; }
        }
        public SQLBase.Repayment Repayment
        {
            get { return (SQLBase.Repayment)RepaymentIndex; }
        }
        private int id;
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Amount <= 0)
                    throw new Exception(Messages.ioan_invalid_amount);
                if (Amount > MaximumAmount)
                    throw new Exception(Messages.ioan_invalid_amount);
                if (PaybackDate == DateTime.MinValue || PaybackDate > DateTime.Now)
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
