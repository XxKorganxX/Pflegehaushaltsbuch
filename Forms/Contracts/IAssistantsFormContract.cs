namespace Pflegehaushaltsbuch.Forms
{
    public interface IAssistantsFormContract
    {
        void ConnectTableToDataBase();
        void UpdateTotalAmount();
        void Create();
        void Change();
        void Delete();
        void PayOut();
        void Back();
        void Update();
        void Print();
        void ChangeAssistant();
        void Export();
        void ButtonImport();
    }
}
