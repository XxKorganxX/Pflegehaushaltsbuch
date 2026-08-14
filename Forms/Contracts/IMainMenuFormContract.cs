using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IMainMenuFormContract
    {
        void SetWorkPanelsEnabled(bool enabled);
        void SetAdminVisible(bool visible);
        void ApplyCurrentRights();
        void ShowError(Exception exception);
        bool ShowUserLoginDialog(SqlSession session);
        void ShowForm(Enums.Forms form);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
