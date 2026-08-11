using System;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreateClientDialogContract
    {
        int ClientID { get; set; }
        decimal Amount { get; set; }
        DateTime BornDate { get; set; }
        bool UseAdvisorChecked { get; set; }
        CreateClientDialog.ClientData Data { get; set; }
        int? SelectedAdvisorId { get; }
        void AddTitle(string title);
        void BindSaldo();
        void BindClientID();
        void BindClient(object client);
        void SetDebitorEnabled(bool enabled);
        void SetSaldoEnabled(bool enabled);
        void SetAdvisorsEnabled(bool enabled);
        void SetAdvisorsDataSource(DataTable advisorTable);
        void SelectAdvisorByName(string advisorName);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
