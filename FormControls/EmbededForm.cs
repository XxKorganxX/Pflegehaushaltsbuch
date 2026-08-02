using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents the Embeded Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EmbededForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Runs the release DC operation and updates the related application state.
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        /// <summary>
        /// Gets the window DC value from the current application state.
        /// </summary>
        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        /// <summary>
        /// Runs the dwm Enable Blur Behind Window operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(
            IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);
        /// <summary>
        /// Runs the dwm Extend Frame Into Client Area operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(
            IntPtr hWnd, ref MARGINS pMargins);
        /// <summary>
        /// Runs the dwm Is Composition Enabled operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();
        /// <summary>
        /// Runs the dwm Enable Composition operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(bool bEnable);
        /// <summary>
        /// Runs the dwm Get Colorization Color operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(
            ref int pcrColorization,
        /// <summary>
        /// Runs the dwm Register Thumbnail operation and updates the related application state.
        /// </summary>
            [MarshalAs(UnmanagedType.Bool)]ref bool pfOpaqueBlend);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern IntPtr DwmRegisterThumbnail(
            IntPtr dest, IntPtr source);
        /// <summary>
        /// Runs the dwm Unregister Thumbnail operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);
        /// <summary>
        /// Runs the dwm Update Thumbnail Properties operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUpdateThumbnailProperties(
            IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);
        /// <summary>
        /// Runs the dwm Query Thumbnail Source Size operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmQueryThumbnailSourceSize(
            IntPtr hThumbnail, out Size size);
        /// <summary>
        /// Runs the dwm Is Composition Enabled operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(ref bool pfEnabled);
        /// <summary>
        /// Represents a custom dWM THUMBNAIL PROPERTIES control used by the application user interface.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class DWM_THUMBNAIL_PROPERTIES
        {
            public uint dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fSourceClientAreaOnly;
            public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
            public const uint DWM_TNP_RECTSOURCE = 0x00000002;
            public const uint DWM_TNP_OPACITY = 0x00000004;
            public const uint DWM_TNP_VISIBLE = 0x00000008;
            public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
        }
        /// <summary>
        /// Represents the mARGINS data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;      // width of left border that retains its size
            public int cxRightWidth;     // width of right border that retains its size
            public int cyTopHeight;      // height of top border that retains its size
            public int cyBottomHeight;   // height of bottom border that retains its size
        };
        /// <summary>
        /// Represents a custom dWM BLURBEHIND control used by the application user interface.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class DWM_BLURBEHIND
        {
            public uint dwFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fEnable;
            public IntPtr hRegionBlur;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fTransitionOnMaximized;
            public const uint DWM_BB_ENABLE = 0x00000001;
            public const uint DWM_BB_BLURREGION = 0x00000002;
            public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
        }
        /// <summary>
        /// Represents the rECT data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
            /// <summary>
            /// Creates a new RECT instance and initializes the required state.
            /// </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left; this.top = top;
                this.right = right; this.bottom = bottom;
            }
        }
        /// <summary>
        /// Represents the dWM COLORIZATION PARAMS data used by the application.
        /// </summary>
        private struct DWM_COLORIZATION_PARAMS
        {
            public uint clrColor;
            public uint clrAfterGlow;
            public uint nIntensity;
            public uint clrAfterGlowBalance;
            public uint clrBlurBalance;
            public uint clrGlassReflectionIntensity;
            public bool fOpaque;
            /// <summary>
            /// Creates a new DWM COLORIZATION PARAMS instance and initializes the required state.
            /// </summary>
            public DWM_COLORIZATION_PARAMS(
                uint clrColor,
                uint clrAfterGlow,
                uint nIntensity,
                uint clrAfterGlowBalance,
                uint clrBlurBalance,
                uint clrGlassReflectionIntensity,
                bool fOpaque)
            {
                this.clrColor = clrColor;
                this.clrAfterGlow = clrAfterGlow;
                this.nIntensity = nIntensity;
                this.clrAfterGlowBalance = clrAfterGlowBalance;
                this.clrBlurBalance = clrBlurBalance;
                this.clrGlassReflectionIntensity = clrGlassReflectionIntensity;
                this.fOpaque = fOpaque;
            }
        }
        /// <summary>
        /// Runs the dwm Set Colorization Parameters operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", EntryPoint = "#131", PreserveSig = false)]
        private static extern void DwmSetColorizationParameters(ref DWM_COLORIZATION_PARAMS parameters, bool unknown);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        /// <summary>
        /// Runs the send Message operation and updates the related application state.
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private Rectangle nonClientRect, closeButtonRect, maximizeRect, minimizeRect;
        private Control ParentNotice;
        public EmbededForm OwnerNotice { get; set; }
        /// <summary>
        /// Creates a new Embeded Form instance and initializes the required state.
        /// </summary>
        public EmbededForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            /*
            MARGINS margins = new MARGINS();
            margins.cxLeftWidth = 58;// LEFTEXTENDWIDTH;      // 8
            margins.cxRightWidth = -48;// RIGHTEXTENDWIDTH;    // 8
            margins.cyBottomHeight = 80;// BOTTOMEXTENDWIDTH; // 20
            margins.cyTopHeight = 27;// TOPEXTENDWIDTH;       // 27
            DwmExtendFrameIntoClientArea(GetWindowDC(Handle), ref margins);
            */
            //var t1 = ClientRectangle;
            //var t2 = DisplayRectangle;
            //var t3 = PreferredSize;
        }
        private bool minimize = false, maximize = false, connect = false;
 
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            //const int WM_NCPAINT = 0x85;
            //if(m.Msg == Win32.WM_NCHITTEST)
            //{
            //}
            //else 
            if (m.Msg == Win32.WM_ACTIVATE)
            {
                DrawNonClientArea(m.HWnd);
                //base.WndProc(ref m);
                /*
                // Extend the frame into the client area.
                MARGINS margins = new MARGINS();
                margins.cxLeftWidth = 58;// LEFTEXTENDWIDTH;      // 8
                margins.cxRightWidth = -48;// RIGHTEXTENDWIDTH;    // 8
                margins.cyBottomHeight = 80;// BOTTOMEXTENDWIDTH; // 20
                margins.cyTopHeight = 27;// TOPEXTENDWIDTH;       // 27
                DwmExtendFrameIntoClientArea(GetWindowDC(Handle), ref margins);
                 */
            }
            else if (m.Msg == Win32.WM_NCHITTEST)
            {
                //int x = unchecked((short)m.LParam);
                //int y = unchecked((short)((uint)m.LParam >> 16));
                //Rectangle rect = new Rectangle(x, y, 1, 1);
                //rect = this.RectangleToClient(rect);
                //rect.Y += SystemInformation.CaptionHeight + 8;
                //bool minimize = minimizeRect.IntersectsWith(rect);
                //bool maximize = maximizeRect.IntersectsWith(rect);
                //bool connect = closeButtonRect.IntersectsWith(rect);
                //Win32.ReleaseCapture();
                //if ((minimize != this.minimize) || (maximize != this.maximize) || (connect != this.connect))
                //{
                //    this.minimize = minimize;
                //    this.maximize = maximize;
                //    this.connect = connect;
                //    if (minimize && Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
                //        SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, 0x2, 0);
                //    else if (maximize && Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
                //        SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, 0x2, 0);
                //    if (connect && Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
                //        SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, 0x2, 0);
                //    DrawButtons();
                //    m.Result = (IntPtr)0x2;// Win32.HTCAPTION;
                //}
                //else
                //{
                //    if (WindowState == FormWindowState.Normal)
                //    {
                //        if (Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
                //            Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HT_CAPTION, 0);
                //    }
                //}
                //Console.WriteLine(unchecked((short)m.Result));
                //if (!minimize && !maximize && !connect)
                    base.WndProc(ref m);
                int result = unchecked((short)m.Result);
                bool paintButtons = false;
                if (result == 8) //Minimize
                {
                    if (!minimize)
                        paintButtons = true;
                    minimize = true;
                    maximize = false;
                    connect = false;
                }
                else if (result == 9) //Maximize
                {
                    if (!maximize)
                        paintButtons = true;
                    minimize = false;
                    maximize = true;
                    connect = false;
                }
                else if (result == 20) //Close Connect/Disconnect
                {
                    if (!connect)
                        paintButtons = true;
                    minimize = false;
                    maximize = false;
                    connect = true;
                }
                else
                {
                    if (minimize || maximize || connect)
                        paintButtons = true;
                    minimize = false;
                    maximize = false;
                    connect = false;
                }
                if (paintButtons)
                    DrawButtons();
                //DrawNonClientArea(m.HWnd);
            }
            else if (m.Msg == Win32.WM_NCLBUTTONDOWN)
            {
                int wParam  = unchecked((short)m.WParam);
                //Console.WriteLine(m.WParam);
                if (Parent == null)
                {
                    if (wParam == 8) //Minimize
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                    else if (wParam == 9) //Maximize
                    {
                        if (WindowState == FormWindowState.Maximized)
                            WindowState = FormWindowState.Normal;
                        else
                            WindowState = FormWindowState.Maximized;
                    }
                    else if (wParam == 20) //Close Connect/Disconnect
                    {
                        Connect();
                    }
                    else
                        base.WndProc(ref m);
                }
                else
                {
                    if (wParam == 20) //Close Connect/Disconnect
                    {
                        Disconnect();
                    }
                }
                /*
                int x = unchecked((short)m.LParam);
                int y = unchecked((short)((uint)m.LParam >> 16));
                Rectangle rect = new Rectangle(x, y, 1, 1);
                minimize = minimizeRect.IntersectsWith(rect);
                maximize = maximizeRect.IntersectsWith(rect);
                connect = closeButtonRect.IntersectsWith(rect);
                Console.WriteLine("NC Button Down");
                Console.WriteLine(minimize +" "+ maximize+" "+ connect);
                 */
                /*
                if(!minimize && !maximize && !connect)
                    base.WndProc(ref m);
                DrawNonClientArea(m.HWnd);
                 */
                 
            }
            else if (m.Msg == Win32.WM_NCLBUTTONUP)
            {
                //Console.WriteLine("NC Button Up");
                //int x = unchecked((short)m.LParam);
                //int y = unchecked((short)((uint)m.LParam >> 16));
                //Rectangle rect = new Rectangle(x, y, 1, 1);
                //if (minimize && minimizeRect.IntersectsWith(rect))
                //{ 
                //}
                //else if (maximize && maximizeRect.IntersectsWith(rect))
                //{ 
                //}
                //else if (connect && closeButtonRect.IntersectsWith(rect))
                //{ 
                //}
                //else
                //if(Parent == null)
                //    base.WndProc(ref m);
                //DrawNonClientArea(m.HWnd);
                //minimize = false; maximize = false; connect = false;
            }
            else if (m.Msg == Win32.WM_NCPAINT)
            {
                //base.WndProc(ref m);
                //Drabase.WndProc(ref m);wNonClientArea(m.HWnd);
                //base.WndProc(ref m);
                DrawNonClientArea(m.HWnd);
            }
            else if (m.Msg == Win32.WM_NCCALCSIZE)
            {
                //MARGINS mar = new MARGINS() { cxLeftWidth = 1, cxRightWidth=1,cyBottomHeight=1,cyTopHeight=1};
                //DwmExtendFrameIntoClientArea(Handle, ref mar);
                base.WndProc(ref m);
                Win32.RECT ncRect = (Win32.RECT)m.GetLParam(typeof(Win32.RECT));
                if (WindowState == FormWindowState.Normal)
                {
                    //Rectangle proposed = ncRect.Rect;
                    //ncRect = Win32.RECT.FromRectangle(proposed);
                    ncRect.Left -= 6;
                    ncRect.Top -= 6;
                    ncRect.Right += 6;
                    ncRect.Bottom += 6;
                    Marshal.StructureToPtr(ncRect, m.LParam, false);
                    m.Result = IntPtr.Zero;
                }
                else if (WindowState == FormWindowState.Maximized)
                {
                    ncRect.Left += 1;
                    ncRect.Top += 1;
                    ncRect.Right -= 1;
                    ncRect.Bottom -= 1;
                    Marshal.StructureToPtr(ncRect, m.LParam, false);
                    m.Result = IntPtr.Zero;
                }
                RecalcNonClientArea(new Rectangle(ncRect.Left, ncRect.Top, ncRect.Width, ncRect.Height));
            }
            else
                base.WndProc(ref m);
        }
        /// <summary>
        /// Disconnects the disconnect data source or control from the current workflow.
        /// </summary>
        public void Disconnect()
        {
            if (Parent == null)
                return;
            if (ParentNotice == null)
                ParentNotice = Parent;
            WindowState = FormWindowState.Normal;
            Parent = null;
            TopLevel = true;
            Owner = OwnerNotice;
        }
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public void Connect()
        {
            if (Parent != null)
                return;
            Owner = null;
            TopLevel = false;
            Parent = ParentNotice;
        }
        /// <summary>
        /// Draws the non Client Area output on the provided graphics surface.
        /// </summary>
        private void DrawNonClientArea(IntPtr handleWindow)
        {
                int captionHeight = SystemInformation.CaptionHeight;
                int test2 = SystemInformation.Border3DSize.Width;
                int borderWidth = 2;// SystemInformation.Border3DSize.Width + (SystemInformation.BorderSize.Width * 2);
                IntPtr hdc = GetWindowDC(Handle);
                Graphics gr = Graphics.FromHdc(hdc);//handleWindow));
                gr.Clear(Color.FromArgb(12, 27, 36));
                //nonClientRect = new Rectangle((int)rectF.X, (int)rectF.Y, (int)rectF.Width, (int)rectF.Height);
                nonClientRect = Bounds;
                nonClientRect.X = 0;
                nonClientRect.Y = 0;
                SolidBrush brush = new SolidBrush(Color.FromArgb(60, 72, 79));
                gr.FillRectangle(brush, new Rectangle(0, 0, nonClientRect.Width, captionHeight));
                gr.FillRectangle(brush, new Rectangle(new Point(0, captionHeight), new Size(Bounds.Width, borderWidth)));
                gr.FillRectangle(brush, new Rectangle(new Point(0, captionHeight + borderWidth), new Size(borderWidth, Bounds.Height)));
                gr.FillRectangle(brush, new Rectangle(new Point(0, Bounds.Height - borderWidth), new Size(Bounds.Width, borderWidth)));
                gr.FillRectangle(brush, new Rectangle(new Point(Bounds.Width - borderWidth, captionHeight), new Size(borderWidth, Bounds.Width - captionHeight)));
                // Draw the Window Title
                //Font titlebarFont = new Font(FontFamily.GenericSerif, 12.0f, (FontStyle.Italic | FontStyle.Bold));
                float textHeight = gr.MeasureString(this.Text, Font).Height;
                gr.DrawString(this.Text,// Font, Brushes.White, PointF.Empty); 
                    new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Regular), Brushes.White, new PointF((float)borderWidth, (captionHeight / 2 - textHeight / 2) + 2));
                gr.Dispose();
                ReleaseDC(Handle, hdc);
                DrawButtons();
                //ControlPaint.DrawCaptionButton(gr, closeButtonRect, CaptionButton.Close, ButtonState.Normal);
                //ControlPaint.DrawCaptionButton(gr, minimizeRect, CaptionButton.Minimize, ButtonState.Normal);
                //ControlPaint.DrawCaptionButton(gr, maximizeRect, CaptionButton.Maximize, ButtonState.Normal);
                //ControlPaint.DrawCaptionButton(gr, closeButtonRect, CaptionButton.Close, ButtonState.Normal);
                //ControlPaint.DrawCaptionButton(gr, minimizeRect, CaptionButton.Minimize, ButtonState.Normal);
                //ControlPaint.DrawCaptionButton(gr, maximizeRect, CaptionButton.Maximize, ButtonState.Normal);
        }
        /// <summary>
        /// Draws the buttons output on the provided graphics surface.
        /// </summary>
        private void DrawButtons()
        {
            IntPtr hdc = GetWindowDC(Handle);
            Graphics gr = Graphics.FromHdc(hdc);
            var backBrush = new SolidBrush(BackColor);
            if (minimize)
            {
                gr.FillRectangle(Brushes.Green, minimizeRect);
            }
            else
                gr.FillRectangle(backBrush, minimizeRect);
            if (maximize)
            {
                gr.FillRectangle(Brushes.Green, maximizeRect);
            }
            else
                gr.FillRectangle(backBrush, maximizeRect);
            if (connect)
            {
                gr.FillRectangle(Brushes.Green, closeButtonRect);
            }
            else
                gr.FillRectangle(backBrush, closeButtonRect);
            ///******************** Paint minimize symbol ***********************
            gr.DrawRectangle(new Pen(Brushes.DimGray), minimizeRect);
            gr.DrawRectangle(new Pen(Brushes.DimGray), maximizeRect);
            gr.DrawRectangle(new Pen(Brushes.DimGray), closeButtonRect);
            Rectangle min = minimizeRect;
            min.X += 14;
            min.Width = 9;
            min.Y += 12;
            min.Height = 2;
            gr.FillRectangle(Brushes.White, min);
            ///******************** Paint maximize symbol ***********************
            Rectangle max = maximizeRect;
            max.X += maximizeRect.Width / 2 - 4;
            max.Width = 7;
            max.Y += 7;
            max.Height = 7;
            gr.DrawRectangle(new Pen(Brushes.White), max);
            max.X += 2;
            max.Y -= 2;
            gr.DrawRectangle(new Pen(Brushes.White), max);
            ///******************** Paint pin symbol ***********************
            Rectangle connectRect = closeButtonRect;
            connectRect.X += closeButtonRect.Width / 2 - 9;
            connectRect.Y += 1;
            var pin = new Point[] 
            { 
                new Point(7, 4), new Point(7, 10),
                new Point(6, 10), new Point(12, 10),
                new Point(9, 10), new Point(9, 14),
                new Point(7, 4), new Point(11, 4),
                new Point(11, 4), new Point(11, 10),
                new Point(10, 4), new Point(10, 10),
            };
            for (int i = 0; i < pin.Length; i++)
            {
                pin[i].X += connectRect.X;
                pin[i].Y += connectRect.Y;
            }
            gr.DrawLines(new Pen(Brushes.White), pin);
            gr.Dispose();
            ReleaseDC(Handle, hdc);
        }
        /// <summary>
        /// Runs the recalc Non Client Area operation and updates the related application state.
        /// </summary>
        private void RecalcNonClientArea(Rectangle nonClientRect)//rect Message m)
        {
            //if (m.WParam.ToInt32() == 1)
            //{
                //nonClientRect = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
                
                //Console.Out.WriteLine("Recalc: {0}", nonClientRect.ToString());
                Size buttonSize = SystemInformation.CaptionButtonSize;
                buttonSize.Height -= 2;
                closeButtonRect = new Rectangle(new Point(Bounds.Width - buttonSize.Width - 5, 0),//((SystemInformation.CaptionHeight - buttonSize.Height) / 2) + 1),
                                               buttonSize);
                maximizeRect = new Rectangle(new Point(Bounds.Width - (buttonSize.Width * 2) - 10, 0),// ((SystemInformation.CaptionHeight - buttonSize.Height) / 2) + 1),
                                                buttonSize);
                minimizeRect = new Rectangle(new Point(Bounds.Width - (buttonSize.Width * 3) - 15, 0),//((SystemInformation.CaptionHeight - buttonSize.Height) / 2) + 1),
                                                  buttonSize);
            //}
        }
        /// <summary>
        /// Runs the paint Non Client Area operation and updates the related application state.
        /// </summary>
        private void PaintNonClientArea(IntPtr hWnd, IntPtr hRgn)
        {
            //CWin32.RECT rWnd = new CWin32.RECT();
            //if (CWin32.GetWindowRect(hWnd, ref rWnd) == 0)
            //    return;
            //Rectangle rClip = new Rectangle(0, 0, rWnd.Width, rWnd.Height);
            //IntPtr hDC = GetWindowDC(hWnd);
            //IntPtr hCDC = CreateCompatibleDC(hDC);
            //IntPtr hBitmap = CreateCompatibleBitmap(hDC, rClip.Width, rClip.Height);
            //CWin32.SelectObject(hCDC, hBitmap);
            //BitBlt(hCDC, 0, 0, rClip.Width, rClip.Height, hDC, 0, 0, (uint)CWin32.TernaryRasterOperations.SRCCOPY);
            //using (Graphics g = Graphics.FromHdc(hCDC))
            //{
            //    this.OnNonClientAreaPaint(new PaintEventArgs(g, rClip));
            //}
            //CWin32.BitBlt(hDC, 0, 0, rClip.Width, rClip.Height, hCDC, 0, 0, (uint)CWin32.TernaryRasterOperations.SRCCOPY);
            //CWin32.DeleteObject(hBitmap);
            //CWin32.DeleteDC(hCDC);
            //CWin32.ReleaseDC(hWnd, hDC);
        }
        //    //switch (m.Msg)
        //    //{
        //    //    case Win32.WM_NCPAINT:
        //    //        { 
        //    //        }
        //    //        break;
        //    //    default:
        //    //        base.WndProc(ref m);
        //    //        break;
        //    //}
        //    /*
        //        Select Case msg
        //Case WM_NCPAINT
        //    DefWindowProc hwnd, msg, wParam, lParam
        //    Form1.PaintActive
        //Case WM_NCACTIVATE
        //    If wParam Then
        //        ' The form is active.
        //        DefWindowProc hwnd, msg, wParam, lParam
        //        Form1.PaintActive
        //    Else
        //        ' The form is inactive.
        //        DefWindowProc hwnd, msg, wParam, lParam
        //        Form1.PaintInactive
        //    End If
        //Case WM_SETTEXT
        //    DefWindowProc hwnd, msg, wParam, lParam
        //    Form1.PaintActive
        //Case WM_SYSCOMMAND
        //    DefWindowProc hwnd, msg, wParam, lParam
        //    If wParam <> SC_CLOSE Then
        //        Form1.PaintActive
        //    End If
        //Case Else
        //    ' Invoke the original WindowProc.
        //    NewWindowProc = CallWindowProc( _
        //        OldWindowProc, hwnd, msg, wParam, _
        //        lParam)
        //    */
        //}
        
    }
}
