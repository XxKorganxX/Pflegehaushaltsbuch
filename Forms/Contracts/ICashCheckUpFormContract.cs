namespace Pflegehaushaltsbuch.Forms
{
    public interface ICashCheckUpFormContract
    {
        void Back();
        void ConnectTableToDataBase();
        void UpdateCashHolding();
        void GetHardCashAmount();
        void Print();
    }
}
