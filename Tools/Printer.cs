using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Tools
{
    /// <summary>
    /// Represents the printer component used by the application.
    /// </summary>
    sealed class Printer
    {
        /// <summary>
        /// Draws the row Table Cells output on the provided graphics surface.
        /// </summary>
        public static void DrawRowTableCells(Graphics g, int[] column, Pen p, int y, int height)
        {
            for (int i = 0; i < column.Length - 1; i++)
            {
                int width = column[i + 1] - column[i];
                g.DrawRectangle(p, new Rectangle(column[i], y, width, height));
            }
        }
        //sql.Company.Footer
        /// <summary>
        /// Prints the footer output for the current workflow.
        /// </summary>
        public static void PrintFooter(PrintPageEventArgs e, Graphics g, Font font, string text)
        {
            RectangleF pageRect = e.PageSettings.PrintableArea;
            Font smalText = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
            //Fusszeile drucken
            if (e.PageSettings.PrinterSettings.ToPage == 1)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                g.DrawLine(new System.Drawing.Pen(Brushes.Orange, 1.0f), 80.0f, 1000.0f, 770.0f, 1000.0f);
                g.DrawString(text, smalText, Brushes.Gray, new System.Drawing.RectangleF(70.0f, 1020.0f, 700.0f, 100.0f), format);
            }
            //Seitenzahl drucken
            if (e.HasMorePages || e.PageSettings.PrinterSettings.ToPage > 1)
            {
                g.DrawString("- " + e.PageSettings.PrinterSettings.ToPage + " -", font, Brushes.Black, new RectangleF(pageRect.Width - 40, pageRect.Height - 25, 25, 25));
            }
        }
    }
}
