using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class SQLTest
    {
        [TestMethod]
        public async Task CreatesUsesRollsBackAndDropsSqlServerDatabase()
        {
            const string settingsFile = "sql.ini";
            string host = DatabaseIntegrationTestSupport.GetRequiredSetting(settingsFile, "Host", "PFLEGE_TEST_SQLSERVER_HOST");
            string user = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "User", "PFLEGE_TEST_SQLSERVER_USER", string.Empty);
            string password = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "Password", "PFLEGE_TEST_SQLSERVER_PASSWORD", string.Empty);
            string database = DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_sql");
            SQL sql = new SQL();

            try
            {
                await sql.CreateDataBaseAsync(host, user, password, database);
                sql.Dispose();

                sql = new SQL();
                await sql.ConnectAsync(host, user, password, database);
                await DatabaseIntegrationTestSupport.RunSmokeAndRollbackChecks(sql);
            }
            finally
            {
                sql.Dispose();
                SQL dropper = new SQL();
                try
                {
                    await dropper.DropDatabaseAsync(host, user, password, database);
                }
                finally
                {
                    dropper.Dispose();
                }
            }
        }
    }
}
