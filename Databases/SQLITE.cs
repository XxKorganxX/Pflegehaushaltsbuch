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
        /// <summary>
        /// Creates a new SQLITE instance and initializes the required state.
        /// </summary>
        public SQLITE()
        {
            
            selectCommand[SQLBase.SELECT.BargeByPeriod] = "SELECT * FROM barge WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.BankByPeriod] = "SELECT * FROM bank WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.OfficeByPeriod] = "SELECT * FROM office_cash WHERE date(date) >= date({0:yyyy-MM-dd}) AND date(date) < date({1:yyyy-MM-dd})";
            selectCommand[SQLBase.SELECT.BooksByPeriod] = "SELECT * FROM books WHERE id='{0}' AND date(date) >= date({1:yyyy-MM-dd}) AND date(date) < date({2:yyyy-MM-dd})";
            
            selectCommand[SQLBase.SELECT.BargeFromMonth] = "SELECT * FROM barge WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.BankByDate] = "SELECT * FROM bank WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.Book] = "SELECT * FROM books WHERE id='{0}' AND date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.Deadline] = "SELECT * FROM deadlines WHERE id='{0}' AND strftime(char(37,109), date)=printf(char(37,48,50,100), {1})";
            selectCommand[SQLBase.SELECT.DeadlineByDay] = "SELECT * FROM deadlines WHERE strftime(char(37,100), date)=printf(char(37,48,50,100), {0})";
            selectCommand[SQLBase.SELECT.RecordsByClientAndDate] = "SELECT * FROM record WHERE client_id='{0}' AND date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {2}, {1}), char(43,49,32,109,111,110,116,104))";
            selectCommand[SQLBase.SELECT.OfficeCashByDate] = "SELECT * FROM office_cash WHERE date(date) >= date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0})) AND date(date) < date(printf(char(37,48,52,100,45,37,48,50,100,45,48,49), {1}, {0}), char(43,49,32,109,111,110,116,104))";
        }
        /// <summary>
        /// Creates the data Base data or user interface element for the current workflow.
        /// </summary>
        public override async Task CreateDataBaseAsync(string host, string username, string password, string database)
        {
            try
            {
                SQLiteConnection.CreateFile(host);
                connection = new SQLiteConnection(new SQLiteConnectionStringBuilder { DataSource = host, Version = 3 }.ConnectionString);
                connection.SetPassword(password);
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                CreateFixedTables(sb);
                await CreateUserTablesAsync(sb);
                sb.AppendLine("INSERT INTO hard_cash VALUES (0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);");
                sb.AppendLine("INSERT INTO version VALUES (0, '1.0.8.0');");
                var command = new SQLiteCommand(sb.ToString(), connection);
                await command.ExecuteNonQueryAsync();
                await CreateTriggerAsync();
                sb.Clear();
                sb.AppendLine("CREATE VIEW bank_total_amount AS Select COALESCE(SUM(amount),0) from bank;");
                sb.AppendLine("CREATE VIEW barge_total_amount AS Select COALESCE(SUM(amount),0) from barge;");
                sb.AppendLine("CREATE VIEW office_total_amount AS Select COALESCE(SUM(amount),0) from office_cash;");
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
        }
        /// <summary>
        /// Runs the call Functions operation and updates the related application state.
        /// </summary>
        public override async Task<object> CallFunctionsAsync(string name, params object[] values)
        {
            SQLiteCommand cmd = new SQLiteCommand("new_function", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ireturnvalue", DbType.Int32);
            cmd.Parameters["@ireturnvalue"].Direction = ParameterDirection.ReturnValue;
            return await cmd.ExecuteScalarAsync();
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
        private SQLiteCommand CreateSelectCommand(AdapterCommandInfo commandInfo)
        {
            SQLiteCommand command = new SQLiteCommand(commandInfo.CommandText, connection);
            if (ActiveTransaction != null)
                command.Transaction = (SQLiteTransaction)ActiveTransaction;
            foreach (AdapterCommandParameter parameter in commandInfo.Parameters)
                command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
            return command;
        }
        public override async Task FillAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                System.Data.DataTable loadedTable = new System.Data.DataTable();
                using (SQLiteCommand command = CreateSelectCommand(commandInfo))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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
        public override async Task FillAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table, params object[] values)
        {
            await connectionLock.WaitAsync();
            try
            {
                AdapterCommandInfo commandInfo = CreateAdapterCommandInfo(select, values);
                table.ExtendedProperties[AdapterSelectCommandProperty] = commandInfo;

                System.Data.DataTable loadedTable = new System.Data.DataTable();
                using (SQLiteCommand command = CreateSelectCommand(commandInfo))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync()
        {
        }
        /// <summary>
        /// Gets the view value from the current application state.
        /// </summary>
        public override async Task<object> GetViewAsync(string name)
        {
            SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT * FROM {0}", ValidateSqlIdentifier(name)), connection);
            cmd.CommandType = CommandType.Text;
            return await cmd.ExecuteScalarAsync();
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public override async Task UpdateAsync(Version version)
        {
            await Task.CompletedTask;
        }
        /// <summary>
        /// Updates the adapter data and refreshes the related application state.
        /// </summary>
        public override async Task<bool> UpdateAdapterAsync(SQLBase.SELECT select, System.Data.DataTable table)
        {
            await connectionLock.WaitAsync();
            try
            {
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
                    if (ActiveTransaction != null)
                    {
                        adapter.InsertCommand.Transaction = (SQLiteTransaction)ActiveTransaction;
                        adapter.DeleteCommand.Transaction = (SQLiteTransaction)ActiveTransaction;
                        adapter.UpdateCommand.Transaction = (SQLiteTransaction)ActiveTransaction;
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
            SQLiteCommand cmd = connection.CreateCommand();
            connection.ChangeDatabase(dataBase);
            cmd.CommandText = command;
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
                connection.Close();
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
            sb.AppendLine("[name] varchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[login] varchar(255) NOT NULL UNIQUE,");
            sb.AppendLine("[pw] varchar(255) NOT NULL,");
            sb.AppendLine("[phone] varchar(255),");
            sb.AppendLine("[fax] varchar(255),");
            sb.AppendLine("[email] varchar(255) UNIQUE,");
            sb.AppendLine("[access] int(11) NOT NULL,");
            sb.AppendLine("[admin] BOOLEAN NOT NULL,");
            sb.AppendLine("PRIMARY KEY(name,login)");
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
            sb.AppendLine("assistants BLOB,");
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
            sb.AppendLine("DROP TABLE IF EXISTS assistants;");
            sb.AppendLine("create table");
            sb.AppendLine("assistants");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY,");
            sb.AppendLine("name varchar(255) UNIQUE,");
            sb.AppendLine("account_transfer DECIMAL(18,2),");
            sb.AppendLine("amount_payout DECIMAL(18,2),");
            sb.AppendLine("amount_payback DECIMAL(18,2),");
            sb.AppendLine("amount_payback_type int(11),");
            sb.AppendLine("date DATE,");
            sb.AppendLine("active int(11),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS bank;");
            sb.AppendLine("create table");
            sb.AppendLine("bank");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account varchar(64),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("handsign varchar(64)");
            sb.AppendLine(");");
            sb.AppendLine("DROP TABLE IF EXISTS barge;");
            sb.AppendLine("create table");
            sb.AppendLine("barge");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("book_cat int(11),");
            sb.AppendLine("book_to int(11),");
            sb.AppendLine("amount DECIMAL(18,2),");
            sb.AppendLine("account varchar(64),");
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
            sb.AppendLine("DROP TABLE IF EXISTS books;");
            sb.AppendLine("create table");
            sb.AppendLine("books");
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
            sb.AppendLine("DROP TABLE IF EXISTS office_cash;");
            sb.AppendLine("create table");
            sb.AppendLine("office_cash");
            sb.AppendLine("(");
            sb.AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("date DATE,");
            sb.AppendLine("note varchar(512),");
            sb.AppendLine("account INTEGER,");
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
            sb.AppendLine("CREATE TRIGGER books_AUPD AFTER UPDATE ON books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("end;");
            sb.AppendLine("CREATE TRIGGER books_AINS AFTER INSERT ON books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from books where NEW.id=id) where NEW.id=clients.id;");
            sb.AppendLine("end;");
            sb.AppendLine("CREATE TRIGGER books_ADEL AFTER DELETE ON books FOR EACH ROW ");
            sb.AppendLine("begin ");
            sb.AppendLine("Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from books where OLD.id=id) where OLD.id=clients.id;");
            sb.AppendLine("Update clients SET lastbook=(Select max(date) from books where OLD.id=id) where OLD.id=clients.id;");
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
