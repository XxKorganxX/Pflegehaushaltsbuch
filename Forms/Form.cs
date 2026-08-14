using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Label = Pflegehaushaltsbuch.FormControls.Label;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class Form : System.Windows.Forms.Form
    {
        public static Font baseFont = new Font("Segoe UI", 10.0f);
        public static int BackColorMode = 0;
        protected SqlSession Session { get; set; }
        protected IFormatProvider CurrencyFormatProvider => Session?.Company?.Currencies ?? CultureInfo.CurrentCulture;
        private bool automaticTabOrderApplied;

        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms selectForm);
        public event OnShowForm ShowForm;

        /// <summary>
        /// Runs the show form event action.
        /// </summary>
        public void ShowFormEvent(Enums.Forms selectForm)
        {
            ShowForm?.Invoke(selectForm);
        }

        /// <summary>
        /// Runs the show message action.
        /// </summary>
        public virtual void ShowMessage(string msg)
        {
            FormControls.MessageBox.ShowDialog(this, msg);
        }

        /// <summary>
        /// Runs the show error action.
        /// </summary>
        public virtual void ShowError(string msg)
        {
            FormControls.MessageBox.ShowDialog(this, msg, Messages.error_caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ApplyCurrencyFormat(params DataGridViewColumn[] columns)
        {
            IFormatProvider currencyFormatProvider = CurrencyFormatProvider;
            foreach (DataGridViewColumn column in columns.Where(column => column != null))
            {
                column.DefaultCellStyle.Format = "C";
                column.DefaultCellStyle.FormatProvider = currencyFormatProvider;
            }
        }

        /// <summary>
        /// Runs the confirm message action.
        /// </summary>
        public virtual bool ConfirmMessage(string msg)
        {
            return FormControls.MessageBox.ShowDialog(this, msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Runs the show save file dialog action.
        /// </summary>
        public virtual bool ShowSaveFileDialog(string fileName, string filter, string defaultExt, out string selectedFileName)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.FileName = fileName;
                dialog.Filter = filter;
                dialog.DefaultExt = defaultExt;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    selectedFileName = null;
                    return false;
                }

                selectedFileName = dialog.FileName;
                return true;
            }
        }

        /// <summary>
        /// Runs the show open file dialog action.
        /// </summary>
        public virtual bool ShowOpenFileDialog(string fileName, string filter, out string selectedFileName)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.FileName = fileName;
                dialog.Filter = filter;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    selectedFileName = null;
                    return false;
                }

                selectedFileName = dialog.FileName;
                return true;
            }
        }

        /// <summary>
        /// Runs the apply user rights action.
        /// </summary>
        public virtual void ApplyUserRights(UserRights rights)
        {
        }

        /// <summary>
        /// Runs the apply current user rights action.
        /// </summary>
        protected void ApplyCurrentUserRights()
        {
            ApplyUserRights(UserRights.FromUser(Session == null ? null : Session.User));
        }

        /// <summary>
        /// Provides the font value.
        /// </summary>
        public override Font Font
        {
            get
            {
                return baseFont;
            }
            set 
            {
                base.Font = value;
            }
        }

        /// <summary>
        /// Creates a new Form view.
        /// </summary>
        public Form()
        {
            InitializeComponent();

            DoubleBuffered = true;
            this.Font = baseFont;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (Program.DesignMode || automaticTabOrderApplied)
                return;

            automaticTabOrderApplied = true;
            ApplyAutomaticTabOrder(this);
        }

        private static void ApplyAutomaticTabOrder(Control parent)
        {
            int tabIndex = 0;
            foreach (Control child in GetTabOrderedControls(parent))
            {
                child.TabIndex = tabIndex++;

                if (IsPassiveTabControl(child))
                    child.TabStop = false;

                if (child.HasChildren)
                    ApplyAutomaticTabOrder(child);
            }
        }

        private static IEnumerable<Control> GetTabOrderedControls(Control parent)
        {
            IEnumerable<Control> controls = parent.Controls.Cast<Control>();

            System.Windows.Forms.TableLayoutPanel tableLayoutPanel = parent as System.Windows.Forms.TableLayoutPanel;
            if (tableLayoutPanel != null)
            {
                return controls
                    .OrderBy(control => NormalizeTablePosition(tableLayoutPanel.GetRow(control)))
                    .ThenBy(control => NormalizeTablePosition(tableLayoutPanel.GetColumn(control)))
                    .ThenBy(control => control.Top)
                    .ThenBy(control => control.Left);
            }

            if (parent is System.Windows.Forms.FlowLayoutPanel)
                return controls;

            return controls
                .OrderBy(control => control.Top)
                .ThenBy(control => control.Left);
        }

        private static int NormalizeTablePosition(int position)
        {
            return position < 0 ? int.MaxValue : position;
        }

        private static bool IsPassiveTabControl(Control control)
        {
            return control is Label ||
                control is System.Windows.Forms.PictureBox ||
                control is System.Windows.Forms.Panel ||
                control is System.Windows.Forms.TableLayoutPanel ||
                control is System.Windows.Forms.FlowLayoutPanel ||
                control is System.Windows.Forms.GroupBox ||
                control is System.Windows.Forms.SplitContainer ||
                control is System.Windows.Forms.TabPage ||
                control is System.Windows.Forms.ProgressBar ||
                control is System.Windows.Forms.ToolStrip;
        }
        /// <summary>
        /// Runs the window Move operation and updates the related application state.
        /// </summary>
        public void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32.ReleaseCapture();
                Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HT_CAPTION, 0);
            }
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (ClientRectangle.Height == 0)
                return;
            Rectangle rect = ClientRectangle;
            //Blue
            if (BackColorMode == 0)
            {
                Label.GradiantColor1 = Color.FromArgb(24, 73, 132);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, ClientRectangle.Height),
                        Color.FromArgb(149, 182, 213),
                        Color.FromArgb(24, 73, 132)
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Orange
            else if (BackColorMode == 1)
            {
                Label.GradiantColor1 = ControlPaint.Dark(Color.Orange, 0.25f);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, ClientRectangle.Height),
                        Color.FromArgb(
                            Math.Min(255, Color.Orange.R + 20),
                            Math.Min(255, Color.Orange.G + 20),
                            Math.Min(255, Color.Orange.B + 20)),
                            Color.FromArgb(
                            Math.Min(255, (int)Color.Orange.R),
                            Math.Min(255, (int)Color.Orange.G),
                            Math.Min(255, (int)Color.Orange.B))
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Pink
            else if (BackColorMode == 2)
            {
                Label.GradiantColor1 = Color.FromArgb(
                        Math.Max(0, Color.Pink.R - 40),
                        Math.Max(0, Color.Pink.G - 40),
                        Math.Max(0, Color.Pink.B - 40));
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        Point.Empty,
                        new Point(0, Bounds.Height),
                        Color.FromArgb(
                            Math.Min(255, Color.Pink.R + 20),
                            Math.Min(255, Color.Pink.G + 20),
                            Math.Min(255, Color.Pink.B + 20)),
                        Color.FromArgb(
                            Math.Max(0, Color.Pink.R - 40),
                            Math.Max(0, Color.Pink.G - 40),
                            Math.Max(0, Color.Pink.B - 40))
                    ))
                    g.FillRectangle(brush, rect);
            }
            //Gray
            else
            {
                Label.GradiantColor1 = ControlPaint.Light(Color.Black, 0.4f);// Color.FromArgb(24, 73, 132);
                Label.GradiantColor2 = Color.Transparent;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    Point.Empty,
                    new Point(0, ClientRectangle.Height),
                    Color.Gray,
                    ControlPaint.Light(Color.Black, 0.4f)
                ))
                    g.FillRectangle(brush, rect);
            }
        }
    }
}
