using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom checked List Box control used by the application user interface.
    /// </summary>
    public class CheckedListBox : System.Windows.Forms.CheckedListBox
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);
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
            if (message.Msg == Win32.WM_NCPAINT || message.Msg == Win32.WM_ERASEBKGND ||
                message.Msg == Win32.WM_PAINT)
            {
                //get handle to a display device context
                IntPtr hdc = GetDCEx(message.HWnd, message.WParam, 1 | 0x0020);
                if (hdc != IntPtr.Zero)
                {
                    //get Graphics object from the display device context
                    Graphics g = Graphics.FromHdc(hdc);
                    Rectangle rectangle = new Rectangle(0, 0, width, height);
                    ControlPaint.DrawBorder(g, rectangle, Color.Black,
                                 ButtonBorderStyle.Solid);
                    message.Result = (IntPtr)1;
                    ReleaseDC(message.HWnd, hdc);
                }
            }
        }
    }
}
