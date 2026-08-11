namespace Pflegehaushaltsbuch.Forms
{
    public interface IMainFormContract
    {
        void InitializeAutomation();
        void ConnectForm(Form form, Enums.Forms page);
        void SelectForm(Enums.Forms form);
        void SetTitle(string title);
        void CloseView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
