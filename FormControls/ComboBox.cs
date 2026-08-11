using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom combo Box control used by the application user interface.
    /// </summary>
    public class ComboBox : System.Windows.Forms.ComboBox, IMessageFilter
    {
        /// <summary>
        /// Creates a new Combo Box instance and initializes the required state.
        /// </summary>
        public ComboBox()
        {
        }
        /// <summary>
        /// Handles the validating lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (e.Cancel && SelectedIndex == -1 || SelectedItem == null)
                e.Cancel = false;
        }
        private static ColorSet colorSet = new ColorSet();
        public static ColorSet ColorSet
        {
            get
            {
                return colorSet;
            }
            set
            {
                colorSet = value;
            }
        }
        private ListBox listbox = null;
        private ToolStripControlHost host;
        private ToolStripDropDown dropDown;
        private IntPtr hbrush = IntPtr.Zero;
        object lastSelectedItem;
        private bool requestedSorted;
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        private bool mouseOver = false;
        /// <summary>
        /// Runs the del On List Box Closed operation and updates the related application state.
        /// </summary>
        public delegate void DelOnListBoxClosed();
        public event DelOnListBoxClosed OnListBoxClosed;
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseOver = true;
        }
        /// <summary>
        /// Handles the mouse Leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseOver = false;
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            if (DropDownStyle == ComboBoxStyle.DropDownList)
                return;
            if (AutoCompleteMode == AutoCompleteMode.None)
                return;
            Application.AddMessageFilter(this);
            //if (AutoCompleteMode == System.Windows.Forms.AutoCompleteMode.None)
            //    AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                //return;
            var c = colorSet.BackColor;
            int rgb = (c.B << 16) | (c.G << 8) | c.R;
            hbrush = Win32.CreateSolidBrush((uint)rgb);
            AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            listbox = new ListBox();
            listbox.Font = Font;
            listbox.ItemHeight = ItemHeight;// Font.Height;
            listbox.MultiColumn = false;
            listbox.BackColor = BackColor;
            listbox.ForeColor = ForeColor;
       
            listbox.SelectedIndexChanged += Listbox_SelectedIndexChanged;
            listbox.Click += listbox_Click;
            listbox.Visible = true;
            listbox.Width = Width;
            listbox.DisplayMember = DisplayMember;
            host = new ToolStripControlHost(listbox);
            host.Margin = Padding.Empty;
            host.Padding = Padding.Empty;
            host.AutoSize = false;
            host.Width = listbox.Width;
            dropDown = new ToolStripDropDown();
            dropDown.AutoSize = false;
            dropDown.Items.Add(host);
            dropDown.Margin = Padding.Empty;
            dropDown.Padding = Padding.Empty;
            dropDown.AutoClose = false;
            dropDown.AutoSize = false;
            KeyDown += comboBox_KeyDown;
            TextUpdate += ComboBox_TextUpdate;
        }
        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Application.RemoveMessageFilter(this);
            base.Dispose(disposing);
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DropDownStyle != ComboBoxStyle.DropDownList)
                return;
            Color foreColor;
            if (Enabled)
                foreColor = colorSet.ForeColor;
            else
                foreColor = colorSet.Disabled;
            Rectangle rect = ClientRectangle;
            rect.Width -= 17;
            if (Text != null)
            {
                TextRenderer.DrawText(e.Graphics, Text.Trim(), Font, rect, foreColor, TextFormatFlags.Default | TextFormatFlags.VerticalCenter);
                //g.DrawString(Text.Trim(), Font, new SolidBrush(foreColor), rect, new StringFormat() { LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox });
            }
            else
                TextRenderer.DrawText(e.Graphics, "", Font, ClientRectangle, foreColor, TextFormatFlags.Default | TextFormatFlags.VerticalCenter);
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                //using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
                {
                    //var g = bg.Graphics;
                    g.Clear(colorSet.BackColor);
                    Rectangle rectangle = Bounds;
                    rectangle.X = Math.Max(0, ClientRectangle.Width - 17);
                    rectangle.Width = 17;
                    Rectangle clientRect = ClientRectangle;
                    Rectangle rect = new Rectangle(clientRect.Width - 18, 0, 18, clientRect.Height);
                    LinearGradientBrush DropButton1Brush;
                    if (DroppedDown)// || Focused)
                    {
                        DropButton1Brush = new LinearGradientBrush(
                           new Point(0, 0),
                           new Point(0, ClientRectangle.Bottom),
                           Color.White,
                           Color.Gray);
                    }
                    else
                    {
                        DropButton1Brush = new LinearGradientBrush(
                            new Point(0, 0),
                            new Point(0, ClientRectangle.Bottom),
                            Color.White,
                            Color.FromArgb(200, 200, 200));
                    }
                    if (DropDownStyle == ComboBoxStyle.DropDownList)
                        g.FillRectangle(DropButton1Brush, new Rectangle(Point.Empty, ClientRectangle.Size));
                    else
                        g.FillRectangle(DropButton1Brush, rect);
                    //Pfeil
                    using (Bitmap arrowBitmap = new Bitmap(100, 100))
                    {
                        RectangleF arrowsRect = new RectangleF(rect.X, rect.Y, rect.Width * 0.5f, rect.Width * 0.5f * 0.5f);
                        arrowsRect.X += (rect.Width - arrowsRect.Width) * 0.5f;
                        arrowsRect.Y += (rect.Height - arrowsRect.Height) * 0.5f;
                        using (Graphics gr = Graphics.FromImage(arrowBitmap))
                        {
                            //Pfeil
                            System.Drawing.Drawing2D.GraphicsPath pth = new System.Drawing.Drawing2D.GraphicsPath();
                            PointF TopLeft = new PointF(0, 0);
                            PointF TopRight = new PointF(100, 0);
                            PointF Bottom = new PointF(50, 100);
                            pth.AddLine(TopLeft, TopRight);
                            pth.AddLine(TopRight, Bottom);
                            pth.AddLine(Bottom, TopLeft);
                            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            if (!Enabled)
                                gr.FillPath(new SolidBrush(Color.LightGray), pth);
                            else
                                //Draw the arrow
                                gr.FillPath(Brushes.Black, pth);
                            //gr.DrawPath(new Pen(Color.Black) { Width=10.0f}, pth);
                            g.DrawImage(arrowBitmap, arrowsRect);
                        }
                        //gr.Dispose();
                        //gr.FillPath(ArrowBrush, pth);
                    }
                    rect.Width -= 1;
                    rect.Height -= 1;
                    g.DrawRectangle(Pens.Black, rect);
                    if (mouseOver | Focused)
                        g.DrawRectangle(ControlColors.AccentPen, new Rectangle(0, 0, clientRect.Width - 1, clientRect.Height - 1));
                    else
                        g.DrawRectangle(Pens.Black, new Rectangle(0, 0, clientRect.Width - 1, clientRect.Height - 1));
                }
                e.Graphics.DrawImage(bitmap, ClientRectangle);
            }
        }
        public new string DisplayMember
        {
            get { return base.DisplayMember; }
            set 
            { 
                base.DisplayMember = value;
                ApplyDataSourceSort();
                if (listbox != null)
                    listbox.DisplayMember = value; 
            }
        }
        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                if (value != null && base.Sorted)
                    base.Sorted = false;

                base.DataSource = value;
                ApplyDataSourceSort();

                if (value == null)
                    base.Sorted = requestedSorted;
            }
        }
        public new bool Sorted
        {
            get { return requestedSorted; }
            set
            {
                requestedSorted = value;
                if (base.DataSource == null)
                    base.Sorted = value;
                else
                {
                    base.Sorted = false;
                    ApplyDataSourceSort();
                }
            }
        }
        protected override void OnDisplayMemberChanged(EventArgs e)
        {
            base.OnDisplayMemberChanged(e);
            ApplyDataSourceSort();
            if (listbox != null)
                listbox.DisplayMember = DisplayMember;
            Invalidate();
        }
        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            if (listbox != null)
                listbox.DisplayMember = DisplayMember;
            Invalidate();
        }
        private void ApplyDataSourceSort()
        {
            if (!requestedSorted || string.IsNullOrWhiteSpace(DisplayMember))
                return;

            DataView dataView = base.DataSource as DataView;
            if (dataView != null)
            {
                if (dataView.Table.Columns.Contains(DisplayMember) && string.IsNullOrWhiteSpace(dataView.Sort))
                    dataView.Sort = DisplayMember;
                return;
            }

            DataTable dataTable = base.DataSource as DataTable;
            if (dataTable != null && dataTable.Columns.Contains(DisplayMember) && string.IsNullOrWhiteSpace(dataTable.DefaultView.Sort))
                dataTable.DefaultView.Sort = DisplayMember;
        }
        private string GetDisplayText(object item)
        {
            if (item == null)
                return string.Empty;

            string displayMember = DisplayMember;
            DataRowView rowView = item as DataRowView;
            if (rowView != null)
            {
                if (!string.IsNullOrWhiteSpace(displayMember) && rowView.Row.Table.Columns.Contains(displayMember))
                    return Convert.ToString(rowView[displayMember]);

                foreach (DataColumn column in rowView.Row.Table.Columns)
                {
                    object value = rowView.Row[column];
                    if (value != null && value != DBNull.Value)
                        return Convert.ToString(value);
                }

                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(displayMember))
            {
                PropertyInfo info = item.GetType().GetProperty(displayMember);
                if (info != null)
                    return Convert.ToString(info.GetValue(item, null));
            }

            return item.ToString();
        }
        /// <summary>
        /// Handles the lost Focus lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            mouseOver = false;
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            lastSelectedItem = SelectedItem;
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
                Invalidate();
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
        /// <summary>
        /// Handles the leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (dropDown != null && dropDown.Visible)
                FinishAutoComplete();
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
                Invalidate();
        }
        /// <summary>
        /// Runs the close Drop Down operation and updates the related application state.
        /// </summary>
        private void CloseDropDown()
        {
            if (dropDown != null && dropDown.Visible)
                dropDown.Close();
        }
        /// <summary>
        /// Runs the finish Auto Complete operation and updates the related application state.
        /// </summary>
        private void FinishAutoComplete()
        {
            if (dropDown == null || !dropDown.Visible)
                return;
            CloseDropDown();
            if (listbox.SelectedItem == null)
            {
                int index = listbox.FindStringExact(Text);
                if (index >= 0)
                    SelectedItem = listbox.Items[index];
                else
                    SelectedItem = lastSelectedItem;
            }
            else
                SelectedItem = listbox.SelectedItem;
            Select();
            OnSelectionChangeCommitted(new EventArgs());
        }
        void listbox_Click(object sender, EventArgs e)
        {
            FinishAutoComplete();
        }
        private bool textUpdateEnabled = true;
        void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                FinishAutoComplete();
                if (OnListBoxClosed != null)
                    OnListBoxClosed();
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (listbox.Visible && listbox.Items.Count != 0)
                {
                    textUpdateEnabled = false;
                    if (listbox.SelectedIndex <= 0)
                        listbox.SelectedIndex = listbox.Items.Count - 1;
                    else
                        listbox.SelectedIndex--;
                    textUpdateEnabled = true;
                }
                else
                {
                    if (SelectedIndex <= 0)
                        SelectedIndex = Items.Count - 1;
                    else
                        SelectedIndex--;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (listbox.Visible && listbox.Items.Count != 0)
                {
                    textUpdateEnabled = false;
                    if (listbox.SelectedIndex >= listbox.Items.Count - 1 || listbox.SelectedIndex < 0 )
                        listbox.SelectedIndex = 0;
                    else
                        listbox.SelectedIndex++;
                    textUpdateEnabled = true;
                }
                else
                {
                    if (SelectedIndex >= Items.Count - 1 || SelectedIndex < 0)
                        SelectedIndex = 0;
                    else
                        SelectedIndex++;
                }
            }
            
        }
        void Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = listbox.Text;
            if (listbox.Text != null)
            {
                Select(listbox.Text.Length, 0);
            }
        }
        void ComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (SelectionLength != 0)
                return;
            if (SelectionStart == 0)
            {
                if (!dropDown.Visible)
                {
                    if (SelectedItem != null)
                    {
                        SelectedItem = null;
                        OnSelectionChangeCommitted(new EventArgs());
                    }
                }
                return;
            }
            if (DroppedDown)
                return;
            if (listbox == null || listbox.Focused)
                return;
            if (!Focused)
                return;
            if (!textUpdateEnabled)
                return;
            string currentText = Text;
            if (string.IsNullOrWhiteSpace(currentText))
            {
                if (dropDown.Visible)
                {
                    dropDown.Close();
                    Select();
                }
                return;
            }
            string compare = currentText.ToLower();
            listbox.SuspendLayout();
            listbox.Items.Clear();
            foreach (var item in Items)
            {
                string s = GetDisplayText(item);
                if (s.ToLower().Contains(compare))
                {
                    listbox.Items.Add(item);
                }
            }
            listbox.ResumeLayout();
            ShowAutoList();
        }
        /// <summary>
        /// Runs the show Auto List operation and updates the related application state.
        /// </summary>
        public void ShowAutoList()
        {
            host.Height = dropDown.Height = Math.Min(MaxDropDownItems, Math.Max(1,listbox.Items.Count)) * ItemHeight;
            
            if (dropDown.Visible)
                return;
            dropDown.Width = DropDownWidth;
            dropDown.Show(this, new Point(0, Height));
        }
        /// <summary>
        /// Handles the draw Item lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            var listSelectBackColor = colorSet.ListSelectBackColor;
            var listSelectForeColor = colorSet.ListSelectForeColor;
            Color backColor = Color.White;
            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                (e.State & DrawItemState.Checked) == DrawItemState.Checked
                )
            {
                backColor = listSelectBackColor;
            }
            //else
            string value = GetDisplayText(Items[e.Index]);
            if (value == null)
                return;
            Color foreColor = Color.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                (e.State & DrawItemState.Checked) == DrawItemState.Checked
                )
            {
                foreColor = listSelectForeColor;
            }
            //else
            TextFormatFlags flags = new TextFormatFlags();
            flags = TextFormatFlags.Default;
            flags |= TextFormatFlags.TextBoxControl;
            TextRenderer.DrawText(e.Graphics, value, e.Font, e.Bounds, foreColor, backColor, flags);
        }
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Win32.WM_CTLCOLOREDIT)
            {
                if (this.DropDownStyle != ComboBoxStyle.DropDown)
                    return;
                var c = BackColor;
                int rgb = (c.B << 16) | (c.G << 8) | c.R;
                Win32.SetBkColor(m.WParam, rgb);
                c = ForeColor;
                rgb = (c.B << 16) | (c.G << 8) | c.R;
                Win32.SetTextColor(m.WParam, rgb);
                m.Result = hbrush;
            }
            //else if (m.Msg == Win32.WM_CTLCOLORLISTBOX)
            //{
            //    if (this.DropDownStyle != ComboBoxStyle.DropDown)
            //        return;
            //    var c = colorSet.ListSelectBackColor;
            //    int rgb = (c.B << 16) | (c.G << 8) | c.R;
            //    Win32.SetBkColor(m.WParam, rgb);
            //    c = colorSet.ListSelectForeColor;
            //    rgb = (c.B << 16) | (c.G << 8) | c.R;
            //    Win32.SetTextColor(m.WParam, rgb);
            //}
            //else if (m.Msg == Win32.WM_CTLCOLORMSGBOX ||
            //         m.Msg == Win32.WM_CTLCOLORLISTBOX ||
            //          m.Msg == Win32.WM_CTLCOLORDLG ||
            //    m.Msg == Win32.WM_CTLCOLOR
            //)
            //{
            //    var c = BackColor;
            //    int rgb = (c.B << 16) | (c.G << 8) | c.R;
            //    Win32.SetBkColor(m.WParam, rgb);
            //    c = Color.Blue;
            //    rgb = (c.B << 16) | (c.G << 8) | c.R;
            //    Win32.SetTextColor(m.WParam, rgb);
            //    m.Result = (IntPtr)1;
            //}
            //else if (m.Msg == Win32.WM_CTLCOLORSTATIC)
            //{
            //}
        }
        /// <summary>
        /// Runs the pre Filter Message operation and updates the related application state.
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            if (dropDown != null && dropDown.Visible)
            {
                if (m.Msg == Win32.WM_LBUTTONDOWN ||
                    m.Msg == Win32.WM_RBUTTONDOWN ||
                    m.Msg == Win32.WM_MBUTTONDOWN ||
                    m.Msg == Win32.WM_NCLBUTTONDOWN ||
                    m.Msg == Win32.WM_NCRBUTTONDOWN ||
                    m.Msg == Win32.WM_NCMBUTTONDOWN)
                {
                    if (m.HWnd != Handle && m.HWnd != dropDown.Handle && m.HWnd != listbox.Handle)
                    {
                        FinishAutoComplete();
                        return true;
                    }
                }
            }
        
            return false;
        }
    }
}
