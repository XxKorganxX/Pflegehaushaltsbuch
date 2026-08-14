using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class PrintPreviewDialogPresenter
    {
        private readonly IPrintPreviewDialogContract view;
        private readonly SqlSession session;
        private readonly PrintDocument document;
        private readonly string documentPath;

        public PrintPreviewDialogPresenter(IPrintPreviewDialogContract view, SqlSession session, PrintDocument document, string documentPath, string documentName)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
            this.document = document;
            this.documentPath = documentPath;
            view.DocumentName = documentName;
        }

        public virtual void Initialize()
        {
            view.Document = document;
            view.BindPrinterDropDown();

            foreach (string name in PrinterSettings.InstalledPrinters)
            {
                view.AddPrinter(name);
            }

            view.SelectDefaultPrinter();
            view.MovePreviewControlIntoPanel();
            view.BindZoomMouseWheel();
            view.BindPreviewMouseWheel();
            view.BindCopies(document);
        }

        public virtual void ScrollPreview(int delta)
        {
            view.ScrollPreview(delta);
        }

        public virtual void Shown()
        {
            if (Program.DesignMode)
            {
                return;
            }

            view.ZoomText = view.PreviewZoom.ToString("p0");
            view.BindDocumentPrintEvents(view.Document);
        }

        public virtual void BeginPrint()
        {
            view.Document.PrinterSettings.ToPage = 0;
        }

        public virtual void EndPrint()
        {
            view.Pages = view.Document.PrinterSettings.ToPage;
            view.RowText = view.Pages.ToString();
        }

        public virtual void ChangePageByWheel(int delta)
        {
            int value;
            if (!int.TryParse(view.PageText, out value))
            {
                return;
            }

            value += Math.Min(1, Math.Max(-1, delta));
            value = Math.Max(0, value);
            view.PageText = value.ToString();
        }

        public virtual void ChangeZoomByWheel(int delta)
        {
            view.PreviewZoom = Math.Max(0.01, view.PreviewZoom + ((double)Math.Min(1, Math.Max(-1, delta))) * 0.05);
            view.ZoomText = view.PreviewZoom.ToString("p0");
        }

        public virtual void PrinterDropDown()
        {
            if (Program.DesignMode)
            {
                return;
            }

            view.MeasurePrinterDropDown();
        }

        public virtual void ZoomTextChanged()
        {
            if (!view.ZoomFocused)
            {
                return;
            }

            double value;
            string zoomText = view.ZoomText
                .Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, string.Empty)
                .Replace("%", string.Empty)
                .Trim();

            if (double.TryParse(zoomText, NumberStyles.Float, CultureInfo.CurrentCulture, out value) ||
                double.TryParse(zoomText, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                view.PreviewZoom = Math.Max(0.01, value * 0.01);
            }
        }

        public virtual void SavePdf()
        {
            string filename = CreatePdfFileName();
            using (FileStream fs = File.Create(filename))
            {
                view.RaisePrintPdf(fs);
            }

            view.ShowDocumentSaved(filename);
        }

        public virtual void PrinterSelected()
        {
            if (Program.DesignMode)
            {
                return;
            }

            if (view.Document != null)
            {
                view.Document.PrinterSettings.PrinterName = view.SelectedPrinter;
            }
        }

        public virtual void Print()
        {
            view.PrintDocument();
        }

        public virtual void PageTextChanged()
        {
            if (Program.DesignMode)
            {
                return;
            }

            int value;
            if (!int.TryParse(view.PageText, out value))
            {
                return;
            }

            if (value < 0)
            {
                view.PageText = "1";
            }
        }

        public virtual void RowTextChanged()
        {
            if (Program.DesignMode)
            {
                return;
            }

            int value;
            if (!int.TryParse(view.RowText, out value))
            {
                return;
            }

            if (value < 1)
            {
                view.RowText = "1";
                return;
            }

            view.PreviewRows = value;
        }

        public virtual void AutoZoom()
        {
            view.PreviewAutoZoom = true;
        }

        public virtual void ZoomValidated()
        {
            view.ZoomText = view.PreviewZoom.ToString("p0");
        }

        public virtual void ShowPrinterSettings()
        {
            view.ApplyPrinterSettings();
        }

        private string CreatePdfFileName()
        {
            string path = Path.Combine(Settings.Default.documentPath, documentPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            int sufixNumber = 1;
            string sufix = string.Empty;
            string filename =
                view.DocumentName +
                "_" +
                DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            filename = Path.Combine(path, filename);

            while (File.Exists(filename + sufix + ".pdf"))
            {
                sufixNumber++;
                sufix = "(" + sufixNumber + ")";
            }

            return filename + sufix + ".pdf";
        }
    }
}
