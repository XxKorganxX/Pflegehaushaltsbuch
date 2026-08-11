using Pflegehaushaltsbuch.Databases;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IUserLoginFormContract
    {
        string UserName { get; }
        string Password { get; }
        void ClearPassword();
        bool ShowChangeUserDialog(SqlSession session, string user, string keyword, DataTable users);
        void ShowUserDataChanged();
        void SetAccepted();
        void CloseView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
