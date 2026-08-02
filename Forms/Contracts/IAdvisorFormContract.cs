namespace Pflegehaushaltsbuch.Forms
{
    public interface IAdvisorFormContract
    {
        void ConnectTableToDataBase();
        void ChangeAdvisorAsync();
        void CreateAccount();
        void Change();
        void Back();
        void Print();
        void Delete();
        void Import();
        void Export();
        void Update();
    }
}
