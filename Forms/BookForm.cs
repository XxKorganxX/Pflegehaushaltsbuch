using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.WPFControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pflegehaushaltsbuch.Databases.SQLBase;
using Exception = System.Exception;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class BookForm : Pflegehaushaltsbuch.FormControls.Form, IBookFormContract
    {
        private readonly BookFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        private DataTable table, clientTable = new DataTable();
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private int clientID;
        UserTextBox commentBox = null;
        /// <summary>
        /// Creates a new Book Form instance and initializes the required state.
        /// </summary>
        public BookForm()
        {
            InitializeComponent();
            presenter = new BookFormPresenter(this);
            commentBox = new UserTextBox();
            commentBox.Validated += CommentBox_Validated;
            elementHost.Child = commentBox;
            bookView.AutoGenerateColumns = false;
            DoubleBuffered = true;
            foreach (SQLBase.ClientActive enumval in Enum.GetValues(typeof(SQLBase.ClientActive)))
                accountStatusBox.Items.Add(enumval.GetDisplayName());
            accountBinding.DataSource = clientTable;
            Enter += bookPanel_Enter;
            Leave += bookPanel_Leave;
            
            bookView.CellFormatting += bookView_CellFormatting;
        }
        /// <summary>
        /// Handles the validated event for comment Box and updates the related state.
        /// </summary>
        private void CommentBox_Validated()
        {
            if (clientTable == null || clientTable.Rows.Count == 0)
                return;
            clientTable.Rows[0]["note"] = commentBox.Text;
            UpdateClientNote();
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
                bookView.AllowUserToDeleteRows = true;
            }
            bookButton.Enabled = sql.User.CanInsert | sql.User.CanModify;
            elementHost.Enabled = stornoButton.Enabled = sql.User.CanModify;
        }
        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnClientID_Changed(int clientID)
        {
            this.clientID = clientID;
        }
        void bookView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (e.ColumnIndex == bookCategoryColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                if (index == (int)SQLBase.BookCategory.Einzahlung)
                    e.CellStyle.ForeColor = Color.Green;
                else if (index == (int)SQLBase.BookCategory.Auszahlung)
                    e.CellStyle.ForeColor = Color.Red;
                e.Value = ((SQLBase.BookCategory)index).GetDisplayName();
            }
            else if (e.ColumnIndex == bookToColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                e.Value = ((SQLBase.BookingTo)index).GetDisplayName();
            }
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
                await sql.FillAdapterAsync(SQLBase.SELECT.Book, table, clientID.ToString(), date.Month, date.Year);
            }
            else
            {
                var fromDate = fromDateBox.Date;
                var toDate = toDateBox.Date.AddMonths(1);
                await sql.FillAdapterAsync(SQLBase.SELECT.BooksByPeriod, table, clientID.ToString(), fromDate, toDate);
            }
            UpdateDocumentNumbers(table);
            if (bookView.SortedColumn != null)
                table.DefaultView.Sort = bookView.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;
            bookView.DataSource = table;
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Clients, sql);
        }
        /// <summary>
        /// Gets the client Info value from the current application state.
        /// </summary>
        private async Task GetClientInfo()
        {
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, clientID.ToString());
        }
        /// <summary>
        /// Handles the enter event for book Panel and updates the related state.
        /// </summary>
        private async void bookPanel_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            clientNameBox.DataBindings.Clear();
            accountStatusBox.DataBindings.Clear();
            totalAmountBox.DataBindings.Clear();
            lastBookBox.DataBindings.Clear();
