using Pflegehaushaltsbuch.Databases;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IUserManagerFormContract
    {
        DataRow SelectedUserRow { get; }
        void BindUsers(DataTable table);
        void ClearUsers();
        void ShowAdministrationForm();
        bool ShowCreateUserDialog(SqlSession session);
        bool ShowUpdateUserDialog(SqlSession session, DataRow row);
        void ShowUsersMissing();
        bool ConfirmLastAdminDelete();
        bool ConfirmUserDelete(string userName);
        void ShowUserDeleted();
        void ShowUserNotDeleted();
        void ShowDataTableUpdateFailed();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
