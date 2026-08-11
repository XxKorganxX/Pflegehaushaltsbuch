using System;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreateEmployeesDialogContract
    {
        int ID { get; set; }
        string AssistantName { get; set; }
        DateTime Date { get; set; }
        decimal Amount { get; set; }
        void AddBookAccount(string account);
        void SetBookAccountIndex(int index);
        void BindFields();
        void SetAmountEnabled(bool enabled);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
