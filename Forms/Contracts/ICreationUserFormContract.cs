namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreationUserFormContract
    {
        string Handsign { get; set; }
        string Login { get; set; }
        bool InsertAllowed { get; set; }
        bool ChangeAllowed { get; set; }
        bool BookAllowed { get; set; }
        bool CancelBookingAllowed { get; set; }
        bool CashBalanceAllowed { get; set; }
        bool BankBalanceAllowed { get; set; }
        bool PettyCashAllowed { get; set; }
        bool ClientsAllowed { get; set; }
        bool RepresentativesAllowed { get; set; }
        bool EmployeesAllowed { get; set; }
        bool DocumentsAllowed { get; set; }
        bool CashAuditAllowed { get; set; }
        bool StatisticsAllowed { get; set; }
        bool Admin { get; set; }
        void AcceptDialog();
        void BindData();
        void ShowUserChanged();
        void ShowUserCreated();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
