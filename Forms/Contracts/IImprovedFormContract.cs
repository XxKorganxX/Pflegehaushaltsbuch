namespace Pflegehaushaltsbuch.Forms
{
    public interface IImprovedFormContract
    {
        string TextInput { get; }
        void ShowRemovedLicenseServer();
        void ShowForm(Enums.Forms form);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
