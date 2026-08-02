using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom layout control used by the application user interface.
    /// </summary>
    public class Layout : PictureBox
    {
        /// <summary>
        /// Handles the change Rectangle Delegate lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnChangeRectangleDelegate(RectangleF r);
        public static event OnChangeRectangleDelegate OnChangeRectangle;
        /// <summary>
        /// Handles the update Selected Item Delegate lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnUpdateSelectedItemDelegate(GraphicsItem[] items);
        public static event OnUpdateSelectedItemDelegate OnUpdateSelectedItem;
        public SQLBase sql;
        private DocumentLayer document;
        public DocumentLayer Document 
        {
            get { return document; }
            set
            { 
                if(document == value)
                    return;
                document = value;
                Size = document.GetSize();
            }
        }
        public List<GraphicsItem> SelectedItems = new List<GraphicsItem>();
        private Point mouseLastPosition = Point.Empty, mousePosition = Point.Empty;
        public Color Fore { get; set; }
        public Color Back { get; set; }
        public Color Border { get; set; }
        public StringAlignment Horizontal { get; set; }
        public StringAlignment Vertical { get; set; }
        public int PrintOnPage { get; set; }
        public float BorderWidth { get; set; }
        public bool CalculateTextHeight { get; set; }
        public Font CurrentFont { get; set; }
        /// <summary>
        /// Creates a new Layout instance and initializes the required state.
        /// </summary>
        public Layout()
        {
            Fore = Color.Black;
            Back = Color.White;
            Border = Color.Black;
            CurrentFont = new Font("Arial", 10.0f, FontStyle.Regular, GraphicsUnit.World);
        }
        public int CurrentPage = 0;
        /// <summary>
        /// Defines the available draw Element values used by the application.
        /// </summary>
        public enum DrawElement
        {
            Select,
            Logo,
            Footer,
            Image,
            Table,
            Text,
            Line,
            Arc,
            Container,
            Move,
            Size,
            TextSizeAndInsert,
            Join
        }
        private DrawElement mode = DrawElement.Select;
        /// <summary>
        /// Checks whether the input Key condition is true for the current value.
        /// </summary>
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }
        /// <summary>
        /// Runs the change Draw Mode operation and updates the related application state.
        /// </summary>
        public void ChangeDrawMode(DrawElement element)
        {
            if (!Visible)
                return;
            mode = element;
        }
        /// <summary>
        /// Handles the key Down lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!Visible)
                return;
            if (SelectedItems == null)
                return;
            if (mode == DrawElement.Select && 
                (e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right)
                )
            {
                foreach (GraphicsItem item in SelectedItems)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        RectangleF r = item.GetRectangle(CurrentPage);
                        r.Y -= 1;
                        item.SetRectangle(CurrentPage, r);
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        RectangleF r = item.GetRectangle(CurrentPage);
                        r.Y += 1;
                        item.SetRectangle(CurrentPage, r);
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        RectangleF r = item.GetRectangle(CurrentPage);
                        r.X -= 1;
                        item.SetRectangle(CurrentPage, r);
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        RectangleF r = item.GetRectangle(CurrentPage);
                        r.X += 1;
                        item.SetRectangle(CurrentPage, r);
                    }
                }
                Invalidate();
            }
        }
        /// <summary>
        /// Handles the key Up lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!Visible)
                return;
            if (SelectedItems == null)
                return;
            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                    InsertByClipBoard();
                else if (e.KeyCode == Keys.C)
                    Copy2ClipBoard();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (Focused)
                {
                    foreach (GraphicsItem item in SelectedItems.ToArray())
                    {
                        if (item.Parent != null)
                            item.Parent.Items.Remove(item);
                        Document.Items.Remove(item);
                    }
                    SelectedItems.Clear();
                    OnUpdateSelectedItem(SelectedItems.ToArray());
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Runs the copy2 Clip Board operation and updates the related application state.
        /// </summary>
        public void Copy2ClipBoard()
        {
            if (SelectedItems != null && SelectedItems.Count > 0)
                System.Windows.Forms.Clipboard.SetDataObject(SelectedItems);
        }
        /// <summary>
        /// Runs the insert By Clip Board operation and updates the related application state.
        /// </summary>
        public void InsertByClipBoard()
        {
            IDataObject dataObject = System.Windows.Forms.Clipboard.GetDataObject();
            if (dataObject != null && dataObject.GetDataPresent(typeof(List<GraphicsItem>)))
            {
                List<GraphicsItem> items = dataObject.GetData(typeof(List<GraphicsItem>)) as List<GraphicsItem>;
                foreach (var allItems in items)
                {
                    allItems.Items.Clear();
                    allItems.Parent = null;
                }
                Document.Items.AddRange(items);
                SelectedItems.Clear();
                SelectedItems.AddRange(items);
                OnUpdateSelectedItem(items.ToArray());
            }
            else
            {
                string text = System.Windows.Forms.Clipboard.GetText();
                foreach (TextItem item in SelectedItems.OfType<TextItem>())
                    item.Text = text;
            }
            Invalidate();
        }
        /// <summary>
        /// Handles the mouse Down lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (mode == DrawElement.Select)
                {
                    //SelectedItem = null;
                    List<GraphicsItem> selectedItems = new List<GraphicsItem>();
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        selectedItems.AddRange(this.SelectedItems);
                    }
                    Rectangle mouseRectLine = new Rectangle(new Point(e.X - 8, e.Y - 8), new Size(16, 16));
                    Rectangle mouseRect = new Rectangle(new Point(e.X, e.Y), new Size(1, 1));
                    GraphicsItem IsSelected = null;
                    int x = int.MaxValue,
                        y = int.MaxValue;
                    foreach (GraphicsItem item in Document.Items)
                    {
                        if (!item.IsVisible(CurrentPage))
                            continue;
                        RectangleF rect = item.GetRectangle(CurrentPage);
                        bool hit = false;
                        if (item is LineItem)
                            hit = rect.IntersectsWith(mouseRectLine);
                        else
                            hit = rect.IntersectsWith(mouseRect);
                        if(hit)
                        {
                            int tTop = (e.Y - (int)rect.Top);
                            int tBottom = ((int)rect.Bottom - e.Y);
                            int tLeft = (e.X - (int)rect.Left);
                            int tRight = ((int)rect.Right - e.X);
                            if ((tTop <= y ||
                                 tBottom <= y) 
                                // &&
                                //(tLeft <= x ||
                                // tRight<= x)
                            )
                            {
                                y = Math.Min(tTop, tBottom);
                                x = Math.Min(tLeft, tRight);
                                IsSelected = item;
                            }
                        }
                    }
                    if (IsSelected != null)
                    {
                        if (!selectedItems.Contains(IsSelected))
                            selectedItems.Add(IsSelected);
                        else
                            selectedItems.Remove(IsSelected);
                    }
                    SelectedItems = selectedItems;
                    if (OnUpdateSelectedItem != null)
                        OnUpdateSelectedItem(SelectedItems.ToArray());
                }
                else if (mode == DrawElement.Image)
                {
                    string filename = null;
                    using(OpenFileDialog fileDialog = new OpenFileDialog())
                    {
                        if (fileDialog.ShowDialog(this) != DialogResult.OK)
                        {
                            mode = DrawElement.Select;
                            return;
                        }
                        filename = fileDialog.FileName;
                    }
                    ImageItem item = new ImageItem(new Rectangle(e.Location, new Size(1,1)))
                    {
                        ForeColor = Fore,
                        BackColor = Back,
                        BorderColor = Border,
                        Image = Image.FromFile(filename),
                        BorderWidth = BorderWidth,
                        PrintOn = (Pflegehaushaltsbuch.Data.Graphics.GraphicsItem.PrintPage)PrintOnPage
                    };
                    
                    Document.Items.Add(item);
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                    if (OnUpdateSelectedItem != null)
                        OnUpdateSelectedItem(SelectedItems.ToArray());
                    mode = DrawElement.Size;
                }
                else if (mode == DrawElement.Table)
                {
                    DataTableItem item = new DataTableItem(new Rectangle(e.Location, new Size(1,1)))
                    {
                        ForeColor = Fore,
                        BackColor = Back,
                        BorderColor = Border,
                        Font = CurrentFont,
                        BorderWidth = BorderWidth,
                        PrintOn = (Pflegehaushaltsbuch.Data.Graphics.GraphicsItem.PrintPage)PrintOnPage
                    };
                    Document.Items.Add(item);
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                    if (OnUpdateSelectedItem != null)
                        OnUpdateSelectedItem(SelectedItems.ToArray());
                    mode = DrawElement.Size;
                }
                else if (mode == DrawElement.Text)
                {
                    TextItem item = new TextItem(new Rectangle(e.Location, new Size(1,1)))
                    {
                        ForeColor = Fore,
                        BackColor = Back,
                        BorderColor = Border,
                        Font = CurrentFont,
                        Text = "",
                        CalculateTextHeight = CalculateTextHeight,
                        HorizontalAlignment = Horizontal,
                        VerticalAlignment = Vertical,
                        BorderWidth = BorderWidth,
                        PrintOn = (Pflegehaushaltsbuch.Data.Graphics.GraphicsItem.PrintPage)PrintOnPage
                    };
                    Document.Items.Add(item);
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                    if (OnUpdateSelectedItem != null)
                        OnUpdateSelectedItem(SelectedItems.ToArray());
                    mode = DrawElement.TextSizeAndInsert;
                }
                else if (mode == DrawElement.Line)
                {
                    LineItem item = new LineItem(new Rectangle(e.Location, new Size(1,1)))
                    {
                        ForeColor = Fore,
                        BackColor = Back,
                        BorderColor = Border,
                        BorderWidth = BorderWidth,
                        PrintOn = (Pflegehaushaltsbuch.Data.Graphics.GraphicsItem.PrintPage)PrintOnPage
                    };
                    Document.Items.Add(item);
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                    if (OnUpdateSelectedItem != null)
                        OnUpdateSelectedItem(SelectedItems.ToArray());
                    mode = DrawElement.Size;
                }
            }
            Invalidate();
        }
        /// <summary>
        /// Handles the mouse Move lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            mousePosition = e.Location;
            if (SelectedItems != null)
            {
                foreach (GraphicsItem item in SelectedItems)
                {
                    //Objekt verschieben
                    if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                    {
                        item.Move(CurrentPage, new Size(mousePosition.X - mouseLastPosition.X, mousePosition.Y - mouseLastPosition.Y));
                    }
                    else if (mode == DrawElement.Size || mode == DrawElement.TextSizeAndInsert)
                    {
                        PointF p = item.GetRectangle(CurrentPage).Location;
                        int width = (int)(e.X - p.X);
                        int height = (int)(e.Y - p.Y);
                        SizeF size = Size.Empty;
                        if (item is LineItem)
                        {
                            if (width > height)
                                size = new Size(width, 0);
                            else
                                size = new Size(0, height);
                        }
                        else
                        {
                            size = new SizeF(e.X - p.X, e.Y - p.Y);
                        }
                        item.SetRectangle(CurrentPage, new RectangleF(p, size));
                    }
                    if (OnChangeRectangle != null)
                        OnChangeRectangle(item.GetRectangle(CurrentPage));
                }
                Invalidate();
            }
            else
            {
                if (OnChangeRectangle != null)
                    OnChangeRectangle(new RectangleF(e.Location, new SizeF()));
            }
            mouseLastPosition = mousePosition;
        }
        /// <summary>
        /// Handles the mouse Up lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (SelectedItems == null)
                return;
            if (mode == DrawElement.TextSizeAndInsert)
            {
                string text = "";
                using (EditTextDialog form = new EditTextDialog(sql, text))
                {
                    if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        text = form.Content;
                        ((TextItem)SelectedItems.Last()).Text = text;
                    }
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (mode == DrawElement.Join)
                {
                    List<GraphicsItem> unSelectedItems = new List<GraphicsItem>();
                    unSelectedItems.AddRange(Document.Items.ToArray());
                    foreach (var selectItem in SelectedItems.ToArray())
                        unSelectedItems.Remove(selectItem);
                    foreach (GraphicsItem item in unSelectedItems)
                    {
                        if (item.GetRectangle(CurrentPage).IntersectsWith(new Rectangle(new Point(e.X, e.Y), new Size(1, 1))))
                        {
                            if (MessageBox.ShowDialog(this, Messages.layout_connect_elements, Messages.layout_connect_title, MessageBoxButtons.OKCancel) != DialogResult.OK)
                                break;
                            foreach (var selectItem in SelectedItems.ToArray())
                                selectItem.Connect(item);
                            break;
                        }
                    }
                }
            }
            if (OnUpdateSelectedItem != null)
                OnUpdateSelectedItem(SelectedItems.ToArray());
            mode = DrawElement.Select;
            Invalidate();
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            //using (Bitmap bitmap = new Bitmap(clientRect.Width, clientRect.Height))
            {
                Graphics g = bg.Graphics;
                g.Clear(Color.White);
                //Graphics g = e.Graphics;
                if (Document == null)
                    return;
                foreach (var grItem in Document.Items)
                    grItem.PaintDesign(g, sql, CurrentPage);
                List<Point[]> points = new List<Point[]>();
                foreach (GraphicsItem selectedItem in SelectedItems)
                {
                    RectangleF rect = selectedItem.GetRectangle(CurrentPage);
                    RectangleF r = RectangleF.Inflate(rect, 2, 2);
                    using (Pen p = new Pen(ControlColors.AccentColor, 2.0f))
                    {
                        p.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                    }
                    foreach (GraphicsItem currentItem in Document.Items)
                    {
                        if (currentItem == selectedItem)
                            continue;
                        RectangleF itemRectangle = currentItem.GetRectangle(CurrentPage);
                        if ((int)rect.Top == (int)itemRectangle.Top)
                        {
                            Point p0 = new Point(), p1 = new Point();
                            p0.X = (int)Math.Min(rect.Left, itemRectangle.Left);
                            p1.X = (int)Math.Max(rect.Right, itemRectangle.Right);
                            p0.Y = p1.Y = (int)rect.Top;
                            points.Add(new Point[]{p0,p1});
                        }
                        if ((int)rect.Bottom == (int)itemRectangle.Bottom)
                        {
                            Point p0 = new Point(), p1 = new Point();
                            p0.X = (int)Math.Min(rect.Left, itemRectangle.Left);
                            p1.X = (int)Math.Max(rect.Right, itemRectangle.Right);
                            p0.Y = p1.Y = (int)rect.Bottom;
                            points.Add(new Point[] { p0, p1 });
                        }
                        
                        if ((int)rect.Left == (int)itemRectangle.Left)
                        {
                            Point p0 = new Point(), p1 = new Point();
                            p0.Y = (int)Math.Min(rect.Top, itemRectangle.Top);
                            p1.Y = (int)Math.Max(rect.Bottom, itemRectangle.Bottom);
                            p0.X = p1.X = (int)rect.Left;
                            points.Add(new Point[] { p0, p1 });
                        }
                        if ((int)rect.Right == (int)itemRectangle.Right)
                        {
                            Point p0 = new Point(), p1 = new Point();
                            p0.Y = (int)Math.Min(rect.Top, itemRectangle.Top);
                            p1.Y = (int)Math.Max(rect.Bottom, itemRectangle.Bottom);
                            p0.X = p1.X = (int)rect.Right;
                            points.Add(new Point[] { p0, p1 });
                        }
                    }
                }
                Document.PageNumber.Paint(g, 1);
                foreach (var p in points)
                {
                    g.DrawLine(new Pen(Brushes.Magenta) { DashStyle = DashStyle.Dot}, p[0], p[1]);
                }
                
                bg.Render();
            }
        }
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Layout
            // 
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Layout_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Layout_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Handles the drag Drop event for layout and updates the related state.
        /// </summary>
        private void Layout_DragDrop(object sender, DragEventArgs e)
        {
        }
        /// <summary>
        /// Handles the drag Enter event for layout and updates the related state.
        /// </summary>
        private void Layout_DragEnter(object sender, DragEventArgs e)
        {
        }
    }
}
