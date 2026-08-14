using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom button control used by the application user interface.
    /// </summary>
    public class Button : System.Windows.Forms.Button
    {
        private bool mouseOver = false;
        private float radius = 0;
        private Color borderColor = Color.Black;
        private bool paintBackground = true;
        [DefaultValue(true)]
        public bool PaintBackGround
        {
            get { return paintBackground; }
            set { paintBackground = value; }
        }
        
        private bool isChecked = false;
        [DefaultValue(false)]
        public bool RoundEdges { get; set; }
        [DefaultValue(0f)]
        public float Radius { get { return radius; } set { radius = value; } }
        [DefaultValue(false)]
        public bool Checked 
        { 
            get { return isChecked; } 
            set 
            { 
                isChecked = value;
                Invalidate(); 
            } 
        }
        [DefaultValue(false)]
        public bool CheckedState { get; set; }
        /// <summary>
        /// Creates a new Button instance and initializes the required state.
        /// </summary>
        public Button()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
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
        [Category("Darstellung")]
        [DefaultValue(typeof(Color), "0xFF0000")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
        [DefaultValue(typeof(Color), "0x000000")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        [DefaultValue(typeof(Color), "0xFFFFFFFF")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
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
        /// Handles the mouse Down lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (CheckedState)
            {
                Checked = !Checked;
            }
            base.OnMouseDown(mevent);
        }
        /// <summary>
        /// Handles the mouse Click lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }
        
        
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle bounds = ClientRectangle;
            Rectangle borderBounds = bounds;
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
            {
                Graphics g = bg.Graphics;
                if (this.Parent != null)
                {
                    GraphicsContainer cstate = g.BeginContainer();
                    g.TranslateTransform(-this.Left, -this.Top);
                    Rectangle clip = ClientRectangle;
                    clip.Offset(this.Left, this.Top);
                    PaintEventArgs pe = new PaintEventArgs(g, clip);
                    //paint the container's bg
                    InvokePaintBackground(this.Parent, pe);
                    InvokePaint(this.Parent, pe);
                    g.EndContainer(cstate);
                }

                borderBounds.Width -= 1;
                borderBounds.Height -= 1;

                float r = 0;
                float pad = 0;
                Brush brush = null;
                Pen pen = Pens.Black;
                if ((CheckedState && Checked))
                {
                    brush = new LinearGradientBrush(
                       new Point(bounds.Left, bounds.Top),
                       new Point(bounds.Left, bounds.Bottom),
                       Color.LightGreen,
                       Color.Green);
                }

                else
                {
                    if (!PaintBackGround)
                        brush = Brushes.Transparent;
                    else
                        brush = new LinearGradientBrush(
                            new Point(bounds.Left, bounds.Top),
                            new Point(bounds.Left, bounds.Bottom),
                            Color.White,
                            Color.FromArgb(200, 200, 200));
                }
                if (!RoundEdges)
                {
                    g.FillRectangle(brush, borderBounds);
                    if ((CheckedState && Checked) || mouseOver || Focused)
                        g.DrawRectangle(ControlColors.AccentPen, borderBounds);
                    else
                        g.DrawRectangle(pen, borderBounds);
                }
                else
                {
                    if (radius <= 0)
                    {
                        r = (float)bounds.Height * 0.2f;
                    }
                    else
                        r = radius;
                    Rectangle rect = borderBounds;
                    GraphicsPath path = new GraphicsPath();
                    path.AddArc(pad, pad, r, r, 180, 90);
                    path.AddArc(rect.Width - r - pad, pad, r, r, 270, 90);
                    path.AddArc(rect.Width - r - pad, rect.Height - r - pad, r, r, 0, 90);
                    path.AddArc(pad, rect.Height - r - pad, r, r, 90, 90);
                    path.CloseFigure();
                    g.FillPath(brush, path);
                    if ((CheckedState && Checked) || mouseOver || Focused)
                        g.DrawPath(ControlColors.AccentPen, path);
                    else
                        g.DrawPath(pen, path);
                }

                if (BackgroundImage != null)
                {
                    var si = Size;
                    var bo = Bounds;
                    var mg = Margin;
                    var dx = DisplayRectangle;
                    var te = e.ClipRectangle;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.CompositingMode = CompositingMode.SourceOver;

                    g.InterpolationMode = InterpolationMode.High;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.DrawImage(BackgroundImage, e.ClipRectangle);
                }
                bg.Render();
                Color forecColor = Color.Black;
                if (!Enabled)
                    forecColor = Color.DimGray;
                if (Image != null)
                {
                    Rectangle textBounds = ClientRectangle;
                    textBounds.Inflate(-Padding.Horizontal / 2, -Padding.Vertical / 2);
                    bool hasText = !string.IsNullOrWhiteSpace(Text);
                    int imageX = hasText ? Math.Max(Padding.Left, 4) : Math.Max(0, (Width - Image.Width) / 2);
                    int imageY = Math.Max(0, (Height - Image.Height) / 2);
                    Rectangle imageBounds = new Rectangle(imageX, imageY, Image.Width, Image.Height);
                    InterpolationMode oldInterpolationMode = e.Graphics.InterpolationMode;
                    PixelOffsetMode oldPixelOffsetMode = e.Graphics.PixelOffsetMode;
                    CompositingQuality oldCompositingQuality = e.Graphics.CompositingQuality;
                    SmoothingMode oldSmoothingMode = e.Graphics.SmoothingMode;
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.DrawImage(Image, imageBounds);
                    e.Graphics.InterpolationMode = oldInterpolationMode;
                    e.Graphics.PixelOffsetMode = oldPixelOffsetMode;
                    e.Graphics.CompositingQuality = oldCompositingQuality;
                    e.Graphics.SmoothingMode = oldSmoothingMode;
                    if (hasText)
                    {
                        textBounds.X = imageBounds.Right + Math.Max(Padding.Right, 4);
                        textBounds.Width = Math.Max(0, ClientRectangle.Right - textBounds.X - Padding.Right);
                        TextRenderer.DrawText(e.Graphics, Text, Font, textBounds, forecColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                }
                else
                {
                    TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, forecColor);
                }
            }
        }
    }
}
