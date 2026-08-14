using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class UserAuthenticatorTests
    {
        [TestMethod]
        public async Task LoginAsync_LocksAccountAfterFiveFailedAttemptsAndResetsAfterSuccess()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddUser("Alice", "alice", "correct-password");

            for (int attempt = 0; attempt < 5; attempt++)
                await Assert.ThrowsExactlyAsync<Exception>(() => UserAuthenticator.LoginAsync(sql, "alice", "wrong-password"));

            DataRow lockedUser = sql.Users.Rows[0];
            Assert.AreEqual(5, Convert.ToInt32(lockedUser["failed_login_attempts"], CultureInfo.InvariantCulture));
            Assert.IsTrue(Convert.ToDateTime(lockedUser["locked_until"], CultureInfo.InvariantCulture) > DateTime.Now);

            await Assert.ThrowsExactlyAsync<Exception>(() => UserAuthenticator.LoginAsync(sql, "alice", "correct-password"));

            lockedUser["locked_until"] = DateTime.Now.AddSeconds(-1);
            await UserAuthenticator.LoginAsync(sql, "alice", "correct-password");

            DataRow authenticatedUser = sql.Users.Rows[0];
            Assert.AreEqual(0, Convert.ToInt32(authenticatedUser["failed_login_attempts"], CultureInfo.InvariantCulture));
            Assert.AreEqual(DBNull.Value, authenticatedUser["last_failed_login"]);
            Assert.AreEqual(DBNull.Value, authenticatedUser["locked_until"]);
            Assert.AreEqual("alice", sql.User.Login);
        }

        [TestMethod]
        public async Task LoginAsync_DoesNotUseHandsignAsLoginName()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddUser("AL", "alice", "correct-password");

            await Assert.ThrowsExactlyAsync<Exception>(() => UserAuthenticator.LoginAsync(sql, "AL", "correct-password"));
        }

        [TestMethod]
        public async Task LoginAsync_AllowsEmptyInitialPassword()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddUser("Admin", "Admin", string.Empty);

            await UserAuthenticator.LoginAsync(sql, "Admin", string.Empty);

            Assert.AreEqual("Admin", sql.User.Login);
        }

        [TestMethod]
        public async Task LoginAsync_AuthenticatesUserCreatedInRealSQLiteDatabase()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_login") + ".db");
            string databasePassword = "integration-test";
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, databasePassword, databaseFile);
                await User.CreateUser(sql, "AL", "alice", "correct-password", 0, false);

                await UserAuthenticator.LoginAsync(sql, "alice", "correct-password");

                Assert.AreEqual("AL", sql.User.Handsign);
                Assert.AreEqual("alice", sql.User.Login);
            }
            finally
            {
                sql.Dispose();
                await SQLiteTestDropDatabaseWithRetryAsync(databaseFile, databasePassword);
            }
        }

        [TestMethod]
        public async Task EnsureDatabaseUpdatedAsync_AddsInitialAdminWhenRealSQLiteUsersTableIsEmpty()
        {
            string databaseFile = Path.Combine(Path.GetTempPath(), DatabaseIntegrationTestSupport.CreateDatabaseName("pflege_admin_seed") + ".db");
            string databasePassword = "integration-test";
            SQLITE sql = new SQLITE();

            try
            {
                await sql.CreateDataBaseAsync(databaseFile, string.Empty, databasePassword, databaseFile);
                await sql.EnsureDatabaseUpdatedAsync();

                await UserAuthenticator.LoginAsync(sql, "Admin", string.Empty);

                Assert.AreEqual("🛡️", sql.User.Handsign);
                Assert.AreEqual("Admin", sql.User.Login);
                Assert.IsTrue(sql.User.Admin);
            }
            finally
            {
                sql.Dispose();
                await SQLiteTestDropDatabaseWithRetryAsync(databaseFile, databasePassword);
            }
        }

        private static async Task SQLiteTestDropDatabaseWithRetryAsync(string fileName, string password)
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

        private sealed class FakeSqlDatabase : SQLBase
        {
            public FakeSqlDatabase()
            {
                Users = new DataTable("users");
                Users.Columns.Add("handsign", typeof(string));
                Users.Columns.Add("login", typeof(string));
                Users.Columns.Add("pw", typeof(string));
                Users.Columns.Add("access", typeof(int));
                Users.Columns.Add("admin", typeof(bool));
                Users.Columns.Add("failed_login_attempts", typeof(int));
                Users.Columns.Add("last_failed_login", typeof(DateTime));
                Users.Columns.Add("locked_until", typeof(DateTime));
                VersionTable = new DataTable("version");
                VersionTable.Columns.Add("id", typeof(int));
                VersionTable.Columns.Add("main", typeof(string));
                DataRow version = VersionTable.NewRow();
                version["id"] = 1;
                version["main"] = "1.0.13.0";
                VersionTable.Rows.Add(version);
                VersionTable.AcceptChanges();
            }

            public DataTable Users { get; private set; }
            private DataTable VersionTable { get; }

            public void AddUser(string handsign, string login, string password)
            {
                DataRow row = Users.NewRow();
                row["handsign"] = handsign;
                row["login"] = login;
                row["pw"] = string.IsNullOrEmpty(password) ? string.Empty : PasswordHasher.Hash(password);
                row["access"] = 0;
                row["admin"] = false;
                row["failed_login_attempts"] = 0;
                row["last_failed_login"] = DBNull.Value;
                row["locked_until"] = DBNull.Value;
                Users.Rows.Add(row);
                Users.AcceptChanges();
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table)
            {
                if (select == SELECT.Version)
                {
                    FillTable(table, VersionTable);
                    return Task.CompletedTask;
                }

                if (select != SELECT.Users)
                    throw new NotSupportedException(select.ToString());

                FillTable(table, Users);
                return Task.CompletedTask;
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table, params object[] values)
            {
                return FillAdapterAsync(select, table);
            }

            public override Task<bool> UpdateAdapterAsync(SELECT select, DataTable table)
            {
                if (select != SELECT.Users)
                    throw new NotSupportedException(select.ToString());

                Users = CopyWithoutDeletedRows(table);
                return Task.FromResult(true);
            }

            protected override DbTransaction BeginDbTransaction()
            {
                throw new NotSupportedException();
            }

            protected override Task InsertTableAsync(SELECT select, DataTable to)
            {
                throw new NotSupportedException();
            }

            public override Task<bool> TestConnectionAsync(string host, string database, string username, string password)
            {
                return Task.FromResult(true);
            }

            public override Task ConnectAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task DropDatabaseAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task CreateDataBaseAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task CreateNewPasswordAsync(string host, string username, string password, string new_password)
            {
                return Task.CompletedTask;
            }

            public override Task UpdateAsync()
            {
                return Task.CompletedTask;
            }

            public override Task UpdateAsync(Version version)
            {
                return Task.CompletedTask;
            }

            public override int UpdateJournal(Enums.UpdateJournal param, DateTime date, string note, string changes = "")
            {
                return 0;
            }

            public override Task<int> UpdateDataBaseAsync(string command)
            {
                return Task.FromResult(0);
            }

            public override Task<object> CallFunctionsAsync(string name, params object[] values)
            {
                return Task.FromResult<object>(null);
            }

            protected override void CreateFixedTables(StringBuilder sb)
            {
            }

            protected override Task CreateUserTablesAsync(StringBuilder sb)
            {
                return Task.CompletedTask;
            }

            protected override Task CreateTriggerAsync()
            {
                return Task.CompletedTask;
            }

            private static void FillTable(DataTable target, DataTable source)
            {
                target.Clear();
                target.Columns.Clear();
                foreach (DataColumn column in source.Columns)
                    target.Columns.Add(column.ColumnName, column.DataType);

                foreach (DataRow row in source.Rows.Cast<DataRow>().Where(row => row.RowState != DataRowState.Deleted))
                {
                    DataRow copy = target.NewRow();
                    foreach (DataColumn column in source.Columns)
                        copy[column.ColumnName] = row[column.ColumnName];
                    target.Rows.Add(copy);
                }
                target.AcceptChanges();
            }

            private static DataTable CopyWithoutDeletedRows(DataTable source)
            {
                DataTable copy = source.Clone();
                foreach (DataRow row in source.Rows.Cast<DataRow>().Where(row => row.RowState != DataRowState.Deleted))
                {
                    DataRow newRow = copy.NewRow();
                    foreach (DataColumn column in copy.Columns)
                        newRow[column.ColumnName] = row[column.ColumnName];
                    copy.Rows.Add(newRow);
                }
                copy.AcceptChanges();
                return copy;
            }
        }
    }
}
