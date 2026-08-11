using System.Data;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface ICreateDocumentDialogContract
    {
        string FilePath { get; set; }
        string Description { get; }
        int SelectedClientID { get; }
        void BindClients(DataTable table, int clientID);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
