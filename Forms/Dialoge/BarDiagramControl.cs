using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Bar Diagram window and coordinates its user interface behavior.
    /// </summary>
    public class BarDiagramControl : Pflegehaushaltsbuch.FormControls.PictureBox
    {
        private string[] month = new string[]
        {
            "Jan.",
            "Feb.",
            "Mrz.",
            "Apr.",
            "Mai",
            "Jun.",
            "Jul.",
            "Aug.",
            "Sep.",
            "Okt.",
            "Nov.",
            "Dez."
        };
        private decimal maxAmount=0;
        private Dictionary<DateTime, decimal[]> values = new Dictionary<DateTime, decimal[]>();
        /// <summary>
        /// Creates a new Bar Diagram instance and initializes the required state.
        /// </summary>
        public BarDiagramControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Updates the table data and refreshes the related application state.
        /// </summary>
        public void UpdateTable(Dictionary<DateTime, decimal[]> values, decimal maxAmount)
        {
            this.maxAmount = maxAmount;
            this.values = values;
            Invalidate();
        }
        /// <summary>
        /// Handles the paint Background lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Pen gridPen = null, gridscalaPen = null;
            LinearGradientBrush lineareBrush = null;
            SolidBrush barDarkBrush = null, payOutsDarkBrush = null;
            Brush barBrush = null, payOutsBrush = null;
            try
            {
                Bitmap bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    if (this.Parent != null)
                    {
                        GraphicsContainer cstate = g.BeginContainer();
                        g.TranslateTransform(-this.Left, -this.Top);
                        Rectangle clip = ClientRectangle;
                        clip.Offset(this.Left, this.Top);
                        PaintEventArgs pe = new PaintEventArgs(g, clip);
                        //paint the container's bg
                        InvokePaintBackground(this.Parent, pe);
                        //paints the container fg
                        InvokePaint(this.Parent, pe);
                        //restores graphics to its original state
                        g.EndContainer(cstate);
                    }
                    Rectangle clipRect = new Rectangle(Point.Empty, bitmap.Size);
                    Rectangle rect = new Rectangle(clipRect.X + 100, clipRect.Y, clipRect.Width - 240, clipRect.Height - 75);
                    //Graphics g = e.Graphics;
                    gridPen = new Pen(ControlPaint.LightLight(Color.DimGray));
                    gridscalaPen = new Pen(Color.Black);
                    lineareBrush = new LinearGradientBrush(
                       new Point(0, 0),
                       new Point(0, clipRect.Height),
                       Color.LightSteelBlue,
                       Color.White);
                    barBrush =
                        new LinearGradientBrush(
                            new Rectangle(0, 0, 10, rect.Height),
                            ControlPaint.LightLight(Color.FromArgb(111, 182, 118)),
                            Color.FromArgb(111, 182, 118), 90.0f);
                    //new SolidBrush(Color.FromArgb(255, Color.FromArgb(111,182,118)));
                    barDarkBrush = new SolidBrush(Color.FromArgb(255, Color.DarkBlue));
                    payOutsBrush =
                        new LinearGradientBrush(
                            new Rectangle(0, 0, 10, rect.Height),
                            ControlPaint.LightLight(Color.FromArgb(172, 67, 62)),
                            Color.FromArgb(172, 67, 62), 90.0f);
                    //new SolidBrush(Color.FromArgb(255, Color.FromArgb(172,67,62)));
                    //payOutsDarkBrush = new SolidBrush(Color.FromArgb(255, Color.DarkRed));
                    g.FillRectangle(new SolidBrush(ControlPaint.Light(Color.FromArgb(111, 182, 118))), new Rectangle(rect.Right + 25, 50, 30, 20));
                    g.DrawRectangle(gridscalaPen, new Rectangle(rect.Right + 25, 50, 30, 20));
                    g.DrawString("Einnahmen", Font, Brushes.White, rect.Right + 60, 52);
                    g.FillRectangle(new SolidBrush(ControlPaint.Light(Color.FromArgb(172, 67, 62))), new Rectangle(rect.Right + 25, 80, 30, 20));
                    g.DrawRectangle(gridscalaPen, new Rectangle(rect.Right + 25, 80, 30, 20));
                    g.DrawString("Ausgaben", Font, Brushes.White, rect.Right + 60, 82);
                    int cells = values.Keys.Count + 1;// 13;
                    float cell = (rect.Width) / (float)cells;
                    g.FillRectangle(lineareBrush, rect);
                    g.DrawRectangle(gridscalaPen, rect);
                    float yBegin = rect.Y + rect.Height;
                    int maxY_Cells = (int)(rect.Height / cell);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 1; i <= maxY_Cells; i++)// y > rect.Y; y -= cell)
                    {
                        float x = rect.X;// +cell * i;
                        float y = yBegin - cell * i;
                        g.DrawLine(gridPen, rect.X, y, rect.X + rect.Width, y);
                        g.DrawLine(gridscalaPen, rect.X + 5, y, rect.X - 5, y);
                        g.DrawString((maxAmount * ((decimal)(cell * i) / (decimal)rect.Height)).ToString("C"), Font, Brushes.White,
                            new RectangleF(0, y - FontHeight / 2, x - 10, 25), sf);
                    }
                    //g.DrawString("Gesamt: "+maxAmount.ToString("C"), Font, Brushes.White, rect.Left, rect.Top-30);
                    for (int i = 1; i < cells; i++)         // float x = rect.X + cell; x < rect.Right; x += cell)
                    {
                        float x = rect.X + cell * i;
                        g.DrawLine(gridPen, x, rect.Y, x, rect.Bottom);
                        g.DrawLine(gridscalaPen, x, rect.Bottom - 5, x, rect.Bottom + 5);
                        //g.DrawString(month[(i - 1) % month.Length], Font, Brushes.Black, x - 10, rect.Bottom + 10);
                    }
                    Random rand = new Random();
                    rect.Height -= 5;
                    float depth = cell * 0.25f;
                    float cellWidth = Math.Min(Width * 0.03f, cell * 0.4f);
                    //foreach(var pair in values)
                    DateTime[] keys = values.Keys.ToArray();
                    for (int i = 0; i < values.Count; i++)
                    {
                        DateTime date = keys[i];
                        //Grünes Bar
                        float x = rect.X + i * cell - cellWidth + cell;// cell * 0.2f;
                        float height = rect.Height * (float)values[date][0];//  (float)rand.NextDouble();
                        float y = rect.Bottom - height + 5;
                        if (cellWidth != 0 && height != 0)
                        {
                            g.FillRectangle(barBrush, x, y, cellWidth, height);
                            g.DrawRectangle(gridscalaPen, x, y, cellWidth, height);
                        }
                        // Rote Bar
                        x = rect.X + i * cell + cell;
                        height = rect.Height * (float)values[date][1]; //(float)rand.NextDouble();
                        y = rect.Bottom - height + 5;
                        g.FillRectangle(payOutsBrush, x, y, cellWidth, height);
                        g.DrawRectangle(gridscalaPen, x, y, cellWidth, height);
                        float rotation = 40.0f;
                        string text = string.Format("{0} {1}", month[date.Month - 1], date.Year);
                        SizeF size = g.MeasureString(text, Font);
                        //size.Height += size.Width * (float)Math.Cos(rotation) + 3;
                        //size.Width = size.Width * (float)Math.Sin(rotation);
                        size.Width += 5;
                        size.Height = 0;
                        g.TranslateTransform(x, rect.Bottom + 5);
                        g.RotateTransform(-rotation);
                        g.TranslateTransform(-size.Width, size.Height);
                        g.DrawString(text, Font, Brushes.White, 0, 0);// x - 10, rect.Bottom + 10);
                        g.ResetTransform();
                        /*
                        g.RotateTransform(rotation);
                        g.TranslateTransform(-(x - size.Width), -(rect.Bottom + size.Height));
                         */
                    }
                }
                e.Graphics.DrawImageUnscaled(bitmap, Point.Empty);
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceWarning("BarDiagram paint err: " + err);
            }
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // BarDiagramControl
            // 
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
