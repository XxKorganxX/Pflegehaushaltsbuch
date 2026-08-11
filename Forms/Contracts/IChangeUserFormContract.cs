namespace Pflegehaushaltsbuch.Forms
{
    public interface IChangeUserFormContract
    {
        string UserName { get; set; }
        string Login { get; set; }
        string Keyword { get; }
        string KeywordAgain { get; }
        void AcceptDialog();
        void KeepDialogOpen();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
