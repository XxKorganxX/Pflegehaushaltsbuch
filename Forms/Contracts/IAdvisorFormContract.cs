using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IAdvisorFormContract
    {
        string DefaultSortColumn { get; }
        string CurrentSortColumn { get; }
        bool ChangeButtonEnabled { get; }
        int SelectedAdvisorPosition { get; }
        int? SelectedAdvisorId { get; }
        void BindAdvisors(DataView advisors);
        void ClearAdvisors();
        void BindAdvisorDate(DataView advisors);
        bool ShowCreateAdvisorDialog(DataTable table);
        bool ShowChangeAdvisorDialog(DataTable table, int position);
        void PrintAdvisors(DataRow[] advisors);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
