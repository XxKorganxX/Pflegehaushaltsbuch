namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreationUserFormContract
    {
        string UserName { get; set; }
        string Login { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string Email { get; set; }
        bool InsertAllowed { get; set; }
        bool ChangeAllowed { get; set; }
        bool DeleteAllowed { get; set; }
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
