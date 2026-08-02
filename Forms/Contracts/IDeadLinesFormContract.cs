namespace Pflegehaushaltsbuch.Forms
{
    public interface IDeadLinesFormContract
    {
        void ConnectTableToDataBase();
        void ConnectToClient();
        void Export();
        void Back();
        void Update();
    }
}
