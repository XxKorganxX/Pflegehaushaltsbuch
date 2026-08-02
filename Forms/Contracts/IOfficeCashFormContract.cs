namespace Pflegehaushaltsbuch.Forms
{
    public interface IOfficeCashFormContract
    {
        void ConnectTableToDataBase();
        void Back();
        void Book();
        void Print();
        void PeriodCheck();
        void Export();
    }
}
