using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom numeric Up Down control used by the application user interface.
    /// </summary>
    internal class NumericUpDown : System.Windows.Forms.NumericUpDown, INotifyPropertyChanged
    {
        /// <summary>
        /// Runs the find Window operation and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string sClassName, string sAppName);
        /// <summary>
        /// Runs the post Message operation and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(int hWnd, uint msg, int wParam, int lParam);
        /// <summary>
        /// Runs the kill Tab Tip operation and updates the related application state.
        /// </summary>
        private static void KillTabTip()
        {
            // Kill the previous process so the registry change will take effect.
            var processlist = Process.GetProcesses();
            foreach (var process in processlist.Where(process => process.ProcessName.Equals("TabTip")))
            {
                process.Kill();
                break;
            }
        }
        //RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\TabletTip\\1.7");
        //Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
        //http://stackoverflow.com/questions/36179917/automaticaly-pop-up-tablet-touch-keyboard-on-winforms-input-focus
        //[return: MarshalAs(UnmanagedType.Bool)]
        //[DllImport("user32.dll", SetLastError = true)]
        //extern bool PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);
        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //extern IntPtr FindWindow(String sClassName, String sAppName);
        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);
        ///// <summary>
        ///// Show the On Screen Keyboard
        ///// </summary>
        //#region ShowOSK
        //public static void ShowOnScreenKeyboard()
        //{
        //    IntPtr parent = FindWindow("Shell_TrayWnd", null);
        //    IntPtr child1 = FindWindowEx(parent, IntPtr.Zero, "TrayNotifyWnd", "");
        //    IntPtr keyboardWnd = FindWindowEx(child1, IntPtr.Zero, null, "Touch keyboard");
        //    uint WM_LBUTTONDOWN = 0x0201;
        //    uint WM_LBUTTONUP = 0x0202;
        //    UIntPtr x = new UIntPtr(0x01);
        //    UIntPtr x1 = new UIntPtr(0);
        //    IntPtr y = new IntPtr(0x0240012);
        //    PostMessage(keyboardWnd, WM_LBUTTONDOWN, x, y);
        //    PostMessage(keyboardWnd, WM_LBUTTONUP, x1, y);
        //}
        //#endregion ShowOSK
        ///// <summary>
        ///// Hide the On Screen Keyboard
        ///// </summary>
        //#region HideOSK
        //public static void HideOnScreenKeyboard()
        //{
        //    uint WM_SYSCOMMAND = 0x0112;
        //    UIntPtr SC_CLOSE = new UIntPtr(0xF060);
        //    IntPtr y = new IntPtr(0);
        //    IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
        //    PostMessage(KeyboardWnd, WM_SYSCOMMAND, SC_CLOSE, y);
        //}
        //#endregion HideOSK
        protected bool topDown, bottomDown, mouseOver;
        /// <summary>
        /// Creates a new Numeric Up Down instance and initializes the required state.
        /// </summary>
        public NumericUpDown()
        {
            InitializeComponent();
            SetStyle(ControlStyles.StandardClick, true);
            
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Controls[0].Visible = false;
            SetStyle(System.Windows.Forms.ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            if (Program.DesignMode)
                return;
            MouseUp += NumericUpDown_MouseUp;
            MouseDown += NumericUpDown_MouseDown;
        }
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
            set
            {
                OnFontChanged(new EventArgs());
            }
        }
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseOver = true;
        }
        /// <summary>
        /// Handles the mouse Leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            mouseOver = false;
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            Invalidate();
        }
        /// <summary>
        /// Handles the leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Invalidate();
        }
        void NumericUpDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.X < Width - 18)
                    return;
                if ((float)e.X <= Width - 9)
                    bottomDown = true;
                else
                    topDown = true;
            }
        }
        void NumericUpDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.X < Width - 18)
                    return;
                if ((float)e.X <= Width - 9)
                {
                    Value = Math.Max(0, Value - Increment);
                    bottomDown = false;
                }
                else
                {
                    Value += Increment;
                    topDown = false;
                }
            }
        }
        public new decimal Value 
        {
            get
            { 
                return base.Value; 
            }
            set 
            {
                if (base.Value == value)
                    return;
                base.Value = value;
            }
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Brush brushUp, brushDown;
            brushDown = new LinearGradientBrush(
                   new Point(rect.Left, rect.Top),
                   new Point(rect.Left, rect.Bottom),
                   Color.White,
                   Color.Gray);
            brushUp = new LinearGradientBrush(
                    new Point(rect.Left, rect.Top),
                    new Point(rect.Left, rect.Bottom),
                    Color.White,
                    Color.FromArgb(200, 200, 200));
            
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            //using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height))
            {
                //rect.Width -= 1;
                //rect.Height -= 1;
                Graphics gr = bg.Graphics;// Graphics.FromImage(bitmap);
                gr.Clear(Color.White);
                StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center };
                gr.DrawString(Value.ToString(), Font, new SolidBrush(ForeColor), rect, sf);
                Brush buttonUpColor = brushUp;
                Brush buttonDownColor = brushDown;
                rect = new Rectangle(rect.Width - 18, 0, 18, rect.Height);
                //if (!topDown)
                //gr.FillRectangle(buttonUpColor, new Rectangle(rect.X - rect.Height/2, rect.Y, rect.Width, rect.Height));
                //else
                gr.FillRectangle(buttonUpColor, new Rectangle(rect.X, rect.Y, 18, rect.Height));
                gr.DrawRectangle(Pens.Black, new Rectangle(rect.X, rect.Y, 18, rect.Height));
                /*
                RectangleF arrowsRect = new RectangleF(rect.X, rect.Y, rect.Width * 0.5f, rect.Width * 0.5f * 0.5f);
                arrowsRect.X += (rect.Width - arrowsRect.Width) * 0.5f;
                arrowsRect.Y += (rect.Height - arrowsRect.Height) * 0.5f;
                */
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                using (Bitmap arrowBitmap = new Bitmap(100, 100))
                {
                    Graphics graphicsArrow = Graphics.FromImage(arrowBitmap);
                    System.Drawing.Drawing2D.GraphicsPath pth = new System.Drawing.Drawing2D.GraphicsPath();
                    PointF TopLeft = new PointF(0, 0);
                    PointF TopRight = new PointF(100, 0);
                    PointF Bottom = new PointF(50, 100);
                    pth.AddLine(TopLeft, TopRight);
                    pth.AddLine(TopRight, Bottom);
                    pth.AddLine(Bottom, TopLeft);
                    graphicsArrow.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //Draw the arrow
                    graphicsArrow.FillPath(Brushes.Black, pth);
                    //gr.DrawPath(new Pen(Color.Black) { Width=10.0f}, pth);
                    RectangleF arrowLeft = rect;
                    //arrowLeft.X = arrowLeft.X + arrowLeft.Width * 0.05f;
                    gr.TranslateTransform((arrowLeft.X + arrowLeft.Width * 0.5f), (arrowLeft.Y + arrowLeft.Height * 0.5f));
                    gr.ScaleTransform(0.2f, 0.5f);
                    gr.TranslateTransform(-arrowLeft.Width * 0.9f, 0);
                    gr.RotateTransform(90);
                    gr.TranslateTransform(-(arrowLeft.X + arrowLeft.Width * 0.5f), -(arrowLeft.Y + arrowLeft.Height * 0.5f));
                    gr.DrawImage(arrowBitmap, arrowLeft);
                    gr.ResetTransform();
                    
                    RectangleF arrowRight = rect;
                    //arrowRight.X = arrowLeft.X + arrowLeft.Width * 0.65f;
                    gr.TranslateTransform((arrowRight.X + arrowRight.Width * 0.5f), (arrowRight.Y + arrowRight.Height * 0.5f));
                    gr.ScaleTransform(0.2f, 0.5f);
                    gr.TranslateTransform(arrowLeft.Width * 0.9f, 0);
                    gr.RotateTransform(270.0f);
                    gr.TranslateTransform(-(arrowRight.X + arrowRight.Width * 0.5f), -(arrowRight.Y + arrowRight.Height * 0.5f));
                    gr.DrawImage(arrowBitmap, arrowRight);
                    gr.ResetTransform();
                    
                    graphicsArrow.Dispose();
                }
                gr.ResetTransform();
                if(Focused)
                    gr.DrawRectangle(ControlColors.AccentPen, new Rectangle(0, 0, Width - 1, Height - 1));
                else
                    gr.DrawRectangle(Pens.Black, new Rectangle(0,0, Width-1, Height-1));
                //e.Graphics.DrawImage(bitmap, Point.Empty);
                
                //gr.Dispose();
                bg.Render();
            }
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // NumericUpDown
            // 
            this.Click += new System.EventHandler(this.NumericUpDown_Click);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NumericUpDown_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Handles the click event for numeric Up Down and updates the related state.
        /// </summary>
        private void NumericUpDown_Click(object sender, EventArgs e)
        {
            //Focus();
        }
        /// <summary>
        /// Handles the mouse Click event for numeric Up Down and updates the related state.
        /// </summary>
        private void NumericUpDown_MouseClick(object sender, MouseEventArgs e)
        {
            //ShowTouchKeyboard(true, true);
        }
        /// <summary>
        /// Runs the fire Property Changed operation and updates the related application state.
        /// </summary>
        protected void FirePropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
