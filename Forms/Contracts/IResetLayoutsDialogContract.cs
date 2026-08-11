namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IResetLayoutsDialogContract
    {
        bool AllChecked { get; set; }
        bool ClientsChecked { get; set; }
        bool AdvisorsChecked { get; set; }
        bool EmployeeChecked { get; set; }
        bool CashChecked { get; set; }
        bool BankChecked { get; set; }
        bool BillChecked { get; set; }
        bool CashCheckChecked { get; set; }
        bool QuittanceChecked { get; set; }
        bool OfficeCashChecked { get; set; }
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
