using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Forms.Contracts;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DataExchangeFormPresenter
    {
        private readonly IDataExchangeFormContract view;
        private readonly SqlSession session;
        private DataGridViewColumn activeColumn;

        public DataExchangeFormPresenter(IDataExchangeFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
            this.session = session;
        }

        public async Task LoadTablesAsync()
        {
            var clients = new DataTable();
            var advisors = new DataTable();
            var employees = new DataTable();
            var cash = new DataTable();
            var bank = new DataTable();
            var officeCash = new DataTable();
            var deadlines = new DataTable();

            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clients);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Advisors, advisors);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, employees);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, cash);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, bank);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.OfficeCash, officeCash);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Deadlines, deadlines);

            view.ClientTable = clients;
            view.AdvisorTable = advisors;
            view.EmployeeTable = employees;
            view.CashTable = cash;
            view.BankTable = bank;
            view.OfficeCashTable = officeCash;
            view.DeadlinesTable = deadlines;
        }

        public void CreateControl()
        {
            view.InitializeExchangeGrids();
        }       

        public void Reset()
        {
            view.ResetGridSources();
        }

        public async Task ImportAsync()
        {
        }

        public async Task ExportAsync(
            DataTable clientsTable,
            DataTable advisorTable,
            DataTable employeesTable,
            DataTable cashTable,
            DataTable bankTable,
            DataTable officeCashTable,
            DataTable deadlinesTable)
        {
            string folder;
            if (!view.ShowExportFolderDialog(out folder))
                return;

            await ExportAsync(folder,
                clientsTable,
                advisorTable,
                employeesTable,
                cashTable,
                bankTable,
                officeCashTable,
                deadlinesTable);

            view.ShowExportSuccess(folder);
        }

        public async Task ExportAsync(string folder,
            DataTable clientTable,
            DataTable advisorTable,
            DataTable employeesTable,
            DataTable cashTable,
            DataTable bankTable,
            DataTable officeCashTable,
            DataTable deadlinesTable)
        {
            Excel.ExportToExcel(clientTable, Path.Combine(folder, "clients.xlsx"));
            Excel.ExportToExcel(advisorTable, Path.Combine(folder, "advisors.xlsx"));
            Excel.ExportToExcel(employeesTable, Path.Combine(folder, "employees.xlsx"));
            Excel.ExportToExcel(cashTable, Path.Combine(folder, "cash.xlsx"));
            Excel.ExportToExcel(bankTable, Path.Combine(folder, "bank.xlsx"));
            Excel.ExportToExcel(officeCashTable, Path.Combine(folder, "office_cash.xlsx"));
            Excel.ExportToExcel(deadlinesTable, Path.Combine(folder, "deadlines.xlsx"));
        }

        public void Back()
        {
            view.ShowAdministrationForm();
        }

        /*
        public void ColumnHeaderMouseClick(DataGridView dgv, int columnIndex)
        {
            if (dgv == null || columnIndex < 1)
            {
                activeColumn = null;
                return;
            }

            if (activeColumn != null && activeColumn.DataGridView == dgv)
                view.ResetColumnHeaderStyle(activeColumn);

            activeColumn = dgv.Columns[columnIndex];
            view.HighlightColumnHeader(activeColumn);
            dgv.Invalidate();
        }

        public void DataBindingComplete(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells[0].Value = true;
            }

            view.ApplyGridDefaults(grid);
            view.ApplyCheckedColumnHeaders(grid);
        }

        private void SetSelectedRowsIncluded(bool included)
        {
            DataGridView dgv = view.CurrentGrid;
            if (dgv == null || dgv.SelectedRows.Count == 0)
                return;

            foreach (DataGridViewRow row in dgv.SelectedRows)
                row.Cells[0].Value = included;
        }
        */
    }
}
