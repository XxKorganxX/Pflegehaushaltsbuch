using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class PrintClientsBooksDialogPresenter
    {
        private readonly IPrintClientsBooksDialogContract view;
        private readonly SqlSession session;
        private readonly DataTable clientTable = new DataTable();

        public PrintClientsBooksDialogPresenter(IPrintClientsBooksDialogContract view, SqlSession session)
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
        }

        public virtual async Task ShownAsync()
        {
            if (Program.DesignMode)
            {
                return;
            }

            PrinterSettings settings = new PrinterSettings();
            List<string> printerNames = new List<string>();
            foreach (string name in PrinterSettings.InstalledPrinters)
                printerNames.Add(name);
            view.BindPrinters(printerNames, settings.PrinterName);

            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns[Columns.Id] };

            DataRow[] rows = clientTable.Select(Columns.Active + "=1");
            List<ID_Client_Data> clients = new List<ID_Client_Data>();
            foreach (DataRow row in rows)
            {
                clients.Add(new ID_Client_Data
                {
                    Name = row[Columns.Name].ToString(),
                    ID = Int32.Parse(row[Columns.Id].ToString())
                });
            }
            view.BindClients(clients);
        }

        public virtual async Task PrintAsync()
        {
            if (!view.HasSelectedPrinter)
            {
                throw new Exception(Messages.print_select_printer);
            }

            if (!view.HasSelectedClients)
            {
                throw new Exception(Messages.print_select_client);
            }

            DateTime date = view.SelectedDate;
            date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            if (date > DateTime.Now)
            {
                date = DateTime.Now;
            }

            DataTable advisorTable = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Representatives, advisorTable);
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns[Columns.Id] };

            foreach (ID_Client_Data clientData in view.SelectedClients)
            {
                await PrintClientAsync(advisorTable, clientData, date);
            }
        }

        private async Task PrintClientAsync(DataTable advisorTable, ID_Client_Data clientData, DateTime date)
        {
            int clientID = clientData.ID;
            string clientName = clientData.Name;
            DataRow clientRow = clientTable.Rows.Find(clientID);
            if (clientRow == null)
            {
                throw new Exception(string.Format(Messages.client_not_found_name, clientName));
            }

            DataRow advisorRow = clientRow[Columns.AdvisorId] == DBNull.Value ? null : advisorTable.Rows.Find(clientRow[Columns.AdvisorId]);
            DataTable allBookTable = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.BooksByUser, allBookTable, clientID);

            decimal einnahmen = 0;
            decimal ausgaben = 0;
            DataTable bookOfMonth = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Book, bookOfMonth, clientID, date.Month, date.Year);
            DataRow[] bookRows = bookOfMonth.Select("", Columns.Date);
            foreach (DataRow row in bookRows)
            {
                int category = Int32.Parse(row[Columns.BookCategory].ToString());
                decimal value = Convert.ToDecimal(row[Columns.Amount]);
                if (category == 0)
                {
                    einnahmen += value;
                }
                else
                {
                    ausgaben += value;
                }
            }

            decimal oldAmount = Convert.ToDecimal(clientRow[Columns.AccountTransfer]);
            DateTime previousAmountLimit = new DateTime(date.Year, date.Month, 1);
            foreach (DataRow row in allBookTable.Rows.OfType<DataRow>()
                .Where(row => row[Columns.Date] != DBNull.Value && Convert.ToDateTime(row[Columns.Date]) < previousAmountLimit))
            {
                oldAmount += Convert.ToDecimal(row[Columns.Amount]);
            }

            PrintAddress address = CreatePrintAddress(clientRow, advisorRow);
            string accountName = clientRow[Columns.Name].ToString();
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_title, address.Title);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_name, address.Name);
            if (string.IsNullOrWhiteSpace(address.Co))
            {
                session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, address.Street);
            }
            else
            {
                session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, address.Co + "\n" + address.Street);
            }

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_zip, address.Zipcode);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.advisor_city, address.City);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, date.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, date.ToString("MMMM yyyy"));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, oldAmount.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, (oldAmount + ausgaben + einnahmen).ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.client, accountName);
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C", session.Company.Currencies));
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, view.StatementNote);
            view.PrintClientBooks(view.SelectedPrinter, clientName, bookRows, address.Email);
        }

        private static PrintAddress CreatePrintAddress(DataRow clientRow, DataRow advisorRow)
        {
            if (advisorRow != null)
            {
                return new PrintAddress
                {
                    Title = advisorRow[Columns.Title].ToString(),
                    Co = advisorRow[Columns.Co].ToString(),
                    Name = advisorRow[Columns.Name].ToString(),
                    Street = advisorRow[Columns.Street].ToString(),
                    Zipcode = advisorRow[Columns.Zipcode].ToString(),
                    City = advisorRow[Columns.City].ToString(),
                    Email = advisorRow[Columns.Email].ToString()
                };
            }

            return new PrintAddress
            {
                Title = clientRow[Columns.Title].ToString(),
                Name = clientRow[Columns.Name].ToString(),
                Street = clientRow[Columns.Street].ToString(),
                Zipcode = clientRow[Columns.Zipcode].ToString(),
                City = clientRow[Columns.City].ToString(),
                Email = string.Empty
            };
        }

        private struct PrintAddress
        {
            public string Title { get; set; }
            public string Co { get; set; }
            public string Name { get; set; }
            public string Street { get; set; }
            public string Zipcode { get; set; }
            public string City { get; set; }
            public string Email { get; set; }
        }
    }
}