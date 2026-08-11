using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CashCheckUpFormPresenter
    {
        private readonly SqlSession session;
        private CashCheckUpSummary summary;

        public CashCheckUpFormPresenter(ICashCheckUpFormContract view, SqlSession session)
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

        protected ICashCheckUpFormContract View { get; private set; }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            decimal bargeHardmoneyAmount = await GetHardCashAmountAsync();

            DataTable table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, table, string.Empty);
            decimal clientsActive = 0;
            decimal clientsInActive = 0;
            decimal clientsHistory = 0;
            foreach (DataRow row in table.Rows)
            {
                SQLBase.ClientActive clientActive = (SQLBase.ClientActive)Enum.Parse(typeof(SQLBase.ClientActive), row["active"].ToString(), true);
                if (clientActive == SQLBase.ClientActive.Active)
                    clientsActive += decimal.Parse(row["amount"].ToString());
                else if (clientActive == SQLBase.ClientActive.Inactive)
                    clientsInActive += decimal.Parse(row["amount"].ToString());
                else if (clientActive == SQLBase.ClientActive.History)
                    clientsHistory += decimal.Parse(row["amount"].ToString());
            }

            decimal clientTotal = clientsActive + clientsInActive + clientsHistory;

            table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, table);
            decimal assistantsAmount = 0;
            foreach (DataRow row in table.Rows)
            {
                assistantsAmount += decimal.Parse(row["amount_payout"].ToString());
            }

            object bankTotalAmount = await session.SQL.GetViewAsync("bank_total_amount");
            decimal bankAmount = decimal.Parse(bankTotalAmount.ToString());

            decimal calculatedSaldo = clientTotal - assistantsAmount - bankAmount;

            object bargeTotalAmount = await session.SQL.GetViewAsync("cash_total_amount");
            decimal bargeAmount = decimal.Parse(bargeTotalAmount.ToString());
            summary = new CashCheckUpSummary
            {
                ClientsActive = clientsActive,
                ClientsInactive = clientsInActive,
                ClientsHistory = clientsHistory,
                ClientsTotal = clientTotal,
                AssistantsAmount = assistantsAmount,
                BankSaldo = bankAmount,
                CalculatedSaldo = calculatedSaldo,
                DifferenceAmount = bargeHardmoneyAmount - calculatedSaldo,
                CashHolding = bargeAmount,
                HardCashAmount = bargeHardmoneyAmount
            };
            View.ShowCashAudit(summary);
        }

        public virtual async Task<decimal> GetHardCashAmountAsync()
        {
            decimal totalAmount = 0;
            DataTable hardCashTable = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Hardcash, hardCashTable);
            foreach (DataRow row in hardCashTable.Rows)
            {
                totalAmount += Int32.Parse(row["001"].ToString()) * 0.01m;
                totalAmount += Int32.Parse(row["002"].ToString()) * 0.02m;
                totalAmount += Int32.Parse(row["005"].ToString()) * 0.05m;
                totalAmount += Int32.Parse(row["010"].ToString()) * 0.1m;
                totalAmount += Int32.Parse(row["020"].ToString()) * 0.2m;
                totalAmount += Int32.Parse(row["050"].ToString()) * 0.5m;
                totalAmount += Int32.Parse(row["1"].ToString()) * 1.0m;
                totalAmount += Int32.Parse(row["2"].ToString()) * 2.0m;
                totalAmount += Int32.Parse(row["5"].ToString()) * 5.0m;
                totalAmount += Int32.Parse(row["10"].ToString()) * 10.0m;
                totalAmount += Int32.Parse(row["20"].ToString()) * 20.0m;
                totalAmount += Int32.Parse(row["50"].ToString()) * 50.0m;
                totalAmount += Int32.Parse(row["100"].ToString()) * 100.0m;
                totalAmount += Int32.Parse(row["200"].ToString()) * 200.0m;
                totalAmount += Int32.Parse(row["500"].ToString()) * 500.0m;
            }
            return totalAmount;
        }

        public virtual void Print()
        {
            if (summary == null)
                return;

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_active, summary.ClientsActive.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_inactive, summary.ClientsInactive.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients_history, summary.ClientsHistory.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients, summary.ClientsTotal.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_assistants, summary.AssistantsAmount.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_bank, summary.BankSaldo.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_hardmoney_calculated, summary.CalculatedSaldo.ToString("C"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_hardmoney_actually, summary.HardCashAmount.ToString("C"));
            View.PrintCashAudit();
        }
    }
}
