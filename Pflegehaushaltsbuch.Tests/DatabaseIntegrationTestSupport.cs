using System;
using System.Data;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                args: new object[] { "Integration Test", string.Empty, string.Empty, "integration@example.invalid", 0, true, true },
                culture: CultureInfo.InvariantCulture);

            typeof(SQLBase)
                .GetProperty("User", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(sql, user, null);
        }

        public static async Task RunSmokeAndRollbackChecks(SQLBase sql)
        {
            SetTestUser(sql);

            DataTable advisors = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisors);
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
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Advisors, advisors));

            advisors.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisors);
            Assert.AreEqual(1, advisors.Rows.Count);
            Assert.AreEqual("Integration Advisor", advisors.Rows[0]["name"].ToString());

            DataTable assistants = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Assistants, assistants);
            DataRow assistant = assistants.NewRow();
            assistant["id"] = 20;
            assistant["name"] = "Integration Assistant";
            assistant["account_transfer"] = 100m;
            assistant["amount_payout"] = 100m;
            assistant["amount_payback"] = 0m;
            assistant["amount_payback_type"] = 0;
            assistant["date"] = new DateTime(2026, 8, 1);
            assistant["active"] = 1;
            assistant["handsign"] = "Integration Test";
            assistants.Rows.Add(assistant);
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Assistants, assistants));
            Assert.IsTrue(await sql.UpdateAsistanceAsync("Integration Assistant", new DateTime(2026, 8, 2), 40m, 2));

            assistants.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Assistants, assistants);
            Assert.AreEqual(1, assistants.Rows.Count);
            AssertDecimal(60m, assistants.Rows[0]["amount_payout"]);
            AssertDecimal(40m, assistants.Rows[0]["amount_payback"]);
            Assert.AreEqual(2, Convert.ToInt32(assistants.Rows[0]["amount_payback_type"], CultureInfo.InvariantCulture));

            DataTable clients = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clients);
            DataRow client = clients.NewRow();
            client["id"] = 1;
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
            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clients));

            Assert.IsTrue(await sql.ToBankAsync(new DateTime(2026, 8, 1), "Bank booking", 10m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand));
            Assert.IsTrue(await sql.ToBargeAsync(new DateTime(2026, 8, 1), "Cash booking", 5m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand));
            var bookResult = await sql.ToBooksAsync("Integration Client", 1, new DateTime(2026, 8, 1), "Client booking", 7m, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);
            Assert.IsTrue(bookResult.Item1);
            var officeResult = await sql.Book2CashOfficeAsync(new DateTime(2026, 8, 1), "Office booking", 3m, SQLBase.BookCategory.Auszahlung, 1);
            Assert.IsTrue(officeResult.Item1);

            DataTable bank = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bank);
            Assert.AreEqual(1, bank.Rows.Count);
            Assert.AreEqual("Bank booking", bank.Rows[0]["note"].ToString());
            AssertDecimal(10m, bank.Rows[0]["amount"]);
            Assert.AreEqual("K001", bank.Rows[0]["account"].ToString());
            Assert.AreEqual((int)SQLBase.BookCategory.Einzahlung, Convert.ToInt32(bank.Rows[0]["book_cat"], CultureInfo.InvariantCulture));
            Assert.AreEqual((int)SQLBase.BookingTo.Bankbestand, Convert.ToInt32(bank.Rows[0]["book_to"], CultureInfo.InvariantCulture));

            DataTable barge = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Barge, barge);
            Assert.AreEqual(1, barge.Rows.Count);
            Assert.AreEqual("Cash booking", barge.Rows[0]["note"].ToString());
            AssertDecimal(5m, barge.Rows[0]["amount"]);
            Assert.AreEqual("K001", barge.Rows[0]["account"].ToString());

            DataTable books = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Books, books);
            Assert.AreEqual(1, books.Rows.Count);
            Assert.AreEqual("Client booking", books.Rows[0]["note"].ToString());
            AssertDecimal(7m, books.Rows[0]["amount"]);
            Assert.AreEqual(1, Convert.ToInt32(books.Rows[0]["document_id"], CultureInfo.InvariantCulture));

            DataTable officeCash = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.OfficeCash, officeCash);
            Assert.AreEqual(1, officeCash.Rows.Count);
            Assert.AreEqual("Office booking", officeCash.Rows[0]["note"].ToString());
            AssertDecimal(-3m, officeCash.Rows[0]["amount"]);

            DataTable clientById = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientById, 1);
            Assert.AreEqual(1, clientById.Rows.Count);
            AssertDecimal(7m, clientById.Rows[0]["amount"]);
            AssertDate(new DateTime(2026, 8, 1), clientById.Rows[0]["lastbook"]);

            using (var transaction = sql.BeginTransaction())
            {
                Assert.IsTrue(await sql.ToBankAsync(new DateTime(2026, 8, 2), "Rolled back bank", 11m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand));
                Assert.IsTrue(await sql.ToBargeAsync(new DateTime(2026, 8, 2), "Rolled back cash", 6m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand));
                transaction.Rollback();
            }

            bank.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bank);
            Assert.AreEqual(1, bank.Rows.Count);

            barge.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.Barge, barge);
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
