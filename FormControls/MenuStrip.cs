using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom menu Strip control used by the application user interface.
    /// </summary>
    public class MenuStrip : System.Windows.Forms.MenuStrip
    {
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Menu Strip instance and initializes the required state.
        /// </summary>
        public MenuStrip()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
        }
        /*
        /// <summary>
        /// Handles the layout lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnLayout(LayoutEventArgs e)
        {
            if(!DesignMode)
                BackColor = Color.FromArgb(12, 27, 36);
            base.OnLayout(e);
        }
        */
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // _MenuStrip
            // 
            this.ResumeLayout(false);
        }
        
    }
}
