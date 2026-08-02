using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Print Books Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PrintBooksDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private DataTable bookTable, bookAll = new DataTable(), clientTable = new DataTable(), advisorTable = new DataTable(), companyTable = new DataTable();
        private int clientID;
        private string accountName;
        decimal einnahmen = 0;
        decimal ausgaben = 0;
        decimal oldAmount = 0;
        //decimal currentAccount = 0;
        DateTime dateBegin, dateEnd;
        /// <summary>
        /// Creates a new Print Books Form instance and initializes the required state.
        /// </summary>
        public PrintBooksDialog(SQLBase sql, DataTable bookTable, int clientID, string clientAmount, DateTime from, DateTime to)
        {
            InitializeComponent();
            if (Program.DesignMode)
                return;
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                titleBox.Items.Add(enumval.GetDisplayName());
            this.sql = sql;
            this.bookTable = bookTable;
            this.clientID = clientID;
            dateBegin = from;
            dateEnd = to;
            //fromDate = new DateTime(fromDate.Year, fromDate.Month, DateTime.DaysInMonth(fromDate.Year, fromDate.Month));
            //if (fromDate < DateTime.Now)
            //    this.date = fromDate;
            titleBox.SelectedIndex = 0;
        }
        /// <summary>
        /// Handles the shown event for PDF Books Form and updates the related state.
        /// </summary>
        private async void PDFBooksForm_Shown(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns["id"] };
            DataRow clientRow = clientTable.Rows.Find(clientID);
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisorTable);
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns["id"] };
            DataRow advisorRow = advisorTable.Rows.Find(clientRow["advisor_id"]);
            await sql.FillAdapterAsync(SQLBase.SELECT.BooksByUser, bookAll, clientID);
            int rowCount1 = bookAll.Rows.Count;
            oldAmount = decimal.Parse(clientRow["account_transfer"].ToString());
            DateTime previousAmountLimit = new DateTime(dateBegin.Year, dateBegin.Month, 1);
            foreach (DataRow row in bookAll.Rows.OfType<DataRow>()
                .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
                oldAmount += decimal.Parse(row["amount"].ToString());
            //DateTime nextMonth = date.AddMonths(1);
            //foreach (DataRow row in bookAll.Select(string.Format("Date < #{0}/1/{1}#", nextMonth.Month, nextMonth.Year)))
            //{
            //    currentAccount += decimal.Parse(row["amount"].ToString());
            //}
            accountName = clientRow["name"].ToString();
            if (advisorRow != null)
            {
                titleBox.Text = advisorRow["title"].ToString();
                coBox.Text = advisorRow["co"].ToString();
                clientNameBox.Text = advisorRow["name"].ToString();
                clientStreetBox.Text = advisorRow["street"].ToString();
                clientZipcodeBox.Text = advisorRow["zipcode"].ToString();
                clientCityBox.Text = advisorRow["city"].ToString();
                emailBox.Text = advisorRow["email"].ToString();
            }
            else
            {
                titleBox.Text = clientRow["title"].ToString();
                clientNameBox.Text = clientRow["name"].ToString();
                clientStreetBox.Text = clientRow["street"].ToString();
                clientZipcodeBox.Text = clientRow["zipcode"].ToString();
                clientCityBox.Text = clientRow["city"].ToString();
            }
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            PreparePrint();
        }
        /// <summary>
        /// Runs the prepare Print operation and updates the related application state.
        /// </summary>
        private void PreparePrint()
        {
            DataRow[] rows = bookTable.Select("", "date");
            einnahmen = 0;
            ausgaben = 0;
            foreach(DataRow row in rows)
            {
                int category = Int32.Parse(row["book_cat"].ToString());
                decimal value = decimal.Parse(row["amount"].ToString());
                if (category == 0)
                    einnahmen += Math.Abs(value);
                else
                    ausgaben += Math.Abs(value);
            }
            sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_title, titleBox.Text);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_name, clientNameBox.Text);
            if (string.IsNullOrWhiteSpace(coBox.Text))
                sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, clientStreetBox.Text);
            else
                sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, coBox.Text+"\n"+clientStreetBox.Text);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_zip, clientZipcodeBox.Text);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_city, clientCityBox.Text);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            sql.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, dateEnd.ToShortDateString());
            string ouputDate = "";
            if(dateBegin.Year == dateEnd.Year && dateBegin.Month == dateEnd.Month)
                ouputDate = dateBegin.ToString("MMMM yyyy");
            else
                ouputDate = dateBegin.ToString("MMMM yyyy") + " - " + dateEnd.ToString("MMMM yyyy");
            sql.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, ouputDate);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, oldAmount.ToString("C"));
            sql.Printing.UpdateVariable(Data.Printing.VarNames.amount, (oldAmount - ausgaben + einnahmen).ToString("C"));
            sql.Printing.UpdateVariable(Data.Printing.VarNames.client, accountName);
            sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C"));
            sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C"));
            sql.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, accountText.Text);
            foreach (var item in sql.Printing.Layouts[Data.Printing.LayoutEnum.accounts].Items)
                item.Encrypt(sql);
            DataRow[] bookRows = bookTable.Select("", "date");
            PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.accounts);
            printer.Print(accountName, Text + "_"+accountName, this, bookRows, emailBox.Text);
        }
    }
}
