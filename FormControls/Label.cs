using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom label control used by the application user interface.
    /// </summary>
    public class Label : System.Windows.Forms.Label
    {
        /// <summary>
        /// Updates the UI Delegate data and refreshes the related application state.
        /// </summary>
        public delegate void UpdateUIDelegate();
        public static event UpdateUIDelegate OnUpdateUI;
        public static Color GradiantColor1 = Color.FromArgb(24, 73, 132), GradiantColor2 = Color.Transparent;
        [DefaultValue(false)]
        public bool IsSelectable { get; set; }
        /// <summary>
        /// Creates a new Label instance and initializes the required state.
        /// </summary>
        public Label()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            if (IsSelectable)
            {
                SetStyle(ControlStyles.Selectable, true);
                Click += Label_Click;
            }
            //    ForeColor = Color.Black;
            //else
            //    ForeColor = Color.White;
        }
        /// <summary>
        /// Handles the click event for label and updates the related state.
        /// </summary>
        private void Label_Click(object sender, EventArgs e)
        {
            if (IsSelectable)
                Select();
        }
        [DefaultValue(false)]
        public bool Gradiant { get; set; }
        [DefaultValue(0f)]
        public float Radius { get; set; }
        [Category("Verhalten")]
        [DefaultValue(false)]
        public bool AttachRegion { get; set; }
        [DefaultValue(false)]
        public bool DrawLine { get; set; }
        [DefaultValue(0)]
        public int LinePadding { get; set; }
        /// <summary>
        /// Updates the UI Design data and refreshes the related application state.
        /// </summary>
        public static void UpdateUIDesign()
        {
            if (OnUpdateUI != null)
                OnUpdateUI();
        }
        /// <summary>
        /// Updates the UI data and refreshes the related application state.
        /// </summary>
        private void UpdateUI()
        {
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Label
            // 
            this.VisibleChanged += new System.EventHandler(this.Label_VisibleChanged);
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
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
            if (Gradiant)
            {
                Rectangle rect = ClientRectangle;
                LinearGradientBrush brush = new LinearGradientBrush(rect,
                    GradiantColor1, GradiantColor2,
                    Radius
                );
                e.Graphics.FillRectangle(brush, rect);
            }
            else if (BackColor != Color.Transparent)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            }
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    if (AutoSize)
        //    {
        //        base.OnPaint(e);
        //        return;
        //    }
            
        //    var g = e.Graphics;
        //    using (Brush brush = new SolidBrush(ForeColor))
        //        g.DrawString(Text, Font, brush, new RectangleF(0,0, Width, 1000));// new StringFormat() { FormatFlags = StringFormatFlags.FitBlackBox});
        //}
        /// <summary>
        /// Handles the visible Changed event for label and updates the related state.
        /// </summary>
        private void Label_VisibleChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (Visible)
            {
                if (AttachRegion)
                {
                    if (!Text.EndsWith(RegionInfo.CurrentRegion.CurrencySymbol))
                        Text += " " + RegionInfo.CurrentRegion.CurrencySymbol;
                }
            }
        }
    }
}
