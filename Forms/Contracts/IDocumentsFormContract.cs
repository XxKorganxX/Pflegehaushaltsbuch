namespace Pflegehaushaltsbuch.Forms
{
    public interface IDocumentsFormContract
    {
        void ConnectToClients();
        void ConnectTableToDataBase();
        void Insert();
        void Delete();
        void Change();
        void Back();
    }
}
