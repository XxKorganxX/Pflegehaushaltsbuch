using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.Tools;
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
    /// Represents the Cash Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashForm : Pflegehaushaltsbuch.FormControls.Form, ICashFormContract
    {
        private readonly CashFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private DataTable table, hardCashTable = new DataTable();
        BindingSource hard_cash_bs;
        /// <summary>
        /// Creates a new Cash Form instance and initializes the required state.
        /// </summary>
        public CashForm()
        {
            InitializeComponent();
            presenter = new CashFormPresenter(this);
            view.AutoGenerateColumns = false;
            //toDateBox.DataBindings.Add("Visible", periodCheckBox, "Checked", true, DataSourceUpdateMode.OnPropertyChanged);
            //fromToLabel.DataBindings.Add("Visible", periodCheckBox, "Checked", true, DataSourceUpdateMode.OnPropertyChanged);
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
 	        base.OnCreateControl();
            if (Program.DesignMode)
                return;
            view.CellFormatting += CellFormatting;
            Enter += CashForm_Enter;
            Leave += CashForm_Leave;
            hard_cash_bs = new BindingSource();
            DataTable table = new DataTable();
            table.Columns.Add("001"); table.Columns.Add("002"); table.Columns.Add("005");
            table.Columns.Add("010"); table.Columns.Add("020"); table.Columns.Add("050");
            table.Columns.Add("1"); table.Columns.Add("2"); table.Columns.Add("5");
            table.Columns.Add("10"); table.Columns.Add("20"); table.Columns.Add("50");
            table.Columns.Add("100"); table.Columns.Add("200"); table.Columns.Add("500");
            
            hard_cash_bs.DataSource = table;
            _1centBox.DataBindings.Add("Value", hard_cash_bs, "001", false, DataSourceUpdateMode.OnPropertyChanged);
            _2centBox.DataBindings.Add("Value", hard_cash_bs, "002", false, DataSourceUpdateMode.OnPropertyChanged);
            _5centBox.DataBindings.Add("Value", hard_cash_bs, "005", false, DataSourceUpdateMode.OnPropertyChanged);
            _10centBox.DataBindings.Add("Value", hard_cash_bs, "010", false, DataSourceUpdateMode.OnPropertyChanged);
            _20centBox.DataBindings.Add("Value", hard_cash_bs, "020", false, DataSourceUpdateMode.OnPropertyChanged);
            _50centBox.DataBindings.Add("Value", hard_cash_bs, "050", false, DataSourceUpdateMode.OnPropertyChanged);
            _1EuroBox.DataBindings.Add("Value", hard_cash_bs, "1", false, DataSourceUpdateMode.OnPropertyChanged);
            _2EuroBox.DataBindings.Add("Value", hard_cash_bs, "2", false, DataSourceUpdateMode.OnPropertyChanged);
            _5EuroBox.DataBindings.Add("Value", hard_cash_bs, "5", false, DataSourceUpdateMode.OnPropertyChanged);
            _10EuroBox.DataBindings.Add("Value", hard_cash_bs, "10", false, DataSourceUpdateMode.OnPropertyChanged);
            _20EuroBox.DataBindings.Add("Value", hard_cash_bs, "20", false, DataSourceUpdateMode.OnPropertyChanged);
            _50EuroBox.DataBindings.Add("Value", hard_cash_bs, "50", false, DataSourceUpdateMode.OnPropertyChanged);
            _100EuroBox.DataBindings.Add("Value", hard_cash_bs, "100", false, DataSourceUpdateMode.OnPropertyChanged);
            _200EuroBox.DataBindings.Add("Value", hard_cash_bs, "200", false, DataSourceUpdateMode.OnPropertyChanged);
            _500EuroBox.DataBindings.Add("Value", hard_cash_bs, "500", false, DataSourceUpdateMode.OnPropertyChanged);
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
        void hard_cash_binding_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateHardCashAmount();
        }
        /// <summary>
        /// Handles the value Changed event for hard cash and updates the related state.
        /// </summary>
        private void hard_cash_ValueChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            hard_cash_bs.EndEdit();
        }
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == categoryColumn.Index)
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
            if (DesignMode)
                return;
            await sql.FillAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
            hard_cash_bs.DataSource = hardCashTable;
            hard_cash_bs.ListChanged -= hard_cash_binding_ListChanged;
            hard_cash_bs.ListChanged += hard_cash_binding_ListChanged;
            UpdateHardCashAmount();
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
                await sql.FillAdapterAsync(SQLBase.SELECT.BargeFromMonth, table, date.Month, date.Year);
            }
            else
            {
                var fromDate = fromDateBox.Date;
                var toDate = toDateBox.Date.AddMonths(1);
                await sql.FillAdapterAsync(SQLBase.SELECT.BargeByPeriod, table, fromDate, toDate);
                //ToString("yyyy-MM"), toDate.ToString("yyyy-MM"));
                //DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;
            view.DataSource = table;
            var barge_total_amount = await sql.GetViewAsync("barge_total_amount");
            totalAmountBox.Text = decimal.Parse(barge_total_amount.ToString()).ToString("C");
        }
        /// <summary>
        /// Updates the hard Cash Amount data and refreshes the related application state.
        /// </summary>
        private void UpdateHardCashAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in hardCashTable.Rows)
            {
                totalAmount += Int32.Parse(row["001"].ToString())* 0.01m;
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
            hardCashAmountBox.Text = totalAmount.ToString("C");
            if (!totalAmountBox.Text.Equals(hardCashAmountBox.Text))
            {
                hardCashAmountBox.BackColor = Color.FromArgb(255,74,74);
                hardCashAmountBox.ForeColor = Color.White;
            }
            else
            {
                hardCashAmountBox.BackColor = Color.White;
                hardCashAmountBox.ForeColor = Color.Black;
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
        /// Handles the click event for save Button and updates the related state.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool value = await sql.UpdateAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
                if (value)
                {
                    await ConnectTableToDataBase();
                    MessageBox.ShowDialog(this, Messages.hardmoney_changed);
                }
                else
                    throw new Exception(Messages.hardmoney_changed_failed);
                
            }
            catch
            {
                table.RejectChanges();
                throw;
            }
            finally
            { 
                databaseOperationLock.Release(); 
            }
        }
        /// <summary>
        /// Handles the validated event for hardcash and updates the related state.
        /// </summary>
        private void hardcash_Validated(object sender, EventArgs e)
        {
            hardCashTable.Rows[0].EndEdit();
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
                    if(value < 0)
                        ausgaben += Math.Abs(value);
                    else
                        einnahmen += Math.Abs(value);
                }
                DataTable prevoisMonthTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Barge, prevoisMonthTable);
                decimal prevoiusAmount = 0;
                DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
                var books = prevoisMonthTable.Rows.OfType<DataRow>()
                    .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit);
                foreach (DataRow row in books)
                    prevoiusAmount += (decimal)(row["amount"]);
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
                PrintBase cashPrinting = new PrintBase(sql, Data.Printing.LayoutEnum.cash);
                cashPrinting.Print(Text, Text, this, rows);
            }
            catch
            {
                throw;
            }
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
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Barge, table);
                if (!valid)
                    throw new Exception(Messages.datatable_update_failed);
                else
                {
                    await ConnectTableToDataBase();
                    MessageBox.ShowDialog(this, Messages.datatable_updated);
                }
            }
            catch
            {
                table.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for undo Button and updates the related state.
        /// </summary>
        private void undoButton_Click(object sender, EventArgs e)
        {
            hardCashTable.RejectChanges();
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
                using (CashBookDialog bookingForm = new CashBookDialog(sql))
                {
                    if (bookingForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    var bookText = bookingForm.BookText;
                    var amount = bookingForm.Amount;
                    var clientActive = bookingForm.ClientActive;
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
                                foreach (ID_Client_Data clientData in clients)
                                {
                                    int clientID = clientData.ID;
                                    string clientName = clientData.Name;
                                    DataRow currentBook = null;
                                    if (bookCategory == BookCategory.Einzahlung)
                                    {
                                        var result = await sql.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, amount, BookCategory.Einzahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                        {
                                            valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientID), BookCategory.Einzahlung, bookTo);
                                        }
                                    }
                                    else
                                    {
                                        var result = await sql.ToBooksAsync(clientName, clientID, payInDate.Date.Date, bookText, -amount, BookCategory.Auszahlung, bookTo);
                                        currentBook = result.Item2;
                                        if (valid = result.Item1)
                                        {
                                            valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientID), BookCategory.Auszahlung, bookTo);
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
                                    MessageBox.ShowDialog(this, Messages.booking_sucess);
                                }
                            }
                            else
                            {
                                if (bookCategory == BookCategory.Einzahlung)
                                {
                                    if (valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, amount, BookingTo.Bankbestand.GetDisplayName(), BookCategory.Einzahlung, bookTo))
                                    {
                                        valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, -amount, BookingTo.Barbestand.GetDisplayName(), BookCategory.Auszahlung, bookTo);
                                    }
                                }
                                else
                                {
                                    if (valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, -amount, BookingTo.Bankbestand.GetDisplayName(), BookCategory.Auszahlung, bookTo))
                                    {
                                        valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, amount, BookingTo.Barbestand.GetDisplayName(), BookCategory.Einzahlung, bookTo);
                                    }
                                }
                                if (valid)
                                {
                                    MessageBox.ShowDialog(this, Messages.booking_sucess);
                                }
                                else
                                    throw new Exception(Messages.booking_failed);
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
        /// Handles the click event for period Check Box and updates the related state.
        /// </summary>
        private void periodCheckBox_Click(object sender, EventArgs e)
        {
            toDateBox.Visible = fromToLabel.Visible = periodCheckBox.Checked;
            date_ValueChanged();
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
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = Messages.cash_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;
                
                Excel.ExportToExcel(table.DefaultView.ToTable(), fileDialog.FileName);
                MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
            }
        }
        /// <summary>
        /// Handles the click event for automatic Button and updates the related state.
        /// </summary>
        private void automaticButton_Click(object sender, EventArgs e)
        {
            hard_cash_bs.SuspendBinding();
            var amount = Math.Abs(decimal.Parse(this.totalAmountBox.Text.Replace("€", "").Trim()));// * (decimal)100.0;
                                                                             //amount -= _200 * 200;
            int _100 = (int)(amount / 100.0m);
            amount -= _100 * 100;
            int _50 = (int)(amount / 50.0m);
            amount -= _50 * 50;
            int _20 = (int)(amount / 20.0m);
            amount -= _20 * 20;
            int _10 = (int)(amount / 10.0m);
            amount -= _10 * 10;
            int _5 = (int)(amount / 5.0m);
            amount -= _5 * 5;
            int _2 = (int)(amount / 2.0m);
            amount -= _2 * 2;
            int _1 = (int)(amount / 1.0m);
            amount -= _1 * 1;
            int _050 = (int)(amount / 0.5m);
            amount -= _050 * 0.5m;
            int _020 = (int)(amount / 0.2m);
            amount -= _020 * 0.2m;
            int _010 = (int)(amount / 0.1m);
            amount -= _010 * 0.1m;
            int _005 = (int)(amount / 0.05m);
            amount -= _005 * 0.05m;
            int _002 = (int)(amount / 0.02m);
            amount -= _002 * 0.02m;
            int _001 = (int)(amount / 0.01m);
            amount -= _001 * 0.01m;
            foreach (DataRow row in hardCashTable.Rows)
            {
                row["001"] = _001;
                row["002"] = _002;
                row["005"] = _005;
                row["010"] = _010;
                row["020"] = _020;
                row["050"] = _050;
                row["1"] = _1;
                row["2"] = _2;
                row["5"] = _5;
                row["10"] = _10;
                row["20"] = _20;
                row["50"] = _50;
                row["100"] = _100;
                row["200"] = 0;// _200;
                row["500"] = 0;// _500;
            }
            hard_cash_bs.ResumeBinding();
        }
    }
}
