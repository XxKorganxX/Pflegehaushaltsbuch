using System;
using System.Data;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    internal static class DatabaseIntegrationTestSupport
    {
        public static string GetRequiredEnvironment(string name)
        {
            string value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
                Assert.Inconclusive("Set " + name + " to run this integration test.");
            return value;
        }

        public static string GetRequiredSetting(string fileName, string key, string environmentName)
        {
            string value = GetLocalSetting(fileName, key);
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            value = Environment.GetEnvironmentVariable(environmentName);
            if (string.IsNullOrWhiteSpace(value))
                Assert.Inconclusive("Set " + key + " in " + fileName + " or set " + environmentName + " to run this integration test.");
            return value;
        }

        public static string GetSetting(string fileName, string key, string environmentName, string defaultValue)
        {
            string value = GetLocalSetting(fileName, key);
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            value = Environment.GetEnvironmentVariable(environmentName);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public static string GetEnvironment(string name, string defaultValue)
        {
            string value = Environment.GetEnvironmentVariable(name);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public static IDisposable UseEnvironment(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new EnvironmentScope(null, null, false);

            return new EnvironmentScope(name, value, true);
        }

        public static string CreateDatabaseName(string prefix)
        {
            return prefix + "_" + Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        public static void SetTestUser(SQLBase sql)
        {
            Type userType = typeof(SQLBase).Assembly.GetType("Pflegehaushaltsbuch.Data.User");
            object user = Activator.CreateInstance(
                userType,
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                args: new object[] { "Integration Test", "integration.test", 0, true, true },
                culture: CultureInfo.InvariantCulture);

            typeof(SQLBase)
                .GetProperty("User", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SetValue(sql, user, null);
        }

        public static async Task RunSmokeAndRollbackChecks(SQLBase sql)
        {
            await sql.EnsureDatabaseUpdatedAsync();
            await UserAuthenticator.LoginAsync(sql, "Admin", string.Empty);
            Assert.AreEqual("🛡️", sql.User.Handsign);
            Assert.AreEqual("Admin", sql.User.Login);
            Assert.IsTrue(sql.User.Admin);

            SetTestUser(sql);

            await User.CreateUser(sql, "IT", "integration.test", "integration-password", 0, true);
            await UserAuthenticator.LoginAsync(sql, "integration.test", "integration-password");
            Assert.AreEqual("IT", sql.User.Handsign);
            Assert.AreEqual("integration.test", sql.User.Login);

            await AssertUserManagementAsync(sql);
            SetTestUser(sql);

            int clientAccountId = await sql.CreateAccountIdAsync("Client");
            int employeeAccountId = await sql.CreateAccountIdAsync("Employee");
            int clientWithoutBalanceAccountId = await sql.CreateAccountIdAsync("Client");
            int employeeWithoutBalanceAccountId = await sql.CreateAccountIdAsync("Employee");
            Assert.AreEqual(clientAccountId, await GetAccountIdFromAccountsTableAsync(sql, clientAccountId, "Client"));
            Assert.AreEqual(employeeAccountId, await GetAccountIdFromAccountsTableAsync(sql, employeeAccountId, "Employee"));

            DataTable advisors = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Representatives, advisors);
            DataRow advisor = advisors.NewRow();
            advisor["id"] = 10;
            advisor["title"] = "Frau";
            advisor["name"] = "Integration Advisor";
            advisor["email"] = "advisor@example.invalid";
            advisor["co"] = string.Empty;
            advisor["street"] = "Advisor Street 1";
            advisor["zipcode"] = "54321";
            advisor["city"] = "Advisor City";
            advisor["date"] = new DateTime(2026, 8, 1);
            advisor["handsign"] = "Integration Test";
            advisors.Rows.Add(advisor);
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Representatives, advisors));

            advisors.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Representatives, advisors);
            Assert.AreEqual(1, advisors.Rows.Count);
            Assert.AreEqual("Integration Advisor", advisors.Rows[0]["name"].ToString());

            DataTable assistants = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Emploees, assistants);
            DataRow assistant = assistants.NewRow();
            assistant["id"] = 20;
            assistant["account_id"] = employeeAccountId;
            assistant["name"] = "Integration Assistant";
            assistant["account_transfer"] = 100m;
            assistant["amount_payout"] = 100m;
            assistant["amount_payback"] = 0m;
            assistant["amount_payback_type"] = 0;
            assistant["date"] = new DateTime(2026, 8, 1);
            assistant["active"] = 1;
            assistant["handsign"] = "Integration Test";
            assistants.Rows.Add(assistant);
            DataRow assistantWithoutBalance = assistants.NewRow();
            assistantWithoutBalance["id"] = 21;
            assistantWithoutBalance["account_id"] = employeeWithoutBalanceAccountId;
            assistantWithoutBalance["name"] = "Integration Assistant Without Balance";
            assistantWithoutBalance["account_transfer"] = 0m;
            assistantWithoutBalance["amount_payout"] = 0m;
            assistantWithoutBalance["amount_payback"] = 0m;
            assistantWithoutBalance["amount_payback_type"] = 0;
            assistantWithoutBalance["date"] = new DateTime(2026, 8, 1);
            assistantWithoutBalance["active"] = 1;
            assistantWithoutBalance["handsign"] = "Integration Test";
            assistants.Rows.Add(assistantWithoutBalance);
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Emploees, assistants));
            Assert.IsTrue(await sql.UpdateAsistanceAsync("Integration Assistant", new DateTime(2026, 8, 2), 40m, 2));

            assistants.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Emploees, assistants);
            Assert.AreEqual(2, assistants.Rows.Count);
            DataRow repaidAssistant = FindRowByValue(assistants, "name", "Integration Assistant");
            AssertDecimal(60m, repaidAssistant["amount_payout"]);
            AssertDecimal(40m, repaidAssistant["amount_payback"]);
            Assert.AreEqual(2, Convert.ToInt32(repaidAssistant["amount_payback_type"], CultureInfo.InvariantCulture));
            Assert.AreEqual(employeeAccountId, await sql.GetEmployeeAccountIdAsync(20));
            DataRow noBalanceAssistant = FindRowByValue(assistants, "name", "Integration Assistant Without Balance");
            AssertDecimal(0m, noBalanceAssistant["amount_payout"]);
            AssertDecimal(0m, noBalanceAssistant["amount_payback"]);
            Assert.AreEqual(employeeWithoutBalanceAccountId, await sql.GetEmployeeAccountIdAsync(21));

            DataTable clients = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clients);
            DataRow client = clients.NewRow();
            client["id"] = 1;
            client["account_id"] = clientAccountId;
            client["title"] = "Herr";
            client["name"] = "Integration Client";
            client["street"] = "Teststrasse 1";
            client["zipcode"] = "12345";
            client["city"] = "Teststadt";
            client["born"] = new DateTime(1980, 1, 1);
            client["date"] = new DateTime(2026, 8, 1);
            client["account_transfer"] = 0m;
            client["amount"] = 0m;
            client["active"] = 1;
            client["info"] = 0;
            client["note"] = string.Empty;
            client["advisor_id"] = DBNull.Value;
            client["handsign"] = "Integration Test";
            clients.Rows.Add(client);
            DataRow clientWithoutBalance = clients.NewRow();
            clientWithoutBalance["id"] = 2;
            clientWithoutBalance["account_id"] = clientWithoutBalanceAccountId;
            clientWithoutBalance["title"] = "Frau";
            clientWithoutBalance["name"] = "Integration Client Without Balance";
            clientWithoutBalance["street"] = "Second Street 1";
            clientWithoutBalance["zipcode"] = "23456";
            clientWithoutBalance["city"] = "Second City";
            clientWithoutBalance["born"] = new DateTime(1975, 2, 3);
            clientWithoutBalance["date"] = new DateTime(2026, 8, 1);
            clientWithoutBalance["account_transfer"] = 0m;
            clientWithoutBalance["amount"] = 0m;
            clientWithoutBalance["active"] = 1;
            clientWithoutBalance["info"] = 0;
            clientWithoutBalance["note"] = string.Empty;
            clientWithoutBalance["advisor_id"] = DBNull.Value;
            clientWithoutBalance["handsign"] = "Integration Test";
            clients.Rows.Add(clientWithoutBalance);
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clients));
            Assert.AreEqual(clientAccountId, await sql.GetClientAccountIdAsync(1));
            Assert.AreEqual(clientWithoutBalanceAccountId, await sql.GetClientAccountIdAsync(2));

            await AssertDeadlinesAsync(sql);

            Assert.IsTrue(await sql.ToBankAsync(new DateTime(2026, 8, 1), "Bank booking", 10m, clientAccountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand));
            Assert.IsTrue(await sql.ToBargeAsync(new DateTime(2026, 8, 1), "Cash booking", 5m, clientAccountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand));
            var bookResult = await sql.ToBooksAsync("Integration Client", 1, new DateTime(2026, 8, 1), "Client booking", 7m, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);
            Assert.IsTrue(bookResult.Item1);
            var officeResult = await sql.Book2CashOfficeAsync(new DateTime(2026, 8, 1), "Office booking", 3m, SQLBase.BookCategory.Auszahlung, 1);
            Assert.IsTrue(officeResult.Item1);

            DataTable bank = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bank);
            Assert.AreEqual(1, bank.Rows.Count);
            Assert.AreEqual("Bank booking", bank.Rows[0]["note"].ToString());
            AssertDecimal(10m, bank.Rows[0]["amount"]);
            Assert.AreEqual(clientAccountId, Convert.ToInt32(bank.Rows[0]["account_id"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)SQLBase.BookCategory.Einzahlung, Convert.ToInt32(bank.Rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)SQLBase.BookingTo.Bankbestand, Convert.ToInt32(bank.Rows[0]["book_to"], CultureInfo.InvariantCulture));

            DataTable barge = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Cash, barge);
            Assert.AreEqual(1, barge.Rows.Count);
            Assert.AreEqual("Cash booking", barge.Rows[0]["note"].ToString());
            AssertDecimal(5m, barge.Rows[0]["amount"]);
            Assert.AreEqual(clientAccountId, Convert.ToInt32(barge.Rows[0]["account_id"], CultureInfo.InvariantCulture));

            DataTable books = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Books, books);
            Assert.AreEqual(1, books.Rows.Count);
            Assert.AreEqual("Client booking", books.Rows[0]["note"].ToString());
            AssertDecimal(7m, books.Rows[0]["amount"]);
            Assert.AreEqual(1, Convert.ToInt32(books.Rows[0]["document_id"], CultureInfo.InvariantCulture));

            DataTable officeCash = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.PettyCash, officeCash);
            Assert.AreEqual(1, officeCash.Rows.Count);
            Assert.AreEqual("Office booking", officeCash.Rows[0]["note"].ToString());
            AssertDecimal(-3m, officeCash.Rows[0]["amount"]);

            DataTable clientById = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientById, 1);
            Assert.AreEqual(1, clientById.Rows.Count);
            AssertDecimal(7m, clientById.Rows[0]["amount"]);
            AssertDate(new DateTime(2026, 8, 1), clientById.Rows[0]["lastbook"]);
            DataTable clientWithoutBalanceById = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientWithoutBalanceById, 2);
            Assert.AreEqual(1, clientWithoutBalanceById.Rows.Count);
            AssertDecimal(0m, clientWithoutBalanceById.Rows[0]["amount"]);

            using (var transaction = sql.BeginTransaction())
            {
                Assert.IsTrue(await sql.ToBankAsync(new DateTime(2026, 8, 2), "Rolled back bank", 11m, clientAccountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand));
                Assert.IsTrue(await sql.ToBargeAsync(new DateTime(2026, 8, 2), "Rolled back cash", 6m, clientAccountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand));
                transaction.Rollback();
            }

            bank.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bank);
            Assert.AreEqual(1, bank.Rows.Count);

            barge.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Cash, barge);
            Assert.AreEqual(1, barge.Rows.Count);

            books.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Books, books);
            books.Rows[0].Delete();
            using (var transaction = sql.BeginTransaction())
            {
                Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Books, books));
                transaction.Rollback();
            }

            books.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Books, books);
            Assert.AreEqual(1, books.Rows.Count);
        }

        private static async Task AssertUserManagementAsync(SQLBase sql)
        {
            await User.CreateUser(sql, "BL", "blank.login", string.Empty, 0, false);
            await UserAuthenticator.LoginAsync(sql, "blank.login", string.Empty);
            Assert.AreEqual("BL", sql.User.Handsign);
            Assert.AreEqual("blank.login", sql.User.Login);
            Assert.IsFalse(sql.User.Admin);

            await User.UpdateUser(sql, "blank.login", "BZ", "blank.changed", 0, false);
            await UserAuthenticator.LoginAsync(sql, "blank.changed", string.Empty);
            Assert.AreEqual("BZ", sql.User.Handsign);
            Assert.AreEqual("blank.changed", sql.User.Login);

            await User.UpdatePassword(sql, "blank.changed", "changed-password", "blank.changed");
            await UserAuthenticator.LoginAsync(sql, "blank.changed", "changed-password");
            Assert.AreEqual("blank.changed", sql.User.Login);
        }

        private static async Task AssertDeadlinesAsync(SQLBase sql)
        {
            DateTime todayInDifferentYear = DateTime.Today.AddYears(-1);
            DataTable deadlines = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Deadlines, deadlines);

            DataRow deadline = deadlines.NewRow();
            deadline["id"] = 1;
            deadline["date"] = todayInDifferentYear.Date;
            deadline["note"] = "Integration deadline";
            deadline["handsign"] = "Integration Test";
            deadlines.Rows.Add(deadline);
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Deadlines, deadlines));

            deadlines.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByClient, deadlines, 1);
            Assert.AreEqual(1, deadlines.Rows.Count);
            Assert.AreEqual("Integration deadline", deadlines.Rows[0]["note"].ToString());

            deadlines.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByDay, deadlines, DateTime.Today.Date);
            Assert.AreEqual(1, deadlines.Rows.Count);
            Assert.AreEqual("Integration deadline", deadlines.Rows[0]["note"].ToString());

            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, deadlines);
            await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByDay, deadlines, DateTime.Today.Date);
            Assert.IsTrue(deadlines.Columns.Contains("no"));
            Assert.AreEqual(1, deadlines.Rows.Count);
        }

        private static async Task<int> GetAccountIdFromAccountsTableAsync(SQLBase sql, int accountId, string type)
        {
            DataTable accounts = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Accounts, accounts);
            DataRow row = accounts.Rows
                .OfType<DataRow>()
                .FirstOrDefault(item => Convert.ToInt32(item["id"], CultureInfo.InvariantCulture) == accountId);
            Assert.IsNotNull(row);
            Assert.AreEqual(type, row["type"].ToString());
            return Convert.ToInt32(row["id"], CultureInfo.InvariantCulture);
        }

        private static DataRow FindRowByValue(DataTable table, string columnName, string value)
        {
            DataRow row = table.Rows
                .OfType<DataRow>()
                .FirstOrDefault(item => string.Equals(item[columnName].ToString(), value, StringComparison.Ordinal));
            Assert.IsNotNull(row);
            return row;
        }

        public static SQLBase CreateInternalProvider(string typeName)
        {
            Type type = typeof(SQLBase).Assembly.GetType(typeName, throwOnError: true);
            return (SQLBase)Activator.CreateInstance(type);
        }

        private static string GetLocalSetting(string fileName, string key)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", fileName);
            if (!File.Exists(path))
                return null;

            foreach (string line in File.ReadAllLines(path))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal))
                    continue;

                int separator = trimmed.IndexOf('=');
                if (separator < 0)
                    continue;

                string currentKey = trimmed.Substring(0, separator).Trim();
                if (!string.Equals(currentKey, key, StringComparison.OrdinalIgnoreCase))
                    continue;

                return trimmed.Substring(separator + 1).Trim();
            }

            return null;
        }

        private sealed class EnvironmentScope : IDisposable
        {
            private readonly string name;
            private readonly string previousValue;
            private readonly bool active;

            public EnvironmentScope(string name, string value, bool active)
            {
                this.name = name;
                this.active = active;
                if (!active)
                    return;

                previousValue = Environment.GetEnvironmentVariable(name);
                Environment.SetEnvironmentVariable(name, value);
            }

            public void Dispose()
            {
                if (active)
                    Environment.SetEnvironmentVariable(name, previousValue);
            }
        }

        private static void AssertDecimal(decimal expected, object actual)
        {
            Assert.AreEqual(expected, Convert.ToDecimal(actual, CultureInfo.InvariantCulture));
        }

        private static void AssertDate(DateTime expected, object actual)
        {
            Assert.AreEqual(expected.Date, Convert.ToDateTime(actual, CultureInfo.InvariantCulture).Date);
        }
    }
}
