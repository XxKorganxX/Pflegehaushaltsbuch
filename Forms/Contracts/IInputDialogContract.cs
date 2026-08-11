using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IInputDialogContract
    {
        string InputTxt { get; set; }
        string OutputTxt { get; set; }

        void BindTextFields();
        DialogResult ShowView(IWin32Window owner);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
