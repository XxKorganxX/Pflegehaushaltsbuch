using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
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
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Exception = System.Exception;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Assistants Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AssistantsForm : Pflegehaushaltsbuch.FormControls.Form, IAssistantsFormContract
    {
        private readonly AssistantsFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private DataTable table;
        /// <summary>
        /// Creates a new Assistants Form instance and initializes the required state.
        /// </summary>
        public AssistantsForm()
        {
            InitializeComponent();
            presenter = new AssistantsFormPresenter(this);
            view.AutoGenerateColumns = false;
            view.CellPainting += CellPainting;
            view.CellFormatting += CellFormatting;
            this.Enter += AssistantsForm_Enter;
            this.Leave += AssistantsForm_Leave;
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
            createButton.Enabled = sql.User.CanInsert;
            changeButton.Enabled = payOutButton.Enabled = sql.User.CanModify;
            deleteButton.Visible = sql.User.CanDelete;
        }
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dateColumn.Index && e.Value != null && e.Value != DBNull.Value)
            {
                DateTime date;
                if (DateTime.TryParse(e.Value.ToString(), out date))
                {
                    e.Value = date.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == paybackTypeColumn.Index && e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = ((SQLBase.Repayment)Int32.Parse(e.Value.ToString())).GetDisplayName();
                e.FormattingApplied = true;
            }
        }
        void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == activeColumn.Index && e.RowIndex >= 0)
            {
                SolidBrush backColor = new SolidBrush(view.BackgroundColor);
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
                bool isActive = false;
                if (e.Value != null && e.Value != DBNull.Value && !Boolean.TryParse(e.Value.ToString(), out isActive))
                {
                    int value = 0;
                    if (Int32.TryParse(e.Value.ToString(), out value))
                    {
                        isActive = value != 0;
                    }
                }
                Rectangle rect = e.CellBounds;
                rect.Height -= 10;
                rect.X += 5;
                rect.Y += 5;
                rect.Width = rect.Height;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = rect;
                using (var ellipsePath = new GraphicsPath())
                {
                    ellipsePath.AddEllipse(bounds);
                    using (var brush = new PathGradientBrush(ellipsePath))
                    {
                        brush.CenterPoint = new PointF(bounds.X + bounds.Width / 3f, bounds.Y + bounds.Height / 3f);
                        brush.CenterColor = Color.White;
                        if (isActive)//!= 0)
                            brush.SurroundColors = new[] { Color.Green };
                        else
                            brush.SurroundColors = new[] { Color.Red };
                        e.Graphics.FillEllipse(brush, rect);
                    }
                }
                e.Graphics.DrawEllipse(new Pen(Brushes.Black), rect);
                e.Graphics.SmoothingMode = SmoothingMode.Default;
                e.Handled = true;
            }
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            nameBox.DataSource = null;
            table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Assistants, table);
            UpdateTotalAmount();
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = nameColumn.DataPropertyName;
            view.DataSource = table;
            nameBox.DataSource = table;
            nameBox.DisplayMember = "name";
            dateBox.DataBindings.Clear();
            dateBox.DataBindings.Add("Text", table, "date", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
        }
        /// <summary>
        /// Updates the total Amount data and refreshes the related application state.
        /// </summary>
        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in table.Rows)
            {
                decimal amount = 0;
                if (decimal.TryParse(row[amountPayoutColumn.DataPropertyName].ToString(), out amount))
                    totalAmount += amount;
            }
            totalAmountBox.Text = totalAmount.ToString("C");
        }
        async void AssistantsForm_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
        }
        void AssistantsForm_Leave(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                using (CreateAssistantsDialog form = new CreateAssistantsDialog(sql.GetID(table)))
                {
                    if (form.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                        return;
                    try
                    {
                        DataRow row = table.NewRow();
                        row[SQLBase.Names(SQLBase.ColumnNames.id)] = form.ID;
                        row[nameColumn.DataPropertyName] = form.AssistantName;
                        row[amountPayoutColumn.DataPropertyName] = form.Amount;
                        row["account_transfer"] = 0;
                        row[amountPayBackColumn.DataPropertyName] = 0;
                        row[paybackTypeColumn.DataPropertyName] = 0;
                        row[dateColumn.DataPropertyName] = form.Date.Date;
                        row[activeColumn.DataPropertyName] = true;
                        row[handSignColumn.DataPropertyName] = sql.User.Name;
                        table.Rows.Add(row);
                        bool valid = false;
                        using (var transaction = sql.BeginTransaction())
                        {
                            try
                            {
                                valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistants, table);
                                if (valid && form.Amount != 0)
                                    valid = await sql.ToBargeAsync(form.Date, string.Format(Messages.ioan_to, form.AssistantName), -Math.Abs(form.Amount), string.Format("M{0:000}", form.ID), SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand);
                                if (!valid)
                                    throw new Exception(Messages.assistants_created_failed);
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
                            await ConnectTableToDataBase();
                            MessageBox.ShowDialog(this, Messages.assistants_created);
                        }
                        else
                        {
                            table?.RejectChanges();
                            MessageBox.ShowDialog(this, Messages.assistants_created_failed);
                        }
                    }
                    catch
                    {
                        table?.RejectChanges();
                        throw;
                    }
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private void changeButton_Click(object sender, EventArgs e)
        {
            ChangeAssistant();
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {

                if (view.SelectedRows.Count <= 0)
                    return;

                DataGridViewRow rowView = view.SelectedRows[0];
                DataRow rowOfView = (rowView.DataBoundItem as DataRowView).Row;
                int id = Int32.Parse(rowOfView[idColumn.DataPropertyName].ToString());
                if ((decimal)rowOfView[amountPayoutColumn.DataPropertyName] != 0)
                {
                    MessageBox.ShowDialog(this, string.Format(Messages.asistants_not_deleteable, nameBox.Text));
                    return;
                }
                if (MessageBox.ShowDialog(this, string.Format(Messages.assistants_delete, nameBox.Text), base.Text, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
                DataTable bankTable = new DataTable(), bargeTable = new DataTable(), assistantsTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bankTable);
                await sql.FillAdapterAsync(SQLBase.SELECT.Barge, bargeTable);
                string idNumber = string.Format("M{0:000}", id);

                foreach (DataRow row in bargeTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row["account"] == DBNull.Value || string.IsNullOrWhiteSpace(row["account"].ToString()))
                        throw new Exception(Messages.assistants_not_deleteable_book);
                    if (row["account"].ToString().Equals(idNumber))
                        row.Delete();
                }
                foreach (DataRow row in bankTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row["account"] == DBNull.Value || string.IsNullOrWhiteSpace(row["account"].ToString()))
                        throw new Exception(Messages.assistants_not_deleteable_book);
                    if (row["account"].ToString().Equals(idNumber))
                        row.Delete();
                }

                using (var transaction = sql.BeginTransaction())
                {
                    try
                    {
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Barge, bargeTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Bank, bankTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        await sql.FillAdapterAsync(SQLBase.SELECT.Assistant, assistantsTable, id);
                        if (assistantsTable.Rows.Count == 0)
                            throw new Exception(Messages.assistant_not_found);
                        assistantsTable.Rows[0].Delete();
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistant, assistantsTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                if (rowOfView.Table == table)
                {
                    table.Rows.Remove(rowOfView);
                    UpdateTotalAmount();
                }
                else
                {
                    await ConnectTableToDataBase();
                }

            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for pay Out Button and updates the related state.
        /// </summary>
        private async void payOutButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (view.SelectedRows.Count == 0)
                    return;
                DataGridViewRow rowView = view.SelectedRows[0];
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                if (decimal.Parse(row[amountPayoutColumn.DataPropertyName].ToString()) == 0)
                    throw new Exception(Messages.ioan_repaid_needless);
                using (IoanPaybackDialog form = new IoanPaybackDialog(sql, row[nameColumn.DataPropertyName].ToString(), Int32.Parse(row[idColumn.DataPropertyName].ToString()),
                    decimal.Parse(row[amountPayoutColumn.DataPropertyName].ToString())))
                {
                    if (form.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                        return;

                    using (var transaction = sql.BeginTransaction())
                    {
                        try
                        {
                            bool valid = await sql.UpdateAsistanceAsync(form.AssistantName, form.PaybackDate, form.Amount, form.RepaymentIndex);
                            if (!valid)
                                throw new Exception(Messages.ioan_repaid_failed);
                            switch (form.Repayment)
                            {
                                case SQLBase.Repayment.Payout:
                                    valid = await sql.ToBargeAsync(form.PaybackDate,
                                        string.Format(Messages.ioan_repaid_by, form.AssistantName), form.Amount, string.Format("M{0:000}", form.AssistantId), SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand);
                                    break;
                                case SQLBase.Repayment.Transfered:
                                case SQLBase.Repayment.Direct_Debit:
                                    valid = await sql.ToBankAsync(form.PaybackDate, string.Format(Messages.ioan_repaid_by, form.AssistantName), form.Amount, string.Format("M{0:000}", form.AssistantId), SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);
                                    break;
                            }

                            if (!valid)
                                throw new Exception(Messages.ioan_repaid_failed);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    MessageBox.ShowDialog(this, Messages.ioan_repaid);
                    await ConnectTableToDataBase();
                }
            }
            catch
            {
                table?.RejectChanges();
                throw;
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
            ShowForm?.Invoke(Enums.Forms.Main, sql);
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
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistants, table);
                if (!valid)
                {
                    table?.RejectChanges();
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
        private void printButton_Click(object sender, EventArgs e)
        {
            sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_assistants, totalAmountBox.Text);
            DataRow[] rows = table.Select("", "date");
            PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.assistants);
            printer.Print(Text, Text, this, rows);
        }
        /// <summary>
        /// Handles the cell Content Double Click event for view and updates the related state.
        /// </summary>
        private void view_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (changeButton.Enabled)
                ChangeAssistant();
        }
        /// <summary>
        /// Runs the change Assistant operation and updates the related application state.
        /// </summary>
        private async void ChangeAssistant()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (view.SelectedRows.Count == 0)
                    return;
                DataGridViewRow rowView = view.SelectedRows[0];
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                using (CreateAssistantsDialog form = new CreateAssistantsDialog(
                    Int32.Parse(row[SQLBase.Names(SQLBase.ColumnNames.id)].ToString()),
                    row[nameColumn.DataPropertyName].ToString(),
                    DateTime.Parse(row[dateColumn.DataPropertyName].ToString()),
                    decimal.Parse(row[amountPayoutColumn.DataPropertyName].ToString())
                ))
                {
                    if (form.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                        return;
                    row[SQLBase.Names(SQLBase.ColumnNames.id)] = form.ID;
                    row[nameColumn.DataPropertyName] = form.AssistantName;
                    row[dateColumn.DataPropertyName] = form.Date;
                    row[handSignColumn.DataPropertyName] = sql.User.Name;
                    row[activeColumn.DataPropertyName] = true;
                    bool bookAssistant = decimal.Parse(row[amountPayoutColumn.DataPropertyName].ToString()) == 0;
                    if (bookAssistant)
                    {
                        row[amountPayoutColumn.DataPropertyName] = form.Amount;
                        row["amount_payback"] = 0;
                        row["amount_payback_type"] = 0;
                    }
                    bool valid = false;
                    using (var transaction = sql.BeginTransaction())
                    {
                        try
                        {
                            valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistants, table);
                            if (valid && bookAssistant)
                                valid = await sql.ToBargeAsync(form.Date, string.Format(Messages.ioan_to, form.AssistantName), -Math.Abs(form.Amount), string.Format("M{0:000}", form.ID), SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand);
                            if (!valid)
                                throw new Exception(Messages.assistants_changed_failed);
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
                        if (bookAssistant)
                            await ConnectTableToDataBase();
                        MessageBox.ShowDialog(this, Messages.assistants_changed);
                    }
                    else
                    {
                        table?.RejectChanges();
                        MessageBox.ShowDialog(this, Messages.assistants_changed_failed);
                    }
                }
            }
            catch
            {
                table?.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = Messages.assistants_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                var currentTable = table.DefaultView.ToTable();
                currentTable.Columns.Remove("account_transfer");
                currentTable.Columns.Remove("amount_payback");
                currentTable.Columns.Remove("amount_payback_type");
                currentTable.Columns.Remove("date");
                Excel.ExportToExcel(currentTable, fileDialog.FileName);
                MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
            }
        }
        /// <summary>
        /// Handles the click event for button Import and updates the related state.
        /// </summary>
        private async void buttonImport_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.FileName = Messages.assistants_export_filename;
                    fileDialog.Filter = "Excel|*.xlsx";
                    if (fileDialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    DataTable importTable = new DataTable();
                    await sql.FillAdapterAsync(SQLBase.SELECT.Assistants, importTable);

                    Excel.Import(fileDialog.FileName, importTable, int.MaxValue, new HashSet<string>() { "handsign" });
                    var ids = importTable.Rows
                        .OfType<DataRow>()
                        .Where(a => a.RowState == DataRowState.Added)
                        .Select(a => a["id"].ToString())
                        .ToArray();

                    foreach (DataRow row in importTable.Rows)
                    {
                        if (row.RowState != DataRowState.Added)
                            continue;
                        row["date"] = DateTime.Now.Date;
                        row["handsign"] = sql.User.Name;
                        row["amount_payback"] = 0;
                        row["amount_payback_type"] = 0;
                    }

                    using (var transaction = sql.BeginTransaction())
                    {
                        try
                        {
                            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistants, importTable))
                            {
                                MessageBox.ShowDialog(this, Messages.assistants_import_failed);
                                return;
                            }

                            foreach (var addedID in ids)
                            {
                                DataRow row = importTable.Select("id=" + addedID)[0];
                                var name = row["name"].ToString();
                                var payout = Convert.ToDecimal(row[amountPayoutColumn.DataPropertyName]);
                                int id = Convert.ToInt32(row["id"]);

                                if (payout != 0 && !await sql.ToBargeAsync(DateTime.Now.Date, string.Format(Messages.ioan_to, name),
                                    -Math.Abs(payout), string.Format("M{0:000}", id), SQLBase.BookCategory.Auszahlung,
                                    SQLBase.BookingTo.Barbestand))
                                    throw new Exception(Messages.assistants_import_failed);
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    MessageBox.ShowDialog(this, Messages.assistants_created);
                    await ConnectTableToDataBase();
                }
            }
            finally
            { 
                databaseOperationLock.Release(); 
            }
        }
    }
}
