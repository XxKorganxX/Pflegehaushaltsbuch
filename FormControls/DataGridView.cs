using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom data Grid View control used by the application user interface.
    /// </summary>
    public class DataGridView : System.Windows.Forms.DataGridView
    {
        private static ColorSet colorSet = null;
        public static ColorSet ColorSet
        {
            get
            {
                return colorSet;
            }
            set
            {
                colorSet = value;
            }
        }
        public override Font Font
        {
            get
            {
                return Forms.Form.baseFont;
            }
        }
        /// <summary>
        /// Creates a new Data Grid View instance and initializes the required state.
        /// </summary>
        public DataGridView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            DoubleBuffered = true;
            base.OnCreateControl();
            if (colorSet == null)
                return;
            if (Program.DesignMode)
                return;
            ForeColor = Color.Black;
            //RowPrePaint += ViewRowPrePaint;
            AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 235, 255);
            RowTemplate.DefaultCellStyle.SelectionBackColor = colorSet.ListSelectBackColor;//.FromArgb(144, 177, 210);//listSelectBackColor;
            RowTemplate.DefaultCellStyle.SelectionForeColor = colorSet.ListSelectForeColor;// Color.White;
        }
        
        /// <summary>
        /// Handles the key Down lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }
        /// <summary>
        /// Updates the UI data and refreshes the related application state.
        /// </summary>
        public void UpdateUI()
        {
            //Color backColor, foreColor, selectionBackColor, selectionForeColor;
            //ControlColors.Get(out backColor, out foreColor, out selectionBackColor, out selectionForeColor);
            //Color backColor, foreColor, selectionBackColor, selectionForeColor, listSelectBackColor, listSelectForeColor, disabledColor;
            //ControlColors.Get(out backColor, out foreColor, out selectionBackColor, out selectionForeColor, out listSelectBackColor, out listSelectForeColor, out disabledColor);
            // listSelectForeColor;
            /*
            switch (Properties.Settings.Default.UIDesign)
            {
                case 0:
                    return;
                case 1:
                    //Orange
                    RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Orange;
                    RowTemplate.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;
                case 2:
                    {
                        //black
                        Color selectionColor = Color.FromArgb(49, 49, 49);
                        RowTemplate.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(selectionColor);
                        RowTemplate.DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    break;
                case 3:
                    {
                        //Gray
                        Color selectionColor = Color.FromArgb(89, 89, 89);
                        RowTemplate.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(selectionColor);
                        RowTemplate.DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    break;
                case 4:
                    //blue
                    {
                        //Gray
                        Color selectionColor = Color.FromArgb(100, 123, 164);
                        RowTemplate.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(selectionColor);
                        RowTemplate.DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    break;
            }
             * */
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView
            // 
            this.StandardTab = true;
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.DataGridView_Layout);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Handles the layout event for data Grid View and updates the related state.
        /// </summary>
        private void DataGridView_Layout(object sender, LayoutEventArgs e)
        {
            UpdateUI();
        }
    }
}
