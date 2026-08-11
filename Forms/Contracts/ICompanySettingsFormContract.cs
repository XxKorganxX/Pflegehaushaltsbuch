using Pflegehaushaltsbuch.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICompanySettingsFormContract
    {
        string Email { get; }
        void BindCompany(Company company);
        void ShowCompanyLogo();
        bool ShowLogoDialog(out string fileName);
        void ShowCompanySaved();
        void ShowAdministrationForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
