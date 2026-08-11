using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.FormControls
{
    public interface IPrintPreviewDialogContract
    {
        string DocumentName { get; set; }
        int Pages { get; set; }
        PrintDocument Document { get; set; }
        PrintPreviewControl PreviewControl { get; }
        IntPtr Handle { get; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        string SmtpPassword { get; set; }
        string FromEmail { get; set; }
        string ToEmail { get; set; }
        string SelectedPrinter { get; set; }
        int PrinterCount { get; }
        string ZoomText { get; set; }
        bool ZoomFocused { get; }
        string PageText { get; set; }
        string RowText { get; set; }
        double PreviewZoom { get; set; }
        int PreviewRows { get; set; }
        bool PreviewAutoZoom { get; set; }
        bool EmailSettingsVisible { get; set; }

        void AddPrinter(string printerName);
        void SelectDefaultPrinter();
        void MovePreviewControlIntoPanel();
        void BindDocumentPrintEvents(PrintDocument document);
        void BindPrinterDropDown();
        void BindPreviewMouseWheel();
        void BindZoomMouseWheel();
        void BindCopies(PrintDocument document);
        void ScrollPreview(int delta);
        void MeasurePrinterDropDown();
        void PrintDocument();
        void RaisePrintPdf(Stream outStream);
        void ShowDocumentSaved(string filename);
        void ShowEmailSent(MailMessage mail);
        void ShowEmailFailed(MailMessage mail);
        void ApplyPrinterSettings();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
