using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents the Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class Form : System.Windows.Forms.Form
    {
        public static Font baseFont = new Font("Segoe UI", 10.0f);
        public static int BackColorMode = 0;
        protected SQLBase sql { get; set; }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public virtual void OnUserRights(SQLBase sql)
        {
            this.sql = sql;
        }
        public override Font Font
        {
            get
            {
                return baseFont;
            }
            set 
            {
                base.Font = value;
            }
        }
        /// <summary>
        /// Creates a new Form instance and initializes the required state.
        /// </summary>
        public Form()
        {
            InitializeComponent();
            //this.SuspendLayout();
            //1.
            //2.
            DoubleBuffered = true;
            this.Font = baseFont;
            //SetStyle(ControlStyles.UserPaint, true);
            //this.ResumeLayout(false);
        }
        /// <summary>
        /// Runs the window Move operation and updates the related application state.
        /// </summary>
        public void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32.ReleaseCapture();
                Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HT_CAPTION, 0);
            }
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //if (bitmap == null)
            //{
            //bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            //using (Graphics g = Graphics.FromImage(bitmap))
            //{
            Graphics g = e.Graphics;
            if (ClientRectangle.Height == 0)
                return;
            Rectangle rect = ClientRectangle;
            //Blue
            if (BackColorMode == 0)
            {
                Label.GradiantColor1 = Color.FromArgb(24, 73, 132);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, ClientRectangle.Height),
                        Color.FromArgb(149, 182, 213),
                        Color.FromArgb(24, 73, 132)
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Orange
            else if (BackColorMode == 1)
            {
                Label.GradiantColor1 = ControlPaint.Dark(Color.Orange, 0.25f);// Color.FromArgb(24, 73, 132);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, ClientRectangle.Height),
                        //ControlPaint.Light(Color.Orange),
                        Color.FromArgb(
                            Math.Min(255, Color.Orange.R + 20),
                            Math.Min(255, Color.Orange.G + 20),
                            Math.Min(255, Color.Orange.B + 20)),
                            Color.FromArgb(
                            Math.Min(255, (int)Color.Orange.R),// + 20),
                            Math.Min(255, (int)Color.Orange.G),// + 20),
                            Math.Min(255, (int)Color.Orange.B))// + 20))
                                                               //Color.FromArgb(
                                                               //    Color.Orange.R, 
                                                               //    Math.Max(0,Color.Orange.G - 40), 
                                                               //    Math.Max(0,Color.Orange.B - 40))
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Pink
            else if (BackColorMode == 2)
            {
                Label.GradiantColor1 = Color.FromArgb(
                        Math.Max(0, Color.Pink.R - 40),
                        Math.Max(0, Color.Pink.G - 40),
                        Math.Max(0, Color.Pink.B - 40));
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, Bounds.Height),
                        //ControlPaint.Light(Color.Orange),
                        Color.FromArgb(
                            Math.Min(255, Color.Pink.R + 20),
                            Math.Min(255, Color.Pink.G + 20),
                            Math.Min(255, Color.Pink.B + 20)),
                        Color.FromArgb(
                            Math.Max(0, Color.Pink.R - 40),
                            Math.Max(0, Color.Pink.G - 40),
                            Math.Max(0, Color.Pink.B - 40))
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Gray
            else
            {
                Label.GradiantColor1 = ControlPaint.Light(Color.Black, 0.4f);// Color.FromArgb(24, 73, 132);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    Point.Empty,
                    new Point(0, ClientRectangle.Height),
                    Color.Gray,
                    ControlPaint.Light(Color.Black, 0.4f)
                ))
                    g.FillRectangle(brush, rect);
            }
            //}
            //}
            //e.Graphics.DrawImage(bitmap, ClientRectangle );
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //}
        //protected override void OnResizeBegin(EventArgs e)
        //{
        //    base.OnResizeBegin(e);
        //}
        //protected override void OnResizeEnd(EventArgs e)
        //{
        //    base.OnResizeEnd(e);
        //}
    }
}
