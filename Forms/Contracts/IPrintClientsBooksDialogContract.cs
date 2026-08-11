using Pflegehaushaltsbuch.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IPrintClientsBooksDialogContract
    {
        string SelectedPrinter { get; }
        bool HasSelectedPrinter { get; }
        bool HasSelectedClients { get; }
        IEnumerable<ID_Client_Data> SelectedClients { get; }
        DateTime SelectedDate { get; }
        string StatementNote { get; }
        void BindPrinters(IEnumerable<string> printerNames, string selectedPrinter);
        void BindClients(IEnumerable<ID_Client_Data> clients);
        void PrintClientBooks(string printerName, string fileName, DataRow[] rows, string email);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
