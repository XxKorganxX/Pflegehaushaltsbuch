using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Dead Lines Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DeadLinesForm : Form, IDeadLinesFormContract
    {
        private readonly DeadLinesFormPresenter presenter;
        private int clientID, maxDaysInMonth, minimumCellHeight, maxRows, startCell;

        /// <summary>
        /// Creates a new DeadLinesForm view.
        /// </summary>
        public DeadLinesForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DeadLinesFormPresenter(this, session);
            view.AutoGenerateColumns = false;

            Enter += DeadLinesForm_Enter;
            Leave += DeadLinesForm_Leave;
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.DefaultCellStyle.Padding = new Padding(1, 21, 1, 1);
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            view.CellPainting += MonthView_CellPainting;
        }

        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            ApplyCurrentUserRights();
            OnResize(e);
        }

        /// <summary>
        /// Handles the resize lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustCellHeight();
        }

        /// <summary>
        /// Runs the adjust cell height action.
        /// </summary>
        private void AdjustCellHeight()
        {
            if (view == null || maxRows == 0)
                return;

            int cellHeight = (view.ClientSize.Height - view.ColumnHeadersHeight - 6) / maxRows;
            if (cellHeight <= 0)
                return;

            minimumCellHeight = cellHeight;
            foreach (DataGridViewRow row in view.Rows)
                row.MinimumHeight = minimumCellHeight;
        }

        /// <summary>
        /// Handles the cell Painting event for month View and updates the related state.
        /// </summary>
        private void MonthView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);
            int number = (e.RowIndex * 7 + e.ColumnIndex + 1) - startCell;
            if (number >= 1 && number <= maxDaysInMonth)
            {
                var rect = e.CellBounds;
                rect.X += 2;
                rect.Y += 2;
                rect.Width -= 4;
                rect.Height -= 4;
                using (var numberFont = new Font("Arial", 13, FontStyle.Regular))
                using (var brush = new SolidBrush(Color.Black))
                    e.Graphics.DrawString(number.ToString(), numberFont, brush, rect);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            exportButton.Visible = false;
        }

        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnClientID_Changed(int clientID)
        {
            this.clientID = clientID;
        }

        /// <summary>
        /// Runs the dead lines form_enter action.
        /// </summary>
        private async void DeadLinesForm_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.EnterAsync();
        }

        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private async void exportButton_Click(object sender, EventArgs e)
        {
            await presenter.ExportAsync();
        }

        /// <summary>
        /// Runs the dead lines form_leave action.
        /// </summary>
        private void DeadLinesForm_Leave(object sender, EventArgs e)
        {
            presenter.Leave();
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the cell Click event for view and updates the related state.
        /// </summary>
        private async void view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            await presenter.CellClickAsync(e.RowIndex, e.ColumnIndex);
        }

        /// <summary>
        /// Handles the value Changed event for from Date Box and updates the related state.
        /// </summary>
        private async void fromDateBox_ValueChanged()
        {
            if (DesignMode)
                return;

            await presenter.DateChangedAsync();
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private void updateButton_Click(object sender, EventArgs e)
        {
            presenter.Update();
        }

        /// <summary>
        /// Provides the current month value for the presenter.
        /// </summary>
        DateTime IDeadLinesFormContract.CurrentMonth
        {
            get { return fromDateBox.Date; }
        }

        /// <summary>
        /// Provides the client id value for the presenter.
        /// </summary>
        int IDeadLinesFormContract.ClientID
        {
            get { return clientID; }
        }

        /// <summary>
        /// Shows the calendar data.
        /// </summary>
        void IDeadLinesFormContract.ShowCalendar(DeadlineCalendar calendar)
        {
            startCell = calendar.StartCell;
            maxDaysInMonth = calendar.MaxDaysInMonth;
            maxRows = calendar.MaxRows;
            view.DataSource = null;
            view.DataSource = calendar.Table;
            AdjustCellHeight();
        }

        /// <summary>
        /// Shows the selected client name.
        /// </summary>
        void IDeadLinesFormContract.ShowClientName(string clientName)
        {
            clientNameBox.Text = clientName;
        }

        /// <summary>
        /// Clears the selected client name.
        /// </summary>
        void IDeadLinesFormContract.ClearClientName()
        {
            clientNameBox.Text = string.Empty;
        }

        /// <summary>
        /// Runs the show export dialog view action for the presenter.
        /// </summary>
        bool IDeadLinesFormContract.ShowExportDialog(string fileName, out string selectedFileName)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = fileName;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    selectedFileName = fileDialog.FileName;
                    return true;
                }
            }

            selectedFileName = null;
            return false;
        }

        /// <summary>
        /// Runs the show create deadline dialog view action for the presenter.
        /// </summary>
        bool IDeadLinesFormContract.ShowCreateDeadlineDialog(DateTime date, string description, out DeadlineInput input)
        {
            using (CreateDeadlineForm dialog = new CreateDeadlineForm(Session, date, description))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new DeadlineInput
                {
                    Description = dialog.Description,
                    ForAllMonths = dialog.ForAllMonths
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the show database changed view action for the presenter.
        /// </summary>
        void IDeadLinesFormContract.ShowDatabaseChanged()
        {
            MessageBox.Show(this, Messages.database_changed);
        }

        /// <summary>
        /// Runs the show clients form view action for the presenter.
        /// </summary>
        void IDeadLinesFormContract.ShowClientsForm()
        {
            ShowFormEvent(Enums.Forms.Clients);
        }

    }
}
