using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

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
            using (XLWorkbook workbook = new XLWorkbook(filename))
            {
                IXLWorksheet worksheet = workbook.Worksheets.First();
                IXLRange usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                    return null;

                int rows = usedRange.RowCount();
                int columns = usedRange.ColumnCount();
                List<DataColumn> columnsList = new List<DataColumn>();
                for (int columnIndex = 1; columnIndex <= columns; columnIndex++)
                {
                    string columnName = GetImportColumnName(usedRange.Cell(1, columnIndex).GetString());
                    if (ignoreColumns != null && ignoreColumns.Contains(columnName, StringComparer.CurrentCultureIgnoreCase))
                        columnsList.Add(null);
                    else
                    {
                        DataColumn column = table.Columns[columnName];
                        columnsList.Add(column);
                    }
                }

                for (int rowIndex = 2; rowIndex <= rows; rowIndex++)
                {
                    if (table.Rows.Count == rowLimit)
                        break;

                    DataRow row = table.NewRow();
                    for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                    {
                        DataColumn column = columnsList[columnIndex];
                        if (column == null)
                            continue;

                        IXLCell cell = usedRange.Cell(rowIndex, columnIndex + 1);
                        if (cell.IsEmpty())
                            row[column] = column.DefaultValue;
                        else
                            row[column] = GetCellValue(cell, column.DataType);
                    }

                    table.Rows.Add(row);
                }
            }

            return null;
        }
        /// <summary>
        /// Runs the export To Excel operation and updates the related application state.
        /// </summary>
        public static void ExportToExcel(
            DataTable tbl, 
            string filename,
            string currencyCode = null)
        {
            DataTable exportTable = tbl.Copy();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(GetWorksheetName(exportTable));

                for (int columnIndex = 0; columnIndex < exportTable.Columns.Count; columnIndex++)
                    worksheet.Cell(1, columnIndex + 1).SetValue(GetExportColumnName(exportTable.Columns[columnIndex], currencyCode));

                for (int rowIndex = 0; rowIndex < exportTable.Rows.Count; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < exportTable.Columns.Count; columnIndex++)
                        SetCellValue(worksheet.Cell(rowIndex + 2, columnIndex + 1), exportTable.Rows[rowIndex][columnIndex]);
                }

                if (exportTable.Columns.Count > 0)
                {
                    IXLRange headerRange = worksheet.Range(1, 1, 1, exportTable.Columns.Count);
                    headerRange.Style.Font.Bold = true;
                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(EnsureExcelExtension(filename));
            }
        }

        private static string GetExportColumnName(DataColumn column, string currencyCode)
        {
            string columnName = column.ColumnName;
            if (IsCurrencyColumn(column) && !string.IsNullOrWhiteSpace(currencyCode))
                return string.Format("{0} ({1})", columnName, currencyCode.Trim().ToUpperInvariant());

            return columnName;
        }

        private static string GetImportColumnName(string columnName)
        {
            int suffixStart = columnName.LastIndexOf(" (", StringComparison.Ordinal);
            if (suffixStart > 0 && columnName.EndsWith(")", StringComparison.Ordinal))
                return columnName.Substring(0, suffixStart);

            return columnName;
        }

        private static bool IsCurrencyColumn(DataColumn column)
        {
            return column.DataType == typeof(decimal)
                && (column.ColumnName.IndexOf("amount", StringComparison.OrdinalIgnoreCase) >= 0
                    || column.ColumnName.Equals("account_transfer", StringComparison.OrdinalIgnoreCase));
        }

        private static string EnsureExcelExtension(string filename)
        {
            if (string.IsNullOrWhiteSpace(Path.GetExtension(filename)))
                return filename + ".xlsx";

            return filename;
        }

        private static string GetWorksheetName(DataTable table)
        {
            string name = string.IsNullOrWhiteSpace(table.TableName) ? "Sheet1" : table.TableName;
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                name = name.Replace(invalidChar, '_');

            name = name.Replace('[', '_')
                .Replace(']', '_')
                .Replace(':', '_')
                .Replace('*', '_')
                .Replace('?', '_')
                .Replace('/', '_')
                .Replace('\\', '_');

            if (name.Length > 31)
                name = name.Substring(0, 31);

            return string.IsNullOrWhiteSpace(name) ? "Sheet1" : name;
        }

        private static void SetCellValue(IXLCell cell, object value)
        {
            if (value == null || value == DBNull.Value)
                return;

            if (value is DateTime)
            {
                cell.SetValue((DateTime)value);
                cell.Style.DateFormat.Format = ((DateTime)value).TimeOfDay == TimeSpan.Zero
                    ? "dd.MM.yyyy"
                    : "dd.MM.yyyy HH:mm:ss";
                return;
            }

            if (value is bool)
            {
                cell.SetValue((bool)value);
                return;
            }

            if (value is byte || value is short || value is int || value is long ||
                value is float || value is double || value is decimal)
            {
                cell.SetValue(Convert.ToDouble(value));
                return;
            }

            cell.SetValue(value.ToString());
        }

        private static object GetCellValue(IXLCell cell, Type targetType)
        {
            Type nullableType = Nullable.GetUnderlyingType(targetType);
            if (nullableType != null)
                targetType = nullableType;

            if (targetType == typeof(string))
                return cell.GetFormattedString();

            if (targetType == typeof(DateTime))
                return cell.GetDateTime();

            if (targetType == typeof(bool))
                return cell.GetBoolean();

            if (targetType == typeof(byte))
                return Convert.ToByte(cell.GetDouble());

            if (targetType == typeof(short))
                return Convert.ToInt16(cell.GetDouble());

            if (targetType == typeof(int))
                return Convert.ToInt32(cell.GetDouble());

            if (targetType == typeof(long))
                return Convert.ToInt64(cell.GetDouble());

            if (targetType == typeof(float))
                return Convert.ToSingle(cell.GetDouble());

            if (targetType == typeof(double))
                return cell.GetDouble();

            if (targetType == typeof(decimal))
                return Convert.ToDecimal(cell.GetDouble());

            return cell.GetFormattedString();
        }
    }
}
