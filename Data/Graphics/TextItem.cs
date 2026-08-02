using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the text Item component used by the application.
    /// </summary>
    [Serializable()]
    public class TextItem : FontItem
    {
        /// <summary>
        /// Creates a new Text Item instance and initializes the required state.
        /// </summary>
        public TextItem() { Text = string.Empty; }
        [NonSerialized]
        protected string plainText;
        public virtual string Text { get; set; }
        public bool CalculateTextHeight { get; set; }
        public StringAlignment HorizontalAlignment { get; set; }
        public StringAlignment VerticalAlignment { get; set; }
        /// <summary>
        /// Creates a new Text Item instance and initializes the required state.
        /// </summary>
        public TextItem(Rectangle rect)
        {
            Rectangles[0] = Rectangles[1] = rect;
        }
        /// <summary>
        /// Runs the encrypt operation and updates the related application state.
        /// </summary>
        public override void Encrypt(SQLBase sql)
        {
            plainText = Text;
            foreach (var pair in sql.Printing.Variables)
            {
                if(pair.Value == null)
                    plainText = plainText.Replace(pair.Key, "");
                else
                    plainText = plainText.Replace(pair.Key, pair.Value.ToString());
            }
        }
        public override bool PaintItem
        {
            get
            {
                return !string.IsNullOrWhiteSpace(plainText);
            }
        }
        /// <summary>
        /// Gets the graphics Rectangle value from the current application state.
        /// </summary>
        public override RectangleF GetGraphicsRectangle(XGraphics g, int page)
        {
            RectangleF rect = GetRectangle(page);
            if (CalculateTextHeight)
            {
                XStringFormat stringFormat = new XStringFormat();
                stringFormat.Alignment = HorizontalAlignment == StringAlignment.Near ?
                    XStringAlignment.Near : HorizontalAlignment == StringAlignment.Center ?
                    XStringAlignment.Center : XStringAlignment.Far;
                stringFormat.LineAlignment = VerticalAlignment == StringAlignment.Near ?
                    XLineAlignment.Near : VerticalAlignment == StringAlignment.Center ?
                    XLineAlignment.Center : XLineAlignment.Far;
                XFont xfont = GetXFont(g);
                XSize noteRect = g.MeasureString(plainText, xfont, 
                    //rect.Width, 
                    stringFormat);
                rect.Height = (int)noteRect.Height;
            }
            return rect;
        }
        /// <summary>
        /// Gets the graphics Rectangle value from the current application state.
        /// </summary>
        public override RectangleF GetGraphicsRectangle(System.Drawing.Graphics g, int page)
        {
            RectangleF rect = base.GetGraphicsRectangle(g, page);
            if (CalculateTextHeight)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = HorizontalAlignment;
                stringFormat.LineAlignment = VerticalAlignment;
                SizeF size = g.MeasureString(plainText, Font, (int)rect.Width, stringFormat);
                rect.Height = (int)size.Height;
            }
            return rect;
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, RectangleF r )
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = HorizontalAlignment;
            stringFormat.LineAlignment = VerticalAlignment;
            /*
            if(CalculateTextHeight)
            {
                SizeF noteRect = g.MeasureString(plainText, Font, r.Width, stringFormat);
                r.Height = (int)noteRect.Height;
            }
            */
            if (BackColor != Color.White)
                g.FillRectangle(new SolidBrush(BackColor), r);
            g.DrawString(plainText, Font, new SolidBrush(ForeColor), r, stringFormat);
            if (BorderWidth > 0)
                g.DrawRectangle(new Pen(new SolidBrush(BorderColor)) { Width = BorderWidth, DashStyle = Style }, 
                    r.X,r.Y,r.Width,r.Height);
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (!IsVisible(page))
                return;
            RectangleF currentRect = GetGraphicsRectangle(g, page);
            currentRect.Y = currentRect.Y + translateY;
            if (translateY != 0)
            { 
            }
            Paint(g, currentRect);
            DrawSubItem(g, sql, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override RectangleF Paint(XGraphics g,  RectangleF source)
        {
            XStringFormat stringFormat = GetStringFormat();
            XFont font = GetXFont(g);
            RectangleF r = source;
            if (CalculateTextHeight)
            {
                XSize noteRect = MeasureString(g, plainText, font,
                    //r.Width, 
                    stringFormat, r);
                //float value = (float)noteRect.Width / (float)r.Width;
                r.Height = (int)noteRect.Height;// (int)(font.Height * (int)Math.Ceiling(value));
                //double height = noteRect.Height + (noteRect.Height * Math.Floor(noteRect.Width / r.Width));
                /*
                double height = noteRect.Height + (noteRect.Height * Math.Floor(noteRect.Width / r.Width));
                r.Height += (int)height;
                 */
            }
            if (BackColor != Color.White)
                g.DrawRectangle(new XSolidBrush(XColor.FromArgb(BackColor.R, BackColor.G, BackColor.B)), r);
            //if (r.Height > font.Height*2)
            //{
                XTextFormatter tf = new XTextFormatter(g);
 
            if(stringFormat.Alignment == XStringAlignment.Center)
                tf.Alignment = XParagraphAlignment.Center;
            else if (stringFormat.Alignment == XStringAlignment.Far)
                tf.Alignment = XParagraphAlignment.Right;
            if (stringFormat.LineAlignment == XLineAlignment.Center)
            {
                if (!CalculateTextHeight)
                {
                    XSize size = MeasureString(g, plainText, font, stringFormat, r);
                    if(r.Height > size.Height)
                    {
                        r.Y += (r.Height - (int)size.Height)/2;
                    }
                    
                }
            }
            else if (stringFormat.LineAlignment == XLineAlignment.Far)
            {
                if (!CalculateTextHeight)
                {
                    XSize size = MeasureString(g, plainText, font, stringFormat, r);
                    if (r.Height > size.Height)
                    {
                        r.Y += (r.Height - (int)size.Height);
                    }
                }
            }
            if(!font.Underline)
                tf.DrawString(plainText, font, new XSolidBrush(XColor.FromArgb(ForeColor.R, ForeColor.G, ForeColor.B)), r);//, stringFormat);
            else
                g.DrawString(plainText, font, new XSolidBrush(XColor.FromArgb(ForeColor.R, ForeColor.G, ForeColor.B)), r,
                    new XStringFormat() { Alignment = stringFormat.Alignment, LineAlignment = XLineAlignment.Near});
            if (BorderWidth > 0)
                g.DrawRectangle(new XPen(XColor.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B), BorderWidth){ DashStyle = GetXStyle() }, source);
            return r;
        }
        /// <summary>
        /// Runs the paint PDF operation and updates the related application state.
        /// </summary>
        public override void PaintPDF(XGraphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (Parent != null && translateY == 0)
                return;
            if (!IsVisible(page))
                return;
            RectangleF currentRect = GetGraphicsRectangle(g, page);
            currentRect.Y = currentRect.Y + translateY;
            
            currentRect = Paint(g, currentRect);
            DrawSubItem(g, sql, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = HorizontalAlignment;
            stringFormat.LineAlignment = VerticalAlignment;
            if (BackColor != Color.White)
                g.FillRectangle(new SolidBrush(BackColor), rect);
            g.DrawString(Text, Font, new SolidBrush(ForeColor), rect, stringFormat);
            if (BorderWidth > 0)
                g.DrawRectangle(new Pen(new SolidBrush(BorderColor)) { Width = BorderWidth, DashStyle = Style }, 
                    rect.X,rect.Y,rect.Width,rect.Height);
        }
        /// <summary>
        /// Gets the string Format value from the current application state.
        /// </summary>
        public XStringFormat GetStringFormat()
        {
            XStringFormat stringFormat = new XStringFormat();
            stringFormat.Alignment = HorizontalAlignment == StringAlignment.Near ?
                XStringAlignment.Near : HorizontalAlignment == StringAlignment.Center ?
                XStringAlignment.Center : XStringAlignment.Far;
            stringFormat.LineAlignment = VerticalAlignment == StringAlignment.Near ?
                XLineAlignment.Near : VerticalAlignment == StringAlignment.Center ?
                XLineAlignment.Center : XLineAlignment.Far;
            return stringFormat;
        }
        /// <summary>
        /// Gets the string Format value from the current application state.
        /// </summary>
        public static XStringFormat GetStringFormat(StringAlignment horizontal, StringAlignment vertical)
        {
            XStringFormat stringFormat = new XStringFormat();
            stringFormat.Alignment = horizontal == StringAlignment.Near ?
                XStringAlignment.Near : horizontal == StringAlignment.Center ?
                XStringAlignment.Center : XStringAlignment.Far;
            stringFormat.LineAlignment = vertical == StringAlignment.Near ?
                XLineAlignment.Near : vertical == StringAlignment.Center ?
                XLineAlignment.Center : XLineAlignment.Far;
            return stringFormat;
        }
    }
}
