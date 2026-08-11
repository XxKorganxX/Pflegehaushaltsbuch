using System;
using System.Drawing;
using System.Linq;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the rectangle Item component used by the application.
    /// </summary>
    public class RectangleItem : GraphicsItem
    {
        /// <summary>
        /// Creates a new Rectangle Item instance and initializes the required state.
        /// </summary>
        public RectangleItem() { }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, RectangleF r)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override RectangleF Paint(PdfSharp.Drawing.XGraphics g, RectangleF r)
        {
            throw new NotImplementedException();
        }
    }
}
