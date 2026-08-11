using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface ILicenseCheckDialogContract
    {
        string Output { get; set; }
        bool OutputEnabled { get; set; }

        void BindOutput();
        void CloseView();
        void MoveWindow(MouseEventArgs e);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
