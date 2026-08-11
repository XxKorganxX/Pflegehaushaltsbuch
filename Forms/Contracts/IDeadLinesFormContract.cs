using System;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IDeadLinesFormContract
    {
        DateTime CurrentMonth { get; }
        int ClientID { get; }
        void ShowCalendar(DeadlineCalendar calendar);
        void ShowClientName(string clientName);
        void ClearClientName();
        bool ShowExportDialog(string fileName, out string selectedFileName);
        bool ShowCreateDeadlineDialog(DateTime date, string description, out DeadlineInput input);
        void ShowDatabaseChanged();
        void ShowClientsForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public class DeadlineCalendar
    {
        public DataTable Table { get; set; }
        public int StartCell { get; set; }
        public int MaxDaysInMonth { get; set; }
        public int MaxRows { get; set; }
    }

    public class DeadlineInput
    {
        public string Description { get; set; }
        public bool ForAllMonths { get; set; }
    }
}
