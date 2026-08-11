using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Provides helper methods for excel operations used by the application.
    /// </summary>
    public static class Excel
    {
        /// <summary>
        /// Runs the import operation and updates the related application state.
        /// </summary>
        public static DataTable Import(string filename, DataTable table, int rowLimit = int.MaxValue, HashSet<string> ignoreColumns = null)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook = excelApp.Workbooks.Open(filename, false, true);
            try
            {
                Microsoft.Office.Interop.Excel.Sheets excelSheets = workBook.Worksheets;
                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range usedRange = workSheet.UsedRange;
                var data = (System.Array)usedRange.Value;
                var rows = data.GetLength(0);
                var columns = data.GetLength(1);
                List<DataColumn> columnsList = new List<DataColumn>();
                for (int i = 1; i <= columns; i++)
                {
                    string columnName = data.GetValue(1, i).ToString();
                    if (ignoreColumns != null && ignoreColumns.Contains(columnName, StringComparer.CurrentCultureIgnoreCase))
                        columnsList.Add(null);
                    else
                    {
                        DataColumn column = table.Columns[columnName];
                        columnsList.Add(column);
                    }
                }
                for (int i = 2; i <= rows; i++)
                {
                    if (table.Rows.Count == rowLimit)
                        break;
                    DataRow row = table.NewRow();
                    for (int j = 0; j < columns; j++)
                    {
                        DataColumn column1 = columnsList[j];
                        if (column1 == null)
                            continue;
                        var value = data.GetValue(i, j + 1);
                        if (value == null)
                            row[column1] = column1.DefaultValue;
                        else
                            row[column1] = value;
                    }
                    table.Rows.Add(row);
                }
            }
            finally
            {
                workBook.Close(false, filename);
                excelApp.Quit();
            }
            return null;
        }
        /// <summary>
        /// Runs the export To Excel operation and updates the related application state.
        /// </summary>
        public static void ExportToExcel(
            DataTable tbl, 
            string filename)
        {
            tbl.Columns.Remove("handsign");
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workBook = excelApp.Workbooks.Add();
            try
            {
                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range HeaderRange =
                    workSheet.get_Range((Microsoft.Office.Interop.Excel.Range)
                    (workSheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)
                    (workSheet.Cells[1, tbl.Columns.Count]));
                object[] Header = new object[tbl.Columns.Count];
                // column headings               
                for (int i = 0; i < Header.Length; i++)
                    Header[i] = tbl.Columns[i].ColumnName;
                HeaderRange.Value = Header;
                Microsoft.Office.Interop.Excel.Range CellRange =
                workSheet.get_Range((Microsoft.Office.Interop.Excel.Range)
                    (workSheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)
                    (workSheet.Cells[tbl.Rows.Count + 1, tbl.Columns.Count]));
                // DataCells
                int RowsCount = tbl.Rows.Count;
                object[,] Cells = new object[RowsCount, tbl.Columns.Count];
                for (int j = 0; j < RowsCount; j++)
                {
                    for (int i = 0; i < tbl.Columns.Count; i++)
                        Cells[j, i] = tbl.Rows[j][i];
                }
                CellRange.Value = Cells;
                var ext = Path.GetExtension(filename);
                if (!string.IsNullOrWhiteSpace(ext))
                    filename = filename.Replace(ext, "");
                workBook.SaveAs(filename);
            }
            finally
            {
                workBook.Close();
                excelApp.Quit();
            }
        }
    }
}
