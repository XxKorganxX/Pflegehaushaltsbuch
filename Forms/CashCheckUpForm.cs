using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Cash Check Up Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashCheckUpForm : Pflegehaushaltsbuch.FormControls.Form, ICashCheckUpFormContract
    {
        private readonly CashCheckUpFormPresenter presenter;


        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms selectForm, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Creates a new Cash Check Up Form instance and initializes the required state.
        /// </summary>
        public CashCheckUpForm()
        {
            InitializeComponent();
            presenter = new CashCheckUpFormPresenter(this);
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the enter event for cash Office Control Form and updates the related state.
        /// </summary>
        private async void CashOfficeControlForm_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            //DateTime date = dateTimeBox.Value;
            //Barkasse
            decimal barge_hardmoney_Amount = await GetHardCashAmount();
            hardCashAmountBox.Text = barge_hardmoney_Amount.ToString("C");
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, table, string.Empty);
            decimal clientsActive = 0,
                    clientsInActive = 0,
                    clientsHistory = 0;
            //decimal clientAccountTransfer = 0;
            //decimal assisstantsAccountTransfer = 0;
            foreach (DataRow row in table.Rows)
            {
                SQLBase.ClientActive clientActive = (SQLBase.ClientActive)Enum.Parse(typeof(SQLBase.ClientActive), row["active"].ToString(), true);
                if (clientActive == SQLBase.ClientActive.Active)
                    clientsActive += decimal.Parse(row["amount"].ToString());
                else if (clientActive == SQLBase.ClientActive.Inactive)
                    clientsInActive += decimal.Parse(row["amount"].ToString());
                else if (clientActive == SQLBase.ClientActive.History)
                    clientsHistory += decimal.Parse(row["amount"].ToString());
                //clientAccountTransfer += decimal.Parse(row["account_transfer"].ToString());
            }
            clientsActiveBox.Text = clientsActive.ToString("C");
            clientsInActiveBox.Text = clientsInActive.ToString("C");
            clientsHistoryBox.Text = clientsHistory.ToString("C");
            decimal clientTotal = clientsActive + clientsInActive + clientsHistory;
            clientsBox.Text = clientTotal.ToString("C");
            //accountTransferBox.Text = clientAccountTransfer.ToString("c");
            table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Assistants, table);
            decimal assistantsAmount = 0;
            foreach (DataRow row in table.Rows)
            {
                assistantsAmount += decimal.Parse(row["amount_payout"].ToString());
                //assisstantsAccountTransfer += decimal.Parse(row["account_transfer"].ToString());
            }
            amountAssistantsBox.Text = assistantsAmount.ToString("C");
            var bank_total_amount = await sql.GetViewAsync("bank_total_amount");
            decimal bankAmount = decimal.Parse(bank_total_amount.ToString());
            bankSaldoBox.Text = bankAmount.ToString("C");
            decimal calculateSaldo = clientTotal - assistantsAmount - bankAmount;// -clientAccountTransfer;// +assisstantsAccountTransfer;
            calculatedSaldoBox.Text = calculateSaldo.ToString("C");
            differenceAmountBox.Text = (barge_hardmoney_Amount - calculateSaldo).ToString("C");
            var barge_total_amount = await sql.GetViewAsync("barge_total_amount");
            decimal bargeAmount = decimal.Parse(barge_total_amount.ToString());
            cashHoldingBox.Text = bargeAmount.ToString("C");
            //UpdateCashHolding();
        }
        /*
        /// <summary>
        /// Updates the cash Holding data and refreshes the related application state.
        /// </summary>
        private void UpdateCashHolding()
        {
            decimal totalAmount = 0;
            DataTable table = new DataTable();
            sql.Adapter(SQL.SELECT.Barge, table);
            foreach (DataRow row in table.Rows)
            {
                decimal amount = 0;
                if (decimal.TryParse(row["amount"].ToString(), out amount))
                    totalAmount += amount;
            }
            cashHoldingBox.Text = totalAmount.ToString("C");
        }
        */
        /// <summary>
        /// Gets the hard Cash Amount value from the current application state.
        /// </summary>
        private async Task<decimal> GetHardCashAmount()
        {
            decimal totalAmount = 0;
            DataTable hardCashTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
            foreach (DataRow row in hardCashTable.Rows)
            {
                totalAmount += Int32.Parse(row["001"].ToString()) * 0.01m;
                totalAmount += Int32.Parse(row["002"].ToString()) * 0.02m;
                totalAmount += Int32.Parse(row["005"].ToString()) * 0.05m;
                totalAmount += Int32.Parse(row["010"].ToString()) * 0.1m;
                totalAmount += Int32.Parse(row["020"].ToString()) * 0.2m;
                totalAmount += Int32.Parse(row["050"].ToString()) * 0.5m;
                totalAmount += Int32.Parse(row["1"].ToString()) * 1.0m;
                totalAmount += Int32.Parse(row["2"].ToString()) * 2.0m;
                totalAmount += Int32.Parse(row["5"].ToString()) * 5.0m;
                totalAmount += Int32.Parse(row["10"].ToString()) * 10.0m;
                totalAmount += Int32.Parse(row["20"].ToString()) * 20.0m;
                totalAmount += Int32.Parse(row["50"].ToString()) * 50.0m;
                totalAmount += Int32.Parse(row["100"].ToString()) * 100.0m;
                totalAmount += Int32.Parse(row["200"].ToString()) * 200.0m;
                totalAmount += Int32.Parse(row["500"].ToString()) * 500.0m;
            }
            return totalAmount;
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_active, clientsActiveBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_inactive, clientsInActiveBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_history, clientsHistoryBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients, clientsBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_assistants, amountAssistantsBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_bank, bankSaldoBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_hardmoney_calculated, calculatedSaldoBox.Text);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_hardmoney_actually, hardCashAmountBox.Text);
                PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.cashaudit);
                printer.Print(Text, Text, this);
            }
            catch
            {
                throw;
            }
        }
    }
}
