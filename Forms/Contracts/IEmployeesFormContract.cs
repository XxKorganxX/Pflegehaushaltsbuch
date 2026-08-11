using Pflegehaushaltsbuch.Databases;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IEmployeesFormContract
    {
        string DefaultSortColumn { get; }
        string CurrentSortColumn { get; }
        bool ChangeButtonEnabled { get; }
        int? SelectedAssistantId { get; }
        string SelectedAssistantName { get; }
        void BindEmployees(DataView employees);
        void ClearEmployees();
        void BindEmployeeDate(DataView employees);
        void SetTotalAmount(string totalAmount);
        void PrintEmployees(DataRow[] employees);
        bool ShowCreateAssistantDialog(int id, out AssistantInput input);
        bool ShowChangeAssistantDialog(int id, string name, System.DateTime date, decimal amount, out AssistantInput input);
        bool ShowIoanPaybackDialog(string assistantName, int assistantId, decimal amount, out AssistantPaybackInput input);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        bool ShowOpenFileDialog(string fileName, string filter, out string selectedFileName);
    }

    public class AssistantInput
    {
        public int ID { get; set; }
        public string AssistantName { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Date { get; set; }
    }

    public class AssistantPaybackInput
    {
        public string AssistantName { get; set; }
        public int AssistantId { get; set; }
        public System.DateTime PaybackDate { get; set; }
        public decimal Amount { get; set; }
        public int RepaymentIndex { get; set; }
        public SQLBase.Repayment Repayment { get; set; }
    }
}
