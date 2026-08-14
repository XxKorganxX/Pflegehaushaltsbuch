using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class SQLiteTest
    {
        [TestMethod]
        public async Task CreatesUsesRollsBackAndDropsSQLiteDatabase()
        {
            const string settingsFile = "sqlite.ini";
            string databaseFile = DatabaseIntegrationTestSupport.GetSetting(
                settingsFile,
                "DatabaseFile",
                "PFLEGE_TEST_SQLITE_DATABASE_FILE",
                Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_sqlite") + ".db"));
            string password = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "Password", "PFLEGE_TEST_SQLITE_PASSWORD", "integration-test");
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, password, databaseFile);
                sql.Dispose();

                sql = new SQLITE();
                await sql.ConnectAsync(databaseFile, string.Empty, password, databaseFile);
                await DatabaseIntegrationTestSupport.RunSmokeAndRollbackChecks(sql);
            }
            finally
            {
                sql.Dispose();
                await new SQLITE().DropDatabaseAsync(databaseFile, string.Empty, password, databaseFile);
            }

            Assert.IsFalse(File.Exists(databaseFile));
        }

        [TestMethod]
        public async Task RepairsPettyCashTableWhenVersionAlreadyCurrent()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_sqlite_petty_cash") + ".db");
            string password = "integration-test";
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, password, databaseFile);
                await sql.UpdateDataBaseAsync(
                    "DROP VIEW IF EXISTS office_total_amount;"
                    + "ALTER TABLE petty_cash RENAME TO office_cash;"
                    + "CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from office_cash;");

                DataTable table = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.PettyCash, table);

                Assert.AreEqual("petty_cash", sql.GetTableName(SQLBase.SELECT.PettyCash));
            }
            finally
            {
                sql.Dispose();
                await DropDatabaseWithRetryAsync(databaseFile, password);
            }
        }

        [TestMethod]
        public async Task FillAdapterAsync_ReplacesExistingIncompatibleColumnSchema()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_sqlite_schema") + ".db");
            string password = "integration-test";
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, password, databaseFile);

                DataTable table = new DataTable();
                table.Columns.Add("no", typeof(int));

                await sql.FillAdapterAsync(SQLBase.SELECT.Deadlines, table, string.Empty);

                Assert.IsTrue(table.Columns.Contains("no"));
                Assert.AreNotEqual(typeof(int), table.Columns["no"].DataType);
            }
            finally
            {
                sql.Dispose();
                await DropDatabaseWithRetryAsync(databaseFile, password);
            }
        }

        private static async Task DropDatabaseWithRetryAsync(string fileName, string password)
        {
            for (int attempt = 0; attempt < 10; attempt++)
            {
                try
                {
                    await new SQLITE().DropDatabaseAsync(fileName, string.Empty, password, fileName);
                    return;
                }
                catch (IOException) when (attempt < 9)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Thread.Sleep(100);
                }
            }
        }
    }
}
