using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Pflegehaushaltsbuch.FormControls
{
    public class DataGridViewCheckedColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        private const int CheckBoxSize = 14;
        private const int CheckBoxMargin = 4;

        public bool Checked { get; private set; } = true;
        public bool Highlighted { get; private set; }

        public event EventHandler CheckedChanged;

        public override object Clone()
        {
            var clone = (DataGridViewCheckedColumnHeaderCell)base.Clone();
            clone.Checked = Checked;
            clone.Highlighted = Highlighted;
            return clone;
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            var headerStyle = cellStyle.Clone();
            headerStyle.Padding = new Padding(
                cellStyle.Padding.Left + CheckBoxSize + (CheckBoxMargin * 2),
                cellStyle.Padding.Top,
                cellStyle.Padding.Right,
                cellStyle.Padding.Bottom);

            if (Highlighted)
            {
                headerStyle.BackColor = SystemColors.Highlight;
                headerStyle.ForeColor = SystemColors.HighlightText;
                headerStyle.SelectionBackColor = SystemColors.Highlight;
                headerStyle.SelectionForeColor = SystemColors.HighlightText;
            }

            base.Paint(
                graphics,
                clipBounds,
                cellBounds,
                rowIndex,
                dataGridViewElementState,
                value,
                formattedValue,
                errorText,
                headerStyle,
                advancedBorderStyle,
                paintParts);

            var checkBoxBounds = GetCheckBoxBounds(cellBounds.Location, cellBounds.Height);

            CheckBoxRenderer.DrawCheckBox(
                graphics,
                checkBoxBounds.Location,
                Checked
                    ? CheckBoxState.CheckedNormal
                    : CheckBoxState.UncheckedNormal);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (!GetCheckBoxBounds(Point.Empty, Size.Height).Contains(e.Location))
            {
                base.OnMouseClick(e);
                return;
            }

            Checked = !Checked;
            DataGridView?.InvalidateCell(this);

            CheckedChanged?.Invoke(this, EventArgs.Empty);

            base.OnMouseClick(e);
        }

        private static Rectangle GetCheckBoxBounds(Point origin, int cellHeight)
        {
            return new Rectangle(
                origin.X + CheckBoxMargin,
                origin.Y + (cellHeight - CheckBoxSize) / 2,
                CheckBoxSize,
                CheckBoxSize);
        }
    }
}
