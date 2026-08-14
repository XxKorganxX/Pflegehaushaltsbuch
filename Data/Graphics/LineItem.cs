using PdfSharp.Drawing;
using System;
using System.Drawing;
using System.Linq;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the line Item component used by the application.
    /// </summary>
    [Serializable]
    public class LineItem : GraphicsItem
    {
        public float Width { get; set; }
        /// <summary>
        /// Creates a new Line Item instance and initializes the required state.
        /// </summary>
        public LineItem() { }
        /// <summary>
        /// Creates a new Line Item instance and initializes the required state.
        /// </summary>
        public LineItem(Rectangle rect)
        {
            Rectangles[0] = Rectangles[1] = rect;
            Width = 5.0f;
            BorderWidth = 1.0f;
        }
        /// <summary>
        /// Sets the rectangle value and updates the related application state.
        /// </summary>
        public override void SetRectangle(int page, RectangleF rect)
        {
            rect.Width = Math.Max(0, rect.Width);
            rect.Height = Math.Max(0, rect.Height);
            if (rect.Width == 0 && rect.Height == 0)
                return;
            if (page == 0)
                Rectangles[0] = Rectangles[1] = rect;
            else if (page == 1)
                Rectangles[0] = rect;
            else if (page == 2)
                Rectangles[1] = rect;
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, RectangleF r )
        {
            Pen linePen = new Pen(ForeColor);
            linePen.Width = BorderWidth;
            linePen.DashStyle = Style;
            g.DrawLine(linePen, r.X, r.Y, r.Right, r.Bottom);
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override RectangleF Paint(XGraphics g, RectangleF r)
        {
            XPen linePen = new XPen(XColor.FromArgb(ForeColor.R, ForeColor.G, ForeColor.B), BorderWidth);
            linePen.DashStyle = GetXStyle();
            g.DrawLine(linePen, r.X, r.Y, r.Right, r.Bottom);
            return r;
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, Company company, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if(!IsVisible(page))
                return;
            RectangleF currentRect = GetRectangle(page);
            currentRect.Y = currentRect.Y + translateY;
            Paint(g, currentRect);
            DrawSubItem(g, sql, company, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Runs the paint PDF operation and updates the related application state.
        /// </summary>
        public override void PaintPDF(XGraphics g, int translateY, SQLBase sql, Company company, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (Parent != null && translateY == 0)
                return;
            if (!IsVisible(page))
                return;
            RectangleF currentRect = GetGraphicsRectangle(g, page);
            currentRect.Y = currentRect.Y + translateY;
            Paint(g, currentRect);
            DrawSubItem(g, sql, company, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page)
        {
            Paint(g, rect);
        }
    }
}
