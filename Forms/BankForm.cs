using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pflegehaushaltsbuch.Databases.SQLBase;
using Exception = System.Exception;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Bank Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class BankForm : Pflegehaushaltsbuch.FormControls.Form, IBankFormContract
    {
        private readonly BankFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private DataTable table;
        /// <summary>
        /// Creates a new Bank Form instance and initializes the required state.
        /// </summary>
        public BankForm()
        {
            InitializeComponent();
            presenter = new BankFormPresenter(this);
            view.AutoGenerateColumns = false;
            Enter += CashForm_Enter;
            Leave += CashForm_Leave;
            view.CellFormatting += CellFormatting;
        }
        /// <summary>
        /// Handles the format event for cash Form and updates the related state.
        /// </summary>
        private void CashForm_Format(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
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
            bookButton.Enabled = sql.User.CanInsert | sql.User.CanModify;
        }
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == bookCategoryColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                if (index == (int)SQLBase.BookCategory.Einzahlung)
                    e.CellStyle.ForeColor = Color.Green;
                else if (index == (int)SQLBase.BookCategory.Auszahlung)
                    e.CellStyle.ForeColor = Color.Red;
                e.Value = ((SQLBase.BookCategory)index).GetDisplayName();
            }
        }
        async void CashForm_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
        }
        void CashForm_Leave(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            table = new DataTable();
            if (!periodCheckBox.Checked)
            {
                DateTime date = fromDateBox.Date;
                await sql.FillAdapterAsync(SQLBase.SELECT.BankByDate, table, date.Month, date.Year);
            }
            else
            {
                var fromDate = fromDateBox.Date;
                var toDate = toDateBox.Date.AddMonths(1);
                await sql.FillAdapterAsync(SQLBase.SELECT.BankByPeriod, table, fromDate, toDate);
            }
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;
            view.DataSource = table;
            var bank_total_amount = await sql.GetViewAsync("bank_total_amount");
            totalAmountBox.Text = decimal.Parse(bank_total_amount.ToString()).ToString("C");
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

                using (BankBookDialog bookingForm = new BankBookDialog(sql))
                {
                    if (bookingForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    var amount = bookingForm.Amount;
                    var bookText = bookingForm.BookText;
                    var payInDate = bookingForm.BookingDate;
                    var bookTo = bookingForm.BookingTarget;
                    var bookCategory = bookingForm.BookingCategory;
                    var printQuittance = bookingForm.PrintQuittance;
                    var clients = bookingForm.SelectedClients;

                    bool valid = false;
                    using (var transaction = sql.BeginTransaction())
                    {
                        try
                        {
                            if (bookTo == BookingTo.Barbestand)
                            {
                                if (bookCategory == BookCategory.Einzahlung)
                                {
                                    if (valid = await sql.ToBargeAsync(payInDate, bookText, -amount, BookingTo.Bankbestand.GetDisplayName(), BookCategory.Auszahlung, bookTo))
                                    {
                                        valid = await sql.ToBankAsync(payInDate, bookText, amount, BookingTo.Barbestand.GetDisplayName(), BookCategory.Einzahlung, bookTo);
                                    }
                                }
                                else
                                {
                                    if (valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, amount, BookingTo.Bankbestand.GetDisplayName(), BookCategory.Einzahlung, bookTo))
                                    {
                                        valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, -amount, BookingTo.Barbestand.GetDisplayName(), BookCategory.Auszahlung, bookTo);
                                    }
                                }
                                if (valid)
                                {
                                    MessageBox.ShowDialog(this, Messages.booking_sucess);
                                }
                                else
                                    throw new Exception(Messages.booking_failed);
                            }
                            else
                            {
                                foreach (ID_Client_Data clientData in clients)
                                {
                                    int clientID = clientData.ID;
                                    string clientName = clientData.Name;
                                    DataRow currentBook = null;
                                    if (bookCategory == BookCategory.Einzahlung)
                                    {
                                        //out currentBook
                                        var result = await sql.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, amount, BookCategory.Einzahlung, bookTo);
                                        currentBook = result.Item2;

                                        if (valid = result.Item1)
                                        {
                                            valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientID), BookCategory.Einzahlung, bookTo);
                                        }
                                    }
                                    else
                                    {
                                        var result = await sql.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, -amount, BookCategory.Auszahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                        {
                                            valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientID), BookCategory.Auszahlung, bookTo);
                                        }
                                    }
                                    if (!valid)
                                        throw new Exception(string.Format(Messages.booking_for_client_failed, clientName));
                                    else
                                    {
                                        if (printQuittance)
                                        {
                                            List<DataRow> currentBooks = new List<DataRow>();
                                            currentBooks.Add(currentBook);
                                            Quittance quittance = new Quittance(sql);
                                            quittance.Print(clientName, clientName, this, currentBooks);
                                        }
                                    }
                                }
                                if (valid)
                                {
                                    MessageBox.ShowDialog(this, Messages.bookings_success);
                                }
                            }
                            if (!valid)
                                throw new Exception(Messages.booking_failed);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                await ConnectTableToDataBase();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Bank, table);
                if (!valid)
                {
                    table.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.datatable_update_failed);
                }
                else
                {
                    await ConnectTableToDataBase();
                    MessageBox.ShowDialog(this, Messages.datatable_updated);
                }
            }
            finally
            { 
                databaseOperationLock.Release(); 
            }
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
                DataRow[] rows = table.Select("", "date");
                decimal einnahmen = 0;
                decimal ausgaben = 0;
                foreach (DataRow row in rows)
                {
                    decimal value = (decimal)(row["amount"]);
                    if (value < 0)
                        ausgaben += Math.Abs(value);
                    else
                        einnahmen += Math.Abs(value);
                }
                DataTable prevoisMonthTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Bank, prevoisMonthTable);
                decimal prevoiusAmount = 0;
                DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
                foreach (DataRow row in prevoisMonthTable.Rows.OfType<DataRow>()
                    .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
                    prevoiusAmount += decimal.Parse(row["amount"].ToString());
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
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount, (prevoiusAmount-ausgaben+einnahmen).ToString("C"));
                PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.bank);
                printer.Print(Text, Text, this, rows);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for all Books Check Box and updates the related state.
        /// </summary>
        private async void allBooksCheckBox_Click(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
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
                fileDialog.FileName = Messages.bank_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;
                
                Excel.ExportToExcel(table.DefaultView.ToTable(), fileDialog.FileName);
                MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
            }
        }
    }
}
