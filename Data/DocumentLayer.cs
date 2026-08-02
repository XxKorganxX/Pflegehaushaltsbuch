using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Data.Graphics;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the document Layer component used by the application.
    /// </summary>
    [Serializable()]
    public class DocumentLayer : ISerializable
    {
        public List<GraphicsItem> Items { get; set; }
        [XmlElement("Paging")]
        public DocumentPageNumber PageNumber { get; set; }
        public bool Landscape { get; set; }
        public PaperKind Kind { get; set; }
        public Size Size { get; set; }
        /// <summary>
        /// Represents the document Page Number component used by the application.
        /// </summary>
        [Serializable()]
        public class DocumentPageNumber : ISerializable
        {
            public bool Print { get; set; }
            public RectangleF Rect { get; set; }
            public StringAlignment Horizontal { get; set; }
            public StringAlignment Vertical { get; set; }
            [XmlIgnore]
            public Font Font { get; set; }
            [XmlElement("Font")]
            public string XmlFont
            {
                get { return new FontConverter().ConvertToString(Font); }
                set { Font = new FontConverter().ConvertFromString(value) as Font; }
            }
            public string Text { get; set; }
            public Margins Margins { get; set; }
            /// <summary>
            /// Creates a new Document Page Number instance and initializes the required state.
            /// </summary>
            public DocumentPageNumber()
            {
                Horizontal = StringAlignment.Far;
                Vertical = StringAlignment.Far;
                Rect = new RectangleF(50, 50, 715, 1050);
                Font = new Font("Arial", 11.0f, FontStyle.Regular, GraphicsUnit.World);
                Text = "- {0} -";
                Margins = new Margins(50, 50, 50, 50);
            }
            /// <summary>
            /// Runs the paint operation and updates the related application state.
            /// </summary>
            public void Paint(System.Drawing.Graphics g, int currentPage)
            {
                if (!Print)
                    return;
                Font font = Font;
                string text = string.Format(Text, currentPage);
                g.DrawString(text,
                    font,
                    Brushes.Black,
                    Rect,
                    new StringFormat() { Alignment = Horizontal, LineAlignment = Vertical });
            }
            /// <summary>
            /// Runs the paint operation and updates the related application state.
            /// </summary>
            public void Paint(XGraphics g, int currentPage)
            {
                if (!Print)
                    return;
               
                XFont xfont = new XFont(Font, new XPdfFontOptions(PdfSharp.Pdf.PdfFontEmbedding.Always));
                string text = string.Format(Text, currentPage);
                g.DrawString(text,
                    xfont,
                    XBrushes.Black,
                    new XRect(Rect),
                    TextItem.GetStringFormat(Horizontal, Vertical)
                );
            }
            /// <summary>
            /// Creates a new Document Page Number instance and initializes the required state.
            /// </summary>
            protected DocumentPageNumber(SerializationInfo info, StreamingContext context)
            {
                foreach (SerializationEntry entry in info)
                {
                    switch (entry.Name)
                    {
                        case "Print":
                            Print = (bool)entry.Value;
                            break;
                        case "Rect":
                            Rect = (RectangleF)entry.Value;
                            break;
                        case "Horizontal":
                            Horizontal = (StringAlignment)(int)entry.Value;
                            break;
                        case "Vertical":
                            Vertical = (StringAlignment)(int)entry.Value;
                            break;
                        case "Font":
                            Font = (Font)entry.Value;
                            break;
                        case "Text":
                            Text = (string)entry.Value;
                            break;
                        case "Margins":
                            Margins = entry.Value as Margins;
                            break;
                    }
                }
            }
            /// <summary>
            /// Gets the object Data value from the current application state.
            /// </summary>
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Print", Print);
                info.AddValue("Rect", Rect);
                info.AddValue("Horizontal", (int)Horizontal);
                info.AddValue("Vertical", (int)Vertical);
                info.AddValue("Font", Font);
                info.AddValue("Text", Text);
                info.AddValue("Margins", Margins);
            }
        }
        /// <summary>
        /// Creates a new Document Layer instance and initializes the required state.
        /// </summary>
        public DocumentLayer()
        {
            Size = new Size(827, 1169);
            Items = new List<GraphicsItem>();
            PageNumber = new DocumentPageNumber();
            Kind = PaperKind.A4;
        }
        /// <summary>
        /// Gets the size value from the current application state.
        /// </summary>
        public Size GetSize()
        {
            if (Landscape)
                return new Size(Size.Height, Size.Width);
            else
                return Size;
        }
        /// <summary>
        /// Runs the reset operation and updates the related application state.
        /// </summary>
        public void Reset()
        {
            Items.Clear();
            PageNumber = new DocumentPageNumber();
            Landscape = false;
            Kind = PaperKind.A4;
            Size = new Size(827, 1169);
        }
        /// <summary>
        /// Creates a new Document Layer instance and initializes the required state.
        /// </summary>
        protected DocumentLayer(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                switch (entry.Name)
                {
                    case "Items":
                        Items = entry.Value as List<GraphicsItem>;
                        break;
                    case "PageNumber":
                        PageNumber = entry.Value as DocumentPageNumber;
                        break;
                    case "PaperType":
                        Kind = PaperKind.A4;
                        //PageSizeIndex = (int)entry.Value;
                        //this.PaperKind = (int)PaperKind.A4;
                        break;
                    case "Landscape":
                        Landscape = (bool)entry.Value;
                        break;
                    //case "PaperSize":
                    //    PaperSize = (SizeF)entry.Value;
                    //    break;
                    case "PaperKind":
                        Kind = (PaperKind)entry.Value;
                        break;
                    case "Size":
                        Size = (Size)entry.Value;
                        break;
                }
            }
            if(Size.IsEmpty)
                Size = new Size(827, 1169);
        }
        /// <summary>
        /// Gets the object Data value from the current application state.
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Items", Items);
            info.AddValue("PageNumber", PageNumber);
            //info.AddValue("PaperType", PageSizeIndex);
            info.AddValue("Landscape", Landscape);
            //info.AddValue("PaperSize", PaperSize);
            info.AddValue("PaperKind", Kind);
            info.AddValue("Size", Size);
            //if (PaperSize == null)
            //    PaperSize = new SizeF();
        }
    }
}
