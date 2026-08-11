using System.Data;

namespace Pflegehaushaltsbuch.Forms.Contracts
{
    public interface IDataExchangeFormContract
    {
        DataTable ClientTable { get; set; }
        DataTable AdvisorTable { get; set; }
        DataTable EmployeeTable { get; set; }
        DataTable CashTable { get; set; }
        DataTable BankTable { get; set; }
        DataTable OfficeCashTable { get; set; }
        DataTable DeadlinesTable { get; set; }        
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
