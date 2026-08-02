using System;
using System.Collections.Generic;
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
    /// Represents a custom text Area control used by the application user interface.
    /// </summary>
    public class TextArea : System.Windows.Forms.RichTextBox
    {
        /// <summary>
        /// Creates a new Text Area instance and initializes the required state.
        /// </summary>
        public TextArea()
        {
            //Font = FormControls.Form.baseFont;
            ////Font = Font;
            //Type settingsType = typeof(Control);
            //var defaultFontField = settingsType.GetField("defaultFont", BindingFlags.Static | BindingFlags.NonPublic);
            //defaultFontField.SetValue(null, Font);// new Font("Segoe UI", 8.25F));
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles., false);
            MouseWheel += TextArea_MouseWheel;
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        /// <summary>
        /// Handles the mouse Wheel event for text Area and updates the related state.
        /// </summary>
        private void TextArea_MouseWheel(object sender, MouseEventArgs e)
        {
            //SuspendLayout();
            //Refresh();
            //ResumeLayout();
        }
        ////public override Font Font
        ////{
        ////    get
        ////    {
        ////        return FormControls.Form.baseFont;
        ////    }
        ////    set
        ////    {
        ////        base.Font = value;
        ////        OnFontChanged(new EventArgs());
        ////    }
        ////}
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (BackColor == Color.Transparent)
            {
                if (this.Parent != null)
                {
                    GraphicsContainer cstate = g.BeginContainer();
                    g.TranslateTransform(-this.Left, -this.Top);
                    Rectangle clip = ClientRectangle;
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
            else
                g.Clear(BackColor);
        }
        /// <summary>
        /// Runs the wnd Proc operation and updates the related application state.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            
            base.WndProc(ref m);
            if (m.Msg == Win32.WM_PAINT)
            {
                //IntPtr hdc = Win32.GetDCEx(m.HWnd, m.WParam, 1 | 0x0020);
                //System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                //timer.Start();
                //Win32.PAINTSTRUCT test;
                //Win32.BeginPaint(Handle, out test);
                //for (int i = 0; i < 10000; i++)
                {
                    //using (var g = Graphics.FromHdc(test.hdc))
                    using (var g = Graphics.FromHwnd(Handle))
                    //using (var g = CreateGraphics())
                    {
                        InvokePaintBackground(this, new PaintEventArgs(g, ClientRectangle));
                        InvokePaint(this, new PaintEventArgs(g, ClientRectangle));
                    }
                }
                //Win32.EndPaint(Handle, ref test);
                //timer.Stop();
                //Console.WriteLine(timer.Elapsed.ToString());
            }
            //if (m.Msg == Win32.WM_CLEAR)
            //{
            //    //Invoke((MethodInvoker)delegate
            //    //{
            //        using (var g = CreateGraphics())
            //    //});
            //}
                //if (m.Msg == Win32.WM_HSCROLL || m.Msg == Win32.WM_VSCROLL)// || m.Msg == Win32.WM_MOUSEHWHEEL)
                //{
                //    SuspendLayout();
                //    //Invalidate();
                //    ResumeLayout();
                //    //BeginInvoke((MethodInvoker)delegate
                //    //{
                //    //Update();
                //    //});
                //}
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //var g = e.Graphics;
            //if (this.Parent != null)
            //{
            //    GraphicsContainer cstate = g.BeginContainer();
            //    g.TranslateTransform(-this.Left, -this.Top);
            //    Rectangle clip = ClientRectangle;
            //    clip.Offset(this.Left, this.Top);
            //    PaintEventArgs pe = new PaintEventArgs(g, clip);
            //    //paint the container's bg
            //    //paints the container fg
            //    InvokePaint(this.Parent, pe);
            //    //restores graphics to its original state
            //    g.EndContainer(cstate);
            //}
            if (Text == null)
                return;
            Point p = new Point(0, 0);
            int startIndex = GetCharIndexFromPosition(p);
            //Point pEnd = new Point(Width, Height);
            //int lastIndex = GetCharIndexFromPosition(pEnd)+1;
            //var text = Text.Substring(startIndex, Math.Max(0, Math.Min(Text.Length, lastIndex - startIndex)));
            //if (string.IsNullOrWhiteSpace(text))
            //    return;
            //StringFormat sf = new StringFormat();
            using (var brush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, brush, ClientRectangle, new StringFormat() {FormatFlags = StringFormatFlags.FitBlackBox });
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TextArea
            // 
            this.ResumeLayout(false);
        }
    }
}
