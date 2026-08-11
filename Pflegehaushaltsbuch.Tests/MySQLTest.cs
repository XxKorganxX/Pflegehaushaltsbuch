using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class MySQLTest
    {
        [TestMethod]
        public async Task CreatesUsesRollsBackAndDropsMySqlDatabase()
        {
            const string settingsFile = "mysql.ini";
            string host = DatabaseIntegrationTestSupport.GetRequiredSetting(settingsFile, "Host", "PFLEGE_TEST_MYSQL_HOST");
            string user = DatabaseIntegrationTestSupport.GetRequiredSetting(settingsFile, "User", "PFLEGE_TEST_MYSQL_USER");
            string password = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "Password", "PFLEGE_TEST_MYSQL_PASSWORD", string.Empty);
            string sslMode = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "SslMode", "PFLEGE_TEST_MYSQL_SSL_MODE", "None");
            string database = DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_mysql");
            SQLBase sql = DatabaseIntegrationTestSupport.CreateInternalProvider("Pflegehaushaltsbuch.Databases.MySQL");

            using (DatabaseIntegrationTestSupport.UseEnvironment("PFLEGE_MYSQL_SSL_MODE", sslMode))
            {
                try
                {
                    await sql.CreateDataBaseAsync(host, user, password, database);
                    sql.Dispose();

                    sql = DatabaseIntegrationTestSupport.CreateInternalProvider("Pflegehaushaltsbuch.Databases.MySQL");
                    await sql.ConnectAsync(host, user, password, database);
                    await DatabaseIntegrationTestSupport.RunSmokeAndRollbackChecks(sql);
                }
                finally
                {
                    sql.Dispose();
                    SQLBase dropper = DatabaseIntegrationTestSupport.CreateInternalProvider("Pflegehaushaltsbuch.Databases.MySQL");
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
}
