using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Drawing;
using Pflegehaushaltsbuch.Data.Graphics;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Data.Print
{
    /// <summary>
    /// Represents the quittance component used by the application.
    /// </summary>
    public class Quittance : PrintBase
    {
        DataTable clientsTable = new DataTable();
        /// <summary>
        /// Creates a new Quittance instance and initializes the required state.
        /// </summary>
        public Quittance(SQLBase sql)
            : base(sql, Printing.LayoutEnum.quittance) 
        {
            //sql.Adapter(SQL.SELECT.Clients, clientsTable, string.Empty);
        }
        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        public override bool Print(string doumentPath, string doumentName, IWin32Window owner, IList<DataRow> rows, string email = "")
        {
            DataRow row = rows.First();
            sql.Printing.UpdateVariable(Data.Printing.VarNames.date, ((DateTime)row["date"]).ToShortDateString());
            sql.Printing.UpdateVariable(Data.Printing.VarNames.amount, ((decimal)row["amount"]).ToString("C2"));
            //DataRow clientRow = clientsTable.Select(string.Format("id='{0}'", row["id"])).First();
            sql.Printing.UpdateVariable(Data.Printing.VarNames.client, doumentName);//clientRow["name"].ToString());
            sql.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, row["note"].ToString());
            sql.Printing.UpdateVariable(Data.Printing.VarNames.booking_kind, ((SQLBase.BookCategory)Int32.Parse(row["book_cat"].ToString())).ToString());
            return base.Print(doumentPath, string.Format(Messages.quittance, doumentName).Replace(" ","_"), owner, rows, email);
        }
       
    }
}
