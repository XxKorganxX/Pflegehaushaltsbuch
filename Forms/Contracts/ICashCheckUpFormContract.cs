namespace Pflegehaushaltsbuch.Forms
{
    public interface ICashCheckUpFormContract
    {
        void ShowCashAudit(CashCheckUpSummary summary);
        void PrintCashAudit();
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public class CashCheckUpSummary
    {
        public decimal ClientsActive { get; set; }
        public decimal ClientsInactive { get; set; }
        public decimal ClientsHistory { get; set; }
        public decimal ClientsTotal { get; set; }
        public decimal AssistantsAmount { get; set; }
        public decimal BankSaldo { get; set; }
        public decimal CalculatedSaldo { get; set; }
        public decimal DifferenceAmount { get; set; }
        public decimal CashHolding { get; set; }
        public decimal HardCashAmount { get; set; }
    }
}
