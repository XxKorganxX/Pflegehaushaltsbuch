using System;
using System.IO;
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
    }
}
