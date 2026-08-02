namespace Pflegehaushaltsbuch.Forms
{
    public interface IClientsFormContract
    {
        void ConnectTableToDataBase();
        void UpdateTotalAmount();
        void CreateAccount();
        void Change();
        void Delete();
        void DeadLines();
        void SelectAccount();
        void Back();
        void Print();
        void Update();
        void ImportClients();
        void Import();
        void ClientBooks();
        void Clients();
        void Export();
    }
}
