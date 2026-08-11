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
    /// Represents a custom geometry Control control used by the application user interface.
    /// </summary>
    public class GeometryControl : System.Windows.Forms.Control
    {
        /// <summary>
        /// Defines the available geometry Type values used by the application.
        /// </summary>
        public enum GeometryType
        {
            Line,
            DoubleLine
        }
        public GeometryType Type { get; set; }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            if (this.Parent != null)
            {
                GraphicsContainer cstate = e.Graphics.BeginContainer();
                e.Graphics.TranslateTransform(-this.Left, -this.Top);
                Rectangle clip = ClientRectangle;
                clip.Offset(this.Left, this.Top);
                PaintEventArgs pe = new PaintEventArgs(e.Graphics, clip);
                //paint the container's bg
                InvokePaintBackground(this.Parent, pe);
                //paints the container fg
                InvokePaint(this.Parent, pe);
                //restores graphics to its original state
                e.Graphics.EndContainer(cstate);
            }
        }
        /// <summary>
        /// Handles the paint lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;
            if (Type == GeometryType.Line)
            {
                g.DrawLine(new Pen(ForeColor), rect.X, rect.Y, rect.Width, rect.Y);
            }
            else if (Type == GeometryType.DoubleLine)
            {
                g.DrawLine(new Pen(ForeColor), rect.X, rect.Y, rect.Width, rect.Y);
                g.DrawLine(new Pen(ForeColor), rect.X, rect.Y+3, rect.Width, rect.Y+3);
            }
        }
    }
}
