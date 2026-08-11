using System;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IMonthCalendarDialogContract
    {
        DateTime SelectedDate { get; set; }

        void SetCalendarDate(DateTime date);
        void CloseView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
