namespace Pflegehaushaltsbuch.Forms
{
    public interface IDesignFormContract
    {
        void BindSettings();
        void SelectTab(int index);
        bool ShowFolderDialog(out string selectedPath);
        void ShowRestartRequired();
        void RestartApplication();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
