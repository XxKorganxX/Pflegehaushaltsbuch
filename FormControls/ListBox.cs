using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom list Box control used by the application user interface.
    /// </summary>
    public class ListBox : System.Windows.Forms.ListBox
    {
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
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        public override int ItemHeight
        {
            get
            {
                return Font.Height;
            }
        }
        /// <summary>
        /// Creates a new List Box instance and initializes the required state.
        /// </summary>
        public ListBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            if (Program.DesignMode)
                return;
            DrawItem += ListBox_DrawItem;
        }
        public override DrawMode DrawMode 
        {
            get { return base.DrawMode; }
            set { }
        }
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            DrawBorder(ref m, Width, Height);
        }
        /// <summary>
        /// Draws the border output on the provided graphics surface.
        /// </summary>
        public void DrawBorder(ref Message message, int width, int height)
        {
            if (//message.Msg == Win32.WM_NCPAINT || message.Msg == Win32.WM_ERASEBKGND ||
                message.Msg == Win32.WM_PAINT)
            {
                //get handle to a display device context
                IntPtr hdc = Win32.GetDCEx(message.HWnd, message.WParam, 1 | 0x0020);
                if (hdc != IntPtr.Zero)
                {
                    //get Graphics object from the display device context
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        Rectangle rectangle = new Rectangle(0, 0, width, height);
                        ControlPaint.DrawBorder(g, rectangle, Color.Black,
                                     ButtonBorderStyle.Solid);
                    }
                    //message.Result = (IntPtr)1;
                    Win32.ReleaseDC(message.HWnd, hdc);
                }
            }
        }
        void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) 
                return;
            var selectionBackColor = ColorSet.ListSelectBackColor;
                //Color.FromArgb(144, 177, 210);
            // Draw the current item text
            {
                //Graphics g = bg.Graphics;// 
                //Graphics g = Graphics.FromImage(bitmap);
                if (Items != null)
                {
                    Color backColor = Color.White;
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        backColor = selectionBackColor;
                    string text = "";
                    if (string.IsNullOrWhiteSpace(DisplayMember))
                        text = Items[e.Index].ToString();
                    else
                    {
                        object o = Items[e.Index];
                        if (o is DataRowView)
                        {
                            DataRowView row = o as DataRowView;
                            text = row[DisplayMember].ToString();
                        }
                        else if(o != null)
                            text = o.ToString();
                    }
                    TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, e.ForeColor, backColor, TextFormatFlags.Default);
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
            // ListBox
            // 
            this.ResumeLayout(false);
        }
    }
}
