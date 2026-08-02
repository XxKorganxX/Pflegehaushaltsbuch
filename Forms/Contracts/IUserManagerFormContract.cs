namespace Pflegehaushaltsbuch.Forms
{
    public interface IUserManagerFormContract
    {
        void ConnectTableToDataBase();
        void Back();
        void Save();
        void Create();
        void Update();
        void Delete();
    }
}
