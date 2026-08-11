using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IAdministrationFormContract
    {
        bool IsDatabaseConnected { get; }
        bool CanAdministrateDatabase { get; }

        void SetAdministrationButtonsEnabled(bool enabled);
        void SetImprovedEnabled(bool enabled);
        void SetConnectDatabaseEnabled(bool enabled);
        void SetViewEnabled(bool enabled);
        void ShowForm(Enums.Forms form);
        bool ShowBackupFileDialog(out string fileName);
        bool ShowRestoreFileDialog(out string fileName);
        IAdministrationProgress ShowProgressDialog(string text);
        void ShowDatabaseBackupSuccess();
        void ShowDatabaseRestoreSuccess();
        bool ConfirmDatabaseReset();
        void RefreshAccessState();
        bool ShowDatabaseServerConnectDialog(SqlSession session, XmlConfig config);
        bool ShowDatabaseManagerDialog(SqlSession session, XmlConfig config, out SQLBase sql);
        bool ShowUserLoginDialog(SqlSession loginSession, out SQLBase authenticatedSql);
        void ShowDesignDialog(SqlSession session);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public interface IAdministrationProgress : IDisposable
    {
        void Close();
        void UpdateText(string text);
        void UpdateProgress(int percent, bool increment);
        void UpdateMaximumProgress(int percent);
    }
}
