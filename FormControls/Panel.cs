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
    /// Represents a custom panel control used by the application user interface.
    /// </summary>
    public class Panel : System.Windows.Forms.Panel
    {
        [Category("Darstellung")]
        [DefaultValue(typeof(Color), "White")]
        public Color BorderColor { get; set; }
        [Category("Darstellung")]
        [DefaultValue(0)]
        public float BorderWidth { get; set; }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        //}
        //protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        //{
            //base.OnPaint(e);
            Graphics g = e.Graphics;
            //g.Clear(BackColor);
            if (BackColor != Color.Transparent)
            {
                g.Clear(BackColor);
            }
            else if (this.Parent != null)
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
            if (BorderWidth > 0 )// BorderStyle != System.Windows.Forms.BorderStyle.None)
            {
                Rectangle rect = ClientRectangle;
                ControlPaint.DrawBorder(g, new Rectangle(rect.X,rect.Y,rect.Width,rect.Height), BorderColor, ButtonBorderStyle.Solid);
            }
        }
        /// <summary>
        /// Runs the scroll To Control operation and updates the related application state.
        /// </summary>
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            // Returning the current location prevents the panel from
            // scrolling to the active control when the panel loses and regains focus
            return this.DisplayRectangle.Location;
        }
    }
}
