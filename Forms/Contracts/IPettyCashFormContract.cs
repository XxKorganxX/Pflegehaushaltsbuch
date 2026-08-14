using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IPettyCashFormContract
    {
        DateTime FromDate { get; }
        DateTime ToDate { get; }
        bool PeriodChecked { get; }
        IEnumerable<DataRow> OfficeCashRows { get; }
        void SetTotalAmount(string totalAmount);
        void BindOfficeCash(DataTable table);
        void SetGridUpdateVisible(bool visible);
        void SetBookButtonsEnabled(bool canBook, bool canCancel);
        void SetPeriodDateRange(DateTime fromDate, DateTime toDate);
        void SetPeriodControlsVisible(bool visible);
        void PrintOfficeCash(IEnumerable<DataRow> rows);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        bool ShowCashOfficeBookDialog(out CashOfficeBookingInput input);
    }
}

namespace Pflegehaushaltsbuch.Forms
{
    public class CashOfficeBookingInput
    {
        public DateTime BookingDate { get; set; }
        public string BookText { get; set; }
        public decimal Amount { get; set; }
        public SQLBase.BookCategory BookingCategory { get; set; }
        public int Account { get; set; }
    }
}
