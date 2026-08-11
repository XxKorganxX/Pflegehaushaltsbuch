using System;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IDatabaseUpdateDialogContract
    {
        void SetVersion(Version version);
        void CloseView();
        void CloseOwner();
        void ShowError(Exception exception);
        void ExitApplication(int exitCode);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
