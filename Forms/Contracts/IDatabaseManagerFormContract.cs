using System.Collections.Generic;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IDatabaseManagerFormContract
    {
        string CreateDatabaseName { get; }
        string SelectedDatabase { get; }
        string UserName { get; }
        string Keyword { get; }
        string FromHost { get; }
        string MasterKeyword { get; }
        string MasterKeywordAgain { get; }
        void ShowDatabases(IEnumerable<string> databases, string selectedDatabase);
        bool ShowOpenDatabaseDialog(out string databaseFileName);
        IAdministrationProgress ShowProgressDialog(string text);
        bool ConfirmDatabaseCreating();
        void ShowEnterDatabaseName();
        void ShowDefaultLoginMessage();
        void ShowDatabaseCreated();
        void ShowUserCreated();
        void ShowMasterPasswordChangedRestart();
        void AcceptDialog();
        void CancelDialog();
        void KeepDialogOpen();
        void ToggleCreateDatabasePanel();
        void ToggleUserPanel();
        void ToggleConnectPanel();
        void ToggleMasterkeyPanel();
        void RestartApplication();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
