using Pflegehaushaltsbuch.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom control Colors control used by the application user interface.
    /// </summary>
    internal class ControlColors
    {
        public static readonly Color AccentColor = Color.FromArgb(0, 120, 215);
        public static readonly Pen AccentPen = new Pen(AccentColor, 2.0f);
        public static readonly Brush AccentBrush = new SolidBrush(AccentColor);
        /// <summary>
        /// Gets the get value from the current application state.
        /// </summary>
        public static void Get(out Color backColor,
            out Color foreColor,
            out Color controlSelectBackColor,
            out Color controlSelectForeColor, 
            out Color listSelectBackColor,
            out Color listSelectForeColor,
            out Color disabled)
        {
            backColor = Color.White;
            foreColor = Color.White;
            controlSelectBackColor = Color.White;
            controlSelectForeColor = Color.White;
            listSelectBackColor = Color.Black;
            listSelectForeColor = Color.White;
            disabled = Color.Gray;
            switch (Settings.Default.UIDesign)
            {
                //case 0:
                //    backColor = Color.FromArgb(155, 100, 0);
                //    foreColor = Color.White;
                //    selectionBackColor = Color.FromArgb(214, 138, 0);
                //    selectionForeColor = Color.White;
                //    return;
                case 2:
                    //black
                    backColor = ControlPaint.Light(Color.FromArgb(49, 49, 49));
                    foreColor = Color.White;
                    controlSelectBackColor = backColor;
                    controlSelectForeColor = Color.White;
                    disabled = Color.DimGray;
                    break;
                //case 3:
                //    //Gray
                //    backColor = Color.FromArgb(89, 89, 89);
                //    foreColor = Color.White;
                //    selectionBackColor = ControlPaint.Light(backColor);
                //    selectionForeColor = Color.White;
                //    break;
                //case 4:
                //    //blue
                //    backColor = Color.FromArgb(100, 123, 164);
                //    foreColor = Color.Black;
                //    selectionBackColor = ControlPaint.Light(backColor);
                //    selectionForeColor = Color.White;
                    //break;
                default:
                    //Orange
                    backColor = Color.FromArgb(155, 100, 0);//Color.FromArgb(155, 100, 0);
                    foreColor = Color.White;
                    controlSelectBackColor = Color.FromArgb(214,138,0);
                    controlSelectForeColor = Color.White;
                    listSelectBackColor = Color.Orange;
                    listSelectForeColor = Color.White;
                    disabled = Color.FromArgb(200, 200, 200);
                    break;
            }
        }
    }
}
