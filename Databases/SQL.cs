using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Databases
{
    /// <summary>
    /// Represents the SQL component used by the application.
    /// </summary>
    public class SQL : SQLBase
    {
        private SqlConnection connect;
        private readonly SemaphoreSlim connectionLock = new SemaphoreSlim(1, 1);
        private Dictionary<SELECT, SqlDataAdapter> adapters = new Dictionary<SELECT, SqlDataAdapter>();
        private string dataBase;
        public bool TrustServerCertificate { get; set; } = true;
        // Quotes a SQL Server identifier and escapes closing brackets inside the name.
        /// <summary>
        /// Quotes the sql Server Identifier value so it can be used safely by the caller.
        /// </summary>
        public static string QuoteSqlServerIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Messages.sql_invalid_login_name);
            return "[" + name.Replace("]", "]]") + "]";
        }
        /// <summary>
        /// Gets the connection String value from the current application state.
        /// </summary>
        public string GetConnectionString(string host, string username, string password)
        {
            this.host = host;
            this.username = username;
            this.password = password;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = host,
                TrustServerCertificate = TrustServerCertificate
            };
            if (string.IsNullOrWhiteSpace(password))
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = username;
                builder.Password = password;
            }
            return builder.ConnectionString;
        }
        /// <summary>
        /// Gets the connection String value from the current application state.
        /// </summary>
        public string GetConnectionString(string host, string username, string password, string database)
        {
            this.host = host;
            this.database = database;
            this.username = username;
            this.password = password;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(GetConnectionString(host, username, password));
            if (!string.IsNullOrWhiteSpace(database))
                builder.InitialCatalog = database;
            return builder.ConnectionString;
        }
        /// <summary>
        /// Runs the test Connection operation and updates the related application state.
        /// </summary>
        public override async Task<bool> TestConnectionAsync(string host, string database, string username, string password)
        {
            this.dataBase = database;
            connect = new SqlConnection(GetConnectionString(host, username, password));            
            await connect.OpenAsync();
            return true;
        }
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public override async Task ConnectAsync(string host, string username, string password, string database)
        {
            this.dataBase = database;

            connect = new SqlConnection(GetConnectionString(host, username, password, database));
            await connect.OpenAsync();
        }
        /// <summary>
        /// Gets the all Databases value from the current application state.
        /// </summary>
        public override async Task<string[]> GetAllDatabasesAsync(string host, string username, string password)
        {
            if (await TestConnectionAsync(host, "", username, password))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM sys.databases", connect);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        List<string> values = new List<string>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                values.Add(reader.GetString(0));
                            }
                        }
                        return values.ToArray();
                    }
                }
                finally
                {
                    connect.Close();
                }
            }
            return new string[0];
        }
        /// <summary>
        /// Runs the drop Database operation and updates the related application state.
        /// </summary>
        public override async Task DropDatabaseAsync(string host, string username, string password, string database)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString(host, username, password, "")))
            {
                await con.OpenAsync();
                // Database names are identifiers, not command parameters.
                string quotedDatabase = QuoteSqlServerIdentifier(database);
                string sqlCommandText =
                    "IF DB_ID(@databaseName) IS NOT NULL" + Environment.NewLine +
                    "BEGIN" + Environment.NewLine +
                    "    ALTER DATABASE " + quotedDatabase + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" + Environment.NewLine +
                    "    DROP DATABASE " + quotedDatabase + ";" + Environment.NewLine +
                    "END";
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, con);
                sqlCommand.Parameters.AddWithValue("@databaseName", database);
                await sqlCommand.ExecuteNonQueryAsync();
                con.Close();
            }
        }
        /// <summary>
        /// Creates the user data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateUserAsync(string username, string pwd, string database, string host)
        {
            try
            {
                using (SqlCommand command = connect.CreateCommand())
                {
                    command.CommandText =
                        $"CREATE LOGIN {QuoteSqlServerIdentifier(username)} " +
                        "WITH PASSWORD = @password, CHECK_POLICY = OFF";
                    command.Parameters.Add("@password", SqlDbType.NVarChar, 128).Value = pwd;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException err)
            {
                if (err.Number != 15025)
                    throw;
            }
            try
            {
                string quotedUser = QuoteSqlServerIdentifier(username);
                SqlCommand command = new SqlCommand("CREATE USER " + quotedUser + " FOR LOGIN " + quotedUser, connect);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException err)
            {
                if (err.Number != 15023)
                    throw;
            }
            try
            {
                string quotedUser = QuoteSqlServerIdentifier(username);
                string grant =
                    "GRANT ALTER TO " + quotedUser + "\n" +
                    "GRANT SELECT TO " + quotedUser + "\n" +
                    "GRANT INSERT TO " + quotedUser + "\n" +
                    "GRANT UPDATE TO " + quotedUser + "\n" +
                    "GRANT DELETE TO " + quotedUser;
                SqlCommand command = new SqlCommand(grant, connect);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException err)
            {
                if (err.Number != 15025)
                    throw;
            }
        }
        /// <summary>
        /// Deletes the user data from the current workflow.
        /// </summary>
        public override async Task DeleteUserAsync(string username, string pwd, string database, string address)
        {
            string quotedUser = QuoteSqlServerIdentifier(username);
            string cmd = "USE " + QuoteSqlServerIdentifier(database) + "\nDROP USER " + quotedUser + "\nDROP LOGIN " + quotedUser;
            SqlCommand command = new SqlCommand(cmd, connect);
            await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync()
        {
            Version checkVersion;
            if (Version < (checkVersion = new Version(1, 0, 7, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 7, 1)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 7, 2)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 9, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 10, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 11, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 12, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version < (checkVersion = new Version(1, 0, 13, 0)))
            {
                await UpdateAsync(checkVersion);
                Version = checkVersion;
            }
            if (Version >= new Version(1, 0, 13, 0))
            {
                await EnsureUserLoginValuesAsync();
                await EnsureUserLoginThrottleColumnsAsync();
                await EnsureInitialAdminUserAsync();
            }
        }
        /// <summary>
        /// Gets the view value from the current application state.
        /// </summary>
        public override async Task<object> GetViewAsync(string name)
        {
            using (SqlCommand cmd = CreateCommand(string.Format("SELECT * FROM {0}", ValidateSqlIdentifier(name))))
            {
                cmd.CommandType = CommandType.Text;
                return await cmd.ExecuteScalarAsync();
            }
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync(Version version)
        {
            OnPrintVersion(version);
            StringBuilder sb = new StringBuilder();
            if (version == new Version("1.0.7.0"))
            {
                sb.AppendLine("ALTER TABLE version ALTER COLUMN main varchar(64) NOT NULL;");
                sb.AppendLine("IF OBJECT_ID('license', 'U') IS NULL");
                sb.AppendLine("CREATE TABLE license");
                sb.AppendLine("(");
                sb.AppendLine("id int IDENTITY PRIMARY KEY,");
                sb.AppendLine("grade int,");
                sb.AppendLine("[begin] date,");
                sb.AppendLine("expired date,");
                sb.AppendLine("[key] varbinary(2048)");
                sb.AppendLine(");");
                sb.AppendLine("IF COL_LENGTH('advisors', 'email') IS NULL");
                sb.AppendLine("ALTER TABLE advisors ADD email varchar(128);");
                sb.AppendLine("IF COL_LENGTH('assistants', 'active') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE assistants SET [active] = 1 WHERE [active] IS NULL; ALTER TABLE assistants ALTER COLUMN [active] int NOT NULL;');");
                sb.AppendLine("IF COL_LENGTH('barge', 'email') IS NOT NULL");
                sb.AppendLine("EXEC(N'ALTER TABLE barge DROP COLUMN email;');");
            }
            if (version == new Version("1.0.7.1"))
            {
                sb.AppendLine("ALTER TABLE deadlines ALTER COLUMN note varchar(512) NOT NULL");
                sb.AppendLine("ALTER TABLE layouts ADD quittance varbinary(MAX)");
                sb.AppendLine("ALTER TABLE layouts ADD officecash varbinary(MAX)");
                sb.AppendLine("ALTER TABLE company ADD web varchar(256)");
                sb.AppendLine("ALTER TABLE company ADD secretary varchar(128)");
                sb.AppendLine("ALTER TABLE company ADD local_court varchar(128)");
                sb.AppendLine("ALTER TABLE company ADD hrb varchar(64)");
                sb.AppendLine("ALTER TABLE company ADD ik varchar(64)");
                sb.AppendLine("ALTER TABLE company ADD smtp_host varchar(512)");
                sb.AppendLine("ALTER TABLE company ADD smtp_user varchar(512)");
                sb.AppendLine("ALTER TABLE company ADD smtp_key varchar(512)");
                sb.AppendLine("CREATE TABLE company_bank");
                sb.AppendLine("(");
                sb.AppendLine("id int IDENTITY PRIMARY KEY,");
                sb.AppendLine("name varchar(128),");
                sb.AppendLine("code varchar(64),");
                sb.AppendLine("account_no varchar(64),");
                sb.AppendLine("iban varchar(128),");
                sb.AppendLine("bic varchar(64)");
                sb.AppendLine(");");
                sb.AppendLine("CREATE TABLE petty_cash");
                sb.AppendLine("(");
                sb.AppendLine("id int IDENTITY PRIMARY KEY,");
                sb.AppendLine("date date,");
                sb.AppendLine("note varchar(512),");
                sb.AppendLine("account_id int,");
                sb.AppendLine("book_cat int,");
                sb.AppendLine("amount decimal(10,2),");
                sb.AppendLine("handsign varchar(64)");
                sb.AppendLine(");");
            }
            if (version == new Version("1.0.7.2"))
            {
                sb.AppendLine("ALTER TABLE company ADD logo varbinary(MAX)");
                sb.AppendLine("ALTER TABLE company ADD logo_alignment int");
            }
            if (version == new Version("1.0.9.0"))
            {
                sb.AppendLine("IF OBJECT_ID('assistants', 'U') IS NOT NULL AND OBJECT_ID('employees', 'U') IS NULL");
                sb.AppendLine("EXEC sp_rename 'assistants', 'employees';");
                sb.AppendLine("IF OBJECT_ID('barge', 'U') IS NOT NULL AND OBJECT_ID('cash_books', 'U') IS NULL");
                sb.AppendLine("EXEC sp_rename 'barge', 'cash_books';");
                sb.AppendLine("IF OBJECT_ID('bank', 'U') IS NOT NULL AND OBJECT_ID('bank_books', 'U') IS NULL");
                sb.AppendLine("EXEC sp_rename 'bank', 'bank_books';");
                sb.AppendLine("IF OBJECT_ID('books', 'U') IS NOT NULL AND OBJECT_ID('client_books', 'U') IS NULL");
                sb.AppendLine("EXEC sp_rename 'books', 'client_books';");
                sb.AppendLine("IF COL_LENGTH('layouts', 'assistants') IS NOT NULL AND COL_LENGTH('layouts', 'employees') IS NULL");
                sb.AppendLine("EXEC sp_rename 'layouts.assistants', 'employees', 'COLUMN';");
                sb.AppendLine("IF OBJECT_ID('accounts', 'U') IS NULL");
                sb.AppendLine("CREATE TABLE accounts");
                sb.AppendLine("(");
                sb.AppendLine("[id] INTEGER PRIMARY KEY,");
                sb.AppendLine("[type] nvarchar(64) NOT NULL,");
                sb.AppendLine("[active] INTEGER NOT NULL DEFAULT 1,");
                sb.AppendLine("[created_at] datetime NOT NULL DEFAULT GETDATE()");
                sb.AppendLine(");");
                sb.AppendLine("IF NOT EXISTS (SELECT 1 FROM accounts WHERE [id] = 0)");
                sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (0, 'Cash', 1, GETDATE());");
                sb.AppendLine("IF NOT EXISTS (SELECT 1 FROM accounts WHERE [id] = 1)");
                sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (1, 'Bank', 1, GETDATE());");
                sb.AppendLine("IF COL_LENGTH('clients', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE clients ADD [account_id] INTEGER NULL;");
                sb.AppendLine("IF COL_LENGTH('employees', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE employees ADD [account_id] INTEGER NULL;");
                sb.AppendLine("EXEC(N'WITH ClientAccounts AS");
                sb.AppendLine("(");
                sb.AppendLine("SELECT [id], ROW_NUMBER() OVER (ORDER BY [id]) + 1 AS [account_id] FROM clients");
                sb.AppendLine(")");
                sb.AppendLine("UPDATE clients SET [account_id] = ClientAccounts.[account_id]");
                sb.AppendLine("FROM clients INNER JOIN ClientAccounts ON clients.[id] = ClientAccounts.[id]");
                sb.AppendLine("WHERE clients.[account_id] IS NULL;');");
                sb.AppendLine("EXEC(N'WITH EmployeeAccounts AS");
                sb.AppendLine("(");
                sb.AppendLine("SELECT [id], ROW_NUMBER() OVER (ORDER BY [id]) + (SELECT COUNT(*) + 1 FROM clients) AS [account_id] FROM employees");
                sb.AppendLine(")");
                sb.AppendLine("UPDATE employees SET [account_id] = EmployeeAccounts.[account_id]");
                sb.AppendLine("FROM employees INNER JOIN EmployeeAccounts ON employees.[id] = EmployeeAccounts.[id]");
                sb.AppendLine("WHERE employees.[account_id] IS NULL;');");
                sb.AppendLine("EXEC(N'INSERT INTO accounts ([id], [type], [active], [created_at])");
                sb.AppendLine("SELECT clients.[account_id], ''Client'', ISNULL(clients.[active], 1), GETDATE()");
                sb.AppendLine("FROM clients");
                sb.AppendLine("WHERE clients.[account_id] IS NOT NULL AND NOT EXISTS (SELECT 1 FROM accounts WHERE accounts.[id] = clients.[account_id]);');");
                sb.AppendLine("EXEC(N'INSERT INTO accounts ([id], [type], [active], [created_at])");
                sb.AppendLine("SELECT employees.[account_id], ''Employee'', ISNULL(employees.[active], 1), GETDATE()");
                sb.AppendLine("FROM employees");
                sb.AppendLine("WHERE employees.[account_id] IS NOT NULL AND NOT EXISTS (SELECT 1 FROM accounts WHERE accounts.[id] = employees.[account_id]);');");
                sb.AppendLine("IF COL_LENGTH('cash_books', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE cash_books ADD [account_id] INTEGER NULL;");
                sb.AppendLine("IF COL_LENGTH('bank_books', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE bank_books ADD [account_id] INTEGER NULL;");
                sb.AppendLine("IF COL_LENGTH('cash_books', 'account') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE cash_books SET [account_id] = COALESCE(clients.[account_id], employees.[account_id], CASE WHEN cash_books.[account] IN (''Barbestand'', ''Cash'', ''Kasse'') THEN 0 WHEN cash_books.[account] IN (''Bankbestand'', ''Bank'') THEN 1 ELSE NULL END)");
                sb.AppendLine("FROM cash_books LEFT JOIN clients ON cash_books.[account] = ''K'' + RIGHT(''000'' + CAST(clients.[id] AS varchar(10)), 3) LEFT JOIN employees ON cash_books.[account] = ''M'' + RIGHT(''000'' + CAST(employees.[id] AS varchar(10)), 3)");
                sb.AppendLine("WHERE cash_books.[account_id] IS NULL;');");
                sb.AppendLine("IF COL_LENGTH('bank_books', 'account') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE bank_books SET [account_id] = COALESCE(clients.[account_id], employees.[account_id], CASE WHEN bank_books.[account] IN (''Barbestand'', ''Cash'', ''Kasse'') THEN 0 WHEN bank_books.[account] IN (''Bankbestand'', ''Bank'') THEN 1 ELSE NULL END)");
                sb.AppendLine("FROM bank_books LEFT JOIN clients ON bank_books.[account] = ''K'' + RIGHT(''000'' + CAST(clients.[id] AS varchar(10)), 3) LEFT JOIN employees ON bank_books.[account] = ''M'' + RIGHT(''000'' + CAST(employees.[id] AS varchar(10)), 3)");
                sb.AppendLine("WHERE bank_books.[account_id] IS NULL;');");
                sb.AppendLine("IF COL_LENGTH('office_cash', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE office_cash ADD [account_id] INTEGER NULL;");
                sb.AppendLine("IF COL_LENGTH('office_cash', 'account') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE office_cash SET [account_id] = [account] WHERE [account_id] IS NULL;');");
                sb.AppendLine("IF OBJECT_ID('books_AUPD', 'TR') IS NOT NULL DROP TRIGGER books_AUPD;");
                sb.AppendLine("IF OBJECT_ID('books_AINS', 'TR') IS NOT NULL DROP TRIGGER books_AINS;");
                sb.AppendLine("IF OBJECT_ID('books_ADEL', 'TR') IS NOT NULL DROP TRIGGER books_ADEL;");
                sb.AppendLine("IF OBJECT_ID('clients_BUPD', 'TR') IS NOT NULL DROP TRIGGER clients_BUPD;");
                sb.AppendLine("IF OBJECT_ID('clients_AUPD', 'TR') IS NOT NULL DROP TRIGGER clients_AUPD;");
                sb.AppendLine("IF OBJECT_ID('client_books_AUPD', 'TR') IS NOT NULL DROP TRIGGER client_books_AUPD;");
                sb.AppendLine("IF OBJECT_ID('client_books_AINS', 'TR') IS NOT NULL DROP TRIGGER client_books_AINS;");
                sb.AppendLine("IF OBJECT_ID('client_books_ADEL', 'TR') IS NOT NULL DROP TRIGGER client_books_ADEL;");
                sb.AppendLine("DROP VIEW IF EXISTS bank_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS barge_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS cash_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS office_total_amount;");
            }
            if (version == new Version("1.0.10.0"))
            {
                sb.AppendLine("IF COL_LENGTH('office_cash', 'account_id') IS NULL");
                sb.AppendLine("ALTER TABLE office_cash ADD [account_id] INTEGER NULL;");
                sb.AppendLine("IF COL_LENGTH('office_cash', 'account') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE office_cash SET [account_id] = [account] WHERE [account_id] IS NULL;');");
                sb.AppendLine("IF COL_LENGTH('cash_books', 'account') IS NOT NULL");
                sb.AppendLine("ALTER TABLE cash_books DROP COLUMN [account];");
                sb.AppendLine("IF COL_LENGTH('bank_books', 'account') IS NOT NULL");
                sb.AppendLine("ALTER TABLE bank_books DROP COLUMN [account];");
                sb.AppendLine("IF COL_LENGTH('office_cash', 'account') IS NOT NULL");
                sb.AppendLine("ALTER TABLE office_cash DROP COLUMN [account];");
            }
            if (version == new Version("1.0.11.0"))
            {
                sb.AppendLine("DECLARE @dropUserContactConstraints nvarchar(max) = N'';");
                sb.AppendLine("SELECT @dropUserContactConstraints += N'ALTER TABLE users DROP CONSTRAINT ' + QUOTENAME(kc.name) + N';'");
                sb.AppendLine("FROM sys.key_constraints kc");
                sb.AppendLine("INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id");
                sb.AppendLine("INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id");
                sb.AppendLine("WHERE kc.parent_object_id = OBJECT_ID('users') AND c.name IN ('phone', 'fax', 'email');");
                sb.AppendLine("IF LEN(@dropUserContactConstraints) > 0 EXEC sp_executesql @dropUserContactConstraints;");
                sb.AppendLine("IF COL_LENGTH('users', 'phone') IS NOT NULL");
                sb.AppendLine("ALTER TABLE users DROP COLUMN [phone];");
                sb.AppendLine("IF COL_LENGTH('users', 'fax') IS NOT NULL");
                sb.AppendLine("ALTER TABLE users DROP COLUMN [fax];");
                sb.AppendLine("IF COL_LENGTH('users', 'email') IS NOT NULL");
                sb.AppendLine("ALTER TABLE users DROP COLUMN [email];");
            }
            if (version == new Version("1.0.12.0"))
            {
                sb.AppendLine("IF OBJECT_ID('office_cash', 'U') IS NOT NULL AND OBJECT_ID('petty_cash', 'U') IS NULL");
                sb.AppendLine("EXEC sp_rename 'office_cash', 'petty_cash';");
                sb.AppendLine("DROP VIEW IF EXISTS office_total_amount;");
                sb.AppendLine("IF COL_LENGTH('users', 'handsign') IS NULL AND COL_LENGTH('users', 'name') IS NOT NULL");
                sb.AppendLine("EXEC sp_rename 'users.name', 'handsign', 'COLUMN';");
                sb.AppendLine("IF COL_LENGTH('users', 'handsign') IS NOT NULL AND COL_LENGTH('users', 'login') IS NOT NULL");
                sb.AppendLine("EXEC(N'UPDATE users SET [login] = [handsign] WHERE [login] IS NULL OR LTRIM(RTRIM([login])) = '''';');");
            }
            if (version == new Version("1.0.13.0"))
            {
                sb.AppendLine("IF COL_LENGTH('company', 'currency_code') IS NULL");
                sb.AppendLine("ALTER TABLE company ADD [currency_code] nvarchar(3) NULL CONSTRAINT DF_company_currency_code DEFAULT 'EUR';");
                sb.AppendLine("EXEC(N'UPDATE company SET [currency_code] = ''EUR'' WHERE [currency_code] IS NULL OR LTRIM(RTRIM([currency_code])) = '''';');");
                sb.AppendLine("IF COL_LENGTH('users', 'failed_login_attempts') IS NULL");
                sb.AppendLine("ALTER TABLE users ADD [failed_login_attempts] INTEGER NOT NULL CONSTRAINT DF_users_failed_login_attempts DEFAULT 0;");
                sb.AppendLine("IF COL_LENGTH('users', 'last_failed_login') IS NULL");
                sb.AppendLine("ALTER TABLE users ADD [last_failed_login] datetime NULL;");
                sb.AppendLine("IF COL_LENGTH('users', 'locked_until') IS NULL");
                sb.AppendLine("ALTER TABLE users ADD [locked_until] datetime NULL;");
            }
            SqlCommand command = new SqlCommand(sb.ToString(), connect);
            await command.ExecuteNonQueryAsync();
            if (version == new Version("1.0.12.0"))
            {
                using (command = new SqlCommand("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) amount from petty_cash;", connect))
                    await command.ExecuteNonQueryAsync();
            }
            if (version == new Version("1.0.9.0"))
            {
                await CreateTriggerAsync();
                sb.Clear();
                sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) amount from bank_books");
                using (command = new SqlCommand(sb.ToString(), connect))
                    await command.ExecuteNonQueryAsync();
                sb.Clear();
                sb.AppendLine("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) amount from cash_books;");
                using (command = new SqlCommand(sb.ToString(), connect))
                    await command.ExecuteNonQueryAsync();
                sb.Clear();
                sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) amount from office_cash;");
                using (command = new SqlCommand(sb.ToString(), connect))
                    await command.ExecuteNonQueryAsync();
            }
            DataTable versionTable = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Version, versionTable);
            DataRow row = versionTable.NewRow();
            row["main"] = version.ToString();
            versionTable.Rows.Add(row);
            if (!await UpdateAdapterAsync(SQLBase.SELECT.Version, versionTable))
                throw new Exception(Messages.datatable_update_failed);
        }
        /// <summary>
        /// Creates the data Base data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateDataBaseAsync(string host, string user, string password, string database)
        {
            string connectionString = GetConnectionString(host, user, password, "");
            connect = new SqlConnection(connectionString);
            await connect.OpenAsync();
            string quotedDatabase = QuoteSqlServerIdentifier(database);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("create DATABASE " + quotedDatabase + ";");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();

            sb.Clear();
            sb.AppendLine("use " + quotedDatabase + ";");
            CreateFixedTables(sb);
            await CreateUserTablesAsync(sb);
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("INSERT INTO hard_cash VALUES (0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);");
            sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (0, 'Cash', 1, GETDATE());");
            sb.AppendLine("INSERT INTO accounts ([id], [type], [active], [created_at]) VALUES (1, 'Bank', 1, GETDATE());");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("INSERT INTO version VALUES ('1.0.13.0');");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            await CreateTriggerAsync();
            sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) amount from bank_books");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) amount from cash_books;");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) amount from petty_cash;");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
        }
        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (SqlDataAdapter adapter in adapters.Values)
                adapter.Dispose();
            adapters.Clear();
            if (connect != null)
                connect.Close();
        }
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        protected override DbTransaction BeginDbTransaction()
        {
            return connect.BeginTransaction();
        }
        private SqlCommand CreateCommand(string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, connect);
            if (ActiveTransaction != null)
                command.Transaction = (SqlTransaction)ActiveTransaction;
            return command;
        }

        private void UseActiveTransaction(SqlCommand command)
        {
            if (command != null && ActiveTransaction != null)
                command.Transaction = (SqlTransaction)ActiveTransaction;
        }

        private SqlCommand CreateSelectCommand(AdapterCommandInfo commandInfo)
        {
            SqlCommand command = CreateCommand(commandInfo.CommandText);
            foreach (AdapterCommandParameter parameter in commandInfo.Parameters)
                command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
            return command;
        }
        public override async Task FillAdapterAsync(SQLBase.SELECT select, DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                DataTable loadedTable = new DataTable();
                using (SqlCommand command = CreateSelectCommand(commandInfo))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    await Task.Run(() => adapter.Fill(loadedTable));

                ReplaceTableContents(table, loadedTable);
            }
            finally
            {
                connectionLock.Release();
            }
        }
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        public override async Task FillAdapterAsync(SQLBase.SELECT select, DataTable table, params object[] values)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select, values);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                DataTable loadedTable = new DataTable();
                using (SqlCommand command = CreateSelectCommand(commandInfo))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    await Task.Run(() => adapter.Fill(loadedTable));

                ReplaceTableContents(table, loadedTable);
            }
            finally
            {
                connectionLock.Release();
            }
        }
        /// <summary>
        /// Updates the adapter data and refreshes the related application state.
        /// </summary>
        public override async Task<bool> UpdateAdapterAsync(SQLBase.SELECT select, DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                DataTable changes = table.GetChanges();
                if (changes == null)
                    return true;

                AdapterCommandInfo commandInfo = table.ExtendedProperties[AdapterSelectCommandProperty] as AdapterCommandInfo ?? CreateAdapterCommandInfo(select);

                using (SqlCommand command = CreateSelectCommand(commandInfo))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                using (SqlCommandBuilder builder = new SqlCommandBuilder(adapter))
                {
                    adapter.InsertCommand = builder.GetInsertCommand();
                    adapter.DeleteCommand = builder.GetDeleteCommand();
                    adapter.UpdateCommand = builder.GetUpdateCommand();
                    UseActiveTransaction(adapter.SelectCommand);
                    UseActiveTransaction(adapter.InsertCommand);
                    UseActiveTransaction(adapter.DeleteCommand);
                    UseActiveTransaction(adapter.UpdateCommand);

                    int value = await Task.Run(() => adapter.Update(table));
                    return value == changes.Rows.Count;
                }
            }
            finally
            {
                connectionLock.Release();
            }
        }        /// <summary>
        /// Updates the journal data and refreshes the related application state.
        /// </summary>
        public override int UpdateJournal(Enums.UpdateJournal param, DateTime date, string note, string changes = "")
        {
            return 0;
        }
        /// <summary>
        /// Updates the data Base data and refreshes the related application state.
        /// </summary>
        public override async Task<int> UpdateDataBaseAsync(string command)
        {
            connect.ChangeDatabase(dataBase);
            using (SqlCommand cmd = CreateCommand(command))
                return await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Runs the call Functions operation and updates the related application state.
        /// </summary>
        public override async Task<object> CallFunctionsAsync(string name, params object[] values)
        {
            using (SqlCommand cmd = CreateCommand("new_function"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ireturnvalue", SqlDbType.Int);
                cmd.Parameters["@ireturnvalue"].Direction = ParameterDirection.ReturnValue;
                return await cmd.ExecuteScalarAsync();
            }
        }
        /// <summary>
        /// Creates the new Password data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateNewPasswordAsync(string host, string username, string password, string new_password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new Exception(Messages.sql_server_required);

            using (SqlCommand command = connect.CreateCommand())
            {
                command.CommandText = "ALTER LOGIN " + QuoteSqlServerIdentifier(username) + " WITH PASSWORD = @newPassword OLD_PASSWORD = @oldPassword;";
                command.Parameters.Add("@newPassword", SqlDbType.NVarChar, 128).Value = new_password;
                command.Parameters.Add("@oldPassword", SqlDbType.NVarChar, 128).Value = password;
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Creates the fixed Tables data or user interface element for the current workflow.
        /// </summary>
        protected override void CreateFixedTables(StringBuilder sb)
        {
            sb.AppendLine("create table");
            sb.AppendLine("users");
            sb.AppendLine("(");
            sb.AppendLine("[handsign] nvarchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[login] nvarchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[pw] nvarchar(255) NOT NULL,");
            sb.AppendLine("[access] INTEGER NOT NULL,");
            sb.AppendLine("[admin] bit NOT NULL,");
            sb.AppendLine("[failed_login_attempts] INTEGER NOT NULL CONSTRAINT DF_users_failed_login_attempts DEFAULT 0,");
            sb.AppendLine("[last_failed_login] datetime NULL,");
            sb.AppendLine("[locked_until] datetime NULL,");
            sb.AppendLine("PRIMARY KEY(handsign,login)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("company");
            sb.AppendLine("(");
            sb.AppendLine("[name] nvarchar(255) PRIMARY KEY,");
            sb.AppendLine("[secretary] nvarchar(255),");
            sb.AppendLine("[phone] nvarchar(45),");
            sb.AppendLine("[fax] nvarchar(45),");
            sb.AppendLine("[email] nvarchar(255) UNIQUE,");
            sb.AppendLine("[street] nvarchar(255),");
            sb.AppendLine("[zipcode] nvarchar(10),");
            sb.AppendLine("[city] nvarchar(255),");
            sb.AppendLine("[language] nvarchar(64),");
            sb.AppendLine("[web] nvarchar(255),");
            sb.AppendLine("[local_court] nvarchar(255),");
            sb.AppendLine("[hrb] nvarchar(255),");
            sb.AppendLine("[ik] nvarchar(255),");
            sb.AppendLine("[smtp_host] nvarchar(255),");
            sb.AppendLine("[smtp_user] nvarchar(255),");
            sb.AppendLine("[smtp_key] nvarchar(255),");
            sb.AppendLine("[currency_code] nvarchar(3) NULL CONSTRAINT DF_company_currency_code DEFAULT 'EUR',");
            sb.AppendLine("[logo] image,");
            sb.AppendLine("[logo_alignment] Integer");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("company_bank");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[name] nvarchar(255),");
            sb.AppendLine("[code] nvarchar(64),");
            sb.AppendLine("[account_no] nvarchar(255),");
            sb.AppendLine("[iban] nvarchar(128),");
            sb.AppendLine("[bic] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("license");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[grade] Integer,");
            sb.AppendLine("[begin] datetime,");
            sb.AppendLine("[expired] datetime,");
            sb.AppendLine("[key] varbinary(2048)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("layouts");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[accounts] image,");
            sb.AppendLine("[advisors] image,");
            sb.AppendLine("[employees] image,");
            sb.AppendLine("[bank] image,");
            sb.AppendLine("[cash] image,");
            sb.AppendLine("[cashaudit] image,");
            sb.AppendLine("[clients] image,");
            sb.AppendLine("[quittance] image,");
            sb.AppendLine("[officecash] image");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("version");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[main] nvarchar(64)");
            sb.AppendLine(");");
        }

        private async Task EnsureUserLoginThrottleColumnsAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF COL_LENGTH('users', 'failed_login_attempts') IS NULL");
            sb.AppendLine("ALTER TABLE users ADD [failed_login_attempts] INTEGER NOT NULL CONSTRAINT DF_users_failed_login_attempts DEFAULT 0;");
            sb.AppendLine("IF COL_LENGTH('users', 'last_failed_login') IS NULL");
            sb.AppendLine("ALTER TABLE users ADD [last_failed_login] datetime NULL;");
            sb.AppendLine("IF COL_LENGTH('users', 'locked_until') IS NULL");
            sb.AppendLine("ALTER TABLE users ADD [locked_until] datetime NULL;");
            using (SqlCommand command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
        }

        private async Task EnsureUserLoginValuesAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF COL_LENGTH('users', 'handsign') IS NOT NULL AND COL_LENGTH('users', 'login') IS NOT NULL");
            sb.AppendLine("EXEC(N'UPDATE users SET [login] = [handsign] WHERE [login] IS NULL OR LTRIM(RTRIM([login])) = '''';');");
            using (SqlCommand command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
        }

        private async Task EnsureInitialAdminUserAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF COL_LENGTH('users', 'handsign') IS NOT NULL AND COL_LENGTH('users', 'login') IS NOT NULL AND NOT EXISTS (SELECT 1 FROM users)");
            sb.AppendLine("INSERT INTO users ([handsign], [login], [pw], [access], [admin], [failed_login_attempts]) VALUES (N'🛡️', 'Admin', '', 0, 1, 0);");
            using (SqlCommand command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the user Tables data or user interface element for the current workflow.
        /// </summary>
        protected override async Task CreateUserTablesAsync(StringBuilder sb)
        {
            sb.AppendLine("create table");
            sb.AppendLine("advisors");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY,");
            sb.AppendLine("[title] nvarchar(64),");
            sb.AppendLine("[name] nvarchar(255) UNIQUE,");
            sb.AppendLine("[email] nvarchar(255),");
            sb.AppendLine("[co] nvarchar(255),");
            sb.AppendLine("[street] nvarchar(255),");
            sb.AppendLine("[zipcode] nvarchar(45),");
            sb.AppendLine("[city] nvarchar(255),");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("accounts");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY,");
            sb.AppendLine("[type] nvarchar(64) NOT NULL,");
            sb.AppendLine("[active] INTEGER NOT NULL DEFAULT 1,");
            sb.AppendLine("[created_at] datetime NOT NULL DEFAULT GETDATE()");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("employees");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY,");
            sb.AppendLine("[account_id] INTEGER,");
            sb.AppendLine("[name] nvarchar(255) UNIQUE,");
            sb.AppendLine("[account_transfer] DECIMAL(18,2),");
            sb.AppendLine("[amount_payout] DECIMAL(18,2),");
            sb.AppendLine("[amount_payback] DECIMAL(18,2),");
            sb.AppendLine("[amount_payback_type] INTEGER,");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[active] INTEGER,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("bank_books");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[amount] DECIMAL(18,2),");
            sb.AppendLine("[account_id] INTEGER,");
            sb.AppendLine("[book_to] INTEGER,");
            sb.AppendLine("[book_cat] INTEGER,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("cash_books");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[book_cat] INTEGER,");
            sb.AppendLine("[book_to] INTEGER,");
            sb.AppendLine("[amount] DECIMAL(18,2),");
            sb.AppendLine("[account_id] INTEGER,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table hard_cash (");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[001] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[002] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[005] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[010] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[020] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[050] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[1] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[2] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[5] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[10] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[20] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[50] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[100] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[200] INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("[500] INTEGER NOT NULL DEFAULT '0'");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("client_books");
            sb.AppendLine("(");
            sb.AppendLine("[index] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[id] INTEGER,");
            sb.AppendLine("[document_id] INTEGER,");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[book_cat] INTEGER,");
            sb.AppendLine("[book_to] INTEGER,");
            sb.AppendLine("[amount] DECIMAL(18,2),");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("clients");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY,");
            sb.AppendLine("[account_id] INTEGER,");
            sb.AppendLine("[title] nvarchar(8),");
            sb.AppendLine("[name] nvarchar(255) UNIQUE,");
            sb.AppendLine("[street] nvarchar(128),");
            sb.AppendLine("[zipcode] nvarchar(45),");
            sb.AppendLine("[city] nvarchar(128),");
            sb.AppendLine("[born] datetime,");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[account_transfer] DECIMAL(18,2),");
            sb.AppendLine("[amount] DECIMAL(18,2),");
            sb.AppendLine("[lastbook] datetime,");
            sb.AppendLine("[active] INTEGER,");
            sb.AppendLine("[info] INTEGER,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[advisor_id] INTEGER,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("deadlines");
            sb.AppendLine("(");
            sb.AppendLine("[no] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[id] INTEGER,");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("petty_cash");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[account_id] INTEGER,");
            sb.AppendLine("[book_cat] INTEGER,");
            sb.AppendLine("[amount] DECIMAL(18,2),");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("create table");
            sb.AppendLine("record");
            sb.AppendLine("(");
            sb.AppendLine("[id] INTEGER PRIMARY KEY IDENTITY(1,1),");
            sb.AppendLine("[client_id] INTEGER,");
            sb.AppendLine("[index] INTEGER,");
            sb.AppendLine("[date] datetime,");
            sb.AppendLine("[note] nvarchar(512),");
            sb.AppendLine("[filename] nvarchar(255),");
            sb.AppendLine("[file] image,");
            sb.AppendLine("[handsign] nvarchar(64)");
            sb.AppendLine(");");
        }
        /// <summary>
        /// Creates the trigger data or user interface element for the current workflow.
        /// </summary>
        protected override async Task CreateTriggerAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CREATE TRIGGER client_books_AUPD ON client_books AFTER UPDATE As");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("Update clients SET amount=account_transfer + (Select COALESCE(sum(amount),0) from client_books where clients.id=client_books.id);");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where clients.id=client_books.id);");
            sb.AppendLine("END");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("CREATE TRIGGER client_books_AINS ON client_books AFTER INSERT As");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from client_books where clients.id=client_books.id);");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where clients.id=client_books.id);");
            sb.AppendLine("END");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("CREATE TRIGGER client_books_ADEL ON client_books AFTER DELETE As");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from client_books where clients.id=client_books.id);");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where clients.id=client_books.id);");
            sb.AppendLine("END");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("CREATE TRIGGER clients_AUPD ON clients AFTER UPDATE As");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("IF UPDATE(account_transfer)");
            sb.AppendLine("UPDATE clients SET amount=inserted.account_transfer + (Select COALESCE(sum(amount),0) from client_books where inserted.id=client_books.id)");
            sb.AppendLine("FROM clients INNER JOIN inserted ON clients.id=inserted.id;");
            sb.AppendLine("END");
            using (var command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            sb.Clear();
        }
        /// <summary>
        /// Restores the restore data from the selected source.
        /// </summary>
        public override async Task RestoreAsync(string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE advisors;");
            sb.AppendLine("DROP TABLE accounts;");
            sb.AppendLine("DROP TABLE employees;");
            sb.AppendLine("DROP TABLE bank_books;");
            sb.AppendLine("DROP TABLE cash_books;");
            sb.AppendLine("DROP TABLE hard_cash;");
            sb.AppendLine("DROP TABLE client_books;");
            sb.AppendLine("DROP TABLE clients;");
            sb.AppendLine("DROP TABLE deadlines;");
            sb.AppendLine("DROP TABLE petty_cash;");
            sb.AppendLine("DROP TABLE record;");
            await CreateUserTablesAsync(sb);
            using (SqlCommand command = new SqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            await base.RestoreAsync(filename);
            await CreateTriggerAsync();
        }
        /// <summary>
        /// Runs the insert Table operation and updates the related application state.
        /// </summary>
        protected override async Task InsertTableAsync(SQLBase.SELECT select, DataTable to)
        {
            if (!await UpdateAdapterAsync(select, to))
                throw new Exception(Messages.datatable_update_failed);
        }
    }
}
