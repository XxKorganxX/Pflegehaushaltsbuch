using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IBankFormContract
    {
        string DefaultSortColumn { get; }
        string CurrentSortColumn { get; }
        DateTime FromDate { get; }
        DateTime ToDate { get; }
        bool PeriodChecked { get; }
        void SetTotalAmount(string totalAmount);
        void BindBank(DataTable table);
        bool ShowBankBookDialog(out BankBookingInput input);
        void SetPeriodControlsVisible(bool visible);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        void PrintQuittance(string clientName, IEnumerable<DataRow> currentBooks);
        void PrintBank(DataRow[] rows);
    }

    public class BankBookingInput
    {
        public decimal Amount { get; set; }
        public string BookText { get; set; }
        public DateTime BookingDate { get; set; }
        public SQLBase.BookingTo BookingTarget { get; set; }
        public SQLBase.BookCategory BookingCategory { get; set; }
        public bool PrintQuittance { get; set; }
        public IEnumerable<ID_Client_Data> SelectedClients { get; set; }
    }
}
