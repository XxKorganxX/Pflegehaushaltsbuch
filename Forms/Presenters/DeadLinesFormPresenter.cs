using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DeadLinesFormPresenter
    {
        public SqlSession session { get; private set; }

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private readonly DataTable datatable = new DataTable();
        private readonly DataTable clientTable = new DataTable();
        private int startCell;
        private int maxDaysInMonth;
        private int maxRows;

        public DeadLinesFormPresenter(IDeadLinesFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IDeadLinesFormContract View { get; private set; }

        public virtual async Task EnterAsync()
        {
            await ConnectTableToDataBaseAsync();
            await ConnectToClientAsync();
            View.ShowClientName(GetClientName());
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            DateTime currentMonth = View.CurrentMonth;
            maxDaysInMonth = currentMonth.AddMonths(1).AddHours(-1).Day;
            datatable.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Deadline, datatable, View.ClientID, currentMonth.Month);

            var startDateMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            startCell = (int)startDateMonth.DayOfWeek - 1;
            if (startCell < 0)
                startCell += 7;

            maxRows = (int)Math.Ceiling((maxDaysInMonth + startCell) / 7.0);

            int currentCell = -1;
            int currentDay = 0;
            DataTable table = new DataTable();
            table.Columns.Add("Mo").DefaultValue = string.Empty;
            table.Columns.Add("Tu").DefaultValue = string.Empty;
            table.Columns.Add("We").DefaultValue = string.Empty;
            table.Columns.Add("Th").DefaultValue = string.Empty;
            table.Columns.Add("Fr").DefaultValue = string.Empty;
            table.Columns.Add("Sa").DefaultValue = string.Empty;
            table.Columns.Add("Su").DefaultValue = string.Empty;

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

            View.ShowCalendar(new DeadlineCalendar
            {
                Table = table,
                StartCell = startCell,
                MaxDaysInMonth = maxDaysInMonth,
                MaxRows = maxRows
            });
        }

        public virtual async Task ConnectToClientAsync()
        {
            clientTable.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, View.ClientID.ToString());
        }

        public virtual async Task ExportAsync()
        {
            string selectedFileName;
            if (!View.ShowExportDialog(string.Format(Messages.deadlines_export_filename, View.ClientID), out selectedFileName))
                return;

            DataTable table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, table, View.ClientID);
            Excel.ExportToExcel(table.DefaultView.ToTable(), selectedFileName, session.Company.CurrencyCode);
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Leave()
        {
            View.ClearClientName();
        }

        public virtual void Back()
        {
            View.ShowClientsForm();
        }

        public static List<DateTime> BuildDeadlineDates(DateTime currentDate, bool forAllMonths)
        {
            if (!forAllMonths)
                return new List<DateTime> { currentDate };

            List<DateTime> dates = new List<DateTime>();
            for (int month = 1; month <= 12; ++month)
            {
                if (currentDate.Day <= DateTime.DaysInMonth(currentDate.Year, month))
                    dates.Add(new DateTime(currentDate.Year, month, currentDate.Day));
            }

            return dates;
        }

        public virtual async Task CellClickAsync(int rowIndex, int columnIndex)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (rowIndex == -1)
                    return;

                int day = rowIndex * 7 + columnIndex + 1 - startCell;
                if (day < 1 || day > maxDaysInMonth)
                    return;

                DateTime currentDate = new DateTime(2000, View.CurrentMonth.Month, day);
                DataTable deadlineTable = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, deadlineTable, View.ClientID);

                var useExistRows = deadlineTable.Rows.OfType<DataRow>()
                    .Where(a => ((DateTime)a["date"]).Day == day &&
                                ((DateTime)a["date"]).Month == View.CurrentMonth.Month)
                    .ToList();

                string description = string.Empty;
                if (useExistRows.Count > 0)
                    description = useExistRows[0][Columns.Note].ToString();

                DeadlineInput input;
                if (!View.ShowCreateDeadlineDialog(currentDate, description, out input))
                    return;

                    string note = input.Description;
                    bool selectAllMonth = input.ForAllMonths;
                    bool changeTable = false;
                    List<DateTime> dates = BuildDeadlineDates(currentDate, selectAllMonth);

                    List<DataRow> existRows = deadlineTable.Rows.OfType<DataRow>()
                        .Where(a => dates.Any(date => ((DateTime)a["date"]).Day == date.Day &&
                                                       ((DateTime)a["date"]).Month == date.Month))
                        .ToList();

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
                            DataRow row = existRows.FirstOrDefault(a => ((DateTime)a["date"]).Day == date.Day &&
                                                                        ((DateTime)a["date"]).Month == date.Month);

                            changeTable = true;
                            bool insertRow = false;
                            if (row == null)
                            {
                                insertRow = true;
                                row = deadlineTable.NewRow();
                            }

                            row[Columns.Id] = View.ClientID;
                            row[Columns.Date] = date;
                            row[Columns.Note] = note;
                            row[Columns.HandSign] = session.SQL.User.Handsign;

                            if (insertRow)
                                deadlineTable.Rows.Add(row);
                        }
                    }

                    if (!changeTable)
                        return;

                    try
                    {
                        bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Deadline, deadlineTable);
                        if (!valid)
                            deadlineTable.RejectChanges();
                        else
                            View.ShowDatabaseChanged();
                    }
                    catch
                    {
                        deadlineTable.RejectChanges();
                        throw;
                    }
            }
            finally
            {
                await ConnectTableToDataBaseAsync();
                databaseOperationLock.Release();
            }
        }

        public virtual async Task DateChangedAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Update()
        {
        }

        private string GetClientName()
        {
            if (clientTable.Rows.Count == 0 || clientTable.Rows[0][Columns.Name] == DBNull.Value)
                return string.Empty;

            return clientTable.Rows[0][Columns.Name].ToString();
        }
    }
}
