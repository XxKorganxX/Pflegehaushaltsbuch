using System;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IClientBookDialogContract
    {
        string BookText { get; set; }
        decimal Amount { get; set; }
        int BookTo { get; set; }
        int BookCategory { get; set; }
        DateTime BookingDate { get; }
        void AddBookingCategory(string text);
        void AddBookingTarget(string text);
        void AddClient(string clientName, string clientID);
        void SelectClient(string clientName, string clientID);
        void BindFields();
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
