using System;
using System.Collections.Generic;
using System.Data;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICashFormContract
    {
        string DefaultSortColumn { get; }
        string CurrentSortColumn { get; }
        DateTime FromDate { get; }
        DateTime ToDate { get; }
        bool PeriodChecked { get; }
        string TotalAmountText { get; set; }
        string HardCashAmountText { get; set; }
        bool ShowCashBookDialog(out CashBookingInput input);
        void SetPeriodControlsVisible(bool visible);
        void SetHardCashAmountWarning(bool warning);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        void SetTable(DataTable hardCashTable);
        void SetCashViewTable(DataTable table);
        void EndEditHardCash();
        void SuspendBindingHardCash();
        void ResumeBindingHardCash();
        void Print(DataRow[] rows);
        void PrintQuittance(string clientName, List<DataRow> currentBooks);
    }

    public class CashBookingInput
    {
        public string BookText { get; set; }
        public decimal Amount { get; set; }
        public DateTime BookingDate { get; set; }
        public SQLBase.BookingTo BookingTarget { get; set; }
        public SQLBase.BookCategory BookingCategory { get; set; }
        public bool PrintQuittance { get; set; }
        public IEnumerable<ID_Client_Data> SelectedClients { get; set; }
    }
}
