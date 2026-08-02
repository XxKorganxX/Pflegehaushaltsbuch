using Microsoft.Data.SqlClient;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.Properties;
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
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Advisor Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AdvisorForm : Pflegehaushaltsbuch.FormControls.Form, IAdvisorFormContract
    {
        private readonly AdvisorFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
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
            insertButton.Enabled = sql.User.CanInsert;
            changeButton.Enabled = sql.User.CanModify;
            deleteButton.Enabled = sql.User.CanDelete;
            importButton.Enabled = sql.User.CanInsert;
        }
        private DataTable table, deadLinesTable = new DataTable();
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Creates a new Advisor Form instance and initializes the required state.
        /// </summary>
        public AdvisorForm()
        {
            InitializeComponent();
            presenter = new AdvisorFormPresenter(this);
            view.AutoGenerateColumns = false;
            this.Enter += clientPanel_Enter;
            this.Leave += clientPanel_Leave;
        }
        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            advisorsBox.DataSource = null;
            table.Clear();
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            advisorsBox.DataSource = null;
            table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, table);

            table.PrimaryKey = new DataColumn[] { table.Columns["name"] };
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = nameColumn.DataPropertyName;
            view.DataSource = table;
            advisorsBox.DataSource = table;
            advisorsBox.DisplayMember = "name";
            dateBox.DataBindings.Clear();
            dateBox.DataBindings.Add("Text", table, "date", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
        }
        /// <summary>
        /// Runs the change Advisor operation and updates the related application state.
        /// </summary>
        private async Task ChangeAdvisorAsync()
        {
            var myCurrencyManager = (CurrencyManager)this.BindingContext[table];

            if (myCurrencyManager.Position < 0)
                return;

            using (CreateAdvisorDialog createAccount = new CreateAdvisorDialog(sql, table, myCurrencyManager.Position))
            {
                if (createAccount.ShowDialog(this) == DialogResult.OK)
                {
                    if (await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table))
                        MessageBox.ShowDialog(this, Messages.advisor_created_changed);
                    else
                        MessageBox.ShowDialog(this, Messages.advisor_changed_failed);
                }
            }
        }
        /// <summary>
        /// Handles the click event for create Account Button and updates the related state.
        /// </summary>
        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                using (CreateAdvisorDialog createAccount = new CreateAdvisorDialog(sql, table))
                {
                    if (createAccount.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table))
                            MessageBox.ShowDialog(this, Messages.advisor_created_changed);
                        else
                            MessageBox.ShowDialog(this, Messages.advisor_changed_failed);
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
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await ChangeAdvisorAsync();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
                DataRow[] rows = table.Select("", "date");
                PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.advisors);
                printer.Print(Text, Text, this, rows);
            }
            catch
            {
                throw;
            }
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
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                if (row == null)
                    return;
                if (MessageBox.ShowDialog(this, Messages.advisor_delete, Messages.advisor_delete_title, MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
                advisorsBox.DataSource = null;
                row.Delete();
                bool value = await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table);
                if(!value)
                {
                    table.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.advisor_delete_failed);
                }
                advisorsBox.DataSource = table;
                advisorsBox.DisplayMember = "name";
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
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private async void importButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            bool imported = false;
            try
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.FileName = Messages.advisors_export_filename;
                    fileDialog.Filter = "Excel|*.xlsx";
                    if (fileDialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    imported = true;
                    Excel.Import(fileDialog.FileName, table, int.MaxValue, new HashSet<string>() { "handsign", "date" });

                    var ids = table.Rows.OfType<DataRow>().Where(a => a.RowState == DataRowState.Added).Select(a => a["id"].ToString()).ToArray();
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.RowState != DataRowState.Added)
                            continue;
                        row["date"] = DateTime.Now.Date;
                        row["handsign"] = sql.User.Name;
                    }

                    if (await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table))
                    {
                        MessageBox.ShowDialog(this, Messages.advisors_imported);
                    }
                    else
                    {
                        table.RejectChanges();
                        MessageBox.ShowDialog(this, Messages.advisors_import_failed);
                    }
                }
            }
            catch
            {
                if (imported)
                    table?.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the cell Double Click event for view and updates the related state.
        /// </summary>
        private async void view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (changeButton.Enabled)
                await ChangeAdvisorAsync();
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = Messages.advisors_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var currentTable = table.DefaultView.ToTable();
                    currentTable.Columns.Remove("date");
                    Excel.ExportToExcel(currentTable, fileDialog.FileName);
                    MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
                }
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
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table);
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
    }
}
