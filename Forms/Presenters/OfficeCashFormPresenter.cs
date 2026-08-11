using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class OfficeCashFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private DataTable table;

        public OfficeCashFormPresenter(IOfficeCashFormContract view, SqlSession session)
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

        protected IOfficeCashFormContract View { get; private set; }

        public virtual void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            View.SetSupervisorRights(rights.IsSupervisor);
            View.SetBookButtonsEnabled(rights.CanInsert | rights.CanModify);
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            object officeTotalAmount = await session.SQL.GetViewAsync("office_total_amount");
            View.SetTotalAmount(decimal.Parse(officeTotalAmount.ToString()).ToString("C"));

            table = new DataTable();
            if (!View.PeriodChecked)
            {
                DateTime date = View.FromDate;
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.OfficeCashByDate, table, date.Month, date.Year);
            }
            else
            {
                DateTime fromDate = View.FromDate;
                DateTime toDate = View.ToDate.AddMonths(1);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.OfficeByPeriod, table, fromDate, toDate);
            }

            table.Columns.Add("document_id");
            DataRow[] rows = table.Select("", "date");
            int documentID = 1;
            foreach (DataRow row in rows)
            {
                row["document_id"] = documentID++;
            }

            View.BindOfficeCash(table);
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual async Task BookAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                CashOfficeBookingInput input;
                if (!View.ShowCashOfficeBookDialog(out input))
                    return;

                await session.SQL.Book2CashOfficeAsync(
                    input.BookingDate,
                    input.BookText,
                    input.Amount,
                    input.BookingCategory,
                    input.Account);

                await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task EnterAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task PrintAsync()
        {
            DataRow[] rows = View.OfficeCashRows.ToArray();
            DateTime dateBegin;
            DateTime dateEnd;
            dateBegin = dateEnd = View.FromDate;
            if (View.PeriodChecked)
                dateEnd = View.ToDate;
            dateEnd = dateEnd.AddMonths(1).AddHours(-1);
            if (dateEnd > DateTime.Now)
                dateEnd = DateTime.Now;

            DataTable fullOfficeCash = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.OfficeCash, fullOfficeCash);

            decimal previousAmount = 0;
            DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            foreach (DataRow row in fullOfficeCash.Rows.OfType<DataRow>()
                .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
            {
                previousAmount += decimal.Parse(row["amount"].ToString());
            }

            decimal einnahmen = 0;
            decimal ausgaben = 0;
            foreach (DataRow row in rows)
            {
                int category = Int32.Parse(row["book_cat"].ToString());
                decimal value = Math.Abs(decimal.Parse(row["amount"].ToString()));
                if (category == 0)
                    einnahmen += value;
                else
                    ausgaben += value;
            }

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());

            string outputDate;
            if (dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
                outputDate = dateBegin.ToString("MMMM yyyy");
            else
                outputDate = dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, outputDate);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, previousAmount.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, (previousAmount - ausgaben + einnahmen).ToString("C"));

            View.PrintOfficeCash(rows);
        }

        public virtual async Task DateChangedAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task PeriodCheckAsync()
        {
            View.SetPeriodControlsVisible(View.PeriodChecked);
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Export()
        {
            if (table == null)
                return;

            string fileName;
            if (!View.ShowSaveFileDialog("B\u00fcrokasse", "Excel|*.xlsx", string.Empty, out fileName))
                return;

            Excel.ExportToExcel(table.DefaultView.ToTable(), fileName);
        }
    }
}
