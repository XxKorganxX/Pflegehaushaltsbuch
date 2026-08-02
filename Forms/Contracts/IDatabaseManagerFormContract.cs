namespace Pflegehaushaltsbuch.Forms
{
    public interface IDatabaseManagerFormContract
    {
        void Close();
        void GetDataabase();
        void Connect();
        void CreateDataBase();
        void SqlUser();
        void Label5();
        void Label7();
        void Label8();
        void UpdateKeyWord();
        void ChangeMasterkeywordLabel();
    }
}
