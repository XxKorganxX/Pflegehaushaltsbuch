namespace Pflegehaushaltsbuch.Forms
{
    public interface IDatabaseFileFormContract
    {
        string Password { get; }
        void AcceptDialog();
        void ShowDefaultLoginMessage();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        bool ShowOpenFileDialog(string fileName, string filter, out string selectedFileName);
    }
}
