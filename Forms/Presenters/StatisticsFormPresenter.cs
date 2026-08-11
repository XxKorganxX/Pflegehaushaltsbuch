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
            View.SelectedStatisticIndex = 0;
            View.BeginDate = new DateTime(DateTime.Now.Year, 1, 1);
            View.EndDate = new DateTime(DateTime.Now.Year, 12, 31);
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

            DateTime current = new DateTime(View.BeginDate.Year, View.BeginDate.Month, 1);
            DateTime next = current.AddMonths(1);
            DateTime end = new DateTime(View.EndDate.Year, View.EndDate.Month, 1);
            if (current > end)
                return;

            decimal maxAmount = 0;
            Dictionary<DateTime, decimal[]> values = new Dictionary<DateTime, decimal[]>();
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
                    decimal value;
                    if (decimal.TryParse(row["amount"].ToString(), out value))
                    {
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

            View.UpdateDiagram(values, maxAmount);
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
            dealings = new DataTable();
            if (View.SelectedStatisticIndex == 0)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Books, dealings);
            if (View.SelectedStatisticIndex == 1)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, dealings);
            if (View.SelectedStatisticIndex == 2)
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, dealings);

            UpdateDealings();
        }
    }
}
