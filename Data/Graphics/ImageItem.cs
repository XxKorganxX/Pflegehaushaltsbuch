using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the image Item component used by the application.
    /// </summary>
    [Serializable]
    public class ImageItem : GraphicsItem
    {
        [NonSerialized]
        private Image printImage;
        [XmlIgnore]
        public Image Image { get; set; }
        [XmlElement("Image")]
        public byte[] XmlImage
        {
            get
            {
                if (Image == null)
                    return null;
                MemoryStream ms = new MemoryStream();
                Image.Save(ms, Image.RawFormat);
                return ms.ToArray();
            }
            set
            {
                if (value == null)
                {
                    Image = null;
                    return;
                }
                MemoryStream ms = new MemoryStream(value);
                Image = Image.FromStream(ms);
            }
        }
        public StringAlignment HorizontalAlignment { get; set; }
        public string Hyperlink { get; set; }
        /// <summary>
        /// Creates a new Image Item instance and initializes the required state.
        /// </summary>
        public ImageItem() { }
        /// <summary>
        /// Creates a new Image Item instance and initializes the required state.
        /// </summary>
        public ImageItem(RectangleF rect)
        {
            Rectangles[0] = Rectangles[1] = rect;
        }
        /// <summary>
        /// Runs the encrypt operation and updates the related application state.
        /// </summary>
        public override void Encrypt(SQLBase sql)
        {
            printImage = Image;
            if (!string.IsNullOrWhiteSpace(Hyperlink))
            {
                object image;
                if (sql.Printing.Variables.TryGetValue(Hyperlink, out image))
                {
                    object outValue;
                    if (sql.Printing.Variables.TryGetValue("<company_logo_alignment>", out outValue))
                    {
                        TextFormatFlags flags = (TextFormatFlags)outValue;
                        if (flags == TextFormatFlags.HorizontalCenter)
                            HorizontalAlignment = StringAlignment.Center;
                        else if (flags == TextFormatFlags.Right)
                            HorizontalAlignment = StringAlignment.Far;
                        else
                            HorizontalAlignment = StringAlignment.Near;
                    }
                    printImage = image as Image;
                }
            }
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, RectangleF r)
        {
            //System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            //attr.SetColorKey(Color.FromArgb(200,200,200), Color.White);// ClearColorKey();
            //attr.SetGamma(1.5f);
            if (BackColor != Color.White)
                g.FillRectangle(new SolidBrush(BackColor), r);
            RectangleF rectImg = r;
            if (printImage != null)
            {
                float w = (float)printImage.Width;
                float h = (float)printImage.Height;
                float scalar = (float)r.Height / (float)h;
                w *= scalar;
                h *= scalar;
                if (w > r.Width)
                {
                    scalar = (float)r.Width / (float)w;
                    w *= scalar;
                    h *= scalar;
                }
                RectangleF destRect = new RectangleF(r.Location, new SizeF(w, h ));
                float gap = r.Width - w;
                if (HorizontalAlignment == StringAlignment.Far)
                    destRect.X += gap;
                else if (HorizontalAlignment == StringAlignment.Center)
                    destRect.X += gap * 0.5f;
                g.DrawImage(printImage, destRect);
            }
            if (BorderWidth > 0)
                g.DrawRectangle(new Pen(new SolidBrush(BorderColor), BorderWidth) { DashStyle = Style }, 
                    r.X, r.Y, r.Width, r.Height);
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override RectangleF Paint(XGraphics g, RectangleF r)
        {
            if (BackColor != Color.White)
                g.DrawRectangle(new XSolidBrush(XColor.FromArgb(BackColor.R,BackColor.G,BackColor.B)), r);
            if (printImage != null)
            {
                float w = (float)printImage.Width;
                float h = (float)printImage.Height;
                float scalar = (float)r.Height / (float)h;
                w *= scalar;
                h *= scalar;
                if (w > r.Width)
                {
                    scalar = (float)r.Width / (float)w;
                    w *= scalar;
                    h *= scalar;
                }
                RectangleF destRect = new RectangleF(r.Location, new SizeF(w, h));
                float gap = r.Width - w;
                if (HorizontalAlignment == StringAlignment.Far)
                    destRect.X += gap;
                else if (HorizontalAlignment == StringAlignment.Center)
                    destRect.X += gap * 0.5f;
                MemoryStream ms = new MemoryStream();
                {
                    printImage.Save(ms, printImage.RawFormat);
                    XImage image = XImage.FromStream(ms);
                    g.DrawImage(image, destRect);
                }
            }
            if (BorderWidth > 0)
                g.DrawRectangle(new XPen(XColor.FromArgb(ForeColor.R, ForeColor.G, ForeColor.B), BorderWidth) { DashStyle = GetXStyle() }, r);
            return r;
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (!IsVisible(page))
                return;
            RectangleF currentRect = GetRectangle(page);
            currentRect.Y = currentRect.Y + translateY;
            Paint(g, currentRect);
            DrawSubItem(g, sql, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Runs the paint PDF operation and updates the related application state.
        /// </summary>
        public override void PaintPDF(XGraphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (Parent != null)
                return;
            if (!IsVisible(page))
                return;
            RectangleF currentRect = GetGraphicsRectangle(g, page);
            currentRect.Y = currentRect.Y + translateY;
            Paint(g, currentRect);
            DrawSubItem(g, sql, page, (int)currentRect.Bottom);
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page)
        {
            Encrypt(sql);
            Paint(g, rect);
        }
    }
}
