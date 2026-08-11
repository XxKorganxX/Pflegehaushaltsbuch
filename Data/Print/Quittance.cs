using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
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
        public Quittance(SqlSession session)
            : base(session, Printing.LayoutEnum.quittance)
        {
            //sql.Adapter(SQL.SELECT.Clients, clientsTable, string.Empty);
        }
        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        public override bool Print(string doumentPath, string doumentName, IWin32Window owner, IList<DataRow> rows, string email = "")
        {
            DataRow row = rows.First();
            Session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, ((DateTime)row["date"]).ToShortDateString());
            Session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount, ((decimal)row["amount"]).ToString("C2"));
            //DataRow clientRow = clientsTable.Select(string.Format("id='{0}'", row["id"])).First();
            Session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.client, doumentName);//clientRow["name"].ToString());
            Session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.statement_note, row["note"].ToString());
            Session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.booking_kind, ((SQLBase.BookCategory)Int32.Parse(row["book_cat"].ToString())).ToString());
            return base.Print(doumentPath, string.Format(Messages.quittance, doumentName).Replace(" ","_"), owner, rows, email);
        }
       
    }
}
