using System;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Data;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class ExcelTests
    {
        [TestMethod]
        public void ExportToExcelAndImportRoundTripsDataWithoutChangingSourceTable()
        {
            DataTable source = CreateTable("Bookings");
            source.Rows.Add(1, "Entry", 12.50m, new DateTime(2026, 8, 12), true, "Tester");

            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xlsx");
            try
            {
                Excel.ExportToExcel(source, filename);

                Assert.IsTrue(File.Exists(filename));
                Assert.IsTrue(source.Columns.Contains("handsign"));

                DataTable imported = CreateTable("Imported");

                Excel.Import(filename, imported);

                Assert.AreEqual(1, imported.Rows.Count);
                Assert.AreEqual(1, imported.Rows[0]["id"]);
                Assert.AreEqual("Entry", imported.Rows[0]["note"]);
                Assert.AreEqual(12.50m, imported.Rows[0]["amount"]);
                Assert.AreEqual(new DateTime(2026, 8, 12), imported.Rows[0]["date"]);
                Assert.AreEqual(true, imported.Rows[0]["active"]);
                Assert.AreEqual("Tester", imported.Rows[0]["handsign"]);
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        [TestMethod]
        public void ImportUsesColumnDefaultValueForEmptyCells()
        {
            DataTable source = CreateTable("Defaults");
            source.Rows.Add(2, DBNull.Value, DBNull.Value, new DateTime(2026, 8, 13), false, "Tester");

            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xlsx");
            try
            {
                Excel.ExportToExcel(source, filename);

                DataTable imported = CreateTable("ImportedDefaults");
                imported.Columns["note"].DefaultValue = "Default note";
                imported.Columns["amount"].DefaultValue = 7.25m;

                Excel.Import(filename, imported);

                Assert.AreEqual(1, imported.Rows.Count);
                Assert.AreEqual("Default note", imported.Rows[0]["note"]);
                Assert.AreEqual(7.25m, imported.Rows[0]["amount"]);
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        [TestMethod]
        public void ExportAddsCurrencyCodeToAmountHeadersAndImportAcceptsThem()
        {
            DataTable source = CreateTable("CurrencyHeaders");
            source.Columns.Add("account_transfer", typeof(decimal));
            source.Columns.Add("amount_payback_type", typeof(int));
            source.Rows.Add(1, "Entry", 12.50m, new DateTime(2026, 8, 12), true, "Tester", 3.75m, 2);

            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xlsx");
            try
            {
                Excel.ExportToExcel(source, filename, "EUR");

                DataTable imported = CreateTable("ImportedCurrencyHeaders");
                imported.Columns.Add("account_transfer", typeof(decimal));
                imported.Columns.Add("amount_payback_type", typeof(int));

                Excel.Import(filename, imported);

                Assert.AreEqual(1, imported.Rows.Count);
                Assert.AreEqual(12.50m, imported.Rows[0]["amount"]);
                Assert.AreEqual("Tester", imported.Rows[0]["handsign"]);
                Assert.AreEqual(3.75m, imported.Rows[0]["account_transfer"]);
                Assert.AreEqual(2, imported.Rows[0]["amount_payback_type"]);
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        private static DataTable CreateTable(string name)
        {
            DataTable table = new DataTable(name);
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("note", typeof(string));
            table.Columns.Add("amount", typeof(decimal));
            table.Columns.Add("date", typeof(DateTime));
            table.Columns.Add("active", typeof(bool));
            table.Columns.Add("handsign", typeof(string));
            return table;
        }
    }
}
