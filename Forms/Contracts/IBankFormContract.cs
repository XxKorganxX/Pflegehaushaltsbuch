namespace Pflegehaushaltsbuch.Forms
{
    public interface IBankFormContract
    {
        void ConnectTableToDataBase();
        void Book();
        void Back();
        void Update();
        void Print();
        void AllBooksCheck();
        void PeriodCheck();
        void Export();
    }
}
