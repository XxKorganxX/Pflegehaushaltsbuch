namespace Pflegehaushaltsbuch.Forms
{
    public interface ICashFormContract
    {
        void ConnectTableToDataBase();
        void UpdateHardCashAmount();
        void Back();
        void Save();
        void Print();
        void Update();
        void Undo();
        void Book();
        void PeriodCheck();
        void Export();
        void Automatic();
    }
}
