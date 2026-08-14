namespace Pflegehaushaltsbuch.Forms
{
    public interface IUserLoginFormContract
    {
        string UserName { get; }
        string Password { get; }
        void ClearPassword();
        bool ShowChangePasswordDialog(out string keyword);
        void ShowUserDataChanged();
        void SetAccepted();
        void CloseView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
