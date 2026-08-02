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
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Print Clients Books Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PrintClientsBooksDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private DataTable clientTable = new DataTable();
        /// <summary>
        /// Creates a new Print Clients Books Form instance and initializes the required state.
        /// </summary>
        public PrintClientsBooksDialog(SQLBase sql)
        {
            InitializeComponent();
            if (Program.DesignMode)
                return;
            this.sql = sql;
        }
        /// <summary>
        /// Handles the shown event for PDF Books Form and updates the related state.
        /// </summary>
        private async void PDFBooksForm_Shown(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            
            foreach (string name in PrinterSettings.InstalledPrinters)
            {
                printerBox.Items.Add(name);
            }
            var test = new PrinterSettings();
            printerBox.SelectedItem = test.PrinterName;
            
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns["id"] };
            DataRow[] rows = clientTable.Select("active=1");
            clientView.Items.Clear();
            foreach (DataRow row in rows)
                clientView.Items.Add(
                    new ID_Client_Data()
                    {
                        Name = row[SQLBase.Names(SQLBase.ColumnNames.name)].ToString(),
                        ID = Int32.Parse(row[SQLBase.Names(SQLBase.ColumnNames.id)].ToString())
                    }
                );
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Print();
        }
        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        private async void Print()
        {
            try
            {
                if (printerBox.SelectedItem == null)
                    throw new Exception(Messages.print_select_printer);
                if (clientView.SelectedItems.Count == 0)
                    throw new Exception(Messages.print_select_client);

                DateTime date = dateTimeBox.Date;
                date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
                if (date > DateTime.Now)
                    date = DateTime.Now;

                DataTable advisorTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisorTable);
                advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns["id"] };

                PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.accounts);
                foreach (ID_Client_Data clientData in clientView.SelectedItems)
                {
                    int clientID = clientData.ID;
                    string clientName = clientData.Name;
                    DataRow clientRow = clientTable.Rows.Find(clientID);
                    if (clientRow == null)
                        throw new Exception(string.Format(Messages.client_not_found_name, clientName));

                    DataRow advisorRow = clientRow["advisor_id"] == DBNull.Value ? null : advisorTable.Rows.Find(clientRow["advisor_id"]);
                    DataTable allBookTable = new DataTable();
                    await sql.FillAdapterAsync(SQLBase.SELECT.BooksByUser, allBookTable, clientID);
                    decimal einnahmen = 0;
                    decimal ausgaben = 0;
                    DataTable bookOfMonth = new DataTable();
                    await sql.FillAdapterAsync(SQLBase.SELECT.Book, bookOfMonth, clientID, date.Month, date.Year);
                    DataRow[] bookRows = bookOfMonth.Select("", "date");
                    foreach (DataRow row in bookRows)
                    {
                        int category = Int32.Parse(row["book_cat"].ToString());
                        decimal value = decimal.Parse(row["amount"].ToString());
                        if (category == 0)
                            einnahmen += value;
                        else
                            ausgaben += value;
                    }
                    decimal oldAmount = decimal.Parse(clientRow["account_transfer"].ToString());
                    DateTime previousAmountLimit = new DateTime(date.Year, date.Month, 1);
                    foreach (DataRow row in allBookTable.Rows.OfType<DataRow>()
                        .Where(row => row["date"] != DBNull.Value && Convert.ToDateTime(row["date"]) < previousAmountLimit))
                        oldAmount += decimal.Parse(row["amount"].ToString());
                    string title, co = string.Empty, name, street, zipcode, city, email = string.Empty;
                    if (advisorRow != null)
                    {
                        title = advisorRow["title"].ToString();
                        co = advisorRow["co"].ToString();
                        name = advisorRow["name"].ToString();
                        street = advisorRow["street"].ToString();
                        zipcode = advisorRow["zipcode"].ToString();
                        city = advisorRow["city"].ToString();
                        email = advisorRow["email"].ToString();
                    }
                    else
                    {
                        title = clientRow["title"].ToString();
                        name = clientRow["name"].ToString();
                        street = clientRow["street"].ToString();
                        zipcode = clientRow["zipcode"].ToString();
                        city = clientRow["city"].ToString();
                    }
                    string accountName = clientRow["name"].ToString();
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_title, title);
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_name, name);
                    if (string.IsNullOrWhiteSpace(co))
                        sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, street);
                    else
                        sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_addr, co + "\n" + street);
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_zip, zipcode);
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.advisor_city, city);
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.date_of_paper, date.ToShortDateString());
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.date_long_of_paper, date.ToString("MMMM yyyy"));
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_previous_month, oldAmount.ToString("C"));
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.amount, (oldAmount + ausgaben + einnahmen).ToString("C"));
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.client, accountName);
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_outflow, ausgaben.ToString("C"));
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.cash_inflow, einnahmen.ToString("C"));
                    sql.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, accountText.Text);
                    printer.PrintDirect(printerBox.SelectedItem.ToString(), Text + "_" + clientName, this, bookRows, email);
                }
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
    }
}
