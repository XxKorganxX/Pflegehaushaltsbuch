using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom text Box control used by the application user interface.
    /// </summary>
    public class TextBox : System.Windows.Forms.TextBox
    {
        [Localizable(true)]
        [DefaultValue("")]
        public string DefaultText { get; set; }
        private ListBox listbox = null;
        private ToolStripControlHost host = null;
        private ToolStripDropDown dropDown = null;
        private int SelectedIndex { get; set; }
        private bool mouseOver = false;
        [DefaultValue(false)]
        public bool OnlyNumeric { get; set; }
        /// <summary>
        /// Creates a new Text Box instance and initializes the required state.
        /// </summary>
        public TextBox()
        {
            DoubleBuffered = true;
            if (Program.DesignMode)
                return;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            if (AutoCompleteMode == System.Windows.Forms.AutoCompleteMode.None)
                return;
            AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            listbox = new ListBox();
            listbox.Font = Font;
            listbox.ItemHeight = Font.Height;
            listbox.MultiColumn = false;
            this.TextChanged += autoList_TextChanged;
            this.KeyDown += TextBox_KeyUp;
            listbox.SelectedIndexChanged += autolist_SelectedIndexChanged;
            listbox.Click += autoList_Click;
            listbox.KeyUp += autolist_KeyUp;
            listbox.Size = Size;
            listbox.Visible = true;
            listbox.Height = 100;
            host = new ToolStripControlHost(listbox);
            host.Margin = Padding.Empty;
            host.Padding = Padding.Empty;
            host.AutoSize = false;
            dropDown = new ToolStripDropDown();
            dropDown.AutoSize = false;
            dropDown.Items.Add(host);
            dropDown.Margin = Padding.Empty;
            dropDown.Padding = Padding.Empty;
            dropDown.AutoClose = false;
            listbox.LostFocus += autoList_LostFocus;
            LostFocus += autoList_LostFocus;
            //FindForm().Move += autoList_Move;
        }
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseOver = true;
            if (Multiline)
                Invalidate();
        }
        /// <summary>
        /// Handles the mouse Leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            mouseOver = false;
            if (Multiline)
                Invalidate();
        }
        public override Font Font
        {
            get
            {
                return FormControls.Form.baseFont;
            }
            set
            {
                OnFontChanged(new EventArgs());
            }
        }
        void autoList_Move(object sender, EventArgs e)
        {
            if (dropDown.Visible)
                dropDown.Close();
        }
        void autoList_LostFocus(object sender, EventArgs e)
        {
            if (dropDown.Visible && !Focused && !dropDown.Focused)
                dropDown.Close();
        }
        void autolist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Text = listbox.SelectedItem.ToString();
                dropDown.Close();
            }
        }
        void autoList_Click(object sender, EventArgs e)
        {
            Text = listbox.SelectedItem.ToString();
            dropDown.Close();
        }
        void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (listbox.Visible)
                {
                    listbox.SelectedIndex = listbox.Items.Count - 1;
                    listbox.Focus();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (listbox.Visible)
                {
                    listbox.SelectedIndex = 0;
                    listbox.Focus();
                }
            }
        }
        void autolist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listbox != null)
                SelectedIndex = listbox.SelectedIndex;
        }
        void autoList_TextChanged(object sender, EventArgs e)
        {
            if (!Focused)
                return;
            listbox.SuspendLayout();
            listbox.Items.Clear();
            if (string.IsNullOrWhiteSpace(Text))
            {
                listbox.ResumeLayout();
                if (dropDown.Visible)
                {
                    dropDown.Close();
                    Select();
                }
                return;
            }
            string compare = Text.ToLower();
            foreach (var s in AutoCompleteCustomSource)
            {
                if (s.ToString().ToLower().Contains(compare))
                    listbox.Items.Add(s);
            }
            if (listbox.Items.Count == 0)
            {
                dropDown.Close();
                Select();
                return;
            }
            ShowAutoList();
        }
        /// <summary>
        /// Runs the show Auto List operation and updates the related application state.
        /// </summary>
        public void ShowAutoList()
        {
            if (dropDown.Visible)
                return;
            dropDown.Width = Width;
            dropDown.Height = listbox.Height;
            dropDown.Show(this, new Point(0, Height));
        }
        /// <summary>
        /// Handles the key Press lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (!OnlyNumeric)
                return;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )// &&
                //(e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Win32.WM_PAINT)
            {
                DrawBorder(ref m);
            }
        }
        /// <summary>
        /// Draws the border output on the provided graphics surface.
        /// </summary>
        public void DrawBorder(ref Message message)
        {
            //if (message.Msg == Win32.WM_NCPAINT)
            //{
            //    //get handle to a display device context
            //    IntPtr hdc = GetDCEx(message.HWnd, message.WParam, 1 | 0x0020);
            //    if (hdc != IntPtr.Zero)
            //    {
            //        //get Graphics object from the display device context
            //        //using (Graphics g = Graphics.FromHwnd(message.HWnd))
            //        using (Graphics g = Graphics.FromHdc(hdc))
            //        {
            //            g.Clear(Color.Red);
            //        }
            //        ReleaseDC(message.HWnd, hdc);
            //    }
            //    return;
            //}
            {
                //get handle to a display device context
                //IntPtr hdc = GetDCEx(message.HWnd, message.WParam, 1 | 0x0020);
                //if (hdc != IntPtr.Zero)
                {
                    //get Graphics object from the display device context
                    using (Graphics g = Graphics.FromHwnd(Handle))
                    //using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        Rectangle rectangle = ClientRectangle;
                        if (mouseOver | Focused)
                            g.DrawRectangle(ControlColors.AccentPen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
                        else
                            ControlPaint.DrawBorder(g, rectangle, Color.Black, ButtonBorderStyle.Solid);
                        if (string.IsNullOrWhiteSpace(Text) && !string.IsNullOrWhiteSpace(DefaultText))
                        {
                            //Font font = Font.Clone(); as Font;
                            TextRenderer.DrawText(g, DefaultText, Font, rectangle, Color.Gray, TextFormatFlags.Default | TextFormatFlags.VerticalCenter);
                            //g.DrawString(DefaultText,
                            //    new Font(Font.FontFamily, Font.Size, FontStyle.Italic),
                            //    Brushes.Gray, rectangle, new StringFormat() { LineAlignment = StringAlignment.Center });
                        }
                    }
                    //ReleaseDC(message.HWnd, hdc);
                }
            }
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.ResumeLayout(false);
        }
    }
}
