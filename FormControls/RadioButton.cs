using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom radio Button control used by the application user interface.
    /// </summary>
    public class RadioButton : System.Windows.Forms.RadioButton
    {
        private bool mouseHover = false;
        private Color buttonBackColor = Color.Transparent,
                      checkedColor = Color.Green,
                      buttonBorderColor = Color.DimGray;
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Radio Button instance and initializes the required state.
        /// </summary>
        public RadioButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // _RadioButton
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.ForeColor = System.Drawing.Color.White;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseHover = true;
        }
        /// <summary>
        /// Handles the mouse Leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            mouseHover = false;
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Appearance != System.Windows.Forms.Appearance.Button)
            {
                base.OnPaint(e);
                return;
            }
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
            Graphics g = e.Graphics;
            //if(BackColor != Color.Transparent)
            g.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            //g.CompositingQuality = CompositingQuality.GammaCorrected;
            Rectangle bounds = ClientRectangle;
            bounds.X += 1;
            bounds.Y += 1;
            bounds.Width -= 2;
            bounds.Height -= 2;
            Color textColor = ForeColor;
            if (!Enabled)
                textColor = Color.DarkGray;
            if (Checked)
            {
                g.FillRectangle(Brushes.Green, bounds);
                g.DrawRectangle(new Pen(Brushes.LightGreen), bounds);
                g.DrawString(Text, Font, new SolidBrush(textColor), new RectangleF(0, 0, Width, Height), ControlConverter.GetStringFormatFromContentAllignment(TextAlign));
            }
            else
            {
                if (mouseHover)
                {
                    g.FillRectangle(new SolidBrush(Color.Green), bounds);
                    g.DrawString(Text, Font, new SolidBrush(textColor), new RectangleF(0, 0, Width, Height), ControlConverter.GetStringFormatFromContentAllignment(TextAlign));
                }
                else
                {
                    if (buttonBackColor != Color.Transparent)
                        g.FillRectangle(new SolidBrush(buttonBackColor), bounds);// Color.FromArgb(12, 27, 36)), bounds);
                    
                    g.DrawRectangle(new Pen(new SolidBrush(buttonBorderColor)), bounds);
                    g.DrawString(Text, Font, new SolidBrush(textColor), new RectangleF(0, 0, Width, Height), ControlConverter.GetStringFormatFromContentAllignment(TextAlign));
                }
            }
        }
    }
}
