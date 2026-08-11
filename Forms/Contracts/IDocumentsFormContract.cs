using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IDocumentsFormContract
    {
        int ActiveClientsIndex { get; set; }
        bool ActiveClientsFocused { get; }
        bool DateFilterChecked { get; }
        bool DateFilterFocused { get; }
        bool DateBoxContainsFocus { get; }
        DateTime DocumentDate { get; }
        int SelectedClientId { get; }
        void AddActiveClientFilterItem(string item);
        void BindClients(DataView clients);
        void ClearClients();
        void BindDocuments(DataTable documents);
        DataRow[] GetSelectedDocuments();
        bool ShowOpenDocumentDialog(out string fileName);
        bool ShowCreateDocumentDialog(SqlSession session, int clientID, string fileName, DataTable clients, out CreateDocumentData document);
        void ShowSelectClientFirst();
        void ShowMainForm();
        void FocusDocumentsView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public class CreateDocumentData
    {
        public int SelectedClientID { get; set; }
        public string FilePath { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Description { get; set; }
        public string DocumentFileName { get; set; }
    }
}
