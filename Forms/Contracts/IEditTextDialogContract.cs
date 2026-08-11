namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IEditTextDialogContract
    {
        string Content { get; set; }
        string EditorText { get; set; }
        string UnicodeText { get; }
        string SelectedVariableText { get; }

        void SetVariableDataSource(object dataSource);
        void ReplaceSelection(string text);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