//noteBox.DataBindings.Clear();
            await ConnectTableToDataBase();
            await GetClientInfo();
            clientNameBox.DataBindings.Add("Text", clientTable, "name");
            totalAmountBox.DataBindings.Add("Text", clientTable, "amount", true, DataSourceUpdateMode.OnValidation, 0, "C");
            lastBookBox.DataBindings.Add("Text", clientTable, "lastbook", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
            //noteBox.DataBindings.Add("Text", noteBinding, "note");
            //commentBox.Bind(System.Windows.Controls.TextBox.TextProperty, clientTable, "note"); // noteBinding
            if(commentBox != null)
                commentBox.Text = clientTable.Rows[0]["note"].ToString();
            accountStatusBox.DataBindings.Add("SelectedIndex", accountBinding, "active");
        }
        /// <summary>
        /// Handles the leave event for book Panel and updates the related state.
        /// </summary>
        private void bookPanel_Leave(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
        }
        /// <summary>
        /// Updates the document Numbers data and refreshes the related application state.
        /// </summary>
        private void UpdateDocumentNumbers(DataTable datatable, DataRow ignore = null)
        {
            DateTime date = DateTime.MinValue;
            int belegNr = 1;
            DataRow[] rows = datatable.Select("", dateColumn.DataPropertyName);
            foreach (DataRow row in rows)
            {
                if (row == ignore)
                    continue;
                var currentDate = (DateTime)row[dateColumn.DataPropertyName];
                if (date.Month != currentDate.Month || date.Year != currentDate.Year)
                {
                    date = currentDate;
                    belegNr = 1;
                }
                row[numberColumn.DataPropertyName] = belegNr++;
            }
        }
        /// <summary>
        /// Handles the click event for storno Button and updates the related state.
        /// </summary>
        private async void stornoButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            bool transactionCommitted = false;
            try
            {
                if (bookView.SelectedRows.Count == 0)
                    throw new Exception(Messages.booking_no_booking_canceled);
                DataGridViewRow rowView = bookView.SelectedRows[0];
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                if (Int32.Parse(row[bookCategoryColumn.DataPropertyName].ToString()) == (int)SQLBase.BookCategory.Storno)
                    throw new Exception(Messages.booking_already_canceled);
                if (MessageBox.ShowDialog(this, Messages.booking_canceling, Messages.booking_cancel, MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
                    return;
                row[bookCategoryColumn.DataPropertyName] = (int)SQLBase.BookCategory.Storno;
                decimal amount = decimal.Parse(row[amountColumn.DataPropertyName].ToString());
                row[amountColumn.DataPropertyName] = 0;



                using (var transaction = sql.BeginTransaction())
                {
                    try
                    {
                        bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Book, table);
                        if (!valid)
                            throw new Exception(Messages.booking_canceled_failed);

                        bool stornoBooked = true;
                        SQLBase.BookingTo bookingTo = (SQLBase.BookingTo)Int32.Parse(row["book_to"].ToString());
                        if (bookingTo == SQLBase.BookingTo.Bankbestand)
                            stornoBooked = await sql.ToBankAsync(DateTime.Parse(row["date"].ToString()), row["note"].ToString(), -amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Storno, SQLBase.BookingTo.Bankbestand);
                        else if (bookingTo == SQLBase.BookingTo.Barbestand)
                            stornoBooked = await sql.ToBargeAsync(DateTime.Parse(row["date"].ToString()), row["note"].ToString(), -amount, string.Format("K{0:000}", clientID), SQLBase.BookCategory.Storno, SQLBase.BookingTo.Barbestand);

                        if (!stornoBooked)
                            throw new Exception(Messages.booking_canceled_failed);

                        transaction.Commit();
                        transactionCommitted = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                await ConnectTableToDataBase();
                await GetClientInfo();
                MessageBox.ShowDialog(this, Messages.booking_canceled_success);
            }
            catch
            {
                if (!transactionCommitted)
                    table.RejectChanges();
                throw;
            }
            finally
            { 
                transactionCommitted = true; 
            }
        }
        /// <summary>
        /// Handles the validated event for note Box and updates the related state.
        /// </summary>
        private void noteBox_Validated(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Updates the client Note data and refreshes the related application state.
        /// </summary>
        private async void UpdateClientNote()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                if (!valid)
                {
                    MessageBox.ShowDialog(this, Messages.book_entry_not_changed);
                }
            }
            finally
            { 
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the validated event for account Status Box and updates the related state.
        /// </summary>
        private async void accountStatusBox_Validated(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                accountBinding.EndEdit();
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable);
                if (!valid)
                {
                    clientTable.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.datatable_update_failed);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for print Account Button and updates the related state.
        /// </summary>
        private void printAccountButton_Click(object sender, EventArgs e)
        {
            DateTime from, to;
            from = to = fromDateBox.Date;
            if (periodCheckBox.Checked)
                to = toDateBox.Date;
            to = to.AddMonths(1).AddHours(-1);
            if (to > DateTime.Now)
                to = DateTime.Now;
            using (PrintBooksDialog pdfBookForm = new PrintBooksDialog(sql, table, clientID, totalAmountBox.Text, from, to))
            {
                pdfBookForm.ShowDialog(this);
            }
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
                using (ClientBookDialog bookingForm = new ClientBookDialog(sql, clientNameBox.Text, clientID.ToString()))
                {
                    if (bookingForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    var amount = bookingForm.Amount;
                    var bookText = bookingForm.BookText;
                    var payInDate = bookingForm.BookingDate;
                    var clientName = bookingForm.ClientName;
                    var clientId = bookingForm.ClientID;
                    var bookTo = bookingForm.BookingTarget;
                    var bookCategory = bookingForm.BookingCategory;
                    var printQuittance = bookingForm.PrintQuittance;
                    bool valid = false;

                    DataRow currentBook = null;
                    using (var transaction = sql.BeginTransaction())
                    {
                        try
                        {
                            if (bookTo == BookingTo.Barbestand)
                            {
                                if (bookCategory == BookCategory.Einzahlung)
                                {
                                    var result = await sql.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, amount, BookCategory.Einzahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                    {
                                        valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientId), BookCategory.Einzahlung, bookTo);
                                    }
                                }
                                else
                                {
                                    var result = await sql.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, -amount, BookCategory.Auszahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                    {
                                        valid = await sql.ToBargeAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientId), BookCategory.Auszahlung, bookTo);
                                    }
                                }
                            }
                            else if (bookTo == BookingTo.Bankbestand)
                            {
                                if (bookCategory == BookCategory.Einzahlung)
                                {
                                    var result = await sql.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, amount, BookCategory.Einzahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                    {
                                        valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, amount, string.Format("K{0:000}", clientId), BookCategory.Einzahlung, bookTo);
                                    }
                                }
                                else
                                {
                                    var result = await sql.ToBooksAsync(clientName, clientId, payInDate.Date.Date, bookText, -amount, BookCategory.Auszahlung, bookTo);
                                    currentBook = result.Item2;
                                    if (valid = result.Item1)
                                    {
                                        valid = await sql.ToBankAsync(payInDate.Date.Date, bookText, -amount, string.Format("K{0:000}", clientId), BookCategory.Auszahlung, bookTo);
                                    }
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
                    if (valid)
                    {
                        if (printQuittance)
                        {
                            Quittance quittance = new Quittance(sql);
                            quittance.Print(clientName, clientName, this, new DataRow[] { currentBook });
                        }
                        MessageBox.ShowDialog(this, Messages.booking_sucess);
                    }
                    else
                        throw new Exception(Messages.booking_failed);
                }
                await ConnectTableToDataBase();
                await GetClientInfo();
            }
            finally
            { 
                databaseOperationLock.Release();
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
                fileDialog.FileName = Messages.books_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                Excel.ExportToExcel(table.DefaultView.ToTable(), fileDialog.FileName);
                MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
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
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Book, table);
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
    }
}
