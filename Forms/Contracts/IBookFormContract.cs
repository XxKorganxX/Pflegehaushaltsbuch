using System;
using System.Data;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IBookFormContract
    {
        string DefaultSortColumn { get; }
        string NumberColumnName { get; }
        string DateColumnName { get; }
        string BookCategoryColumnName { get; }
        string AmountColumnName { get; }
        DateTime FromDate { get; }
        DateTime ToDate { get; }
        bool PeriodChecked { get; }
        void ShowPrintBooksDialog(DataTable table, int clientID, string totalAmount, DateTime from, DateTime to);
        bool ShowClientBookDialog(string clientName, string clientID, out ClientBookingInput input);
        void SetPeriodControlsVisible(bool visible);
        void ShowClientsForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName);
        void SetClientTable(DataTable clientTable);
        void SetBookTable(DataTable table);
        void EndEditAccount();
        void PrintQuittance(string clientName, DataRow[] dataRows);
    }

    public class ClientBookingInput
    {
        public decimal Amount { get; set; }
        public string BookText { get; set; }
        public DateTime BookingDate { get; set; }
        public string ClientName { get; set; }
        public int ClientID { get; set; }
        public SQLBase.BookingTo BookingTarget { get; set; }
        public SQLBase.BookCategory BookingCategory { get; set; }
        public bool PrintQuittance { get; set; }
    }
}
