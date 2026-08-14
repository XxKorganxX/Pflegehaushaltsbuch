using System.Data;

namespace Pflegehaushaltsbuch.Forms.Contracts
{
    public interface IDataExchangeFormContract
    {
        DataTable ClientTable { get; set; }
        DataTable DeadlinesTable { get; set; }
        DataTable RepresentativeTable { get; set; }
        DataTable EmployeeTable { get; set; }
        DataTable CashTransactionsTable { get; set; }
        DataTable BankTransactionsTable { get; set; }
        DataTable PettyCashTransactionsTable { get; set; }
        DataTable ClientTransactionsTable { get; set; }
        DataTable AccountsTable { get; set; }
        DataTable DocumentsTable { get; set; }
        void InitializeExchangeGrids();
        bool ShowExportFolderDialog(out string selectedPath);
        void ResetGridSources();
        void ShowExportSuccess(string folder);
        void ShowAdministrationForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
