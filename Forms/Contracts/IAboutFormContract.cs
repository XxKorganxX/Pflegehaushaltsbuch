namespace Pflegehaushaltsbuch.Forms
{
    public interface IAboutFormContract
    {
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
