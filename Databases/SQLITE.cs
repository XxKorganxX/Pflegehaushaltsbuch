using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Databases
{
    /// <summary>
    /// Represents the SQLITE component used by the application.
    /// </summary>
    public class SQLITE : SQLBase
    {
        SQLiteConnection connection;
        private readonly SemaphoreSlim connectionLock = new SemaphoreSlim(1, 1);
        public Dictionary<SELECT, System.Data.SQLite.SQLiteDataAdapter> adapters = new Dictionary<SELECT, System.Data.SQLite.SQLiteDataAdapter>();
        private string dataBase;
        private bool pettyCashSchemaChecked;
        /// <summary>
        /// Creates a new SQLITE instance and initializes the required state.
        /// </summary>
        public SQLITE()
        {
            
            selectCommand[SQLBase.SELECT.BargeByPeriod] = "SELECT * FROM cash_books WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.BankByPeriod] = "SELECT * FROM bank_books WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.OfficeByPeriod] = "SELECT * FROM petty_cash WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.BooksByPeriod] = "SELECT * FROM client_books WHERE id='{0}' AND date(date) >= date({1:yyyy-MM-dd}) AND date(date) < date({2:yyyy-MM-dd})";
            
            selectCommand[SQLBase.SELECT.BargeFromMonth] = "SELECT * FROM cash_books WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.BankByDate] = "SELECT * FROM bank_books WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.Book] = "SELECT * FROM client_books WHERE id='{0}' AND date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.Deadline] = "SELECT * FROM deadlines WHERE id='{0}' AND strftime(char(37,109), date)=printf(char(37,48,50,100), {1})";
            selectCommand[SQLBase.SELECT.DeadlineByDay] = "SELECT * FROM deadlines WHERE strftime(char(37,109), date)=strftime(char(37,109), {0:yyyy-MM-dd}) AND strftime(char(37,100), date)=strftime(char(37,100), {0:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.RecordsByClientAndDate] = "SELECT * FROM record WHERE client_id='{0}' AND date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.PettyCashByDate] = "SELECT * FROM petty_cash WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
        }
        /// <summary>
        /// Creates the data Base data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateDataBaseAsync(string host, string username, string password, string database)
        {
            try
            {
                string filename = string.IsNullOrWhiteSpace(database) ? host : database;
                if (string.IsNullOrWhiteSpace(filename))
                    throw new ArgumentException(Messages.database_enter_name);

                string directory = Path.GetDirectoryName(filename);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                SQLiteConnection.CreateFile(filename);
                connection = new SQLiteConnection(new SQLiteConnectionStringBuilder { DataSource = filename, Version = 3 }.ConnectionString);
                connection.SetPassword(password);
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                CreateFixedTables(sb);
                await CreateUserTablesAsync(sb);
                sb.AppendLine("INSERT INTO hard_cash VALUES (0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);");
                sb.AppendLine("INSERT INTO accounts (id, type, active, created_at) VALUES (0, 'Cash', 1, CURRENT_TIMESTAMP);");
                sb.AppendLine("INSERT INTO accounts (id, type, active, created_at) VALUES (1, 'Bank', 1, CURRENT_TIMESTAMP);");
                sb.AppendLine("INSERT INTO version VALUES (0, '1.0.13.0');");
                var command = new SQLiteCommand(sb.ToString(), connection);
                await command.ExecuteNonQueryAsync();
                await CreateTriggerAsync();
                sb.Clear();
                sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) from bank_books;");
                sb.AppendLine("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) from cash_books;");
                sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from petty_cash;");
                command = new SQLiteCommand(sb.ToString(), connection);
                await command.ExecuteNonQueryAsync();
            }
            catch
            {
                Dispose();
                throw;
            }
        }
        /// <summary>
        /// Runs the drop Database operation and updates the related application state.
        /// </summary>
        public override async Task DropDatabaseAsync(string host, string username, string password, string database)
        {
            string filename = string.IsNullOrWhiteSpace(database) ? host : database;
            if (string.IsNullOrWhiteSpace(filename))
                return;
            Dispose();
            SQLiteConnection.ClearAllPools();
            if (File.Exists(filename))
                File.Delete(filename);
        }
        /// <summary>
        /// Runs the test Connection operation and updates the related application state.
        /// </summary>
        public override async Task<bool> TestConnectionAsync(string host, string database, string username, string password)
        {
            await ConnectAsync(host, username, password, database);
            return true;
        }
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public override async Task ConnectAsync(string host, string username, string password, string database)
        {
            dataBase = database;
            connection = new System.Data.SQLite.SQLiteConnection();
            connection.ConnectionString = new SQLiteConnectionStringBuilder { DataSource = database, Version = 3, Password = password }.ConnectionString;
            await connection.OpenAsync();
            pettyCashSchemaChecked = false;
        }
        /// <summary>
        /// Runs the call Functions operation and updates the related application state.
        /// </summary>
        public override async Task<object> CallFunctionsAsync(string name, params object[] values)
        {
            using (SQLiteCommand cmd = CreateCommand("new_function"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ireturnvalue", DbType.Int32);
                cmd.Parameters["@ireturnvalue"].Direction = ParameterDirection.ReturnValue;
                return await cmd.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Remove selected adapter from the adapters dictionary and dispose it to free resources.
        /// </summary>
        /// <param name="selected"></param>
        private void DisposeAdapter(SQLBase.SELECT selected)
        {
            if (!adapters.TryGetValue(selected, out var oldAdapter))
                return;
            adapters.Remove(selected);
            oldAdapter.Dispose();
        }

        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        protected override DbTransaction BeginDbTransaction()
        {
            return connection.BeginTransaction();
        }
        private SQLiteCommand CreateCommand(string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            if (ActiveTransaction != null)
                command.Transaction = (SQLiteTransaction)ActiveTransaction;
            return command;
        }

        private void UseActiveTransaction(SQLiteCommand command)
        {
            if (command != null && ActiveTransaction != null)
                command.Transaction = (SQLiteTransaction)ActiveTransaction;
        }

        private SQLiteCommand CreateSelectCommand(AdapterCommandInfo commandInfo)
        {
            SQLiteCommand command = CreateCommand(commandInfo.CommandText);
            foreach (AdapterCommandParameter parameter in commandInfo.Parameters)
                command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
            return command;
        }
        public override async Task FillAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                if (IsPettyCashSelect(select))
                    await EnsurePettyCashTableAsync();

                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                System.Data.DataTable loadedTable = new System.Data.DataTable();
                using (SQLiteCommand command = CreateSelectCommand(commandInfo))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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
        public override async Task FillAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table, params object[] values)
        {
            await connectionLock.WaitAsync();
            try
            {
                if (IsPettyCashSelect(select))
                    await EnsurePettyCashTableAsync();

                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select, values);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                System.Data.DataTable loadedTable = new System.Data.DataTable();
                using (SQLiteCommand command = CreateSelectCommand(commandInfo))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    await Task.Run(() => adapter.Fill(loadedTable));

                ReplaceTableContents(table, loadedTable);
            }
            finally
            {
                connectionLock.Release();
            }
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync()
        {
            Version checkVersion;
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
            if (Version >= new Version(1, 0, 12, 0))
                await EnsurePettyCashTableAsync();
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
            using (SQLiteCommand cmd = CreateCommand(string.Format("SELECT * FROM {0}", ValidateSqlIdentifier(name))))
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
            if (version == new Version("1.0.9.0"))
            {
                StringBuilder sb = new StringBuilder();
                using (SQLiteCommand renameCommand = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='assistants';", connection))
                {
                    object existingAssistants = await renameCommand.ExecuteScalarAsync();
                    if (existingAssistants != null)
                    {
                        renameCommand.CommandText = "ALTER TABLE assistants RENAME TO employees;";
                        await renameCommand.ExecuteNonQueryAsync();
                    }
                }
                using (SQLiteCommand renameCommand = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='barge';", connection))
                {
                    if (await renameCommand.ExecuteScalarAsync() != null)
                    {
                        renameCommand.CommandText = "ALTER TABLE barge RENAME TO cash_books;";
                        await renameCommand.ExecuteNonQueryAsync();
                    }
                }
                using (SQLiteCommand renameCommand = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='bank';", connection))
                {
                    if (await renameCommand.ExecuteScalarAsync() != null)
                    {
                        renameCommand.CommandText = "ALTER TABLE bank RENAME TO bank_books;";
                        await renameCommand.ExecuteNonQueryAsync();
                    }
                }
                using (SQLiteCommand renameCommand = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='books';", connection))
                {
                    if (await renameCommand.ExecuteScalarAsync() != null)
                    {
                        renameCommand.CommandText = "ALTER TABLE books RENAME TO client_books;";
                        await renameCommand.ExecuteNonQueryAsync();
                    }
                }
                bool existingAssistantsLayout = await HasColumnAsync("layouts", "assistants");
                bool existingEmployeesLayout = await HasColumnAsync("layouts", "employees");
                if (existingAssistantsLayout && !existingEmployeesLayout)
                {
                    await RebuildLayoutsWithEmployeesColumnAsync();
                }
                sb.AppendLine("CREATE TABLE IF NOT EXISTS accounts (");
                sb.AppendLine("id INTEGER PRIMARY KEY,");
                sb.AppendLine("type varchar(64) NOT NULL,");
                sb.AppendLine("active INTEGER NOT NULL DEFAULT 1,");
                sb.AppendLine("created_at DATE NOT NULL DEFAULT CURRENT_TIMESTAMP");
                sb.AppendLine(");");
                sb.AppendLine("INSERT OR IGNORE INTO accounts (id, type, active, created_at) VALUES (0, 'Cash', 1, CURRENT_TIMESTAMP);");
                sb.AppendLine("INSERT OR IGNORE INTO accounts (id, type, active, created_at) VALUES (1, 'Bank', 1, CURRENT_TIMESTAMP);");
                sb.AppendLine("ALTER TABLE clients ADD COLUMN account_id INTEGER;");
                sb.AppendLine("ALTER TABLE employees ADD COLUMN account_id INTEGER;");
                sb.AppendLine("UPDATE clients SET account_id = (SELECT COUNT(*) + 1 FROM clients c WHERE c.id <= clients.id) WHERE account_id IS NULL;");
                sb.AppendLine("UPDATE employees SET account_id = (SELECT COUNT(*) + 1 FROM clients) + (SELECT COUNT(*) FROM employees e WHERE e.id <= employees.id) WHERE account_id IS NULL;");
                sb.AppendLine("INSERT OR IGNORE INTO accounts (id, type, active, created_at) SELECT account_id, 'Client', COALESCE(active, 1), CURRENT_TIMESTAMP FROM clients WHERE account_id IS NOT NULL;");
                sb.AppendLine("INSERT OR IGNORE INTO accounts (id, type, active, created_at) SELECT account_id, 'Employee', COALESCE(active, 1), CURRENT_TIMESTAMP FROM employees WHERE account_id IS NOT NULL;");
                sb.AppendLine("ALTER TABLE cash_books ADD COLUMN account_id INTEGER;");
                sb.AppendLine("ALTER TABLE bank_books ADD COLUMN account_id INTEGER;");
                sb.AppendLine("UPDATE cash_books SET account_id = COALESCE((SELECT account_id FROM clients WHERE cash_books.account = printf('K%03d', clients.id)), (SELECT account_id FROM employees WHERE cash_books.account = printf('M%03d', employees.id)), CASE WHEN account IN ('Barbestand', 'Cash', 'Kasse') THEN 0 WHEN account IN ('Bankbestand', 'Bank') THEN 1 ELSE NULL END) WHERE account_id IS NULL;");
                sb.AppendLine("UPDATE bank_books SET account_id = COALESCE((SELECT account_id FROM clients WHERE bank_books.account = printf('K%03d', clients.id)), (SELECT account_id FROM employees WHERE bank_books.account = printf('M%03d', employees.id)), CASE WHEN account IN ('Barbestand', 'Cash', 'Kasse') THEN 0 WHEN account IN ('Bankbestand', 'Bank') THEN 1 ELSE NULL END) WHERE account_id IS NULL;");
                sb.AppendLine("ALTER TABLE office_cash ADD COLUMN account_id INTEGER;");
                sb.AppendLine("UPDATE office_cash SET account_id = account WHERE account_id IS NULL;");
                sb.AppendLine("DROP TRIGGER IF EXISTS books_AUPD;");
                sb.AppendLine("DROP TRIGGER IF EXISTS books_AINS;");
                sb.AppendLine("DROP TRIGGER IF EXISTS books_ADEL;");
                sb.AppendLine("DROP TRIGGER IF EXISTS clients_BUPD;");
                sb.AppendLine("DROP TRIGGER IF EXISTS clients_AUPD;");
                sb.AppendLine("DROP TRIGGER IF EXISTS client_books_AUPD;");
                sb.AppendLine("DROP TRIGGER IF EXISTS client_books_AINS;");
                sb.AppendLine("DROP TRIGGER IF EXISTS client_books_ADEL;");
                sb.AppendLine("DROP VIEW IF EXISTS bank_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS barge_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS cash_total_amount;");
                sb.AppendLine("DROP VIEW IF EXISTS office_total_amount;");
                sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) from bank_books;");
                sb.AppendLine("CREATE VIEW cash_total_amount AS Select COALESCE(SUM(amount),0) from cash_books;");
                sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from office_cash;");
                using (SQLiteCommand command = new SQLiteCommand(sb.ToString(), connection))
                    await command.ExecuteNonQueryAsync();
                await CreateTriggerAsync();
            }
            if (version == new Version("1.0.10.0"))
            {
                await EnsureColumnAsync("office_cash", "account_id", "ALTER TABLE office_cash ADD COLUMN account_id INTEGER;");
                if (await HasColumnAsync("office_cash", "account"))
                    await ExecuteNonQueryAsync("UPDATE office_cash SET account_id = account WHERE account_id IS NULL;");
                await RebuildBookingTableWithoutLegacyAccountAsync("bank_books");
                await RebuildBookingTableWithoutLegacyAccountAsync("cash_books");
                await RebuildOfficeCashWithoutLegacyAccountAsync();
            }
            if (version == new Version("1.0.11.0"))
            {
                await RebuildUsersWithoutContactColumnsAsync();
            }
            if (version == new Version("1.0.12.0"))
            {
                await RenameTableAsync("office_cash", "petty_cash");
                await RecreateOfficeTotalAmountViewAsync();
                await RebuildUsersWithHandSignAsync();
            }
            if (version == new Version("1.0.13.0"))
            {
                await EnsureColumnAsync("company", "currency_code", "ALTER TABLE company ADD COLUMN currency_code varchar(3) DEFAULT 'EUR';");
                await ExecuteNonQueryAsync("UPDATE company SET currency_code = 'EUR' WHERE currency_code IS NULL OR TRIM(currency_code) = '';");
                await EnsureUserLoginValuesAsync();
                await EnsureUserLoginThrottleColumnsAsync();
                await EnsureInitialAdminUserAsync();
            }

            DataTable versionTable = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Version, versionTable);
            DataRow row = versionTable.NewRow();
            row["main"] = version.ToString();
            versionTable.Rows.Add(row);
            if (!await UpdateAdapterAsync(SQLBase.SELECT.Version, versionTable))
                throw new Exception(Messages.datatable_update_failed);
        }

        private async Task EnsureColumnAsync(string tableName, string columnName, string commandText)
        {
            if (!await HasColumnAsync(tableName, columnName))
                await ExecuteNonQueryAsync(commandText);
        }

        private async Task EnsureUserLoginThrottleColumnsAsync()
        {
            await EnsureColumnAsync("users", "failed_login_attempts", "ALTER TABLE users ADD COLUMN failed_login_attempts INTEGER NOT NULL DEFAULT 0;");
            await EnsureColumnAsync("users", "last_failed_login", "ALTER TABLE users ADD COLUMN last_failed_login DATE;");
            await EnsureColumnAsync("users", "locked_until", "ALTER TABLE users ADD COLUMN locked_until DATE;");
        }

        private async Task<bool> HasColumnAsync(string tableName, string columnName)
        {
            using (SQLiteCommand command = new SQLiteCommand("PRAGMA table_info(" + ValidateSqlIdentifier(tableName) + ");", connection))
            using (SQLiteDataReader reader = (SQLiteDataReader)await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (string.Equals(reader["name"].ToString(), columnName, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private async Task RenameTableAsync(string oldTableName, string newTableName)
        {
            if (!await HasTableAsync(oldTableName) || await HasTableAsync(newTableName))
                return;

            await ExecuteNonQueryAsync("ALTER TABLE " + ValidateSqlIdentifier(oldTableName) + " RENAME TO " + ValidateSqlIdentifier(newTableName) + ";");
        }

        private async Task<bool> HasTableAsync(string tableName)
        {
            using (SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;", connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);
                return await command.ExecuteScalarAsync() != null;
            }
        }

        private async Task RecreateOfficeTotalAmountViewAsync()
        {
            await ExecuteNonQueryAsync("DROP VIEW IF EXISTS office_total_amount; CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from petty_cash;");
        }

        private async Task EnsurePettyCashTableAsync()
        {
            if (pettyCashSchemaChecked)
                return;

            await RenameTableAsync("office_cash", "petty_cash");
            if (await HasTableAsync("petty_cash"))
                await RecreateOfficeTotalAmountViewAsync();
            pettyCashSchemaChecked = true;
        }

        private static bool IsPettyCashSelect(SELECT select)
        {
            return select == SELECT.PettyCash
                || select == SELECT.PettyCashByDate
                || select == SELECT.OfficeByPeriod;
        }

        private async Task ExecuteNonQueryAsync(string commandText)
        {
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                if (ActiveTransaction != null)
                    command.Transaction = (SQLiteTransaction)ActiveTransaction;
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task RebuildBookingTableWithoutLegacyAccountAsync(string tableName)
        {
            if (!await HasColumnAsync(tableName, "account"))
                return;

            string newTableName = tableName + "_new";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE IF EXISTS " + newTableName + ";");
            sb.AppendLine("CREATE TABLE " + newTableName + " (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            if (tableName == "bank_books")
            {
                sb.AppendLine("amount DECIMAL(18,2),");
                sb.AppendLine("account_id INTEGER,");
                sb.AppendLine("book_to int(11),");
                sb.AppendLine("book_cat int(11),");
            }
            else
            {
                sb.AppendLine("book_cat int(11),");
                sb.AppendLine("book_to int(11),");
                sb.AppendLine("amount DECIMAL(18,2),");
                sb.AppendLine("account_id INTEGER,");
            }
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            if (tableName == "bank_books")
                sb.AppendLine("INSERT INTO " + newTableName + " (id, date, note, amount, account_id, book_to, book_cat, handsign) SELECT id, date, note, amount, account_id, book_to, book_cat, handsign FROM " + tableName + ";");
            else
                sb.AppendLine("INSERT INTO " + newTableName + " (id, date, note, book_cat, book_to, amount, account_id, handsign) SELECT id, date, note, book_cat, book_to, amount, account_id, handsign FROM " + tableName + ";");
            sb.AppendLine("DROP TABLE " + tableName + ";");
            sb.AppendLine("ALTER TABLE " + newTableName + " RENAME TO " + tableName + ";");
            await ExecuteNonQueryAsync(sb.ToString());
        }

        private async Task RebuildOfficeCashWithoutLegacyAccountAsync()
        {
            if (!await HasColumnAsync("office_cash", "account"))
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE IF EXISTS office_cash_new;");
            sb.AppendLine("CREATE TABLE office_cash_new (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("book_cat INTEGER,");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("INSERT INTO office_cash_new (id, date, note, account_id, book_cat, amount, handsign) SELECT id, date, note, account_id, book_cat, amount, handsign FROM office_cash;");
            sb.AppendLine("DROP TABLE office_cash;");
            sb.AppendLine("ALTER TABLE office_cash_new RENAME TO office_cash;");
            await ExecuteNonQueryAsync(sb.ToString());
        }

        private async Task RebuildUsersWithoutContactColumnsAsync()
        {
            if (!await HasColumnAsync("users", "phone")
                && !await HasColumnAsync("users", "fax")
                && !await HasColumnAsync("users", "email"))
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE IF EXISTS users_new;");
            sb.AppendLine("CREATE TABLE users_new (");
            sb.AppendLine("name varchar(255) PRIMARY KEY,");
            sb.AppendLine("login varchar(64) UNIQUE,");
            sb.AppendLine("pw varchar(255),");
            sb.AppendLine("access int(11) DEFAULT 0,");
            sb.AppendLine("admin bool DEFAULT false");
            sb.AppendLine(");");
            sb.AppendLine("INSERT INTO users_new (name, login, pw, access, admin) SELECT name, login, pw, access, admin FROM users;");
            sb.AppendLine("DROP TABLE users;");
            sb.AppendLine("ALTER TABLE users_new RENAME TO users;");
            await ExecuteNonQueryAsync(sb.ToString());
        }

        private async Task RebuildLayoutsWithEmployeesColumnAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE IF EXISTS layouts_new;");
            sb.AppendLine("CREATE TABLE layouts_new (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("accounts BLOB,");
            sb.AppendLine("advisors BLOB,");
            sb.AppendLine("employees BLOB,");
            sb.AppendLine("bank BLOB,");
            sb.AppendLine("cash BLOB,");
            sb.AppendLine("cashaudit BLOB,");
            sb.AppendLine("clients BLOB,");
            sb.AppendLine("quittance BLOB,");
            sb.AppendLine("officecash BLOB");
            sb.AppendLine(");");
            sb.AppendLine("INSERT INTO layouts_new (id, accounts, advisors, employees, bank, cash, cashaudit, clients, quittance, officecash) SELECT id, accounts, advisors, assistants, bank, cash, cashaudit, clients, quittance, officecash FROM layouts;");
            sb.AppendLine("DROP TABLE layouts;");
            sb.AppendLine("ALTER TABLE layouts_new RENAME TO layouts;");
            await ExecuteNonQueryAsync(sb.ToString());
        }

        private async Task RebuildUsersWithHandSignAsync()
        {
            if (await HasColumnAsync("users", "handsign") || !await HasColumnAsync("users", "name"))
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DROP TABLE IF EXISTS users_new;");
            sb.AppendLine("CREATE TABLE users_new (");
            sb.AppendLine("handsign varchar(255) PRIMARY KEY,");
            sb.AppendLine("login varchar(64) UNIQUE,");
            sb.AppendLine("pw varchar(255),");
            sb.AppendLine("access int(11) DEFAULT 0,");
            sb.AppendLine("admin bool DEFAULT false");
            sb.AppendLine(");");
            sb.AppendLine("INSERT INTO users_new (handsign, login, pw, access, admin) SELECT name, CASE WHEN login IS NULL OR TRIM(login) = '' THEN name ELSE login END, pw, access, admin FROM users;");
            sb.AppendLine("DROP TABLE users;");
            sb.AppendLine("ALTER TABLE users_new RENAME TO users;");
            await ExecuteNonQueryAsync(sb.ToString());
        }

        private async Task EnsureUserLoginValuesAsync()
        {
            if (await HasColumnAsync("users", "handsign") && await HasColumnAsync("users", "login"))
                await ExecuteNonQueryAsync("UPDATE users SET login = handsign WHERE login IS NULL OR TRIM(login) = '';");
        }

        private async Task EnsureInitialAdminUserAsync()
        {
            if (await HasColumnAsync("users", "handsign") && await HasColumnAsync("users", "login"))
                await ExecuteNonQueryAsync("INSERT INTO users (handsign, login, pw, access, admin, failed_login_attempts) SELECT '🛡️', 'Admin', '', 0, 1, 0 WHERE NOT EXISTS (SELECT 1 FROM users);");
        }
        /// <summary>
        /// Updates the adapter data and refreshes the related application state.
        /// </summary>
        public override async Task<bool> UpdateAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                if (IsPettyCashSelect(select))
                    await EnsurePettyCashTableAsync();

                System.Data.DataTable changes = table.GetChanges();
                if (changes == null)
                    return true;

                AdapterCommandInfo commandInfo = table.ExtendedProperties[AdapterSelectCommandProperty] as AdapterCommandInfo ?? CreateAdapterCommandInfo(select);

                using (SQLiteCommand command = CreateSelectCommand(commandInfo))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                using (SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter))
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
            using (SQLiteCommand cmd = CreateCommand(command))
                return await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (SQLiteDataAdapter adapter in adapters.Values)
                adapter.Dispose();
            adapters.Clear();
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
        /// <summary>
        /// Creates the new Password data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateNewPasswordAsync(string host, string username, string password, string new_password)
        {
            if (connection == null)
                await ConnectAsync(host, username, password, host);
            connection.ChangePassword(new_password);
        }
        /// <summary>
        /// Creates the fixed Tables data or user interface element for the current workflow.
        /// </summary>
        protected override void CreateFixedTables(StringBuilder sb)
        {
            sb.AppendLine("DROP TABLE IF EXISTS users;");
            sb.AppendLine("create table");
            sb.AppendLine("users");
            sb.AppendLine("(");
            sb.AppendLine("[handsign] varchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[login] varchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[pw] varchar(255) NOT NULL,");
            sb.AppendLine("[access] int(11) NOT NULL,");
            sb.AppendLine("[admin] BOOLEAN NOT NULL,");
            sb.AppendLine("[failed_login_attempts] INTEGER NOT NULL DEFAULT 0,");
            sb.AppendLine("[last_failed_login] DATE,");
            sb.AppendLine("[locked_until] DATE,");
            sb.AppendLine("PRIMARY KEY(handsign,login)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS company;");
            sb.AppendLine("create table");
            sb.AppendLine("company");
            sb.AppendLine("(");
            sb.AppendLine("name varchar(255) PRIMARY KEY,");
            sb.AppendLine("secretary varchar(255),");
            sb.AppendLine("phone varchar(45),");
            sb.AppendLine("fax varchar(45),");
            sb.AppendLine("email varchar(255) UNIQUE,");
            sb.AppendLine("street varchar(255),");
            sb.AppendLine("zipcode varchar(10),");
            sb.AppendLine("city varchar(255),");
            sb.AppendLine("language varchar(64),");
            sb.AppendLine("web varchar(255),");
            sb.AppendLine("local_court varchar(255),");
            sb.AppendLine("hrb varchar(255),");
            sb.AppendLine("ik varchar(255),");
            sb.AppendLine("smtp_host varchar(255),");
            sb.AppendLine("smtp_user varchar(255),");
            sb.AppendLine("smtp_key varchar(255),");
            sb.AppendLine("currency_code varchar(3) DEFAULT 'EUR',");
            sb.AppendLine("logo BLOB,");
            sb.AppendLine("logo_alignment Integer");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS company_bank;");
            sb.AppendLine("create table");
            sb.AppendLine("company_bank");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("name varchar(255),");
            sb.AppendLine("code varchar(64),");
            sb.AppendLine("account_no varchar(255),");
            sb.AppendLine("iban varchar(128),");
            sb.AppendLine("bic varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS license;");
            sb.AppendLine("create table");
            sb.AppendLine("license");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("grade Integer,");
            sb.AppendLine("begin DATE,");
            sb.AppendLine("expired DATE,");
            sb.AppendLine("key  varbinary(2048)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS layouts;");
            sb.AppendLine("create table");
            sb.AppendLine("layouts");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("accounts BLOB,");
            sb.AppendLine("advisors BLOB,");
            sb.AppendLine("employees BLOB,");
            sb.AppendLine("bank BLOB,");
            sb.AppendLine("cash BLOB,");
            sb.AppendLine("cashaudit BLOB,");
            sb.AppendLine("clients BLOB,");
            sb.AppendLine("quittance BLOB,");
            sb.AppendLine("officecash BLOB");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS version;");
            sb.AppendLine("create table");
            sb.AppendLine("version");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("main varchar(64)");
            sb.AppendLine(");");
        }
        /// <summary>
        /// Creates the user Tables data or user interface element for the current workflow.
        /// </summary>
        protected override async Task CreateUserTablesAsync(StringBuilder sb)
        {
            sb.AppendLine("DROP TABLE IF EXISTS advisors;");
            sb.AppendLine("create table");
            sb.AppendLine("advisors");
            sb.AppendLine("(");
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
            sb.AppendLine("create table");
            sb.AppendLine("accounts");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("type varchar(64) NOT NULL,");
            sb.AppendLine("active INTEGER NOT NULL DEFAULT 1,");
            sb.AppendLine("created_at DATE NOT NULL DEFAULT CURRENT_TIMESTAMP");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS employees;");
            sb.AppendLine("create table");
            sb.AppendLine("employees");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("account_transfer DECIMAL(18,2),");
            sb.AppendLine("amount_payout DECIMAL(18,2),");
            sb.AppendLine("amount_payback DECIMAL(18,2),");
            sb.AppendLine("amount_payback_type int(11),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("active int(11),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS bank_books;");
            sb.AppendLine("create table");
            sb.AppendLine("bank_books");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS cash_books;");
            sb.AppendLine("create table");
            sb.AppendLine("cash_books");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS hard_cash;");
            sb.AppendLine("create table hard_cash (");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL DEFAULT '0',");
            sb.AppendLine("`001` INTEGER NOT NULL DEFAULT '0',");
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
            sb.AppendLine("create table");
            sb.AppendLine("client_books");
            sb.AppendLine("(");
            sb.AppendLine("'index' INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("id INTEGER,");
            sb.AppendLine("document_id int(11),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS clients;");
            sb.AppendLine("create table");
            sb.AppendLine("clients");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("title varchar(8),");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("street varchar(128),");
            sb.AppendLine("zipcode varchar(45),");
            sb.AppendLine("city varchar(128),");
            sb.AppendLine("born DATE,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("account_transfer DECIMAL(18,2),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("lastbook DATE,");
            sb.AppendLine("active INTEGER,");
            sb.AppendLine("info INTEGER,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("advisor_id INTEGER,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS deadlines;");
            sb.AppendLine("create table");
            sb.AppendLine("deadlines");
            sb.AppendLine("(");
            sb.AppendLine("'no' INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("id INTEGER,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS petty_cash;");
            sb.AppendLine("create table");
            sb.AppendLine("petty_cash");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("account_id INTEGER,");
            sb.AppendLine("book_cat INTEGER,");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS record;");
            sb.AppendLine("create table");
            sb.AppendLine("record");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("client_id INTEGER,");
            sb.AppendLine("`index` INTEGER,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("filename varchar(255),");
            sb.AppendLine("file BLOB,");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
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
            sb.AppendLine("CREATE TRIGGER clients_AUPD AFTER UPDATE OF account_transfer ON clients FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=NEW.account_transfer + (Select COALESCE(sum(amount),0) from client_books where NEW.id=id) where id=NEW.id;");
            sb.AppendLine("end;");
            using (var command = new SQLiteCommand(sb.ToString(), connection))
                await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Restores the restore data from the selected source.
        /// </summary>
        public override async Task RestoreAsync(string filename)
        {
            StringBuilder sb = new StringBuilder();
            await CreateUserTablesAsync(sb);
            using (SQLiteCommand command = new SQLiteCommand(sb.ToString(), connection))
                await command.ExecuteNonQueryAsync();
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
            if (IsPettyCashSelect(select))
                await EnsurePettyCashTableAsync();

            foreach (DataColumn column in to.Columns)
            {
                var defaultValue = column.DefaultValue.ToString();
                if (string.IsNullOrWhiteSpace(defaultValue))
                    column.DefaultValue = null;
            }

            List<string> columns = to.Columns.OfType<DataColumn>().Select(column => "`" + column.ColumnName.Replace("`", "``") + "`").ToList();
            List<string> parameters = to.Columns.OfType<DataColumn>().Select((column, index) => "@p" + index).ToList();
            string commandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", GetTableName(select), string.Join(",", columns), string.Join(",", parameters));

            foreach (DataRow row in to.Rows)
            {
                using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
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
