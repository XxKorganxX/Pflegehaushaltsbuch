using Pflegehaushaltsbuch.Properties;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom color Set control used by the application user interface.
    /// </summary>
    public class ColorSet
    {
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public Color BorderColor { get; set; }
        public Color ControlSelectBackColor { get; set; }
        public Color ControlSelectForeColor { get; set; }
        public Color ListSelectBackColor { get; set; }
        public Color ListSelectForeColor { get; set; }
        public Color Disabled { get; set; }
        /// <summary>
        /// Creates a new Color Set instance and initializes the required state.
        /// </summary>
        public ColorSet()
        {
            UpdateColors();
        }
        /// <summary>
        /// Updates the colors data and refreshes the related application state.
        /// </summary>
        public void UpdateColors()
        {
            if (Settings.Default.BackgroundColorMode == 1)
                InitializeOrange();
            else if (Settings.Default.BackgroundColorMode == 2)
                InitializePink();
            else if (Settings.Default.BackgroundColorMode == 3)
                InitializeMonochrom();
            else
                InitializeBlue();
        }
        /// <summary>
        /// Runs the initialize Blue operation and updates the related application state.
        /// </summary>
        public void InitializeBlue()
        {
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderColor = Color.White;
            ControlSelectBackColor = ListSelectBackColor = Color.FromArgb(144, 177, 210);
            ControlSelectForeColor = ListSelectForeColor = Color.White;
            Disabled = Color.Gray;
        }
        /// <summary>
        /// Runs the initialize Orange operation and updates the related application state.
        /// </summary>
        public void InitializeOrange()
        {
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderColor = Color.White;
            ControlSelectBackColor = ListSelectBackColor = ControlPaint.Light(Color.Orange);
            ControlSelectForeColor = ListSelectForeColor = Color.White;
            Disabled = Color.Gray;
        }
        /// <summary>
        /// Runs the initialize Pink operation and updates the related application state.
        /// </summary>
        public void InitializePink()
        {
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderColor = Color.White;
            ControlSelectBackColor = ListSelectBackColor = ControlPaint.Light(Color.Pink);
            ControlSelectForeColor = ListSelectForeColor = Color.White;
            Disabled = Color.Gray;
        }
        /// <summary>
        /// Runs the initialize Monochrom operation and updates the related application state.
        /// </summary>
        public void InitializeMonochrom()
        {
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderColor = Color.White;
            ControlSelectBackColor = ListSelectBackColor = Color.DarkGray;
            ControlSelectForeColor = ListSelectForeColor = Color.White;
            Disabled = Color.Gray;
        }
    }
}
