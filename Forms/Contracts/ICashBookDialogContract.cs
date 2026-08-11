using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface ICashBookDialogContract
    {
        string BookText { get; set; }
        decimal Amount { get; set; }
        int BookTo { get; set; }
        int BookCategory { get; set; }
        DateTime BookingDate { get; }
        SQLBase.BookingTo BookingTarget { get; }
        IEnumerable<ID_Client_Data> SelectedClients { get; }
        string ClientLookupText { get; }
        void AddBookingCategory(string text);
        void AddBookingTarget(string text);
        void BindFields();
        void ClearClients();
        void AddClient(ID_Client_Data client);
        void AddClientLookupName(string name);
        void SetClientSelection(ID_Client_Data client);
        void ToggleSelectedClientChecked();
        void SetClientSelectionVisible(bool visible);
        void SetOkEnabled(bool enabled);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
