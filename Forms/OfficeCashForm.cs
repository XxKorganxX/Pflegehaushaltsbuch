using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Office Cash Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class OfficeCashForm : Pflegehaushaltsbuch.FormControls.Form, IOfficeCashFormContract
    {
        private readonly OfficeCashFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private DataTable table;
        private DataView dataView;
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Creates a new Office Cash Form instance and initializes the required state.
        /// </summary>
        public OfficeCashForm()
        {
            InitializeComponent();
            presenter = new OfficeCashFormPresenter(this);
            view.AutoGenerateColumns = false;
            view.CellFormatting += CellFormatting;
        }
        /// <summary>
        /// Handles the format event for cash Form and updates the related state.
        /// </summary>
        private void CashForm_Format(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
        }
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == bookCat.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                if (index == (int)SQLBase.BookCategory.Einzahlung)
                    e.CellStyle.ForeColor = Color.Green;
                else if (index == (int)SQLBase.BookCategory.Auszahlung)
                    e.CellStyle.ForeColor = Color.Red;
                e.Value = ((SQLBase.BookCategory)index).GetDisplayName();
            }
        }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnUserRights(SQLBase sql)
        {
            base.OnUserRights(sql);
            if (sql.User.Supervisor)
            {
                updateButton.Visible = true;
                view.AllowUserToDeleteRows = true;
            }
            bookButton.Enabled = stornoButton.Enabled = sql.User.CanInsert | sql.User.CanModify;
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            var office_total_amount = await sql.GetViewAsync("office_total_amount");
            totalAmountBox.Text = decimal.Parse(office_total_amount.ToString()).ToString("C");
            table = new DataTable();
            if (!periodCheckBox.Checked)
            {
                DateTime date = fromDateBox.Date;
                await sql.FillAdapterAsync(SQLBase.SELECT.OfficeCashByDate, table, date.Month, date.Year);
            }
            else
            {
                var fromDate = fromDateBox.Date;
                var toDate = toDateBox.Date.AddMonths(1);
                await sql.FillAdapterAsync(SQLBase.SELECT.OfficeByPeriod, table, fromDate, toDate);
            }
            table.Columns.Add("document_id");
            DataRow[] rows = table.Select("", "date");
            int documentID = 1;
            foreach (DataRow row in rows)
            {
                row["document_id"] = documentID++;
            }
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;
            view.DataSource = table;
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the click event for book Button and updates the related state.
        /// </summary>
        private async void bookButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                using (CashOfficeBookDialog cashOfficeForm = new CashOfficeBookDialog(sql))
                {
                    if (cashOfficeForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    await sql.Book2CashOfficeAsync(
                        cashOfficeForm.BookingDate,
                        cashOfficeForm.BookText,
                        cashOfficeForm.Amount,
                        cashOfficeForm.BookingCategory,
                        cashOfficeForm.Account);

                    await ConnectTableToDataBase();
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the enter event for cash Office Form and updates the related state.
        /// </summary>
        private async void CashOfficeForm_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the selection Change Committed event for account Box and updates the related state.
        /// </summary>
        private void accountBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private async void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateBegin, dateEnd;
                dateBegin = dateEnd = fromDateBox.Date;
                if (periodCheckBox.Checked)
                    dateEnd = toDateBox.Date;
                dateEnd = dateEnd.AddMonths(1).AddHours(-1);
                if (dateEnd > DateTime.Now)
                    dateEnd = DateTime.Now;
                DataTable fullofficeCash = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.OfficeCash, fullofficeCash);
                decimal prevoiusAmount = 0;
                DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
                foreach (DataRow row in fullofficeCash.Rows.OfType<DataRow>()
                    .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
                    prevoiusAmount += decimal.Parse(row["amount"].ToString());
                List<DataRow> rows = new List<DataRow>();
                foreach (DataGridViewRow rowView in view.Rows)
                    rows.Add((rowView.DataBoundItem as DataRowView).Row);
                decimal einnahmen = 0;
                decimal ausgaben = 0;
                foreach (DataRow row in rows)
                {
                    int category = Int32.Parse(row["book_cat"].ToString());
                    decimal value = Math.Abs(decimal.Parse(row["amount"].ToString()));
                    if (category == 0)
                        einnahmen += value;
                    else
                        ausgaben += value;
                }
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());
                string ouputDate = "";
                if (dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
                    ouputDate = dateBegin.ToString("MMMM yyyy");
                else
                    ouputDate = dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, ouputDate);
                sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C"));
                sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C"));
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, prevoiusAmount.ToString("C"));
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount, (prevoiusAmount - ausgaben + einnahmen).ToString("C"));
                PrintBase cashPrinting = new PrintBase(sql, Data.Printing.LayoutEnum.officecash);
                cashPrinting.Print(Text, Text, this, rows);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Handles the value Changed event for date and updates the related state.
        /// </summary>
        private async void date_ValueChanged()
        {
            if (DesignMode)
                return;
            
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the click event for period Check Box and updates the related state.
        /// </summary>
        private void periodCheckBox_Click(object sender, EventArgs e)
        {
            toDateBox.Visible = fromToLabel.Visible = periodCheckBox.Checked;
            date_ValueChanged();
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = "Bürokasse";
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    Excel.ExportToExcel(table.DefaultView.ToTable(), fileDialog.FileName);
            }
        }
    }
}
