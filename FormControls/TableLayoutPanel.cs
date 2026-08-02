using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom table Layout Panel control used by the application user interface.
    /// </summary>
    public class TableLayoutPanel : System.Windows.Forms.TableLayoutPanel
    {
        private Color borderColor = Color.White, backColor = Color.Transparent;
        private float borderWidth = 1.0f;
        private string borderText = string.Empty;
        [Category("Darstellung")]
        [DefaultValue(typeof(Color), "0x00FFFFFF")]
        public override Color BackColor
        {
            get { return backColor; }
            set { backColor = value; base.BackColor = value; }
        }
        [Category("Darstellung")]
        [DefaultValue(typeof(Color), "0xFFFFFFFF")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
        [Category("Darstellung")]
        [DefaultValue(1.0f)]
        public float BorderWidth { get { return borderWidth; } set { borderWidth = value; } }
        [Category("Darstellung")]
        [Localizable(true)]
        [DefaultValue("")]
        public string BorderText { get { return borderText; } 
            set 
            { 
                borderText = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Padding v = Padding;
                    if (v.Top < 25)
                        v.Top = 25;
                    if (v.Left < 10)
                        v.Left = 10;
                    if (v.Right < 10)
                        v.Right = 10;
                    if (v.Bottom < 10)
                        v.Bottom = 10;
                    Padding = v;
                }
            } 
        }
        [Category("Darstellung")]
        [DefaultValue(false)]
        public bool Border { get; set; }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Creates a new Table Layout Panel instance and initializes the required state.
        /// </summary>
        public TableLayoutPanel() 
        {
            InitializeComponent();
            DoubleBuffered = true;
            //1.
            //2.
            //DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);// |  ControlStyles.UserPaint, true);
        }
        /*
        /// <summary>
        /// Handles the notify Message lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        */
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            
                //Graphics g = bg.Graphics;
                Rectangle rect = new Rectangle(Point.Empty, ClientRectangle.Size);
                if (this.Parent != null)
                {
                    GraphicsContainer cstate = g.BeginContainer();
                    g.TranslateTransform(-this.Left, -this.Top);
                    Rectangle clip = rect;// e.ClipRectangle;
                    clip.Offset(this.Left, this.Top);
                    PaintEventArgs pe = new PaintEventArgs(g, clip);
                    //paint the container's bg
                    InvokePaintBackground(this.Parent, pe);
                    //paints the container fg
                    InvokePaint(this.Parent, pe);
                    //restores graphics to its original state
                    g.EndContainer(cstate);
                }
                else
                    g.FillRectangle(new SolidBrush(BackColor), rect);
                if (BackColor != Color.Transparent)
                    g.FillRectangle(new SolidBrush(BackColor), rect);
                if (Border && BorderWidth != 0)
                {
                    rect.Width -= (int)borderWidth;
                    rect.Height -= (int)borderWidth;
                    if (!string.IsNullOrWhiteSpace(borderText))
                    {
                        Size borderTextSize = g.MeasureString(BorderText, Font).ToSize();
                        float upperBorder = borderTextSize.Height / 2;// Font.Height / 2;
                        g.DrawLines(new Pen(BorderColor) { Width = borderWidth },
                            new PointF[]
                            {
                                new PointF(borderTextSize.Width+8,upperBorder),
                                new PointF(rect.Width,upperBorder),
                                new PointF(rect.Width,rect.Height),
                                new PointF(0,rect.Height),
                                new PointF(0,upperBorder),
                                new PointF(8,upperBorder),
                            }
                        );
                        Color foreColor = Color.White;
                        if (!Enabled)
                            foreColor = Color.DimGray;
                        g.DrawString(BorderText, Font, new SolidBrush(foreColor), new Rectangle(new Point(10, 0), new Size(borderTextSize.Width+10, borderTextSize.Height)));
                    }
                    else
                    {
                        g.DrawRectangle(new Pen(BorderColor) { Width = borderWidth }, rect);
                    }
                }
        }
    }
}
