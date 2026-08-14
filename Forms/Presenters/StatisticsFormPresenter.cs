using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class StatisticsFormPresenter
    {
        private readonly SqlSession session;
        private DataTable dealings;
        private bool dateRangeInitialized;

        public StatisticsFormPresenter(IStatisticsFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            View = view;
            this.session = session;
        }

        protected IStatisticsFormContract View { get; private set; }

        public virtual async Task EnterAsync()
        {
            await LoadDealingsAsync();
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual void UpdateDealings()
        {
            if (dealings == null)
                return;

            decimal maxAmount;
            Dictionary<DateTime, decimal[]> values = BuildStatisticValues(dealings, View.BeginDate, View.EndDate, out maxAmount);
            View.UpdateDiagram(values, maxAmount);
        }

        public static Dictionary<DateTime, decimal[]> BuildStatisticValues(DataTable dealings, DateTime beginDate, DateTime endDate, out decimal maxAmount)
        {
            maxAmount = 0;
            Dictionary<DateTime, decimal[]> values = new Dictionary<DateTime, decimal[]>();
            if (dealings == null)
                return values;

            DateTime current = new DateTime(beginDate.Year, beginDate.Month, 1);
            DateTime next = current.AddMonths(1);
            DateTime end = new DateTime(endDate.Year, endDate.Month, 1);
            if (current > end)
                return values;

            do
            {
                DataRow[] rows = dealings.Rows.OfType<DataRow>()
                    .Where(row => row["date"] != DBNull.Value)
                    .Where(row =>
                    {
                        DateTime date = Convert.ToDateTime(row["date"]);
                        return date >= current && date < next;
                    })
                    .ToArray();

                values[current] = new decimal[] { 0, 0 };
                foreach (DataRow row in rows)
                {
                    if (row["amount"] == DBNull.Value)
                        continue;

                    decimal value = Convert.ToDecimal(row["amount"]);
                    if (value > 0)
                    {
                        values[current][0] += value;
                        maxAmount = Math.Max(maxAmount, values[current][0]);
                    }
                    else if (value < 0)
                    {
                        values[current][1] += Math.Abs(value);
                        maxAmount = Math.Max(maxAmount, values[current][1]);
                    }
                }

                current = current.AddMonths(1);
                next = next.AddMonths(1);
            }
            while (current <= end);

            if (maxAmount != 0)
            {
                foreach (DateTime key in values.Keys.ToArray())
                {
                    values[key][0] /= maxAmount;
                    values[key][1] /= maxAmount;
                }
            }

            return values;
        }

        public virtual async Task StatisticSelectionChangedAsync()
        {
            if (View.SelectedStatisticIndex < 0)
                return;

            await LoadDealingsAsync();
        }

        public virtual void DateChanged()
        {
            UpdateDealings();
        }

        private async Task LoadDealingsAsync()
        {
            if (session.SQL == null)
                return;

            dealings = new DataTable();
            if (View.SelectedStatisticIndex == 0)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Books, dealings);
            if (View.SelectedStatisticIndex == 1)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, dealings);
            if (View.SelectedStatisticIndex == 2)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, dealings);

            InitializeDateRange();
            UpdateDealings();
        }

        private void InitializeDateRange()
        {
            if (dateRangeInitialized || dealings == null)
                return;

            DateTime? beginDate = null;
            DateTime? endDate = null;
            foreach (DataRow row in dealings.Rows)
            {
                if (row[Columns.Date] == DBNull.Value)
                    continue;

                DateTime date = Convert.ToDateTime(row[Columns.Date]);
                if (!beginDate.HasValue || date < beginDate.Value)
                    beginDate = date;
                if (!endDate.HasValue || date > endDate.Value)
                    endDate = date;
            }

            if (!beginDate.HasValue || !endDate.HasValue)
                return;

            View.SetDateRange(beginDate.Value, endDate.Value);
            dateRangeInitialized = true;
        }
    }
}

