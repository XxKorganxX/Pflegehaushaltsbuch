using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class PrintBooksDialogPresenter
    {
        private readonly IPrintBooksDialogContract view;
        private readonly SqlSession session;
        private readonly DataTable bookTable;
        private readonly DataTable bookAll = new DataTable();
        private readonly DataTable clientTable = new DataTable();
        private readonly DataTable advisorTable = new DataTable();
        private readonly int clientID;
        private readonly DateTime dateBegin;
        private readonly DateTime dateEnd;
        private string accountName;
        private decimal einnahmen;
        private decimal ausgaben;
        private decimal oldAmount;

        public PrintBooksDialogPresenter(IPrintBooksDialogContract view, SqlSession session, DataTable bookTable, int clientID, DateTime from, DateTime to)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
            this.bookTable = bookTable;
            this.clientID = clientID;
            dateBegin = from;
            dateEnd = to;
        }

        public virtual void Initialize()
        {
            if (Program.DesignMode)
            {
                return;
            }

            List<string> titles = new List<string>();
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
            {
                titles.Add(enumval.GetDisplayName());
            }

            view.BindTitles(titles, 0);
        }

        public virtual async Task ShownAsync()
        {
            if (Program.DesignMode)
            {
                return;
            }

            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns[Columns.Id] };
            DataRow clientRow = clientTable.Rows.Find(clientID);

            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Representatives, advisorTable);
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns[Columns.Id] };
            DataRow advisorRow = advisorTable.Rows.Find(clientRow[Columns.AdvisorId]);

            await session.SQL.FillAdapterAsync(SQLBase.SELECT.BooksByUser, bookAll, clientID);

            oldAmount = Convert.ToDecimal(clientRow[Columns.AccountTransfer]);
            DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            foreach (DataRow row in bookAll.Rows.OfType<DataRow>()
                .Where(row => row[Columns.Date] != DBNull.Value && Convert.ToDateTime(row[Columns.Date]) < previousAmountLimit))
            {
                oldAmount += Convert.ToDecimal(row[Columns.Amount]);
            }

            accountName = clientRow[Columns.Name].ToString();
            if (advisorRow != null)
            {
                view.ShowAdvisorContact(new AdvisorPrintContact
                {
                    Title = advisorRow[Columns.Title].ToString(),
                    Co = advisorRow[Columns.Co].ToString(),
                    Name = advisorRow[Columns.Name].ToString(),
                    Street = advisorRow[Columns.Street].ToString(),
                    Zipcode = advisorRow[Columns.Zipcode].ToString(),
                    City = advisorRow[Columns.City].ToString(),
                    Email = advisorRow[Columns.Email].ToString()
                });
            }
            else
            {
                view.ShowAdvisorContact(new AdvisorPrintContact
                {
                    Title = clientRow[Columns.Title].ToString(),
                    Co = string.Empty,
                    Name = clientRow[Columns.Name].ToString(),
                    Street = clientRow[Columns.Street].ToString(),
                    Zipcode = clientRow[Columns.Zipcode].ToString(),
                    City = clientRow[Columns.City].ToString(),
                    Email = string.Empty
                });
            }
        }

        public virtual void PreparePrint()
        {
            DataRow[] rows = bookTable.Select("", Columns.Date);
            einnahmen = 0;
            ausgaben = 0;

            foreach (DataRow row in rows)
            {
                int category = Int32.Parse(row[Columns.BookCategory].ToString());
                decimal value = Convert.ToDecimal(row[Columns.Amount]);
                if (category == 0)
                {
                    einnahmen += Math.Abs(value);
                }
                else
                {
                    ausgaben += Math.Abs(value);
                }
            }

            AdvisorPrintContact advisor = view.AdvisorContact;
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_title, advisor.Title);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_name, advisor.Name);
            if (string.IsNullOrWhiteSpace(advisor.Co))
            {
                session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, advisor.Street);
            }
            else
            {
                session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, advisor.Co + "\n" + advisor.Street);
            }

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_zip, advisor.Zipcode);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_city, advisor.City);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, CreateOutputDate());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, oldAmount.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, (oldAmount - ausgaben + einnahmen).ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.client, accountName);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, view.StatementNote);

            foreach (var item in session.SQL.Printing.Layouts[Data.Printing.LayoutEnum.accounts].Items)
            {
                item.Encrypt(session.SQL);
            }

            DataRow[] bookRows = bookTable.Select("", Columns.Date);
            view.PrintBooks(accountName, accountName, bookRows, advisor.Email);
        }

        private string CreateOutputDate()
        {
            if (dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
            {
                return dateBegin.ToString("MMMM yyyy");
            }

            return dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");
        }
    }
}