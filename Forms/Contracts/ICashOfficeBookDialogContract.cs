using System;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface ICashOfficeBookDialogContract
    {
        decimal Amount { get; set; }
        int Account { get; set; }
        string BookText { get; set; }
        DateTime BookingDate { get; }
        void BindFields();
        void SetBookingCategoryIndex(int index);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
