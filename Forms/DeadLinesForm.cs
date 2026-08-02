using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Dead Lines Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DeadLinesForm : Pflegehaushaltsbuch.FormControls.Form, IDeadLinesFormContract
    {
        private readonly DeadLinesFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        DataTable datatable = new DataTable(),
                  clientTable = new DataTable();
        int clientID, maxDaysInMonth, minimumCellHeight, maxRows, startCell;
        /// <summary>
        /// Creates a new Dead Lines Form instance and initializes the required state.
        /// </summary>
        public DeadLinesForm()
        {
            InitializeComponent();
            presenter = new DeadLinesFormPresenter(this);
            view.AutoGenerateColumns = false;
            
            this.Enter += DeadLinesForm_Enter;
            this.Leave += DeadLinesForm_Leave;
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.DefaultCellStyle.Padding = new Padding(1, 21,1,1);
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            /*
            view.Rows.Add();
            view.Rows.Add();
            view.Rows.Add();
            view.Rows.Add();
            view.Rows.Add();
            */
            view.CellPainting += MonthView_CellPainting;
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
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
        void AdjustCellHeight()
        { 
            if (view == null || maxRows == 0)
                return;
            int cellHeight =  (view.ClientSize.Height-view.ColumnHeadersHeight-6) / maxRows;
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
            if (number >= 1)//startCell < number)
            {
                if (number <= maxDaysInMonth)
                {
                    var rect = e.CellBounds;
                    rect.X += 2; rect.Y += 2;
                    rect.Width -= 4;
                    rect.Height -= 4;
                    using (var numberFont = new Font("Arial", 13, FontStyle.Regular))
                        e.Graphics.DrawString((number).ToString(), numberFont, new SolidBrush(Color.Black), rect);
                }
            }
            e.Handled = true;
        }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnUserRights(SQLBase sql)
        {
            base.OnUserRights(sql);
            if (sql.User.Supervisor)
            {
                exportButton.Visible = true;
            }
        }
        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnClientID_Changed(int clientID)
        {
            this.clientID = clientID;
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            maxDaysInMonth = fromDateBox.Date.AddMonths(1).AddHours(-1).Day;
            //DateTime date = dateTimeBox.Value;
            await sql.FillAdapterAsync(SQLBase.SELECT.Deadline, datatable, clientID, fromDateBox.Date.Month);
            //view.DataSource = datatable;
            //datatable.DefaultView.Sort = dateColum.DataPropertyName;

            var date = fromDateBox.Date;
            var startDateMonth = new DateTime(date.Year, date.Month, 1);
            var dayOfWeek = startDateMonth.DayOfWeek;
            startCell = (int)dayOfWeek - 1;

            if (startCell < 0)
                startCell += 7;
            maxRows = (int)Math.Ceiling((maxDaysInMonth + startCell) / 7.0);
            int currentCell = -1;
            int currentDay = 0;
            DataTable table = new DataTable();
            table.Columns.Add("Mo").DefaultValue = "";
            table.Columns.Add("Tu").DefaultValue = "";
            table.Columns.Add("We").DefaultValue = "";
            table.Columns.Add("Th").DefaultValue = "";
            table.Columns.Add("Fr").DefaultValue = "";
            table.Columns.Add("Sa").DefaultValue = "";
            table.Columns.Add("Su").DefaultValue = "";
            for (int i = 0; i < maxRows; ++i)
                table.Rows.Add(table.NewRow());
            while (currentDay < maxDaysInMonth)
            {
                currentCell++;
                if (currentCell < startCell)
                    continue;
                currentDay++;
                foreach (DataRow row in datatable.Rows)
                {
                    var day = ((DateTime)row["date"]).Day;
                    var note = row["note"];
                    if (currentDay == day)
                    {
                        table.Rows[currentCell / 7][currentCell % 7] = note.ToString();
                        break;
                    }
                }
            }
            view.DataSource = null;
            view.DataSource = table;
            AdjustCellHeight();
        }
        /// <summary>
        /// Connects the to Client data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectToClient()
        {
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, clientID.ToString());
        }
        async void DeadLinesForm_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
            await ConnectToClient();
            clientNameBox.DataBindings.Clear();
            clientNameBox.DataBindings.Add("Text", clientTable, "name");
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private async void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = string.Format(Messages.deadlines_export_filename, clientID);
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                DataTable table = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, table, clientID);
                Excel.ExportToExcel(table.DefaultView.ToTable(), fileDialog.FileName);
                await ConnectTableToDataBase();
            }
        }

        void DeadLinesForm_Leave(object sender, EventArgs e)
        {
            clientNameBox.DataBindings.Clear();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Clients, sql);
        }
        /// <summary>
        /// Handles the cell Click event for view and updates the related state.
        /// </summary>
        private async void view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {                
                if (e.RowIndex == -1)
                    return;

                int day = e.RowIndex * 7 + e.ColumnIndex + 1 - startCell;
                if (day < 1 || day > maxDaysInMonth)
                    return;
                DateTime currentDate = new DateTime(2000, fromDateBox.Date.Month, day);
                DataTable datatable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, datatable, clientID);
                var useExistRows = datatable.Rows.OfType<DataRow>().
                    Where(a => ((DateTime)a["date"]).Day == day &&
                    ((DateTime)a["date"]).Month == fromDateBox.Date.Month).
                    ToList();
                string description = "";
                if (useExistRows.Count > 0)
                    description = useExistRows[0][SQLBase.Names(SQLBase.ColumnNames.note)].ToString();
                using (CreateDeadlineForm createDeadlineForm = new CreateDeadlineForm(currentDate, description))
                {
                    if (createDeadlineForm.ShowDialog(this) != DialogResult.OK)
                        return;
                    var note = createDeadlineForm.Description;
                    bool selectAllMonth = createDeadlineForm.ForAllMonths;
                    bool changeTable = false;
                    //var rows = datatable.Select(string.Format("id='{0}'", clientID));
                    //DataRow row = null;
                    //foreach (DataRow curRow in datatable.Rows)//.ToList())
                    //{
                    //    if (((DateTime)curRow[SQLBase.Names(SQLBase.ColumnNames.date)]).Day == day)
                    //    {
                    //        row = curRow;
                    //        break;
                    //    }
                    //}
                    List<DataRow> existRows;
                    List<DateTime> dates = new List<DateTime>();
                    if (!selectAllMonth)
                    {
                        existRows = datatable.Rows.OfType<DataRow>().
                            Where(a => ((DateTime)a["date"]).Day == currentDate.Day &&
                            ((DateTime)a["date"]).Month == currentDate.Month).
                            ToList();
                        dates.Add(currentDate);
                    }
                    else
                    {
                        existRows = datatable.Rows.OfType<DataRow>().
                           Where(a => ((DateTime)a["date"]).Day == currentDate.Day).
                           ToList();
                        for (int i = 1; i <= 12; ++i)
                            dates.Add(new DateTime(currentDate.Year, i, currentDate.Day));
                    }
                    if (string.IsNullOrWhiteSpace(note))
                    {
                        if (existRows.Count > 0)
                        {
                            foreach (DataRow row in existRows)
                                row.Delete();
                            changeTable = true;
                        }
                    }
                    else
                    {
                        foreach (var date in dates)
                        {
                            DataRow row = null;
                            if (existRows.Count > 0)
                                row = existRows[0];
                            changeTable = true;
                            bool insertRow = false;
                            if (row == null)
                            {
                                insertRow = true;
                                row = datatable.NewRow();
                            }
                            row[SQLBase.Names(SQLBase.ColumnNames.id)] = clientID;
                            row[SQLBase.Names(SQLBase.ColumnNames.date)] = date;
                            row[SQLBase.Names(SQLBase.ColumnNames.note)] = note;
                            row[SQLBase.Names(SQLBase.ColumnNames.handsign)] = sql.User.Name;
                            if (insertRow)
                                datatable.Rows.Add(row);
                        }
                    }
                    if (!changeTable)
                        return;
                    try
                    {
                        bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Deadline, datatable);
                        if (!valid)
                            datatable.RejectChanges();
                        else
                        {
                            MessageBox.Show(this, Messages.database_changed);
                        }
                    }
                    catch
                    {
                        datatable.RejectChanges();
                        throw;
                    }
                }
            }
            finally
            {
                await ConnectTableToDataBase();
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the value Changed event for from Date Box and updates the related state.
        /// </summary>
        private async void fromDateBox_ValueChanged()
        {
            if (DesignMode)
                return;
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private void updateButton_Click(object sender, EventArgs e)
        {
        }
    }
}
