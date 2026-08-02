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
    /// Represents a custom picture Box control used by the application user interface.
    /// </summary>
    public class PictureBox : System.Windows.Forms.PictureBox
    {
        /// <summary>
        /// Creates a new Picture Box instance and initializes the required state.
        /// </summary>
        public PictureBox()
        {
            DoubleBuffered = true;
        }
    }
}
