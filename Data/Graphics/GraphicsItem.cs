using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the graphics Item component used by the application.
    /// </summary>
    [Serializable()]
    public class GraphicsItem //: ISerializable
    {
        /// <summary>
        /// Defines the available print Page values used by the application.
        /// </summary>
        public enum PrintPage
        {
            All,
            First,
            Second
        }
        public int ID { get; set; }
        [XmlIgnore]
        public GraphicsItem Parent { get; set; }
        [XmlIgnore]
        public List<GraphicsItem> Items { get; set; }
        [XmlElement("Parent")]
        public int XmlParent { get; set; }
        [XmlElement("Child")]
        public List<int> XmlItems { get; set; }
        [XmlIgnore]
        public Color ForeColor { get; set; }
        [XmlElement("ForeColor")]
        public string XmlForeColor
        {
            get
            {
                return System.Drawing.ColorTranslator.ToHtml(ForeColor);
            }
            set
            {
                ForeColor = System.Drawing.ColorTranslator.FromHtml(value);
            }
        }
        [XmlIgnore]
        public Color BackColor { get; set; }
        [XmlElement("BackColor")]
        public string XmlBackColor
        {
            get
            {
                return System.Drawing.ColorTranslator.ToHtml(BackColor);
            }
            set
            {
                BackColor = System.Drawing.ColorTranslator.FromHtml(value);
            }
        }
        [XmlIgnore]
        public Color BorderColor { get; set; }
        [XmlElement("BorderColor")]
        public string XmlBorderColor
        {
            get
            {
                return System.Drawing.ColorTranslator.ToHtml(BorderColor);
            }
            set
            {
                BorderColor = System.Drawing.ColorTranslator.FromHtml(value);
            }
        }
        public RectangleF[] Rectangles { get; set; }
        public float BorderWidth { get; set; }
        public PrintPage PrintOn { get; set; }
        public DashStyle Style { get; set; }
        /// <summary>
        /// Creates a new Graphics Item instance and initializes the required state.
        /// </summary>
        public GraphicsItem()
        {
            ForeColor = Color.Black;
            BackColor = Color.White;
            BorderColor = Color.White;
            Items = new List<GraphicsItem>();
            Rectangles = new RectangleF[2];
        }
        /// <summary>
        /// Gets the x Style value from the current application state.
        /// </summary>
        protected XDashStyle GetXStyle()
        {
            return (XDashStyle)Style;
        }
        /// <summary>
        /// Gets the rectangle value from the current application state.
        /// </summary>
        public virtual RectangleF GetRectangle(int page)
        {
            if (page <= 1)
                return Rectangles[0];
            else
                return Rectangles[1];
        }
        /// <summary>
        /// Gets the graphics Rectangle value from the current application state.
        /// </summary>
        public virtual RectangleF GetGraphicsRectangle(System.Drawing.Graphics g, int page)
        {
            return GetRectangle(page);
        }
        /// <summary>
        /// Gets the graphics Rectangle value from the current application state.
        /// </summary>
        public virtual RectangleF GetGraphicsRectangle(XGraphics g, int page)
        {
            return GetRectangle(page);
        }
        /// <summary>
        /// Sets the rectangle value and updates the related application state.
        /// </summary>
        public virtual void SetRectangle(int page, RectangleF rect)
        {
            rect.Width = Math.Max(1, rect.Width);
            rect.Height = Math.Max(1, rect.Height);
            if (page == 0)
                Rectangles[0] = Rectangles[1] = rect;
            else if (page == 1)
                Rectangles[0] = rect;
            else if (page == 2)
                Rectangles[1] = rect;
        }
        /// <summary>
        /// Checks whether the visible condition is true for the current value.
        /// </summary>
        public bool IsVisible(int page)
        {
            return (PrintOn == PrintPage.All || PrintOn == PrintPage.First && page <= 1 || PrintOn == PrintPage.Second && page >= 2);
        }
        /// <summary>
        /// Runs the move operation and updates the related application state.
        /// </summary>
        public virtual void Move(int currentPage, Size size)
        {
            if (currentPage == 0)
            {
                RectangleF rect = GetRectangle(0);
                rect.Location = PointF.Add(rect.Location, size);
                Rectangles[0] = Rectangles[1] = rect;
            }
            else if (currentPage == 1)
            {
                RectangleF rect = GetRectangle(0);
                rect.Location = PointF.Add(rect.Location, size);
                Rectangles[0] = rect;
            }
            else if (currentPage == 2)
            {
                RectangleF rect = GetRectangle(2);
                rect.Location = PointF.Add(rect.Location, size);
                Rectangles[1] = rect;
            }
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public virtual void Paint(System.Drawing.Graphics g, RectangleF r) { }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public virtual RectangleF Paint(XGraphics g, RectangleF r) { return new RectangleF(); }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public virtual void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, Company company, int page, int lastPage, out bool hasMorePages) { hasMorePages = false; }
        /// <summary>
        /// Runs the paint PDF operation and updates the related application state.
        /// </summary>
        public virtual void PaintPDF(XGraphics g, int translateY, SQLBase sql, Company company, int page, int lastPage, out bool hasMorePages) { hasMorePages = false; }
        /// <summary>
        /// Runs the paint Design operation and updates the related application state.
        /// </summary>
        public void PaintDesign(System.Drawing.Graphics g, SQLBase sql, int page)
        {
            if (!IsVisible(page))
                return;
            RectangleF rect = GetRectangle(page);
            OnPaintDesign(g, sql, rect, page);
            Pen p = new Pen(Brushes.Black);
            p.DashStyle = DashStyle.Dash;
            g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
            if (Parent != null)
            {
                g.FillEllipse(Brushes.Red, new RectangleF(rect.X - 10, rect.Y + (rect.Height / 2) - 5, 10, 10));
            }
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public virtual void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page) { }
        public virtual bool PaintItem
        {
            get { return true; }
        }
        private static char[] splitNewLine = new char[] { '\n' };
        /// <summary>
        /// Runs the measure String operation and updates the related application state.
        /// </summary>
        protected XSize MeasureString(XGraphics g, string text, XFont font, XStringFormat format, RectangleF source)
        {
            string[] lines = text.Split(splitNewLine);
            XSize noteRect = new XSize(source.Width, 0);//source.Height);
            foreach (string line in lines)
            {
                XSize rect = g.MeasureString(text, font,
                    //r.Width, 
                    format);
                float value = (float)rect.Width / (float)source.Width;
                noteRect.Height += (int)(font.Height * (int)Math.Ceiling(value));
            }
            return noteRect;
        }
        /// <summary>
        /// Draws the sub Item output on the provided graphics surface.
        /// </summary>
        public bool DrawSubItem(System.Drawing.Graphics g, SQLBase sql, Company company, int page, int y, int maxHeight = int.MaxValue)
        {
            if (Items.Count == 0)
                return true;
            RectangleF allItemsRectangle = Items.First().GetGraphicsRectangle(g, page);
            foreach (GraphicsItem item in Items)
            {
                if (!item.PaintItem)
                    continue;
                allItemsRectangle = RectangleF.Union(allItemsRectangle, item.GetGraphicsRectangle(g, page));
            }
            if ((y + allItemsRectangle.Height) > maxHeight)
                return false;
            y -= (int)allItemsRectangle.Y;
            foreach (GraphicsItem item in Items)
            {
                if (!item.PaintItem)
                    continue;
                
                bool hasMorePages = false;
                item.Paint(g, y, sql, company, page, page, out hasMorePages);
            }
            return true;
        }
        /// <summary>
        /// Draws the sub Item output on the provided graphics surface.
        /// </summary>
        public bool DrawSubItem(XGraphics g, SQLBase sql, Company company, int page, int y, int maxHeight = int.MaxValue)
        {
            if (Items.Count == 0)
                return true;
            RectangleF allItemsRectangle = Items.First().GetGraphicsRectangle(g, page);
            foreach (GraphicsItem item in Items)
            {
                if (!item.PaintItem)
                    continue;
                allItemsRectangle = RectangleF.Union(allItemsRectangle, item.GetGraphicsRectangle(g, page));
            }
            if ((y + allItemsRectangle.Height) > maxHeight)
                return false;
            y -= (int)allItemsRectangle.Y;
            foreach (GraphicsItem item in Items)
            {
                if (!item.PaintItem)
                    continue;
//Rectangle itemR = item.GetRectangle(page);
//itemR.Y = (itemR.Y - r.Y) + y;
                bool hasMorePages = false;
                item.PaintPDF(g, y, sql, company, page, page, out hasMorePages);
//item.Paint(g, itemR);
            }
            return true;
        }
        /// <summary>
        /// Runs the encrypt operation and updates the related application state.
        /// </summary>
        public virtual void Encrypt(SQLBase sql) { }
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public void Connect(GraphicsItem parent)
        {
            if (parent.Items.Contains(this))
                return;
            parent.Items.Add(this);
            Parent = parent;
        }
        /// <summary>
        /// Disconnects the disconnect data source or control from the current workflow.
        /// </summary>
        public void Disconnect()
        {
            if (Parent == null)
                return;
            Parent.Items.Remove(this);
            Parent = null;
        }
        /// <summary>
        /// Runs the deserialized Method operation and updates the related application state.
        /// </summary>
        [OnDeserialized()]
        protected void DeserializedMethod(StreamingContext context)
        {
            /*
            Rectangles = new RectangleF[2];
            var r = Rectangles[0];
            Rectangles[0] = new RectangleF(r.Location, r.Size);
            r = Rectangles[1];
            Rectangles[1] = new RectangleF(r.Location, r.Size);
             * */
        }
    }
}
