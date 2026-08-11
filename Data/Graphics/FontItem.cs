using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the font Item component used by the application.
    /// </summary>
    [Serializable]
    public class FontItem : GraphicsItem
    {
        /// <summary>
        /// Creates a new Font Item instance and initializes the required state.
        /// </summary>
        public FontItem()
        {
            Font = new Font("Arial", 10.0f, FontStyle.Regular);
        }
        [XmlIgnore]
        public Font Font { get; set; }
        [XmlElement("Font")]
        public string XmlFont
        {
            get { return new FontConverter().ConvertToString(Font); }
            set { Font = new FontConverter().ConvertFromString(value) as Font; }
        }
        /// <summary>
        /// Gets the x Font value from the current application state.
        /// </summary>
        public XFont GetXFont(XGraphics g)
        {
            //Font font1 = new Font(Font.FontFamily, Font.GetHeight(), Font.Style, GraphicsUnit.World);
            return new XFont(Font,//.FontFamily, (double)Font.Height, (XFontStyle)Font.Style,
                new XPdfFontOptions(PdfSharp.Pdf.PdfFontEmbedding.Always));
        }
    }
}
