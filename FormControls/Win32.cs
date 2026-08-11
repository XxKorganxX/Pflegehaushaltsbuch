using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom win32 control used by the application user interface.
    /// </summary>
    public class Win32
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const byte AC_SRC_ALPHA = 0x01;
        public const byte AC_SRC_OVER = 0x00;
        public const Int32 ULW_ALPHA = 0x02;
        #region MoveTitleLessWindows
        public const int HT_CAPTION = 0x2;
        /// <summary>
        /// Runs the send Message operation and updates the related application state.
        /// </summary>
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// Runs the release Capture operation and updates the related application state.
        /// </summary>
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion
        /// <summary>
        /// Represents the pOINT data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            /// <summary>
            /// Creates a new POINT instance and initializes the required state.
            /// </summary>
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        /// <summary>
        /// Represents the bLENDFUNCTION data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }
        /// <summary>
        /// Represents the sIZE data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public Int32 cx;
            public Int32 cy;
            /// <summary>
            /// Creates a new SIZE instance and initializes the required state.
            /// </summary>
            public SIZE(Int32 cx, Int32 cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }
        /// <summary>
        /// Gets the window Long value from the current application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        /// <summary>
        /// Sets the window Long value and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /// <summary>
        /// Gets the window DC value from the current application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        /// <summary>
        /// Creates the compatible DC data or user interface element for the current workflow.
        /// </summary>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        /// <summary>
        /// Runs the select Object operation and updates the related application state.
        /// </summary>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        /// <summary>
        /// Updates the layered Window data and refreshes the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
            ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc,
            Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
        /// <summary>
        /// Runs the release DC operation and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        /// <summary>
        /// Deletes the dC data from the current workflow.
        /// </summary>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);
        /// <summary>
        /// Deletes the object data from the current workflow.
        /// </summary>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// Sets the scroll Pos value and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        /// <summary>
        /// Gets the scroll Pos value from the current application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hwnd, int nBar);
        /// <summary>
        /// Runs the post Message operation and updates the related application state.
        /// </summary>
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        /// <summary>
        /// Represents the dRAWTEXTPARAMS data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DRAWTEXTPARAMS
        {
            public uint cbSize;
            public int iTabLength;
            public int iLeftMargin;
            public int iRightMargin;
            public uint uiLengthDrawn;
        }
        /// <summary>
        /// Gets the dC Ex value from the current application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);
        /// <summary>
        /// Draws the text output on the provided graphics surface.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr DrawText(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);
        [DllImport("user32.dll")]
        static extern int DrawText(IntPtr hdc, StringBuilder lpchText, int cchText,
            ref RECT lprc, uint dwDTFormat);//, ref DRAWTEXTPARAMS lpDTParams);
        //private const int DT_TOP = 0x00000000;
        //private const int DT_LEFT = 0x00000000;
        //private const int DT_CENTER = 0x00000001;
        //private const int DT_RIGHT = 0x00000002;
        //private const int DT_VCENTER = 0x00000004;
        //private const int DT_BOTTOM = 0x00000008;
        //private const int DT_WORDBREAK = 0x00000010;
        //private const int DT_SINGLELINE = 0x00000020;
        //private const int DT_EXPANDTABS = 0x00000040;
        //private const int DT_TABSTOP = 0x00000080;
        //private const int DT_NOCLIP = 0x00000100;
        //private const int DT_EXTERNALLEADING = 0x00000200;
        //private const int DT_CALCRECT = 0x00000400;
        //private const int DT_NOPREFIX = 0x00000800;
        //private const int DT_INTERNAL = 0x00001000;
        //private const int DT_EDITCONTROL = 0x00002000;
        //private const int DT_PATH_ELLIPSIS = 0x00004000;
        //private const int DT_END_ELLIPSIS = 0x00008000;
        //private const int DT_MODIFYSTRING = 0x00010000;
        //private const int DT_RTLREADING = 0x00020000;
        //private const int DT_WORD_ELLIPSIS = 0x00040000;
        //private const int DT_NOFULLWIDTHCHARBREAK = 0x00080000;
        //private const int DT_HIDEPREFIX = 0x00100000;
        //private const int DT_PREFIXONLY = 0x00200000;
        /// <summary>
        /// Represents the pAINTSTRUCT data used by the application.
        /// </summary>
     [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }
        /// <summary>
        /// Runs the begin Paint operation and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);
        /// <summary>
        /// Runs the end Paint operation and updates the related application state.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);
        /// <summary>
        /// Sets the bk Color value and updates the related application state.
        /// </summary>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SetBkColor(IntPtr handleWindow, int color);
        /// <summary>
        /// Sets the text Color value and updates the related application state.
        /// </summary>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SetTextColor(IntPtr handleWindow, int color);
        /// <summary>
        /// Creates the solid Brush data or user interface element for the current workflow.
        /// </summary>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(uint color);
        /// <param name="handleWindow"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <summary>
        /// Sets the bk Mode value and updates the related application state.
        /// </summary>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SetBkMode(IntPtr handleWindow, int color);
        public const int DWM_BB_ENABLE = 0x1; 
        #region Structures
        /// <summary>
        /// Represents the dWM BLURBEHIND data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_BLURBEHIND
        {
            public int dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }
        /// <summary>
        /// Represents the mARGINS data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }
        #endregion //Structures
        /// <summary>
        /// Runs the dwm Enable Blur Behind Window operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
        /// <summary>
        /// Runs the dwm Extend Frame Into Client Area operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);
        /// <summary>
        /// Runs the dwm Is Composition Enabled operation and updates the related application state.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();
        /*
        /// <summary>
        /// Represents the rECT data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            /// <summary>
            /// Creates a new RECT instance and initializes the required state.
            /// </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public Rectangle Rect { get { return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top); } }
            /// <summary>
            /// Runs the from XYWH operation and updates the related application state.
            /// </summary>
            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x,
                                y,
                                x + width,
                                y + height);
            }
            /// <summary>
            /// Runs the from Rectangle operation and updates the related application state.
            /// </summary>
            public static RECT FromRectangle(Rectangle rect)
            {
                return new RECT(rect.Left,
                                 rect.Top,
                                 rect.Right,
                                 rect.Bottom);
            }
        }
        */
        /// <summary>
        /// Represents the rECT data used by the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
            /// <summary>
            /// Creates a new RECT instance and initializes the required state.
            /// </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
            /// <summary>
            /// Creates a new RECT instance and initializes the required state.
            /// </summary>
            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }
            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }
            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }
            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }
            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }
            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }
            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }
            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }
            /// <summary>
            /// Creates a new RECT instance and initializes the required state.
            /// </summary>
            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }
            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }
            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }
            /// <summary>
            /// Runs the equals operation and updates the related application state.
            /// </summary>
            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }
            /// <summary>
            /// Runs the equals operation and updates the related application state.
            /// </summary>
            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }
            /// <summary>
            /// Gets the hash Code value from the current application state.
            /// </summary>
            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }
            /// <summary>
            /// Creates the string booking data for the current workflow.
            /// </summary>
            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
        /// <summary>
        /// Runs the hi Word operation and updates the related application state.
        /// </summary>
        public static int HiWord(int number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            else
                return (number >> 16) & 0xffff;
        }
        /// <summary>
        /// Runs the lo Word operation and updates the related application state.
        /// </summary>
        public static int LoWord(int number)
        {
            return number & 0xffff;
        }
        public const int WS_EX_CLIENTEDGE = unchecked((int)0x00000200);
        public const int WS_BORDER = unchecked((int)0x00800000);
        
        public const int WM_NULL = 0x00;
        public const int WM_CREATE = 0x01;
        public const int WM_DESTROY = 0x02;
        public const int WM_MOVE = 0x03;
        public const int WM_SIZE = 0x05;
        public const int WM_ACTIVATE = 0x06;
        public const int WM_SETFOCUS = 0x07;
        public const int WM_KILLFOCUS = 0x08;
        public const int WM_ENABLE = 0x0A;
        public const int WM_SETREDRAW = 0x0B;
        public const int WM_SETTEXT = 0x0C;
        public const int WM_GETTEXT = 0x0D;
        public const int WM_GETTEXTLENGTH = 0x0E;
        public const int WM_PAINT = 0x0F;
        public const int WM_CLOSE = 0x10;
        public const int WM_QUERYENDSESSION = 0x11;
        public const int WM_QUIT = 0x12;
        public const int WM_QUERYOPEN = 0x13;
        public const int WM_ERASEBKGND = 0x14;
        public const int WM_SYSCOLORCHANGE = 0x15;
        public const int WM_ENDSESSION = 0x16;
        public const int WM_SYSTEMERROR = 0x17;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_CTLCOLOR = 0x19;
        public const int WM_WININICHANGE = 0x1A;
        public const int WM_SETTINGCHANGE = 0x1A;
        public const int WM_DEVMODECHANGE = 0x1B;
        public const int WM_ACTIVATEAPP = 0x1C;
        public const int WM_FONTCHANGE = 0x1D;
        public const int WM_TIMECHANGE = 0x1E;
        public const int WM_CANCELMODE = 0x1F;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_CHILDACTIVATE = 0x22;
        public const int WM_QUEUESYNC = 0x23;
        public const int WM_GETMINMAXINFO = 0x24;
        public const int WM_PAINTICON = 0x26;
        public const int WM_ICONERASEBKGND = 0x27;
        public const int WM_NEXTDLGCTL = 0x28;
        public const int WM_SPOOLERSTATUS = 0x2A;
        public const int WM_DRAWITEM = 0x2B;
        public const int WM_MEASUREITEM = 0x2C;
        public const int WM_DELETEITEM = 0x2D;
        public const int WM_VKEYTOITEM = 0x2E;
        public const int WM_CHARTOITEM = 0x2F;
        public const int WM_SETFONT = 0x30;
        public const int WM_GETFONT = 0x31;
        public const int WM_SETHOTKEY = 0x32;
        public const int WM_GETHOTKEY = 0x33;
        public const int WM_QUERYDRAGICON = 0x37;
        public const int WM_COMPAREITEM = 0x39;
        public const int WM_COMPACTING = 0x41;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_WINDOWPOSCHANGED = 0x47;
        public const int WM_POWER = 0x48;
        public const int WM_COPYDATA = 0x4A;
        public const int WM_CANCELJOURNAL = 0x4B;
        public const int WM_NOTIFY = 0x4E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x50;
        public const int WM_INPUTLANGCHANGE = 0x51;
        public const int WM_TCARD = 0x52;
        public const int WM_HELP = 0x53;
        public const int WM_USERCHANGED = 0x54;
        public const int WM_NOTIFYFORMAT = 0x55;
        public const int WM_CONTEXTMENU = 0x7B;
        public const int WM_STYLECHANGING = 0x7C;
        public const int WM_STYLECHANGED = 0x7D;
        public const int WM_DISPLAYCHANGE = 0x7E;
        public const int WM_GETICON = 0x7F;
        public const int WM_SETICON = 0x80;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCDESTROY = 0x82;
        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCPAINT = 0x85;
        public const int WM_NCACTIVATE = 0x86;
        public const int WM_GETDLGCODE = 0x87;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCLBUTTONDBLCLK = 0xA3;
        public const int WM_NCRBUTTONDOWN = 0xA4;
        public const int WM_NCRBUTTONUP = 0xA5;
        public const int WM_NCRBUTTONDBLCLK = 0xA6;
        public const int WM_NCMBUTTONDOWN = 0xA7;
        public const int WM_NCMBUTTONUP = 0xA8;
        public const int WM_NCMBUTTONDBLCLK = 0xA9;
        public const int WM_INPUT = 0xFF;
        public const int WM_KEYFIRST = 0x100;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x102;
        public const int WM_DEADCHAR = 0x103;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WM_SYSCHAR = 0x106;
        public const int WM_SYSDEADCHAR = 0x107;
        public const int WM_KEYLAST = 0x108;
        public const int WM_IME_STARTCOMPOSITION = 0x10D;
        public const int WM_IME_ENDCOMPOSITION = 0x10E;
        public const int WM_IME_COMPOSITION = 0x10F;
        public const int WM_IME_KEYLAST = 0x10F;
        public const int WM_INITDIALOG = 0x110;
        public const int WM_COMMAND = 0x111;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_TIMER = 0x113;
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;
        public const int WM_INITMENU = 0x116;
        public const int WM_INITMENUPOPUP = 0x117;
        public const int WM_MENUSELECT = 0x11F;
        public const int WM_MENUCHAR = 0x120;
        public const int WM_ENTERIDLE = 0x121;
        public const int WM_CTLCOLORMSGBOX = 0x132;
        public const int WM_CTLCOLOREDIT = 0x133;
        public const int WM_CTLCOLORLISTBOX = 0x134;
        public const int WM_CTLCOLORBTN = 0x135;
        public const int WM_CTLCOLORDLG = 0x136;
        public const int WM_CTLCOLORSCROLLBAR = 0x137;
        public const int WM_CTLCOLORSTATIC = 0x138;
        public const int WM_MOUSEFIRST = 0x200;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_MOUSEHWHEEL = 0x20E;
        public const int WM_PARENTNOTIFY = 0x210;
        public const int WM_ENTERMENULOOP = 0x211;
        public const int WM_EXITMENULOOP = 0x212;
        public const int WM_NEXTMENU = 0x213;
        public const int WM_SIZING = 0x214;
        public const int WM_CAPTURECHANGED = 0x215;
        public const int WM_MOVING = 0x216;
        public const int WM_POWERBROADCAST = 0x218;
        public const int WM_DEVICECHANGE = 0x219;
        public const int WM_MDICREATE = 0x220;
        public const int WM_MDIDESTROY = 0x221;
        public const int WM_MDIACTIVATE = 0x222;
        public const int WM_MDIRESTORE = 0x223;
        public const int WM_MDINEXT = 0x224;
        public const int WM_MDIMAXIMIZE = 0x225;
        public const int WM_MDITILE = 0x226;
        public const int WM_MDICASCADE = 0x227;
        public const int WM_MDIICONARRANGE = 0x228;
        public const int WM_MDIGETACTIVE = 0x229;
        public const int WM_MDISETMENU = 0x230;
        public const int WM_ENTERSIZEMOVE = 0x231;
        public const int WM_EXITSIZEMOVE = 0x232;
        public const int WM_DROPFILES = 0x233;
        public const int WM_MDIREFRESHMENU = 0x234;
        public const int WM_IME_SETCONTEXT = 0x281;
        public const int WM_IME_NOTIFY = 0x282;
        public const int WM_IME_CONTROL = 0x283;
        public const int WM_IME_COMPOSITIONFULL = 0x284;
        public const int WM_IME_SELECT = 0x285;
        public const int WM_IME_CHAR = 0x286;
        public const int WM_IME_KEYDOWN = 0x290;
        public const int WM_IME_KEYUP = 0x291;
        public const int WM_MOUSEHOVER = 0x2A1;
        public const int WM_NCMOUSELEAVE = 0x2A2;
        public const int WM_MOUSELEAVE = 0x2A3;
        public const int WM_CUT = 0x300;
        public const int WM_COPY = 0x301;
        public const int WM_PASTE = 0x302;
        public const int WM_CLEAR = 0x303;
        public const int WM_UNDO = 0x304;
        public const int WM_RENDERFORMAT = 0x305;
        public const int WM_RENDERALLFORMATS = 0x306;
        public const int WM_DESTROYCLIPBOARD = 0x307;
        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_PAINTCLIPBOARD = 0x309;
        public const int WM_VSCROLLCLIPBOARD = 0x30A;
        public const int WM_SIZECLIPBOARD = 0x30B;
        public const int WM_ASKCBFORMATNAME = 0x30C;
        public const int WM_CHANGECBCHAIN = 0x30D;
        public const int WM_HSCROLLCLIPBOARD = 0x30E;
        public const int WM_QUERYNEWPALETTE = 0x30F;
        public const int WM_PALETTEISCHANGING = 0x310;
        public const int WM_PALETTECHANGED = 0x311;
        public const int WM_HOTKEY = 0x312;
        public const int WM_PRINT = 0x317;
        public const int WM_PRINTCLIENT = 0x318;
        public const int WM_HANDHELDFIRST = 0x358;
        public const int WM_HANDHELDLAST = 0x35F;
        public const int WM_PENWINFIRST = 0x380;
        public const int WM_PENWINLAST = 0x38F;
        public const int WM_COALESCE_FIRST = 0x390;
        public const int WM_COALESCE_LAST = 0x39F;
        public const int WM_DDE_FIRST = 0x3E0;
        public const int WM_DDE_INITIATE = 0x3E0;
        public const int WM_DDE_TERMINATE = 0x3E1;
        public const int WM_DDE_ADVISE = 0x3E2;
        public const int WM_DDE_UNADVISE = 0x3E3;
        public const int WM_DDE_ACK = 0x3E4;
        public const int WM_DDE_DATA = 0x3E5;
        public const int WM_DDE_REQUEST = 0x3E6;
        public const int WM_DDE_POKE = 0x3E7;
        public const int WM_DDE_EXECUTE = 0x3E8;
        public const int WM_DDE_LAST = 0x3E8;
        public const int WM_USER = 0x400;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_APP = 0x8000;
    }
}
