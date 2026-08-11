using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data.Graphics
{
    /// <summary>
    /// Represents the data Table Item component used by the application.
    /// </summary>
    [Serializable]
    public class DataTableItem : FontItem
    {
        [XmlIgnore]
        [NonSerialized]
        private DataRow[] data;
        [XmlIgnore]
        [NonSerialized]
        private static int dataIndex = 0;
        public DataTable TableDesign { get; set; }
        [XmlIgnore]
        public DataRow[] Data { get { return data; } set { data = value; } }
        /// <summary>
        /// Creates a new Data Table Item instance and initializes the required state.
        /// </summary>
        public DataTableItem() { }
        /// <summary>
        /// Creates a new Data Table Item instance and initializes the required state.
        /// </summary>
        public DataTableItem(Rectangle rect)
        {
            Rectangles[0] = Rectangles[1] = rect;
            TableDesign = new DataTable("dataTable");
            TableDesign.Columns.Add("name");
            TableDesign.Columns.Add("id");
            TableDesign.Columns.Add("columnAlign", typeof(int));
            TableDesign.Columns.Add("textAlign", typeof(int));
            TableDesign.Columns.Add("width", typeof(int));
            TableDesign.Columns["textAlign"].DefaultValue = StringAlignment.Near;
            TableDesign.Columns["columnAlign"].DefaultValue = StringAlignment.Near;
            TableDesign.Columns["width"].DefaultValue = 20;
            DataRow row = TableDesign.NewRow();
            row["name"] = "Column1";
            TableDesign.Rows.Add(row);
            row = TableDesign.NewRow();
            row["name"] = "Column2";
            TableDesign.Rows.Add(row);
        }
        [XmlIgnore]
        public override bool PaintItem
        {
            get { return true; }
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, RectangleF r)
        {
            
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override RectangleF Paint(XGraphics g, RectangleF r)
        {
            return r;
        }
        /// <summary>
        /// Runs the paint operation and updates the related application state.
        /// </summary>
        public override void Paint(System.Drawing.Graphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (TableDesign == null)
                return;
            if (!IsVisible(page))
                return;
            Brush foreBrush = new SolidBrush(ForeColor);
            Pen linePen = new Pen(new SolidBrush(BorderColor), this.BorderWidth);
            float x= 0, y=0;
            RectangleF currentRect = GetRectangle(page);
            x = currentRect.X;
            y = currentRect.Y;
            StringFormat format = new StringFormat();
            
            int height = this.Font.Height;
            Dictionary<string, int> columnWidths = new Dictionary<string, int>();
            List<int> columnHeights= new List<int>();
            columnHeights.Add((int)y);
            foreach (DataRow row in TableDesign.Rows)
            {
                string name = row[0].ToString();
                string id = row[1].ToString();
                StringAlignment alignment = (StringAlignment)row["columnAlign"];
                format.Alignment = alignment;
                int widthPercent = (int)row["width"];
                int columnWidth = (int)((float)currentRect.Width / 100.0f * (float)widthPercent);
                columnWidths.Add(name, columnWidth);
                /*
                string name = row[0].ToString();
                string id = row[1].ToString();
                string alignment = row[2].ToString();
                string width = row[3].ToString();
              
                int widthPercent = 0;
                int.TryParse(width, out widthPercent);
                int columnWidth = (int)((float)currentRect.Width / 100.0f * (float)widthPercent);
                columnWidths.Add(name, columnWidth);
                StringAlignment horizontal;
                Enum.TryParse<StringAlignment>(alignment, out horizontal);
                format.Alignment = horizontal;
                */
                SizeF noteRect = g.MeasureString(name, Font, (int)columnWidth, format);
                height = (int)(Math.Max(height, noteRect.Height));
                if (BackColor != Color.White)
                    g.FillRectangle(new SolidBrush(BackColor), new RectangleF(x, y, columnWidth, height));
                g.DrawString(name, Font, foreBrush, new RectangleF(x, y, columnWidth, height), format);
                x += (int)columnWidth;
            }
            y += height;// Font.Height * 1.5f;
            if (BorderWidth == 0)
            {
                columnHeights.Add((int)(y + Font.Size * 0.5f));
                y += Font.Size;
            }
            else
                columnHeights.Add((int)(y));
            if (Data == null)
                return;
            //y += height * 0.5f;
            //Data
            for (; dataIndex < data.Length; )
            {
                x = currentRect.X;
                DataRow dataRow = data[dataIndex];
                height = Font.Height;
                //int currentColumn = 0;
                foreach (DataRow row in TableDesign.Rows)
                {
                    string id = row[1].ToString();
                    if (dataRow[id].GetType() != typeof(string))
                        continue;
                    string name = row[0].ToString();
                    string text = dataRow[id].ToString();
                    //string width = row[3].ToString();
                    //int widthPercent = 0;
                    //int.TryParse(width, out widthPercent);
                    //int columnWidth = (int)((float)currentRect.Width / 100.0f * (float)widthPercent);
                    int columnWidth = columnWidths[name];
                    SizeF noteRect = g.MeasureString(text, Font, columnWidth, format);
                    height = (int)(Math.Max(height, noteRect.Height));
                }
                if ((y + height) > currentRect.Bottom)
                {
                    hasMorePages = true;
                    break;
                }
                //Schreibt eine Zeile
                foreach (DataRow row in TableDesign.Rows)
                {
                    string name = row[0].ToString();
                    string id = row[1].ToString();
                    string alignment = row[2].ToString();
                    //string width = row[3].ToString();
                    int columnWidth = columnWidths[name];
                    /*
                    int widthPercent = 0;
                    int.TryParse(width, out widthPercent);
                    int columnWidth = (int)((float)currentRect.Width / 100.0f * (float)widthPercent);
                    */
                    StringAlignment horizontal;
                    Enum.TryParse<StringAlignment>(alignment, out horizontal);
                    format.Alignment = horizontal;
                    string text = dataRow[id].ToString();
                    if (dataRow[id].GetType() == typeof(decimal))
                    {
                        format.Alignment = StringAlignment.Far;
                        text = ((decimal)dataRow[id]).ToString("c");
                    }
                    else if (dataRow[id].GetType() == typeof(DateTime))
                        text = ((DateTime)dataRow[id]).Date.ToShortDateString();
                    else if (id.Equals(Columns.BookCategory))
                        text = ((SQLBase.BookCategory)Int32.Parse(text)).GetDisplayName();
                    else if (id.Equals(Columns.Id))
                        text = string.Format("{0:000}", Int32.Parse(text));
                    else if (id.Equals(Columns.Active))
                    {
                        //Alte Version 
                        text = ((SQLBase.ClientActive)Int32.Parse(text)).GetDisplayName();
                    }
                    else if (id.Equals(Columns.AmountPaybackType))
                        text = ((SQLBase.Repayment)Int32.Parse(text)).GetDisplayName();
                    if (BackColor != Color.White)
                        g.FillRectangle(new SolidBrush(BackColor), new RectangleF(x, y, columnWidth, height));
                    g.DrawString(text, Font, foreBrush, new RectangleF(x, y, columnWidth, height), format);
                    x += (int)columnWidth;
                }
                y += height;
                dataIndex++;
                columnHeights.Add((int)y);
            }
            if (BorderWidth > 0)
            {
                x = currentRect.X;
                foreach(int value in columnWidths.Values)
                //for (int i = 0; i < columnWidths.Count; i++)
                {
                    g.DrawLine(linePen, x, currentRect.Top, x, y);
                    x += value;//columnWidths[i];
                }
                g.DrawLine(linePen, currentRect.Right, currentRect.Top, currentRect.Right, y);
                //Horizontale Linien
                for (int i = 0; i < columnHeights.Count; i++)
                {
                    int h = columnHeights[i];
                    g.DrawLine(linePen, currentRect.X, h, currentRect.X + currentRect.Width, h);
                }
            }
            else
            {
                g.DrawLine(linePen, currentRect.X, columnHeights[1], currentRect.X + currentRect.Width, columnHeights[1]);
            }
            if (hasMorePages)
                return;
            y += Font.Height*0.5f;
            if (!DrawSubItem(g, sql, page, (int)y, (int)currentRect.Bottom))
            {
                hasMorePages = true;
                return;
            }
            dataIndex = 0;
        }
        /// <summary>
        /// Runs the paint PDF operation and updates the related application state.
        /// </summary>
        public override void PaintPDF(XGraphics g, int translateY, SQLBase sql, int page, int lastPage, out bool hasMorePages)
        {
            hasMorePages = false;
            if (Parent != null)
                return;
            if (TableDesign == null)
                return;
            if (!IsVisible(page))
                return;
            XBrush foreBrush = new XSolidBrush(XColor.FromArgb(ForeColor.R,ForeColor.G,ForeColor.B ));
            XBrush backBrush = new XSolidBrush(XColor.FromArgb(BackColor.R,BackColor.G,BackColor.B ));
            XPen linePen = new XPen(XColor.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B), 0.5f);//this.BorderWidth);
            float x = 0, y = 0;
            RectangleF tableRect = GetRectangle(page);
            x = tableRect.X;
            y = tableRect.Y;
            XStringFormat format = new XStringFormat();
            //int height = (int)Font.GetHeight(72.0f);
            Dictionary<string, int> columnWidths = new Dictionary<string, int>();
            List<int> columnHeights = new List<int>();
            columnHeights.Add((int)y);
            XFont font = GetXFont(g);
            int height = font.Height;
            XTextFormatter tf = new XTextFormatter(g);
            foreach (DataRow row in TableDesign.Rows)
            {
                string name = row[0].ToString();
                string id = row[1].ToString();
                StringAlignment alignment = (StringAlignment)row["columnAlign"];
                int widthPercent = (int)row["width"];
                int columnWidth = (int)((float)tableRect.Width / 100.0f * (float)widthPercent);
                columnWidths.Add(name, columnWidth);
                //format.Alignment = horizontal;
                /*
                string name = row[0].ToString();
                string id = row[1].ToString();
                string alignment = row[2].ToString();
                string width = row[3].ToString();
                int widthPercent = 0;
                int.TryParse(width, out widthPercent);
                int columnWidth = (int)((float)tableRect.Width / 100.0f * (float)widthPercent);
                columnWidths.Add(name, columnWidth);
                StringAlignment horizontal;
                Enum.TryParse<StringAlignment>(alignment, out horizontal);
                */
                UpdateAlignment(format, alignment);
                XSize noteRect = MeasureString(g, name, font, format, new Rectangle(0, 0, (int)columnWidth, font.Height));
                height = (int)(Math.Max(height, noteRect.Height));
                if (BackColor != Color.White)
                    g.DrawRectangle(backBrush, new RectangleF(x, y, columnWidth, height));
                if (alignment == StringAlignment.Near)
                    tf.Alignment = XParagraphAlignment.Left;
                else if (alignment == StringAlignment.Center)
                    tf.Alignment = XParagraphAlignment.Center;
                else if (alignment == StringAlignment.Far)
                    tf.Alignment = XParagraphAlignment.Right;
                tf.DrawString(name, font, foreBrush, new RectangleF(x+2, y, columnWidth-4, height));
                //g.DrawString(name, font, foreBrush, new RectangleF(x + 2, y, columnWidth - 4, height), format);
                //g.DrawString(name, font, foreBrush, new RectangleF(x, y, columnWidth, height), format);
                x += (int)columnWidth;
            }
            y += height;// Font.Height * 1.5f;
            if (BorderWidth == 0)
            {
                columnHeights.Add((int)(y + Font.Size * 0.5f));
                y += Font.Size;
            }
            else
                columnHeights.Add((int)(y));
            if (Data == null)
                return;
            //y += height * 0.5f;
            //Data
            for (; dataIndex < data.Length; )
            {
                x = tableRect.X;
                DataRow dataRow = data[dataIndex];
                height = Font.Height;
                foreach (DataRow row in TableDesign.Rows)
                {
                    string id = row[1].ToString();
                    if (dataRow[id].GetType() == typeof(string))
                    {
                        string name = row[0].ToString();
                        string text = dataRow[id].ToString();
                        int columnWidth = columnWidths[name];
                        XSize noteRect = MeasureString(g, text, font, 
                            //(int)columnWidth, 
                            format, new Rectangle(2, 0, (int)columnWidth-4, (int)font.Height));
                        height = (int)(Math.Max(height, noteRect.Height));
                    }
                }
                if ((y + height) > tableRect.Bottom)
                {
                    hasMorePages = true;
                    break;
                }
                //Schreibt eine Zeile
                foreach (DataRow row in TableDesign.Rows)
                {
                    string name = row[0].ToString();
                    string id = row[1].ToString();
                    string alignment = row[2].ToString();
                    /*
                    string width = row[3].ToString();
                    float columnWidth = 0;
                    float.TryParse(width, out columnWidth);
                    columnWidth *= (float)tableRect.Width;
                    */
                    int columnWidth = columnWidths[name];
                    StringAlignment horizontal;
                    Enum.TryParse<StringAlignment>(alignment, out horizontal);
                    string text = dataRow[id].ToString();
                    UpdateAlignment(format, horizontal);
                    if (dataRow[id].GetType() == typeof(decimal))
                    {
                        format.Alignment = XStringAlignment.Far; 
                        text = ((decimal)dataRow[id]).ToString("c");
                    }
                    else if (dataRow[id].GetType() == typeof(DateTime))
                        text = ((DateTime)dataRow[id]).Date.ToShortDateString();
                    else if (id.Equals(Columns.BookCategory))
                        text = ((SQLBase.BookCategory)Int32.Parse(text)).GetDisplayName();
                    else if (id.Equals(Columns.Id))
                        text = string.Format("{0:000}", Int32.Parse(text));
                    else if (id.Equals(Columns.Active))
                        text = ((SQLBase.ClientActive)Int32.Parse(text)).GetDisplayName();
                    else if (id.Equals(Columns.AmountPaybackType))
                        text = ((SQLBase.Repayment)Int32.Parse(text)).GetDisplayName();
                    if (BackColor != Color.White)
                        g.DrawRectangle(new SolidBrush(BackColor), new RectangleF(x, y, columnWidth, height));
                    
                    if (format.Alignment == XStringAlignment.Near)
                        tf.Alignment = XParagraphAlignment.Left;
                    else if (format.Alignment == XStringAlignment.Center)
                        tf.Alignment = XParagraphAlignment.Center;
                    else if (format.Alignment == XStringAlignment.Far)
                        tf.Alignment = XParagraphAlignment.Right;
                    
                    tf.DrawString(text, font, foreBrush, new RectangleF(x+2, y, columnWidth-4, height));
                    //g.DrawString(text, font, foreBrush, new RectangleF(x, y, columnWidth, height), format);
                    
                    x += (int)columnWidth;
                }
                y += height;
                dataIndex++;
                columnHeights.Add((int)y);
            }
            if (BorderWidth > 0)
            {
                x = tableRect.X;
                foreach(var width in columnWidths.Values)
                //for (int i = 0; i < columnWidths.Count; i++)
                {
                    g.DrawLine(linePen, x, tableRect.Top, x, y);
                    x += width;// columnWidths[i];
                }
                g.DrawLine(linePen, tableRect.Right, tableRect.Top, tableRect.Right, y);
                //Horizontale Linien
                for (int i = 0; i < columnHeights.Count; i++)
                {
                    int h = columnHeights[i];
                    g.DrawLine(linePen, tableRect.X, h, tableRect.X + tableRect.Width, h);
                }
            }
            else
            {
                g.DrawLine(linePen, tableRect.X, columnHeights[1], tableRect.X + tableRect.Width, columnHeights[1]);
            }
            if (hasMorePages)
                return;
            y += Font.Height * 0.5f;
            if (!DrawSubItem(g, sql, page, (int)y, (int)tableRect.Bottom))
            {
                hasMorePages = true;
                return;
            }
            dataIndex = 0;
        }
        /// <summary>
        /// Handles the paint Design lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnPaintDesign(System.Drawing.Graphics g, SQLBase sql, RectangleF rect, int page)
        {
            if (TableDesign == null)
                return;
            Brush foreBrush = new SolidBrush(ForeColor);
            float x = rect.X;
            float y = rect.Y;
            StringFormat format = new StringFormat();
            if (BackColor != Color.White)
                g.FillRectangle(new SolidBrush(BackColor), rect);
            if (BorderWidth > 0)
                g.DrawRectangle(new Pen(new SolidBrush(BorderColor)) { Width = BorderWidth, DashStyle = Style },
                    rect.X, rect.Y, rect.Width, rect.Height);
            int height = Font.Height;
            List<int> columnWidths = new List<int>();
            foreach (DataRow row in TableDesign.Rows)
            {
                string name = row[0].ToString();
                string id = row[1].ToString();
                StringAlignment horizontal = (StringAlignment)row["columnAlign"];
                int widthPercent = (int)row["width"];
                int columnWidth = (int)((float)rect.Width / 100.0f * (float)widthPercent);
                columnWidths.Add(columnWidth);
                format.Alignment = horizontal;
                SizeF noteRect = g.MeasureString(name, Font, (int)columnWidth, format);
                height = (int)Math.Max(height, noteRect.Height);
                g.DrawString(name, Font, foreBrush, new RectangleF(x, y, columnWidth, height), format);
                x += columnWidth;
            }

            x = rect.X;
            foreach (int value in columnWidths)
            //for (int i = 0; i < columnWidths.Count; i++)
            {
                g.DrawLine(Pens.Black, x, rect.Top, x, rect.Bottom);
                x += value;//columnWidths[i];
            }
            //g.DrawLine(linePen, currentRect.Right, currentRect.Top, currentRect.Right, y);
            ////Horizontale Linien
            //for (int i = 0; i < columnHeights.Count; i++)
            //{
            //    int h = columnHeights[i];
            //    g.DrawLine(linePen, currentRect.X, h, currentRect.X + currentRect.Width, h);
            //}

            y += height;
            g.DrawLine(new Pen(BorderColor), rect.X, y, rect.X + rect.Width, y);
            //Paint(g, sql, 0, 0, out value);
        }
        /// <summary>
        /// Updates the alignment data and refreshes the related application state.
        /// </summary>
        public void UpdateAlignment(XStringFormat sf, StringAlignment horizontalAlignment)
        {
            sf.Alignment = horizontalAlignment == StringAlignment.Near ?
                XStringAlignment.Near : horizontalAlignment == StringAlignment.Center ?
                XStringAlignment.Center : XStringAlignment.Far;
        }
    }
}
