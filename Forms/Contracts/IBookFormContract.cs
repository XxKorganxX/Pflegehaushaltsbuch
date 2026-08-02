namespace Pflegehaushaltsbuch.Forms
{
    public interface IBookFormContract
    {
        void ConnectTableToDataBase();
        void Back();
        void GetClientInfo();
        void UpdateDocumentNumbers();
        void Storno();
        void UpdateClientNote();
        void PrintAccount();
        void Book();
        void PeriodCheck();
        void Export();
        void Update();
    }
}
