using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Reset Layouts window and coordinates its user interface behavior.
    /// </summary>
    public partial class ResetLayoutsDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        /// <summary>
        /// Creates a new Reset Layouts instance and initializes the required state.
        /// </summary>
        public ResetLayoutsDialog(SQLBase sql)
        {
            InitializeComponent();
            this.sql = sql;
        }
        /// <summary>
        /// Handles the checked Changed event for all Box and updates the related state.
        /// </summary>
        private void allBox_CheckedChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (allBox.Checked)
            {
                sql.Printing.ResetDocuments();
                return;
            }
            if (clientsBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.clients);
            if (advisorsBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.advisors);
            if (assistantsBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.assistants);
            if (cashBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.cash);
            if (bankBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.bank);
            if (billBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.accounts);
            if (cashCheckBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.cashaudit);
            if (quittanceBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.quittance);
            if (officeCashBox.Checked)
                sql.Printing.ResetDocument(Data.Printing.LayoutEnum.officecash);
        }
        /// <summary>
        /// Handles the click event for cash Box and updates the related state.
        /// </summary>
        private void cashBox_Click(object sender, EventArgs e)
        {
            if (clientsBox.Checked &&
                advisorsBox.Checked &&
                assistantsBox.Checked &&
                cashBox.Checked &&
                bankBox.Checked &&
                billBox.Checked &&
                cashCheckBox.Checked &&
                quittanceBox.Checked &&
                officeCashBox.Checked
                )
                allBox.Checked = true;
            else
                allBox.Checked = false;
        }
        /// <summary>
        /// Handles the click event for all Box and updates the related state.
        /// </summary>
        private void allBox_Click(object sender, EventArgs e)
        {
            clientsBox.Checked = allBox.Checked;
            advisorsBox.Checked = allBox.Checked;
            assistantsBox.Checked = allBox.Checked;
            cashBox.Checked = allBox.Checked;
            bankBox.Checked = allBox.Checked;
            billBox.Checked = allBox.Checked;
            cashCheckBox.Checked = allBox.Checked;
            quittanceBox.Checked = allBox.Checked;
            officeCashBox.Checked = allBox.Checked;
        }
    }
}
