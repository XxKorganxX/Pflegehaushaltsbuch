namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IProgressDialogContract
    {
        void CloseView();
        void SetText(string text);
        void SetProgress(int percent, bool increment);
        void SetMaximumProgress(int percent);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
