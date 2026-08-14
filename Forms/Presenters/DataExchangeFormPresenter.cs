using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Contracts;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DataExchangeFormPresenter
    {
        private readonly IDataExchangeFormContract view;
        private readonly SqlSession session;

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
            var deadlines = new DataTable();
            var representatives = new DataTable();
            var employees = new DataTable();
            var cashTransactions = new DataTable();
            var bankTransactions = new DataTable();
            var pettyCashTransactions = new DataTable();
            var clientTransactions = new DataTable();
            var accounts = new DataTable();
            var documents = new DataTable();

            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clients);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Deadlines, deadlines);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Representatives, representatives);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, employees);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, cashTransactions);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, bankTransactions);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.PettyCash, pettyCashTransactions);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Books, clientTransactions);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Accounts, accounts);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Records, documents);

            view.ClientTable = clients;
            view.DeadlinesTable = deadlines;
            view.RepresentativeTable = representatives;
            view.EmployeeTable = employees;
            view.CashTransactionsTable = cashTransactions;
            view.BankTransactionsTable = bankTransactions;
            view.PettyCashTransactionsTable = pettyCashTransactions;
            view.ClientTransactionsTable = clientTransactions;
            view.AccountsTable = accounts;
            view.DocumentsTable = documents;
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

        public async Task ExportAsync(params DataTable[] tables)
        {
            string folder;
            if (!view.ShowExportFolderDialog(out folder))
                return;

            int exportCounter = 1;

            await Task.Run(() =>
            {
                foreach (var table in tables)
                {
                    if (table == null)
                        continue;

                    string tableName = string.IsNullOrWhiteSpace(table.TableName)
                        ? $"export_{exportCounter++}" : table.TableName;

                    Excel.ExportToExcel(table, Path.Combine(folder, $"{tableName}.xlsx"), session.Company.CurrencyCode);
                }
            });

            view.ShowExportSuccess(folder);
        }

        public void Back()
        {
            view.ShowAdministrationForm();
        }
    }
}
