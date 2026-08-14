using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Contracts;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Pflegehaushaltsbuch.Forms
{
    public partial class DataExchangeForm : Form, IDataExchangeFormContract
    {
        DataGridViewColumn activeColumn = null;
        private readonly DataExchangeFormPresenter presenter;
        private DataTable clientTable,
            deadlinesTable,
            advisorTable, 
            employeeTable, 
            cashTransactionsTable, 
            bankTransactionsTable, 
            officeCashTransactionsTable, 
            clientTransactionsTable,
            accountsTable,
            documentsTable;

        /// <summary>
        /// Creates a new DataExchangeForm view.
        /// </summary>
        public DataExchangeForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DataExchangeFormPresenter(this, session);
        }

        /// <summary>
        /// Handles the on shown step.
        /// </summary>
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (Program.DesignMode)
                return;

            await presenter.LoadTablesAsync();
        }

        /// <summary>
        /// Handles the on create control step.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (Program.DesignMode)
                return;

            presenter.CreateControl();
        }

        public DataTable ClientTable { get { return clientTable; } set { clientTable = value; clientView.DataSource = value; ApplyCheckedColumnHeaders(clientView); } }
        public DataTable DeadlinesTable { get { return deadlinesTable; } set { deadlinesTable = value; deadlinesView.DataSource = value; ApplyCheckedColumnHeaders(deadlinesView); } }
        public DataTable RepresentativeTable { get { return advisorTable; } set { advisorTable = value; advisorView.DataSource = value; ApplyCheckedColumnHeaders(advisorView); } }
        public DataTable EmployeeTable { get { return employeeTable; } set { employeeTable = value; employeesView.DataSource = value; ApplyCheckedColumnHeaders(employeesView); } }
        public DataTable CashTransactionsTable { get { return cashTransactionsTable; } set { cashTransactionsTable = value; cashTransactionsView.DataSource = value; ApplyCheckedColumnHeaders(cashTransactionsView); } }
        public DataTable BankTransactionsTable { get { return bankTransactionsTable; } set { bankTransactionsTable = value; bankTransactionsView.DataSource = value; ApplyCheckedColumnHeaders(bankTransactionsView); } }
        public DataTable PettyCashTransactionsTable { get { return officeCashTransactionsTable; } set { officeCashTransactionsTable = value; officeCashTransactionsView.DataSource = value; ApplyCheckedColumnHeaders(officeCashTransactionsView); } }
        public DataTable ClientTransactionsTable { get { return clientTransactionsTable; } set { clientTransactionsTable = value; clientTransactionsView.DataSource = value; ApplyCheckedColumnHeaders(clientTransactionsView); } }
        public DataTable AccountsTable { get { return accountsTable; } set { accountsTable = value; accountsView.DataSource = value; ApplyCheckedColumnHeaders(accountsView); } }
        public DataTable DocumentsTable { get { return documentsTable; } set { documentsTable = value; documentsView.DataSource = value; ApplyCheckedColumnHeaders(documentsView); } }

        /// <summary>
        /// Runs the include button_click action.
        /// </summary>
        private void includeButton_Click(object sender, EventArgs e)
        {
            Include();
        }

        /// <summary>
        /// Runs the exclude button_click action.
        /// </summary>
        private void excludeButton_Click(object sender, EventArgs e)
        {
            Exclude();
        }

        /// <summary>
        /// Runs the reset button_click action.
        /// </summary>
        private void resetButton_Click(object sender, EventArgs e)
        {
            presenter.Reset();
        }

        /// <summary>
        /// Runs the move left button_click action.
        /// </summary>
        private void moveLeftButton_Click(object sender, EventArgs e)
        {
            MoveLeft();
        }

        /// <summary>
        /// Runs the move right button_click action.
        /// </summary>
        private void moveRightButton_Click(object sender, EventArgs e)
        {
            MoveRight();
        }

        /// <summary>
        /// Runs the import button_click action.
        /// </summary>
        private async void importButton_Click(object sender, EventArgs e)
        {
            await presenter.ImportAsync();
        }

        /// <summary>
        /// Runs the export button_click action.
        /// </summary>
        private async void exportButton_Click(object sender, EventArgs e)
        {
            await presenter.ExportAsync(
                CreateExportTable(clientView),
                CreateExportTable(deadlinesView),
                CreateExportTable(advisorView),
                CreateExportTable(employeesView),
                CreateExportTable(cashTransactionsView),
                CreateExportTable(bankTransactionsView),
                CreateExportTable(officeCashTransactionsView),
                CreateExportTable(clientTransactionsView),
                CreateExportTable(accountsView),
                CreateExportTable(documentsView));
        }

        /// <summary>
        /// Runs the back button_click action.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        void IDataExchangeFormContract.InitializeExchangeGrids()
        {
            InitializeExchangeGrids();
        }

        void IDataExchangeFormContract.ResetGridSources()
        {
            ResetGridSources();
        }

        /// <summary>
        /// Runs the show export folder dialog view action for the presenter.
        /// </summary>
        bool IDataExchangeFormContract.ShowExportFolderDialog(out string selectedPath)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;
                    return true;
                }
            }

            selectedPath = null;
            return false;
        }

        public void Include()
        {
            SetSelectedRowsIncluded(true);
        }

        public void Exclude()
        {
            SetSelectedRowsIncluded(false);
        }

        /// <summary>
        /// Runs the reset grid sources view action for the presenter.
        /// </summary>
        void ResetGridSources()
        {
            clientView.DataSource = null;
            deadlinesView.DataSource = null;
            advisorView.DataSource = null;
            employeesView.DataSource = null;
            cashTransactionsView.DataSource = null;
            bankTransactionsView.DataSource = null;
            officeCashTransactionsView.DataSource = null;
            clientTransactionsView.DataSource = null;
            accountsView.DataSource = null;
            documentsView.DataSource = null;

            clientView.DataSource = ClientTable;
            deadlinesView.DataSource = DeadlinesTable;
            advisorView.DataSource = RepresentativeTable;
            employeesView.DataSource = EmployeeTable;
            cashTransactionsView.DataSource = CashTransactionsTable;
            bankTransactionsView.DataSource = BankTransactionsTable;
            officeCashTransactionsView.DataSource = PettyCashTransactionsTable;
            clientTransactionsView.DataSource = ClientTransactionsTable;
            accountsView.DataSource = AccountsTable;
            documentsView.DataSource = DocumentsTable;
        }

        /// <summary>
        /// Runs the apply grid defaults view action for the presenter.
        /// </summary>
        void ApplyGridDefaults(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = column.Index != 0;
            }

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToOrderColumns = true;
            grid.AllowUserToAddRows = false;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor;
        }

        private static DataTable CreateExportTable(DataGridView grid)
        {
            grid.EndEdit();

            var result = new DataTable(grid.Name.Replace("View", ""));
            var columns = grid.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                .Where(c => c.HeaderCell is FormControls.DataGridViewCheckedColumnHeaderCell header && header.Checked)
                .ToList();

            foreach (var column in columns)
                result.Columns.Add(column.DataPropertyName, GetExportColumnType(grid, column));

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow || !Convert.ToBoolean(row.Cells[0].Value ?? false))
                    continue;

                DataRow newRow = result.NewRow();

                foreach (var column in columns)
                {
                    object value = row.Cells[column.Name].Value;
                    newRow[column.DataPropertyName] = value ?? DBNull.Value;
                }

                result.Rows.Add(newRow);
            }

            return result;
        }

        private static Type GetExportColumnType(DataGridView grid, DataGridViewColumn column)
        {
            if (grid.DataSource is DataTable table && table.Columns.Contains(column.DataPropertyName))
                return table.Columns[column.DataPropertyName].DataType;

            return column.ValueType ?? typeof(string);
        }

        /// <summary>
        /// Provides the current grid value for the presenter.
        /// </summary>
        DataGridView CurrentGrid
        {
            get
            {
                if (tabControl.SelectedTab == null)
                    return null;

                return tabControl.SelectedTab.Controls
                    .OfType<DataGridView>()
                    .FirstOrDefault();
            }
        }

        public void MoveLeft()
        {
            DataGridView dgv = CurrentGrid;
            if (dgv == null)
                return;

            if (activeColumn != null && activeColumn.DisplayIndex > 1)
                activeColumn.DisplayIndex--;
        }

        public void MoveRight()
        {
            DataGridView dgv = CurrentGrid;
            if (dgv == null)
                return;

            if (activeColumn != null && activeColumn.DisplayIndex < dgv.Columns.Count - 1)
                activeColumn.DisplayIndex++;
        }

        /// <summary>
        /// Runs the initialize exchange grids view action for the presenter.
        /// </summary>
        void InitializeExchangeGrids()
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                var dgv = tab.Controls[0] as DataGridView;
                dgv.Columns.Add(new DataGridViewCheckBoxColumn() { Width = 20 });
                dgv.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
                dgv.DataBindingComplete += DataGridView_DataBindingComplete;
            }
        }

        public void ColumnHeaderMouseClick(DataGridView dgv, int columnIndex)
        {
            if (dgv == null || columnIndex < 1)
            {
                activeColumn = null;
                return;
            }

            if (activeColumn != null && activeColumn.DataGridView == dgv)
                ResetColumnHeaderStyle(activeColumn);

            activeColumn = dgv.Columns[columnIndex];
            HighlightColumnHeader(activeColumn);
            dgv.Invalidate();
        }

        public void DataBindingComplete(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells[0].Value = true;
            }

            ApplyGridDefaults(grid);
            ApplyCheckedColumnHeaders(grid);
        }

        private void SetSelectedRowsIncluded(bool included)
        {
            DataGridView dgv = CurrentGrid;
            if (dgv == null || dgv.SelectedRows.Count == 0)
                return;

            foreach (DataGridViewRow row in dgv.SelectedRows)
                row.Cells[0].Value = included;
        }

        /// <summary>
        /// Runs the reset column header style view action for the presenter.
        /// </summary>
        void ResetColumnHeaderStyle(DataGridViewColumn column)
        {
            DataGridView dgv = column.DataGridView;
            column.HeaderCell.Style.BackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
            column.HeaderCell.Style.ForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;
            column.HeaderCell.Style.SelectionBackColor = Color.Empty;
            column.HeaderCell.Style.SelectionForeColor = Color.Empty;
        }

        /// <summary>
        /// Runs the highlight column header view action for the presenter.
        /// </summary>
        void HighlightColumnHeader(DataGridViewColumn column)
        {
            column.HeaderCell.Style.BackColor = SystemColors.Highlight;
            column.HeaderCell.Style.ForeColor = SystemColors.HighlightText;
            column.HeaderCell.Style.SelectionBackColor = SystemColors.Highlight;
            column.HeaderCell.Style.SelectionForeColor = SystemColors.HighlightText;
        }

        /// <summary>
        /// Runs the show export success view action for the presenter.
        /// </summary>
        void IDataExchangeFormContract.ShowExportSuccess(string folder)
        {
            MessageBox.ShowDialog(this, string.Format(Messages.export_success, folder));
        }

        /// <summary>
        /// Runs the show administration form view action for the presenter.
        /// </summary>
        void IDataExchangeFormContract.ShowAdministrationForm()
        {
            ShowFormEvent(Enums.Forms.Administration);
        }

        /// <summary>
        /// Runs the dgv_column header mouse click action.
        /// </summary>
        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnHeaderMouseClick(sender as DataGridView, e.ColumnIndex);
        }

        /// <summary>
        /// Runs the data grid view_data binding complete action.
        /// </summary>
        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataBindingComplete((DataGridView)sender);
        }

        /// <summary>
        /// Runs the apply checked column headers action.
        /// </summary>
        private void ApplyCheckedColumnHeaders(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.HeaderCell is FormControls.DataGridViewCheckedColumnHeaderCell)
                    continue;

                if (column.Index == 0)
                    continue;

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                column.HeaderCell = new FormControls.DataGridViewCheckedColumnHeaderCell
                {
                    Value = column.HeaderText
                };
            }
        }
    }
}
