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
    /// Represents a custom flow Layout Panel control used by the application user interface.
    /// </summary>
    public class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel
    {
        /// <summary>
        /// Creates a new Flow Layout Panel instance and initializes the required state.
        /// </summary>
        public FlowLayoutPanel()
        {
            //SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            MouseWheel += FlowLayoutPanel_MouseWheel;
            Scroll += FlowLayoutPanel_Scroll;
        }
        /// <summary>
        /// Handles the scroll event for flow Layout Panel and updates the related state.
        /// </summary>
        private void FlowLayoutPanel_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }
        /// <summary>
        /// Handles the mouse Wheel event for flow Layout Panel and updates the related state.
        /// </summary>
        private void FlowLayoutPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            Invalidate();
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;
            if (this.Parent != null)
            {
                GraphicsContainer cstate = g.BeginContainer();
                g.TranslateTransform(-this.Left, -this.Top);
                Rectangle clip = rect;
                clip.Offset(this.Left, this.Top);
                PaintEventArgs pe = new PaintEventArgs(g, clip);
                //paint the container's bg
                InvokePaintBackground(this.Parent, pe);
                //paints the container fg
                InvokePaint(this.Parent, pe);
                //restores graphics to its original state
                g.EndContainer(cstate);
            }
            if (BackColor != Color.Transparent)
                g.FillRectangle(new SolidBrush(BackColor), rect);
        }
    }
}
