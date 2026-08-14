using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom data Grid View control used by the application user interface.
    /// </summary>
    public class DataGridView : System.Windows.Forms.DataGridView
    {
        private const int SafeDefaultRowHeight = 24;
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
            DisableUnsafeRowAutoSizing();
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

        private void DisableUnsafeRowAutoSizing()
        {
            if (AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            if (RowTemplate.Height < SafeDefaultRowHeight)
                RowTemplate.Height = SafeDefaultRowHeight;
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
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.StandardTab = true;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
