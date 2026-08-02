namespace Pflegehaushaltsbuch.Forms
{
    public interface IAdministrationFormContract
    {
        void OnUserRights();
        void UserRights();
        void ClientManagement();
        void Cash();
        void Credit();
        void AccountHoldings();
        void Advisor();
        void CashOfficeControl();
        void Banking();
        void Record();
        void License();
        void DatabaseBackup();
        void Layout();
        void Restore();
        void ResetDatabase();
        void Improved();
        void DbConnect();
        void ConnectEmbeddedDatabase();
        void Company();
        void Design();
    }
}
