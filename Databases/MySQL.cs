using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Databases
{
    /// <summary>
    /// Represents the MySQL component used by the application.
    /// </summary>
    class MySQL : SQLBase
    {
        /// <summary>
        /// Creates a new My SQL instance and initializes the required state.
        /// </summary>
        public MySQL()
        {
            selectCommand[SELECT.BargeByPeriod] = "SELECT * FROM cash_books WHERE (date) >= {0:yyyy-MM-dd} AND (date) < {1:yyyy-MM-dd}";
            selectCommand[SELECT.BankByPeriod] = "SELECT * FROM bank_books WHERE (date) >= {0:yyyy-MM-dd} AND (date) < {1:yyyy-MM-dd}";
            selectCommand[SELECT.OfficeByPeriod] = "SELECT * FROM office_cash WHERE (date) >= {0:yyyy-MM-dd} AND (date) < {1:yyyy-MM-dd}";
            selectCommand[SELECT.BooksByPeriod] = "SELECT * FROM client_books WHERE id='{0}' AND (date) >= {1:yyyy-MM-dd} AND (date) < {2:yyyy-MM-dd}";
        }
        private MySqlConnection connect;
        private readonly SemaphoreSlim connectionLock = new SemaphoreSlim(1, 1);
        private Dictionary<SELECT, MySqlDataAdapter> adapters = new Dictionary<SELECT, MySqlDataAdapter>();
        private string dataBase;
        // Quotes a MySQL identifier and escapes backticks inside the name.
        /// <summary>
        /// Quotes the my Sql Identifier value so it can be used safely by the caller.
        /// </summary>
        private static string QuoteMySqlIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Messages.mysql_invalid_identifier);
            return "`" + name.Replace("`", "``") + "`";
        }
        // Escapes SQL string literals used in account-management statements.
        /// <summary>
        /// Quotes the my Sql String value so it can be used safely by the caller.
        /// </summary>
        private static string QuoteMySqlString(string value)
        {
            if (value == null)
                return "NULL";
            return "'" + value.Replace("\\", "\\\\").Replace("'", "''") + "'";
        }
        /// <summary>
        /// Gets the connection String value from the current application state.
        /// </summary>
        private static string GetConnectionString(string host, string username, string password, string database = null, MySqlSslMode? sslMode = null)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = host,
                UserID = username,
                Password = password,
                SslMode = sslMode ?? GetConfiguredSslMode(),
                AllowUserVariables = true
            };
            if (!string.IsNullOrWhiteSpace(database))
                builder.Database = database;
            return builder.ConnectionString;
        }
        private static MySqlSslMode GetConfiguredSslMode()
        {
            string value = Environment.GetEnvironmentVariable("PFLEGE_MYSQL_SSL_MODE");
            if (string.IsNullOrWhiteSpace(value))
                return MySqlSslMode.Preferred;

            MySqlSslMode sslMode;
            if (Enum.TryParse(value, ignoreCase: true, result: out sslMode))
                return sslMode;

            return MySqlSslMode.Preferred;
        }
        private static async Task<MySqlConnection> OpenConnectionAsync(string host, string username, string password, string database = null)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString(host, username, password, database));
            try
            {
                await connection.OpenAsync();
                return connection;
            }
            catch (Win32Exception) when (GetConfiguredSslMode() == MySqlSslMode.Preferred)
            {
                connection.Dispose();
                connection = new MySqlConnection(GetConnectionString(host, username, password, database, MySqlSslMode.None));
                await connection.OpenAsync();
                return connection;
            }
        }
        /// <summary>
        /// Runs the test Connection operation and updates the related application state.
        /// </summary>
        public override async Task<bool> TestConnectionAsync(string host, string database, string username, string password)
        {
            connect = await OpenConnectionAsync(host, username, password);
            
            return true;
        }
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public override async Task ConnectAsync(string host, string username, string password, string database)
        {
            this.dataBase = database;
            try
            {
                connect = await OpenConnectionAsync(host, username, password, database);
                this.host = host;
                this.database = database;
                this.username = username;
                this.password = password;
            }
            catch
            {
                if (connect != null)
                    connect.Close();
                throw;
            }
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
                    MySqlCommand cmd = new MySqlCommand("show databases;", connect);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
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
        }

        /// <summary>
        /// Creates the user data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateUserAsync(string username, string pwd, string database, string fromHost)
        {
            string account = QuoteMySqlString(username) + "@" + QuoteMySqlString(fromHost);
            string cmd = "CREATE USER " + account + " IDENTIFIED BY " + QuoteMySqlString(pwd) + ";";
            MySqlCommand command = new MySqlCommand(cmd, connect);
            await command.ExecuteNonQueryAsync();
            cmd = "GRANT ALL ON " + QuoteMySqlIdentifier(database) + ".* TO " + account;
            command = new MySqlCommand(cmd, connect);
            await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Gets the view value from the current application state.
        /// </summary>
        public override async Task<object> GetViewAsync(string name)
        {
            MySqlCommand cmd = new MySqlCommand(string.Format("SELECT * FROM {0}", ValidateSqlIdentifier(name)), connect);
            cmd.CommandType = CommandType.Text;
            return await cmd.ExecuteScalarAsync();
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync(Version version)
        {
            OnPrintVersion(version);
            MySqlCommand command;
            string cmd = string.Empty;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("Pflegehaushaltsbuch.Version.update " + version + ".sql")))
            {
                while (!sr.EndOfStream)
                {
                    cmd = sr.ReadToEnd();
                }
            }
            cmd = cmd.Replace(";;", ";");
            cmd = cmd.Replace("DELIMITER ;", "");
            if (!username.ToLower().Equals("root") || !host.ToLower().Equals("localhost"))
            {
                cmd = cmd.Replace("DEFINER=`root`@`localhost`", string.Format("DEFINER=`{0}`@`{1}`", username, host));
            }
            command = new MySqlCommand(cmd, connect);
            await command.ExecuteNonQueryAsync();
            if (version == new Version("1.0.9.0"))
            {
                await CreateTriggerAsync();
                await CreateView();
                await ApplyLegacyAccountIdsToCashAndBankBooksAsync();
            }
            if (version > new Version("1.0.7.0"))
            {
                DataTable versionTable = new DataTable();
                await FillAdapterAsync(SELECT.Version, versionTable);
                DataRow row = versionTable.NewRow();
                row["main"] = version.ToString();
                versionTable.Rows.Add(row);
                if (!await UpdateAdapterAsync(SELECT.Version, versionTable))
                    throw new Exception(Messages.datatable_update_failed);
            }
        }
        /// <summary>
        /// Runs the drop Database operation and updates the related application state.
        /// </summary>
        public override async Task DropDatabaseAsync(string host, string username, string password, string database)
        {
            connect = await OpenConnectionAsync(host, username, password);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP DATABASE IF EXISTS " + QuoteMySqlIdentifier(database) + ";");
            var command = new MySqlCommand(sb.ToString(), connect);
            await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the data Base data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateDataBaseAsync(string host, string username, string password, string database)
        {
            connect = await OpenConnectionAsync(host, username, password);
            StringBuilder sb = new StringBuilder();
            string quotedDatabase = QuoteMySqlIdentifier(database);
            sb.AppendLine("DROP DATABASE IF EXISTS " + quotedDatabase + ";");
            sb.AppendLine("CREATE DATABASE " + quotedDatabase + ";");
            var command = new MySqlCommand(sb.ToString(), connect);
            await command.ExecuteNonQueryAsync();
            sb.Clear();
            sb.AppendLine("USE " + quotedDatabase + ";");
            CreateFixedTables(sb);
            await CreateUserTablesAsync(sb);
            command = new MySqlCommand(sb.ToString(), connect);
            await command.ExecuteNonQueryAsync();
            sb.Clear();
            // MySQL does not support INSERT ... DEFAULT VALUES, so seed the row explicitly.
            await InsertInitialHardCashRow();
            await InsertInitialAccountRows();
            DataTable version = new DataTable();
            await FillAdapterAsync(SELECT.Version, version);
            DataRow row = version.NewRow();
            version.Rows.Add(row);
            row["main"] = "1.0.9.0";
            if (!await UpdateAdapterAsync(SELECT.Version, version))
                throw new Exception(Messages.datatable_update_failed);
            await CreateTriggerAsync();
            await CreateView();            
        }
        /// <summary>
        /// Runs the insert Initial Hard Cash Row operation and updates the related application state.
        /// </summary>
        private async Task InsertInitialHardCashRow()
        {
            string commandText = @"INSERT INTO hard_cash
(`001`,`002`,`005`,`010`,`020`,`050`,`1`,`2`,`5`,`10`,`20`,`50`,`100`,`200`,`500`)
VALUES
(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);";
            using (MySqlCommand command = new MySqlCommand(commandText, connect))
                await command.ExecuteNonQueryAsync();
        }
        private async Task InsertInitialAccountRows()
        {
            string commandText = @"INSERT INTO accounts (id, type, active, created_at)
VALUES
(0, 'Cash', 1, CURRENT_TIMESTAMP),
(1, 'Bank', 1, CURRENT_TIMESTAMP);";
            using (MySqlCommand command = new MySqlCommand(commandText, connect))
                await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the trigger data or user interface element for the current workflow.
        /// </summary>
        protected override async Task CreateTriggerAsync()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("CREATE TRIGGER client_books_AUPD AFTER UPDATE ON client_books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from client_books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("end;");
            sb.AppendLine("CREATE TRIGGER client_books_AINS AFTER INSERT ON client_books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from client_books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("end;");
            sb.AppendLine("CREATE TRIGGER client_books_ADEL AFTER DELETE ON client_books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from client_books where OLD.id=id) where OLD.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from client_books where OLD.id=id) where OLD.id=clients.id;");
            sb.AppendLine("end;");
            sb.AppendLine("CREATE TRIGGER clients_BUPD BEFORE UPDATE ON clients FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Set New.amount=New.account_transfer + (Select COALESCE(sum(amount),0) from client_books where NEW.id=id);");
            sb.AppendLine("end;");
            using (var command = new MySqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the view data or user interface element for the current workflow.
        /// </summary>
        private async Task CreateView()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) from bank_books;");
            sb.AppendLine("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) from cash_books;");
            sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from office_cash;");
            using (var command = new MySqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Runs the call Functions operation and updates the related application state.
        /// </summary>
        public override async Task<object> CallFunctionsAsync(string name, params object[] values)
        {
            MySqlCommand cmd = new MySqlCommand(name, connect);
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlParameter param = new MySqlParameter("amount1", MySqlDbType.Decimal, 10);
            param.Direction = ParameterDirection.Output;
            param.Precision = 10;
            param.Scale = 2;
            cmd.Parameters.Add(param);
            return await cmd.ExecuteScalarAsync();
        }
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        protected override DbTransaction BeginDbTransaction()
        {
            return connect.BeginTransaction();
        }
        private MySqlCommand CreateSelectCommand(AdapterCommandInfo commandInfo)
        {
            MySqlCommand command = new MySqlCommand(commandInfo.CommandText, connect);
            if (ActiveTransaction != null)
                command.Transaction = (MySqlTransaction)ActiveTransaction;
            foreach (AdapterCommandParameter parameter in commandInfo.Parameters)
                command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
            return command;
        }
        public override async Task FillAdapterAsync(SELECT select, DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                DataTable loadedTable = new DataTable();
                using (MySqlCommand command = CreateSelectCommand(commandInfo))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    await Task.Run(() => adapter.Fill(loadedTable));

                table.Clear();
                table.Merge(loadedTable, false, MissingSchemaAction.Add);
            }
            finally
            {
                connectionLock.Release();
            }
        }
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        public override async Task FillAdapterAsync(SELECT select, DataTable table, params object[] values)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select, values);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                DataTable loadedTable = new DataTable();
                using (MySqlCommand command = CreateSelectCommand(commandInfo))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    await Task.Run(() => adapter.Fill(loadedTable));

                table.Clear();
                table.Merge(loadedTable, false, MissingSchemaAction.Add);
            }
            finally
            {
                connectionLock.Release();
            }
        }
        /// <summary>
        /// Updates the adapter data and refreshes the related application state.
        /// </summary>
        public override async Task<bool> UpdateAdapterAsync(SELECT select, DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                DataTable changes = table.GetChanges();
                if (changes == null)
                    return true;

                AdapterCommandInfo commandInfo = table.ExtendedProperties[AdapterSelectCommandProperty] as AdapterCommandInfo ?? CreateAdapterCommandInfo(select);

                using (MySqlCommand command = CreateSelectCommand(commandInfo))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                using (MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter))
                {
                    adapter.InsertCommand = builder.GetInsertCommand();
                    adapter.DeleteCommand = builder.GetDeleteCommand();
                    adapter.UpdateCommand = builder.GetUpdateCommand();
                    if (ActiveTransaction != null)
                    {
                        adapter.InsertCommand.Transaction = (MySqlTransaction)ActiveTransaction;
                        adapter.DeleteCommand.Transaction = (MySqlTransaction)ActiveTransaction;
                        adapter.UpdateCommand.Transaction = (MySqlTransaction)ActiveTransaction;
                    }

                    int value = await Task.Run(() => adapter.Update(table));
                    return value == changes.Rows.Count;
                }
            }
            finally
            {
                connectionLock.Release();
            }
        }        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (MySqlDataAdapter adapter in adapters.Values)
                adapter.Dispose();
            adapters.Clear();
            if (connect != null)
                connect.Close();
        }
        /// <summary>
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
            MySqlCommand cmd = connect.CreateCommand();
            connect.ChangeDatabase(dataBase);
            cmd.CommandText = command;
            return await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the new Password data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateNewPasswordAsync(string host, string username, string password, string new_password)
        {
            await TestConnectionAsync(host, null, username, password);
            string cmd = "ALTER USER " + QuoteMySqlString(username) + "@" + QuoteMySqlString(host) + " IDENTIFIED BY " + QuoteMySqlString(new_password) + ";";
            MySqlCommand command = new MySqlCommand(cmd, connect);
            int returncode = await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Creates the fixed Tables data or user interface element for the current workflow.
        /// </summary>
        protected override void CreateFixedTables(StringBuilder sb)
        {
            sb.AppendLine("DROP TABLE IF EXISTS users;");
            sb.AppendLine("create table users (");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("login varchar(255) UNIQUE,");
            sb.AppendLine("pw varchar(255),");
            sb.AppendLine("phone varchar(64),");
            sb.AppendLine("fax varchar(64),");
            sb.AppendLine("email varchar(255),");
            sb.AppendLine("access int(11),");
            sb.AppendLine("admin BOOLEAN,");
            sb.AppendLine("PRIMARY KEY(name, login)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS company;");
            sb.AppendLine("create table company (");
            sb.AppendLine("name varchar(255) PRIMARY KEY UNIQUE,");
            sb.AppendLine("secretary varchar(255),");
            sb.AppendLine("phone varchar(64),");
            sb.AppendLine("fax varchar(64),");
            sb.AppendLine("email varchar(255),");
            sb.AppendLine("street varchar(255),");
            sb.AppendLine("zipcode varchar(10),");
            sb.AppendLine("city varchar(255),");
            sb.AppendLine("language varchar(64),");
            sb.AppendLine("web varchar(255),");
            sb.AppendLine("local_court varchar(255),");
            sb.AppendLine("hrb varchar(64),");
            sb.AppendLine("ik varchar(64),");
            sb.AppendLine("smtp_host varchar(255),");
            sb.AppendLine("smtp_user varchar(255),");
            sb.AppendLine("smtp_key varchar(255),");
            sb.AppendLine("logo mediumblob,");
            sb.AppendLine("logo_alignment Integer");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS company_bank;");
            sb.AppendLine("create table company_bank (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("name varchar(255),");
            sb.AppendLine("code varchar(64),");
            sb.AppendLine("account_no varchar(64),");
            sb.AppendLine("iban varchar(64),");
            sb.AppendLine("bic varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS license;");
            sb.AppendLine("create table license (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("grade Integer,");
            sb.AppendLine("`begin` DATE,");
            sb.AppendLine("`expired` DATE,");
            sb.AppendLine("`key` varbinary(2048)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS layouts;");
            sb.AppendLine("create table layouts (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("accounts mediumblob,");
            sb.AppendLine("advisors mediumblob,");
            sb.AppendLine("employees mediumblob,");
            sb.AppendLine("bank mediumblob,");
            sb.AppendLine("cash mediumblob,");
            sb.AppendLine("cashaudit mediumblob,");
            sb.AppendLine("clients mediumblob,");
            sb.AppendLine("quittance mediumblob,");
            sb.AppendLine("officecash mediumblob");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS version;");
            sb.AppendLine("create table version (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("main varchar(64)");
            sb.AppendLine(");");
        }
        /// <summary>
        /// Creates the user Tables data or user interface element for the current workflow.
        /// </summary>
        protected override async Task CreateUserTablesAsync(StringBuilder sb)
        {
            sb.AppendLine("DROP TABLE IF EXISTS advisors;");
            sb.AppendLine("create table advisors (");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("title varchar(64),");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("email varchar(255),");
            sb.AppendLine("co varchar(255),");
            sb.AppendLine("street varchar(255),");
            sb.AppendLine("zipcode varchar(45),");
            sb.AppendLine("city varchar(255),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS accounts;");
            sb.AppendLine("create table accounts (");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("type varchar(64) NOT NULL,");
            sb.AppendLine("active INTEGER NOT NULL DEFAULT 1,");
            sb.AppendLine("created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS employees;");
            sb.AppendLine("create table employees (");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("account_transfer DECIMAL(18,2),");
            sb.AppendLine("amount_payout DECIMAL(18,2),");
            sb.AppendLine("amount_payback DECIMAL(18,2),");
            sb.AppendLine("amount_payback_type int(11),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("active int default 1,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS bank_books;");
            sb.AppendLine("create table bank_books (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(255),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account varchar(64),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS cash_books;");
            sb.AppendLine("create table cash_books (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(255),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account varchar(64),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS hard_cash;");
            sb.AppendLine("create table hard_cash (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("`001` INTEGER NOT NULL DEFAULT '2',");
            sb.AppendLine("`002` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`005` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`010` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`020` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`050` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`1` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`2` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`5` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`10` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`20` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`50` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`100` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`200` INTEGER NOT NULL DEFAULT '0',");
            sb.AppendLine("`500` INTEGER NOT NULL DEFAULT '0'");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS client_books;");
            sb.AppendLine("create table client_books (");
            sb.AppendLine("`index` INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("id int,");
            sb.AppendLine("document_id int(11),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(255),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS clients;");
            sb.AppendLine("create table clients (");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("title varchar(8),");
            sb.AppendLine("name varchar(255),");
            sb.AppendLine("street varchar(255),");
            sb.AppendLine("zipcode varchar(45),");
            sb.AppendLine("city varchar(255),");
            sb.AppendLine("born DATE,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("account_transfer DECIMAL(18,2),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("lastbook DATE,");
            sb.AppendLine("active INTEGER,");
            sb.AppendLine("info INTEGER,");
            sb.AppendLine("note varchar(255),");
            sb.AppendLine("advisor_id INTEGER,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS deadlines;");
            sb.AppendLine("create table deadlines (");
            sb.AppendLine("`no` INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("id INTEGER,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS office_cash;");
            sb.AppendLine("create table office_cash (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTO_INCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("account INTEGER,");
            sb.AppendLine("book_cat INTEGER,");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS record;");
            sb.AppendLine("create table record (");
            sb.AppendLine("id INTEGER AUTO_INCREMENT,");
            sb.AppendLine("client_id INTEGER,");
            sb.AppendLine("`index` INTEGER,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("filename varchar(255),");
            sb.AppendLine("file mediumblob,");
            sb.AppendLine("handsign varchar(64),");
            sb.AppendLine("PRIMARY KEY(id)");
            sb.AppendLine(");");
        }
        /// <summary>
        /// Restores the restore data from the selected source.
        /// </summary>
        public override async Task RestoreAsync(string filename)
        {
            StringBuilder sb = new StringBuilder();
            await CreateUserTablesAsync(sb);
            using (MySqlCommand command = new MySqlCommand(sb.ToString(), connect))
                await command.ExecuteNonQueryAsync();
            try
            {
                using (MySqlCommand command = new MySqlCommand("SET GLOBAL max_allowed_packet = 4294967295", connect))
                    await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException)
            {
            }
            await base.RestoreAsync(filename);
            await CreateTriggerAsync();
        }
        /// <summary>
        /// Runs the insert Table operation and updates the related application state.
        /// </summary>
        protected override async Task InsertTableAsync(SQLBase.SELECT select, DataTable to)
        {
            if (to.Rows.Count == 0)
                return;
            foreach (DataColumn column in to.Columns)
            {
                var defaultValue = column.DefaultValue.ToString();
                if (string.IsNullOrWhiteSpace(defaultValue))
                    column.DefaultValue = null;
            }

            List<string> columns = to.Columns.OfType<DataColumn>().Select(column => "`" + column.ColumnName.Replace("`", "``") + "`").ToList();
            List<string> parameters = to.Columns.OfType<DataColumn>().Select((column, index) => "@p" + index).ToList();
            string commandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", QuoteMySqlIdentifier(GetTableName(select)), string.Join(",", columns), string.Join(",", parameters));

            foreach (DataRow row in to.Rows)
            {
                using (MySqlCommand command = new MySqlCommand(commandText, connect))
                {
                    for (int i = 0; i < to.Columns.Count; ++i)
                    {
                        object value = row[i];
                        if (value == null || value == DBNull.Value)
                            value = to.Columns[i].DefaultValue ?? DBNull.Value;
                        command.Parameters.AddWithValue("@p" + i, value ?? DBNull.Value);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
