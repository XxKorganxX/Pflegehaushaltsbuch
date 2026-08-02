using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Print
{
    /// <summary>
    /// Represents the print Base component used by the application.
    /// </summary>
    public class PrintBase
    {
        protected SQLBase sql;
        private Printing.LayoutEnum layer;
        private DocumentLayer document;
        /// <summary>
        /// Creates a new Print Base instance and initializes the required state.
        /// </summary>
        public PrintBase(SQLBase sql, Printing.LayoutEnum layer)
        {
            this.sql = sql;
            this.layer = layer;
            document = sql.Printing.Layouts[layer];
        }
        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        public virtual bool Print(string doumentPath, string doumentName, System.Windows.Forms.IWin32Window owner, string email = "")
        {
            return Print(doumentPath, doumentName, owner, new List<System.Data.DataRow>(), email);
        }
        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        public virtual bool Print(string doumentPath, string documentName, System.Windows.Forms.IWin32Window owner, IList<System.Data.DataRow> rows, string email = "")
        {
            List<GraphicsItem> pageItems = document.Items;
            foreach (DataTableItem item in pageItems.OfType<DataTableItem>())
                item.Data = rows.ToArray();
            PrintDocument printDoc = new PrintDocument() { };
            printDoc.DefaultPageSettings.PaperSize = new PaperSize(
                this.document.Kind.ToString(),
                this.document.Size.Width,
                this.document.Size.Height);
            printDoc.DefaultPageSettings.Landscape = document.Landscape;
            using (FormControls.printPreviewDialog previewDialog = new FormControls.printPreviewDialog(sql, printDoc, doumentPath, documentName))
            {
                previewDialog.PrintPreviewControl.Zoom = 1.5;
                previewDialog.UpdateEmail(email);
                printDoc.PrintPage += printDoc_PrintPage;
                previewDialog.PrintPDF += PrintPDF;
                if (previewDialog.ShowDialog(owner) != System.Windows.Forms.DialogResult.OK)
                    return false;
                return true;
            }
        }
        /// <summary>
        /// Prints the direct output for the current workflow.
        /// </summary>
        public virtual bool PrintDirect(string printer, string doumentName, System.Windows.Forms.IWin32Window owner, IList<System.Data.DataRow> rows, string email = "")
        {
            List<GraphicsItem> pageItems = document.Items;
            foreach (DataTableItem item in pageItems.OfType<DataTableItem>())
                item.Data = rows.ToArray();
            PrintDocument printDoc = new PrintDocument() { };
            printDoc.DefaultPageSettings.PaperSize = new PaperSize(
                this.document.Kind.ToString(),
                this.document.Size.Width,
                this.document.Size.Height);
            printDoc.DefaultPageSettings.Landscape = document.Landscape;
            printDoc.PrinterSettings.PrinterName = printer;
            printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            printDoc.OriginAtMargins = true;
            printDoc.PrintPage += printDoc_PrintPage;
            printDoc.Print();
            /*
            using (FormControls.printPreviewDialog previewDialog = new FormControls.printPreviewDialog(sql, printDoc, doumentName))
            {
                previewDialog.PrintPreviewControl.Zoom = 1.5;
                previewDialog.UpdateEmail(email);
                printDoc.PrintPage += printDoc_PrintPage;
                previewDialog.PrintPDF += PrintPDF;
                if (previewDialog.ShowDialog(owner) != System.Windows.Forms.DialogResult.OK)
                    return false;
                return true;
            }
            */
            return true;
        }
        /// <summary>
        /// Handles the print Page event for print Doc and updates the related state.
        /// </summary>
        public void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var test = e.MarginBounds;
            e.PageSettings.PrinterSettings.ToPage++;
            System.Drawing.Graphics g = e.Graphics;
            List<GraphicsItem> items = document.Items;
            foreach (var item in items)
                item.Encrypt(sql);
            foreach (var item in items)
            {
                if (item.Parent != null)
                    continue;
                bool value;
                item.Paint(g, 0, sql, e.PageSettings.PrinterSettings.ToPage, 0, out value);
                if (value)
                    e.HasMorePages = true;
            }
            if (e.HasMorePages || e.PageSettings.PrinterSettings.ToPage > 1)
            {
                document.PageNumber.Paint(g, e.PageSettings.PrinterSettings.ToPage);
            }
        }
        /// <summary>
        /// Prints the PDF output for the current workflow.
        /// </summary>
        protected void PrintPDF(Stream outStream)
        {
            PdfDocument document = new PdfDocument();
            document.Info.CreationDate = DateTime.Now;
            document.Info.Author = "MR Care Management";
            OnPrint_Pdf_Page(document);
            document.Save(outStream, true);
        }
        /// <summary>
        /// Handles the print Pdf Page lifecycle step and applies the related control behavior.
        /// </summary>
        protected virtual void OnPrint_Pdf_Page(PdfDocument pdfDocument)
        {
            bool HasMorePages = false;
            PdfPage page = pdfDocument.AddPage();
            Size paperSize = this.document.Size;
            page.Width = new XUnit((double)paperSize.Width / 96.0, XGraphicsUnit.Inch);
            page.Height = new XUnit((double)paperSize.Height / 96.0, XGraphicsUnit.Inch);
            
            if (this.document.Landscape)
                page.Orientation = PageOrientation.Landscape;
            else
                page.Orientation = PageOrientation.Portrait;
            XGraphics g = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Presentation);
            
            DocumentLayer document = sql.Printing.Layouts[layer];
            List<GraphicsItem> items = document.Items;
            foreach (var item in items)
                item.Encrypt(sql);
            foreach (var item in items)
            {
                if (item.Parent != null)
                    continue;
                bool value;
                item.PaintPDF(g, 0, sql, pdfDocument.PageCount, 0, out value);
                if (value)
                    HasMorePages = true;
            }
            if (HasMorePages || pdfDocument.PageCount > 1)
            {
                document.PageNumber.Paint(g, pdfDocument.PageCount);
            }
            if (HasMorePages)
            {
                OnPrint_Pdf_Page(pdfDocument);
            }
        }
    }
}
