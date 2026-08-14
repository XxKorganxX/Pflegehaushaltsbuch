using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IClientsFormContract
    {
        string DefaultSortColumn { get; }
        string CurrentSortColumn { get; }
        int ActiveClientsFilterIndex { get; }
        int? SelectedClientId { get; }
        string SelectedClientName { get; }
        string TotalAmountText { get; }
        void BindClients(DataView clients);
        void ClearClients();
        void BindClientDates(DataView clients);
        void SetTotalClients(int totalClients);
        void SetTotalAmount(string totalAmount);
        void SetDeadlineText(string text);
        void SelectClientById(int clientId);
        void NotifyClientIdChanged(int clientID);
        bool ShowCreateClientDialog(out ClientAccountInput clientData);
        bool ShowChangeClientDialog(int clientID, out ClientAccountInput clientData);
        void ShowPrintClientsBooksDialog();
        void PrintClients(DataRow[] clients);
        void ShowMainForm();
        void ShowBookForm();
        void ShowCalendarForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        Task ShowMainFormAsync(CancellationToken cancellationToken = default);
    }

    public class ClientAccountInput
    {
        public int ClientID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public System.DateTime BornDate { get; set; }
        public decimal Amount { get; set; }
        public int? AdvisorId { get; set; }
    }

    public class ClientImportInput
    {        
        public ClientImportRecord[] Clients { get; set; }
    }

    public class ClientImportRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public System.DateTime BornDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public int? AdvisorId { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
