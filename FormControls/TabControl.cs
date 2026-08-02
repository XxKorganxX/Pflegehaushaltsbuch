using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom tab Control control used by the application user interface.
    /// </summary>
    class TabControl : System.Windows.Forms.TabControl
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);//, IntPtr hrgnclip);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out Pflegehaushaltsbuch.FormControls.Win32.RECT lpRect);
        private bool visibleTabs = true, autoSizeTabs = false;
        private float angleColorGradiant = 90.0f, borderWidth = 1.0f;
        private Color tabTopColor = Color.FromArgb(252, 252, 252),
                      tabBottomColor = Color.FromArgb(207, 207, 207),
                      tabForeColor = Color.Black,
                      tabBorderColor = Color.DarkGray,
                      borderColor = Color.White,
                      selectedTabTopColor = Color.Green,
                      selectedTabBottomColor = Color.Green,
                      selectedTabForeColor = Color.White,
                      selectedTabBorderColor = Color.LightGreen,
                      tabBackcolor = Color.FromArgb(12, 27, 36);
        public bool VisibleTabs { get { return visibleTabs; } set { visibleTabs = value; } }
        public bool AutoSizeTabs { get { return autoSizeTabs; } set { autoSizeTabs = value; } }
        public float AngleColorGradiant { get { return angleColorGradiant; } set { angleColorGradiant = value; } }
        public float BorderWidth { get { return borderWidth; } set { borderWidth = value; } }
        public Color TabBackcolor { get { return tabBackcolor; } set { tabBackcolor = value; } }
        public Color TabForeColor { get { return tabForeColor; } set { tabForeColor = value; } }
        public Color TabTopColor { get { return tabTopColor; } set { tabTopColor = value; } }
        public Color TabBottomColor { get { return tabBottomColor; } set { tabBottomColor = value; } }
        public Color TabBorderColor { get { return tabBorderColor; } set { tabBorderColor = value; } }
        public Color SelectedTabForeColor { get { return selectedTabForeColor; } set { selectedTabForeColor = value; } }
        public Color SelectedTabTopColor { get { return selectedTabTopColor; } set { selectedTabTopColor = value; } }
        public Color SelectedTabBottomColor { get { return selectedTabBottomColor; } set { selectedTabBottomColor = value; } }
        public Color SelectedTabBoderColor { get { return selectedTabBorderColor; } set { selectedTabBorderColor = value; } }
        public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
        public override Font Font
        {
            get
            {
                return Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Tab Control instance and initializes the required state.
        /// </summary>
        public TabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, false);
            DoubleBuffered = false;
            //ControlStyles.SupportsTransparentBackColor
            //DrawMode = TabDrawMode.OwnerDrawFixed;
        }
        /// <summary>
        /// Handles the draw Item lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            
            TabPage page = TabPages[e.Index];
            Rectangle rect = GetTabRect(e.Index);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Brush brush = null;
            //ControlPaint.Light(backColor, 0.75f),
            //ControlPaint.Dark(backColor, 0)),
            if (e.State ==  DrawItemState.Checked)//(CheckedState && Checked))
            {
                brush = new LinearGradientBrush(
                   new Point(rect.Left, rect.Top),
                   new Point(rect.Left, rect.Bottom),
                   Color.LightGreen,
                   Color.Green);
            }
            else if (e.State == DrawItemState.Selected)// Focused || mouseOver)
            {
                brush = new LinearGradientBrush(
                   new Point(rect.Left, rect.Top),
                   new Point(rect.Left, rect.Bottom),
                   Color.White,
                   Color.Gray);
            }
            else
            {
                brush = new LinearGradientBrush(
                    new Point(rect.Left, rect.Top),
                    new Point(rect.Left, rect.Bottom),
                    Color.White,
                    Color.FromArgb(200, 200, 200));
            }
            //using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            {
                e.Graphics.FillRectangle(brush, rect);
                e.Graphics.DrawString(page.Text, Font, Brushes.Black, rect, sf);
            }
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
        //        return cp;
        //    }
        //}
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message 
            if (!VisibleTabs && m.Msg == 0x1328 && !DesignMode)
            {
            }
            else if (VisibleTabs &&         //m.Msg == Win32.WM_NCPAINT || 
                m.Msg == Win32.WM_ERASEBKGND)
                //m.Msg == Win32.WM_PAINT)
            {
                //get handle to a display device context
                IntPtr hdc = m.WParam;
                    //GetDC(m.HWnd);//, IntPtr.Zero, 0);// 1 | 0x0020);// m.WParam);
                if (hdc != IntPtr.Zero)
                {
                    //get Graphics object from the display device context
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        /*
                        Win32.RECT rect1;
                        GetWindowRect(m.HWnd, out rect1);
                        Rectangle rect = new Rectangle(rect1.left, rect1.top, rect1.right - rect1.left, rect1.bottom - rect1.top);
                        rect = RectangleToClient(rect);
                        `*/
                        Rectangle rect = ClientRectangle;
                        //Console.WriteLine(rect.ToString());
                        if (this.Parent != null)
                        {
                            GraphicsContainer cstate = g.BeginContainer();
                            g.TranslateTransform(-this.Left, -this.Top);
                            Rectangle clip = rect;
                            clip.Offset(this.Left, this.Top);
                            PaintEventArgs pe = new PaintEventArgs(g, clip);
                            //paint the container's bg
                            InvokePaintBackground(this.Parent, pe);
                            //paints the container fg
                            InvokePaint(this.Parent, pe);
                            //restores graphics to its original state
                            g.EndContainer(cstate);
                        }
                    }
                    m.Result = (IntPtr)1;
                    //g.Dispose();
                    //m.Result = (IntPtr)1;
                    //ReleaseDC(m.HWnd, hdc);
                }
            }
            //else if (m.Msg == Win32.WM_PAINT)
            //{
            //    m.Result = (IntPtr)1;
            //}
            /*
        else if ((m.Msg ==  TCM_ADJUSTRECT))
        {
            RECT rc = (RECT)m.GetLParam(typeof(RECT));
            //Adjust these values to suit, dependant upon Appearance
            rc.Left -= 3;
            rc.Right += 3;
            //rc.Top -= 3;
            rc.Bottom += 3;
            Marshal.StructureToPtr(rc, m.LParam, true);
                  
            base.WndProc(ref m);
        }
             */
            else
                base.WndProc(ref m);
        }
        private const Int32 TCM_FIRST = 0x1300;
        private const Int32 TCM_ADJUSTRECT = (TCM_FIRST + 40);
        //private struct RECT
        //{
        //    public Int32 Left;
        //    public Int32 Top;
        //    public Int32 Right;
        //    public Int32 Bottom;
        //}
        /// <summary>
        /// Handles the control Added lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //return;
            //if (!DesignMode && AutoSizeTabs)
            //    e.Control.VisibleChanged += new EventHandler(Control_VisibleChanged);
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // e.Graphics.Clear(Color.Transparent);
            PaintBackgroundTest(e);
            //e.Graphics.Clear(Color.Transparent);
        }
        /// <summary>
        /// Runs the paint Background Test operation and updates the related application state.
        /// </summary>
        private void PaintBackgroundTest(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this.Parent != null)
            {
                GraphicsContainer cstate = e.Graphics.BeginContainer();
                e.Graphics.TranslateTransform(-this.Left, -this.Top);
                Rectangle clip = e.ClipRectangle;
                clip.Offset(this.Left, this.Top);
                PaintEventArgs pe = new PaintEventArgs(e.Graphics, clip);
                //paint the container's bg
                InvokePaintBackground(this.Parent, pe);
                //paints the container fg
                InvokePaint(this.Parent, pe);
                //restores graphics to its original state
                e.Graphics.EndContainer(cstate);
            }
            //return;
            //e.Graphics.Clear(TabBackcolor);
            //TabPage tab = SelectedTab;
            //if (tab != null)
            //{
            //    rect.X -= 1;
            //    rect.Y -= 1;
            //    rect.Width += 1;
            //    rect.Height += 1;
            //}
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Default | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            //StringFormat format = new StringFormat();
            //format.Alignment = StringAlignment.Center;
            //format.LineAlignment = StringAlignment.Center;
            if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
            {
                //format.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
            }
            for (int i = 0; i < TabPages.Count; i++)
            {
                TabPage tab = TabPages[i];
                Rectangle rect = GetTabRect(i);
                Color backColorTop;
                Color backColorBottom;
                Color foreColor;
                if (SelectedTab == tab)
                {
                    backColorTop = selectedTabTopColor;
                    backColorBottom = selectedTabBottomColor;
                    foreColor = selectedTabForeColor;
                }
                else
                {
                    backColorTop = tabTopColor;
                    backColorBottom = tabBottomColor;
                    foreColor = tabForeColor;
                }
                var brush = new LinearGradientBrush(rect, backColorTop, backColorBottom, angleColorGradiant);
                e.Graphics.FillRectangle(brush, rect);
                Rectangle rectBorder = rect;
                rectBorder.Width -= 1;
                rectBorder.Height -= 1;
                if (SelectedTab != tab)
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(tabBorderColor), 1.0f), rectBorder);
                else
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(selectedTabBorderColor), 1.0f), rectBorder);
                Color forecColor = Color.Black;
                if (!Enabled)
                    forecColor = Color.DimGray;
                TextRenderer.DrawText(e.Graphics, tab.Text, Font, rect, forecColor, flags );
                //e.Graphics.DrawString(tab.Text, Font, new SolidBrush(foreColor), rect, format);
            }
        }
        void Control_VisibleChanged(object sender, EventArgs e)
        {
            if (DesignMode && !autoSizeTabs)
                return;
            Control c = (sender as Control);
            AutoSizeHeight();
        }
        /// <summary>
        /// Runs the auto Size Height operation and updates the related application state.
        /// </summary>
        private void AutoSizeHeight()
        {
            //return;
            ////if (VisibleTabs)
            ////    return;
            //if (SelectedTab == null || SelectedTab.Controls.Count == 0 || SelectedTab.Controls[0] == null)
            //    return;
            //Control c = SelectedTab.Controls[0];
            //Size preferredSize = c.GetPreferredSize(PreferredSize);
            //Height = Math.Max(preferredSize.Height, 50);
            //if (DesignMode)
            //    Height = Math.Max(preferredSize.Height + 35, 50); //30 entspricht Höhe des Menues
            //else
            //    Height = Math.Max(preferredSize.Height, 50);
        }
        /// <summary>
        /// Runs the show Tab operation and updates the related application state.
        /// </summary>
        public void ShowTab(string tabName)
        {
            SelectedTab = TabPages[tabName];
        }
    }
}
