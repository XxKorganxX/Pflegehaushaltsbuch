using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class Verwahrgeld2SeedTest
    {
        [TestMethod]
        public async Task CreatesVerwahrgeld2WithDemoData()
        {
            AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.UseSystemDefaultSecureProtocols", true);
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            const string settingsFile = "sql.ini";
            const string database = "Verwahrgeld2";
            string host = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "Host", "PFLEGE_TEST_SQLSERVER_HOST", @"localhost\SQLEXPRESS");
            string user = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "User", "PFLEGE_TEST_SQLSERVER_USER", string.Empty);
            string password = DatabaseIntegrationTestSupport.GetSetting(settingsFile, "Password", "PFLEGE_TEST_SQLSERVER_PASSWORD", string.Empty);

            SeedSql probe = new SeedSql();
            try
            {
                try
                {
                    string[] databases = await probe.GetAllDatabasesAsync(host, user, password);
                    if (databases.Any(name => string.Equals(name, database, StringComparison.OrdinalIgnoreCase)))
                        Assert.Inconclusive(database + " exists already. Delete or rename it before running this seed test.");
                }
                finally
                {
                    probe.Dispose();
                }

                SeedSql sql = new SeedSql();
                try
                {
                    await sql.CreateDataBaseAsync(host, user, password, database);
                    sql.Dispose();

                    sql = new SeedSql();
                    await sql.ConnectAsync(host, user, password, database);
                    await sql.EnsureDatabaseUpdatedAsync();
                    await UserAuthenticator.LoginAsync(sql, "Admin", string.Empty);
                    await User.UpdatePassword(sql, "Admin", "admin", "Admin");

                    await SeedAsync(sql);
                }
                finally
                {
                    sql.Dispose();
                }
            }
            catch (SqlException ex)
            {
                Assert.Inconclusive("Could not create " + database + " on SQL Server " + host + ": " + ex.Message);
            }
        }

        private static async Task SeedAsync(SQLBase sql)
        {
            int[] advisorIds = await SeedAdvisorsAsync(sql);
            ClientSeed[] clients = await SeedClientsAsync(sql, advisorIds);
            EmployeeSeed[] employees = await SeedEmployeesAsync(sql);

            await SeedClientBookingsAsync(sql, clients);
            await SeedEmployeeBookingsAsync(sql, employees);
            await SeedOfficeCashBookingsAsync(sql, clients, employees);
        }

        private static async Task<int[]> SeedAdvisorsAsync(SQLBase sql)
        {
            DataTable advisors = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Representatives, advisors);

            for (int i = 1; i <= 10; i++)
            {
                DataRow row = advisors.NewRow();
                row["id"] = i;
                row["title"] = i % 2 == 0 ? "Herr" : "Frau";
                row["name"] = "Betreuer " + i.ToString("00");
                row["email"] = "betreuer" + i.ToString("00") + "@example.invalid";
                row["co"] = string.Empty;
                row["street"] = "Betreuerweg " + i;
                row["zipcode"] = "10" + i.ToString("000");
                row["city"] = "Demostadt";
                row["date"] = new DateTime(2020, 1, 1).AddMonths(i - 1);
                row["handsign"] = "Admin";
                advisors.Rows.Add(row);
            }

            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Representatives, advisors));
            return Enumerable.Range(1, 10).ToArray();
        }

        private static async Task<ClientSeed[]> SeedClientsAsync(SQLBase sql, int[] advisorIds)
        {
            DataTable clientsTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientsTable);

            ClientSeed[] clients = new ClientSeed[10];
            for (int i = 1; i <= clients.Length; i++)
            {
                int accountId = await sql.CreateAccountIdAsync("Client");
                decimal openingBalance = 150m + (i * 37.50m);
                DateTime startDate = new DateTime(2020, 1, 15).AddMonths(i - 1);

                DataRow row = clientsTable.NewRow();
                row["id"] = i;
                row["account_id"] = accountId;
                row["title"] = i % 2 == 0 ? "Herr" : "Frau";
                row["name"] = "Klient " + i.ToString("00");
                row["street"] = "Klientenstrasse " + i;
                row["zipcode"] = "20" + i.ToString("000");
                row["city"] = "Musterstadt";
                row["born"] = new DateTime(1950 + i, (i % 12) + 1, (i % 25) + 1);
                row["date"] = startDate;
                row["account_transfer"] = 0m;
                row["amount"] = 0m;
                row["active"] = 1;
                row["info"] = 0;
                row["note"] = "Demo-Klient mit Einstiegsgeld";
                row["advisor_id"] = advisorIds[(i - 1) % advisorIds.Length];
                row["handsign"] = "Admin";
                clientsTable.Rows.Add(row);

                clients[i - 1] = new ClientSeed(i, "Klient " + i.ToString("00"), accountId, openingBalance, startDate);
            }

            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientsTable));
            return clients;
        }

        private static async Task<EmployeeSeed[]> SeedEmployeesAsync(SQLBase sql)
        {
            DataTable employeesTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Emploees, employeesTable);

            EmployeeSeed[] employees = new EmployeeSeed[15];
            for (int i = 1; i <= employees.Length; i++)
            {
                int accountId = await sql.CreateAccountIdAsync("Employee");
                decimal loan = i % 3 == 0 ? 250m + (i * 20m) : 0m;

                DataRow row = employeesTable.NewRow();
                row["id"] = i;
                row["account_id"] = accountId;
                row["name"] = "Mitarbeiter " + i.ToString("00");
                row["account_transfer"] = loan;
                row["amount_payout"] = loan;
                row["amount_payback"] = 0m;
                row["amount_payback_type"] = loan > 0m ? 2 : 0;
                row["date"] = new DateTime(2020, 2, 1).AddMonths(i - 1);
                row["active"] = 1;
                row["handsign"] = "Admin";
                employeesTable.Rows.Add(row);

                employees[i - 1] = new EmployeeSeed(i, "Mitarbeiter " + i.ToString("00"), accountId, loan);
            }

            Assert.IsTrue(await sql.UpdateAdapterAsync(SQLBase.SELECT.Emploees, employeesTable));
            return employees;
        }

        private static async Task SeedClientBookingsAsync(SQLBase sql, ClientSeed[] clients)
        {
            DateTime today = DateTime.Today;

            foreach (ClientSeed client in clients)
            {
                SQLBase.BookingTo openingTarget = client.Id % 2 == 0 ? SQLBase.BookingTo.Barbestand : SQLBase.BookingTo.Bankbestand;
                await BookClientAsync(sql, client, client.StartDate, "Einstiegsgeld " + client.Name, client.OpeningBalance, openingTarget);

                for (DateTime date = client.StartDate.AddMonths(1); date <= today; date = date.AddMonths(1))
                {
                    decimal deposit = 35m + ((client.Id + date.Month) % 6) * 12.50m;
                    decimal payout = 15m + ((client.Id * 3 + date.Month) % 5) * 7.25m;
                    SQLBase.BookingTo depositTarget = (date.Month + client.Id) % 2 == 0 ? SQLBase.BookingTo.Bankbestand : SQLBase.BookingTo.Barbestand;
                    SQLBase.BookingTo payoutTarget = depositTarget == SQLBase.BookingTo.Bankbestand ? SQLBase.BookingTo.Barbestand : SQLBase.BookingTo.Bankbestand;

                    await BookClientAsync(sql, client, date, "Taschengeld Eingang " + date.ToString("yyyy-MM"), deposit, depositTarget);

                    if ((date.Month + client.Id) % 3 != 0)
                        await BookClientAsync(sql, client, date.AddDays(10), "Auszahlung Einkauf " + date.ToString("yyyy-MM"), -payout, payoutTarget);
                }
            }
        }

        private static async Task BookClientAsync(SQLBase sql, ClientSeed client, DateTime date, string note, decimal amount, SQLBase.BookingTo target)
        {
            SQLBase.BookCategory category = amount >= 0m ? SQLBase.BookCategory.Einzahlung : SQLBase.BookCategory.Auszahlung;
            Assert.IsTrue((await sql.ToBooksAsync(client.Name, client.Id, date.Date, note, amount, category, target)).Item1);

            if (target == SQLBase.BookingTo.Bankbestand)
                Assert.IsTrue(await sql.ToBankAsync(date.Date, note, amount, client.AccountId, category, target));
            else
                Assert.IsTrue(await sql.ToBargeAsync(date.Date, note, amount, client.AccountId, category, target));
        }

        private static async Task SeedEmployeeBookingsAsync(SQLBase sql, EmployeeSeed[] employees)
        {
            foreach (EmployeeSeed employee in employees.Where(item => item.Loan > 0m))
            {
                DateTime loanDate = new DateTime(2021, 1, 10).AddMonths(employee.Id);
                SQLBase.BookingTo target = employee.Id % 2 == 0 ? SQLBase.BookingTo.Bankbestand : SQLBase.BookingTo.Barbestand;

                if (target == SQLBase.BookingTo.Bankbestand)
                    Assert.IsTrue(await sql.ToBankAsync(loanDate, "Darlehenausgabe " + employee.Name, -employee.Loan, employee.AccountId, SQLBase.BookCategory.Auszahlung, target));
                else
                    Assert.IsTrue(await sql.ToBargeAsync(loanDate, "Darlehenausgabe " + employee.Name, -employee.Loan, employee.AccountId, SQLBase.BookCategory.Auszahlung, target));

                decimal payback = Math.Round(employee.Loan / 4m, 2);
                for (int i = 1; i <= 4; i++)
                {
                    DateTime paybackDate = loanDate.AddMonths(i);
                    if (target == SQLBase.BookingTo.Bankbestand)
                        Assert.IsTrue(await sql.ToBankAsync(paybackDate, "Darlehensrueckzahlung " + employee.Name, payback, employee.AccountId, SQLBase.BookCategory.Einzahlung, target));
                    else
                        Assert.IsTrue(await sql.ToBargeAsync(paybackDate, "Darlehensrueckzahlung " + employee.Name, payback, employee.AccountId, SQLBase.BookCategory.Einzahlung, target));
                }
            }
        }

        private static async Task SeedOfficeCashBookingsAsync(SQLBase sql, ClientSeed[] clients, EmployeeSeed[] employees)
        {
            for (int monthOffset = 0; monthOffset < 48; monthOffset++)
            {
                DateTime date = new DateTime(2020, 1, 5).AddMonths(monthOffset);
                ClientSeed client = clients[monthOffset % clients.Length];
                EmployeeSeed employee = employees[monthOffset % employees.Length];

                Assert.IsTrue((await sql.Book2CashOfficeAsync(date, "Buerokasse Material " + client.Name, 8m + monthOffset, SQLBase.BookCategory.Auszahlung, client.AccountId)).Item1);
                Assert.IsTrue((await sql.Book2CashOfficeAsync(date.AddDays(12), "Buerokasse Erstattung " + employee.Name, 5m + (monthOffset % 7), SQLBase.BookCategory.Einzahlung, employee.AccountId)).Item1);
            }
        }

        private sealed class ClientSeed
        {
            public ClientSeed(int id, string name, int accountId, decimal openingBalance, DateTime startDate)
            {
                Id = id;
                Name = name;
                AccountId = accountId;
                OpeningBalance = openingBalance;
                StartDate = startDate;
            }

            public int Id { get; }
            public string Name { get; }
            public int AccountId { get; }
            public decimal OpeningBalance { get; }
            public DateTime StartDate { get; }
        }

        private sealed class EmployeeSeed
        {
            public EmployeeSeed(int id, string name, int accountId, decimal loan)
            {
                Id = id;
                Name = name;
                AccountId = accountId;
                Loan = loan;
            }

            public int Id { get; }
            public string Name { get; }
            public int AccountId { get; }
            public decimal Loan { get; }
        }

        private sealed class SeedSql : SQL
        {
            public override async Task<bool> TestConnectionAsync(string host, string database, string username, string password)
            {
                await ConnectWithSeedConnectionStringAsync(host, username, password, database);
                return true;
            }

            public override async Task ConnectAsync(string host, string username, string password, string database)
            {
                await ConnectWithSeedConnectionStringAsync(host, username, password, database);
            }

            public override async Task CreateDataBaseAsync(string host, string username, string password, string database)
            {
                await ConnectWithSeedConnectionStringAsync(host, username, password, string.Empty);
                string quotedDatabase = QuoteSqlServerIdentifier(database);

                using (SqlCommand command = new SqlCommand("CREATE DATABASE " + quotedDatabase + ";", Connection))
                    await command.ExecuteNonQueryAsync();

                using (SqlCommand command = new SqlCommand("USE " + quotedDatabase + ";", Connection))
                    await command.ExecuteNonQueryAsync();
                SetDataBase(database);

                StringBuilder sb = new StringBuilder();
                CreateFixedTables(sb);
                await CreateUserTablesAsync(sb);
                using (SqlCommand command = new SqlCommand(sb.ToString(), Connection))
                    await command.ExecuteNonQueryAsync();

                sb.Clear();
                sb.AppendLine("INSERT INTO hard_cash VALUES (0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);");
                sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (0, 'Cash', 1, GETDATE());");
                sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (1, 'Bank', 1, GETDATE());");
                sb.AppendLine("INSERT INTO version VALUES ('1.0.13.0');");
                using (SqlCommand command = new SqlCommand(sb.ToString(), Connection))
                    await command.ExecuteNonQueryAsync();

                await CreateTriggerAsync();

                using (SqlCommand command = new SqlCommand("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) amount from bank_books", Connection))
                    await command.ExecuteNonQueryAsync();
                using (SqlCommand command = new SqlCommand("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) amount from cash_books;", Connection))
                    await command.ExecuteNonQueryAsync();
                using (SqlCommand command = new SqlCommand("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) amount from petty_cash;", Connection))
                    await command.ExecuteNonQueryAsync();
            }

            private SqlConnection Connection
            {
                get
                {
                    return (SqlConnection)typeof(SQL)
                        .GetField("connect", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(this);
                }
            }

            private async Task ConnectWithSeedConnectionStringAsync(string host, string username, string password, string database)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = host,
                    Encrypt = false,
                    TrustServerCertificate = true
                };

                if (!string.IsNullOrWhiteSpace(database))
                    builder.InitialCatalog = database;

                if (string.IsNullOrWhiteSpace(password))
                {
                    builder.IntegratedSecurity = true;
                }
                else
                {
                    builder.UserID = username;
                    builder.Password = password;
                }

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                await connection.OpenAsync();
                SetConnection(connection);
                SetDataBase(database);
            }

            private void SetConnection(SqlConnection connection)
            {
                typeof(SQL)
                    .GetField("connect", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(this, connection);
            }

            private void SetDataBase(string database)
            {
                typeof(SQL)
                    .GetField("dataBase", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(this, database);
            }
        }
    }
}
