using Pflegehaushaltsbuch.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom group Box control used by the application user interface.
    /// </summary>
    class GroupBox : System.Windows.Forms.GroupBox
    {
        private Color borderColor = Color.White;
        private float borderWidth = 1.0f;
        public override Font Font
        {
            get
            {
                return Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Group Box instance and initializes the required state.
        /// </summary>
        public GroupBox()
        {
            DoubleBuffered = true;
        }
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Refresh(); }
        }
        public float BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; Refresh(); }
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
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
            if(BackColor != Color.Transparent)
                e.Graphics.Clear(BackColor);
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            switch (Settings.Default.UIDesign)
            {
                case 0:
                    borderColor = Color.Black;
                    break;
                case 1:
                    //Orange
                    borderColor = Color.White;
                    break;
                case 2:
                    borderColor = Color.White;
                    break;
                case 3:
                    borderColor = Color.White;
                    break;
                case 4:
                    //blue
                    borderColor = Color.Black;
                    break;
            }
            Brush brush = new SolidBrush(borderColor);
            Pen penLight = new Pen(brush, borderWidth);
            
            Rectangle rectangle = base.ClientRectangle;
            Size s = Size.Ceiling(g.MeasureString(this.Text, this.Font, rectangle.Width, new StringFormat()));
            
            int offsetX = 8;
            int offsetY = s.Height / 2;// base.FontHeight / 2;
            List<Point> points = new List<Point>();
            if (BorderWidth != 0)
            {
                points.Add(new Point(8, offsetY));
                points.Add(new Point(0, offsetY));
                points.Add(new Point(0, (Height - 1)));
                points.Add(new Point(Width - 1, (Height - 1)));
                points.Add(new Point(Width - 1, offsetY));
                points.Add(new Point(8 + s.Width, offsetY));
                GraphicsPath path = new GraphicsPath();
                path.AddLines(points.ToArray());
                g.DrawPath(penLight, path);
            }
            g.DrawString(Text, Font, brush, new PointF(offsetX, 0));
            /*
            g.DrawLine(penLight, 1, offsetY, 1, (int)(base.Height - 1));
            g.DrawLine(penDark, 0, offsetY, 0, (int)(base.Height - 2));
            g.DrawLine(penLight, 0, (int)(base.Height - 1), base.Width, (int)(base.Height - 1));
            g.DrawLine(penDark, 0, (int)(base.Height - 2), (int)(base.Width - 1), (int)(base.Height - 2));
            g.DrawLine(penDark, 0, (int)(offsetY - 1), offsetX, (int)(offsetY - 1));
            g.DrawLine(penLight, 1, offsetY, offsetX, offsetY);
            g.DrawLine(penDark, (int)(offsetX + s.Width), (int)(offsetY - 1), (int)(base.Width - 2), (int)(offsetY - 1));
            g.DrawLine(penLight, (int)(offsetX + s.Width), offsetY, (int)(base.Width - 1), offsetY);
            g.DrawLine(penLight, (int)(base.Width - 1), (int)(offsetY - 1), (int)(base.Width - 1), (int)(base.Height - 1));
            g.DrawLine(penDark, (int)(base.Width - 2), offsetY, (int)(base.Width - 2), (int)(base.Height - 2));
             */
        }
    }
}
