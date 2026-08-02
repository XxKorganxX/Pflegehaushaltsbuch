using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom check Box control used by the application user interface.
    /// </summary>
    public class CheckBox : System.Windows.Forms.CheckBox
    {
        public Image CheckedImage { get; set; }
        public Image UnCheckedImage { get; set; }
        Color buttonBackColor = Color.Transparent,
                      borderColor = Color.DimGray;
        bool mouseOver = false;
        public override Font Font
        {
            get
            {
                return Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Check Box instance and initializes the required state.
        /// </summary>
        public CheckBox()
        {
            //DoubleBuffered = true;
        }
        [Category("Darstellung")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
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
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            {
                Graphics g = bg.Graphics;
                if (this.Parent != null)
                {
                    GraphicsContainer cstate = g.BeginContainer();
                    g.TranslateTransform(-this.Left, -this.Top);
                    Rectangle clip = e.ClipRectangle;
                    clip.Offset(this.Left, this.Top);
                    PaintEventArgs pe = new PaintEventArgs(g, clip);
                    //paint the container's bg
                    InvokePaintBackground(this.Parent, pe);
                    //paints the container fg
                    InvokePaint(this.Parent, pe);
                    //restores graphics to its original state
                    g.EndContainer(cstate);
                }
                Rectangle boxRect = ClientRectangle;
                boxRect.Height = 15;
                boxRect.Width = 15;
                boxRect.X = 0;
                boxRect.Y = (ClientRectangle.Height - boxRect.Height) / 2;// - Margin.Vertical) / 2;
                //boxRect.X = ClientRectangle.Width - boxRect.Width;
                Rectangle textRect = ClientRectangle;
                //textRect.Height -= Margin.Vertical;
                textRect.Width -= boxRect.Width + 2;
                textRect.X += boxRect.Width + 2;
                float pad = 0;
                float r = 1.0f;// radius;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(boxRect.X + pad, boxRect.Y + pad, r, r, 180, 90);
                path.AddArc(boxRect.X + boxRect.Width - r - pad, boxRect.Y + pad, r, r, 270, 90);
                path.AddArc(boxRect.X + boxRect.Width - r - pad, boxRect.Y + boxRect.Height - r - pad, r, r, 0, 90);
                path.AddArc(boxRect.X + pad, boxRect.Y + boxRect.Height - r - pad, r, r, 90, 90);
                path.CloseFigure();
                Brush brush = null;
                if (mouseOver || Focused)
                {
                    brush = new LinearGradientBrush(
                       new Point(0, 0),
                       new Point(0, ClientRectangle.Height),
                       Color.White,
                       Color.Gray);
                }
                else
                {
                    brush = new LinearGradientBrush(
                        new Point(0, 0),
                        new Point(0, ClientRectangle.Height),
                        Color.White,
                        Color.FromArgb(200, 200, 200));
                }
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(brush, path);
                g.DrawPath(Pens.Black, path);
                if ((Checked))
                {
                    Pen p = new Pen(Color.FromArgb(40, 40, 40));
                    p.Width = 2.0f;
                    //Häkchen
                    g.DrawLines(p, new PointF[] {
                            new PointF(boxRect.X + boxRect.Width*0.30f, boxRect.Y + boxRect.Width*0.5f),
                            new PointF(boxRect.X + boxRect.Width*0.45f, boxRect.Y + boxRect.Width*0.7f),
                            new PointF(boxRect.X + boxRect.Width*0.75f, boxRect.Y + boxRect.Height*0.2f)
                        });
                }
                /*
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (Enabled)
                        g.DrawString(Text, Font, Brushes.White, textRect, ControlConverter.GetStringFormatFromContentAllignment(TextAlign));
                    else
                        g.DrawString(Text, Font, Brushes.DimGray, textRect, ControlConverter.GetStringFormatFromContentAllignment(TextAlign));
                }
                */
                bg.Render();
                Color foreColor = Color.White;
                if (!Enabled)
                    foreColor = Color.DimGray;
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, foreColor, TextFormatFlags.Default | TextFormatFlags.VerticalCenter);
            }
            
        }
    }
}
