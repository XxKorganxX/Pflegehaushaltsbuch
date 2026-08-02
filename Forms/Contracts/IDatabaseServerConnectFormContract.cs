namespace Pflegehaushaltsbuch.Forms
{
    public interface IDatabaseServerConnectFormContract
    {
        void ConfigureDatabaseTypeButtons();
        void UpdateDatabaseTypeButtons();
        void LoadDatabaseTypeIcon();
        void CreateStrongIconAttributes();
        void DatabaseType();
        void Connect();
        void Close();
    }
}
