using Pflegehaushaltsbuch.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Pflegehaushaltsbuch.Databases
{
    /// <summary>
    /// Represents the SQL Base component used by the application.
    /// </summary>
    public abstract class SQLBase : IDisposable
    {
        protected const string AdapterSelectCommandProperty = "AdapterSelectCommand";
        protected sealed class AdapterCommandInfo
        {
            public string CommandText { get; set; }
            public List<AdapterCommandParameter> Parameters { get; } = new List<AdapterCommandParameter>();
        }
        protected sealed class AdapterCommandParameter
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
        public sealed class DatabaseTransaction : IDisposable
        {
            private readonly DbTransaction transaction;
            private readonly Action clearTransaction;
            private bool completed;

            internal DatabaseTransaction(DbTransaction transaction, Action clearTransaction)
            {
                this.transaction = transaction;
                this.clearTransaction = clearTransaction;
            }

            public void Commit()
            {
                transaction.Commit();
                completed = true;
                clearTransaction();
            }

            public void Rollback()
            {
                transaction.Rollback();
                completed = true;
                clearTransaction();
            }

            public void Dispose()
            {
                try
                {
                    if (!completed)
                        transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                    clearTransaction();
                }
            }
        }
        protected DbTransaction ActiveTransaction { get; private set; }
        public DatabaseTransaction BeginTransaction()
        {
            if (ActiveTransaction != null)
                throw new InvalidOperationException(Messages.sql_activeDatabase_transaction);

            ActiveTransaction = BeginDbTransaction();
            return new DatabaseTransaction(ActiveTransaction, ClearTransaction);
        }
        protected abstract DbTransaction BeginDbTransaction();
        private void ClearTransaction()
        {
            ActiveTransaction = null;
        }
        /// <summary>
        /// Updates the progress maximum delegate data and refreshes the related application state.
        /// </summary>
        public delegate void UpdateProgressMaximumDelegate(int percent);
        public event UpdateProgressMaximumDelegate UpdateMaximumProgress;
        /// <summary>
        /// Updates the progress delegate data and refreshes the related application state.
        /// </summary>
        public delegate void UpdateProgressDelegate(int percent, bool increment);
        public event UpdateProgressDelegate UpdateProgress;
        /// <summary>
        /// Updates the progress text delegate data and refreshes the related application state.
        /// </summary>
        public delegate void UpdateProgressTextDelegate(string text);
        public event UpdateProgressTextDelegate UpdateProgressText;
        /// <summary>
        /// Handles the update version delegate lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnUpdateVersionDelegate(string sql_class, Version version);
        public static event OnUpdateVersionDelegate UpdateVersion;
        /// <summary>
        /// Runs the test Connection operation and updates the related application state.
        /// </summary>
        public abstract Task<bool> TestConnectionAsync(string host, string database, string username, string password);
        /// <summary>
        /// Connects the connect data source or control used by the current workflow.
        /// </summary>
        public abstract Task ConnectAsync(string host, string username, string password, string database);
        /// <summary>
        /// Runs the drop Database operation and updates the related application state.
        /// </summary>
        public abstract Task DropDatabaseAsync(string host, string username, string password, string database);
        /// <summary>
        /// Creates the data Base data or user interface element for the current workflow.
        /// </summary>
        public abstract Task CreateDataBaseAsync(string host, string username, string password, string database);
        /// <summary>
        /// Creates the new Password data or user interface element for the current workflow.
        /// </summary>
        public abstract Task CreateNewPasswordAsync(string host, string username, string password, string new_password);
        /// <summary>
        /// Gets the all Databases value from the current application state.
        /// </summary>
        public virtual Task<string[]> GetAllDatabasesAsync(string host, string username, string password)
        {
            return Task.FromResult(new string[0]);
        }

        /// <summary>
        /// Handles the update Version Event lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnUpdateVersionEvent(Version version);
        public event OnUpdateVersionEvent PrintCurrentVersion;
        /// <summary>
        /// Handles the print Version lifecycle step and applies the related control behavior.
        /// </summary>
        protected void OnPrintVersion(Version version)
        {
            if (PrintCurrentVersion != null)
                PrintCurrentVersion(version);
        }
        private User user;
        public Version Version { get; protected set; }
        protected string host, username, database, password;
        /// <summary>
        /// Creates a new SQL Base instance and initializes the required state.
        /// </summary>
        public SQLBase()
        {
            Company = new Company();
            Printing = new Printing();
        }
        /// <summary>
        /// Handles the load lifecycle step and applies the related control behavior.
        /// </summary>
        public async Task OnLoadAsync()
        {
            await Printing.LoadDocuments(this);
        }
        public User User
        {
            get { return user; }
            private set { user = value; }
        }
        public Company Company { get; set; }
        public Printing Printing { get; set; }
        protected Dictionary<string, int> LegacyAccountIdsByPreviousCode { get; private set; } = new Dictionary<string, int>();
        public Dictionary<SELECT, string> selectCommand = new Dictionary<SELECT, string>()
        {
            { SELECT.Cash, "SELECT * FROM cash_books"},
            { SELECT.BargeFromMonth, "SELECT * FROM cash_books WHERE MONTH(date)='{0}' AND YEAR(date)='{1}'" },
            { SELECT.BargeByPeriod,  "SELECT * FROM cash_books WHERE (date) >= {0} AND (date) < {1}" },
            { SELECT.Hardcash, "SELECT * FROM hard_cash"},
            { SELECT.Users, "SELECT * FROM users"},
            { SELECT.Accounts, "SELECT * FROM accounts"},
            { SELECT.Advisors, "SELECT * FROM advisors"},
            { SELECT.Clients, "SELECT * FROM clients"},
            { SELECT.Client, "SELECT * FROM clients WHERE id='{0}'"},
            { SELECT.Bank, "SELECT * FROM bank_books"},
            { SELECT.BankByPeriod, "SELECT * FROM bank_books WHERE (date) >= {0} AND (date) < {1}" },
            { SELECT.BankByDate, "SELECT * FROM bank_books WHERE MONTH(date)='{0}' AND YEAR(date)='{1}'" },
            { SELECT.Emploees, "SELECT * FROM employees"},
            { SELECT.Assistant, "SELECT * FROM employees WHERE id='{0}'"},
            { SELECT.Book, "SELECT * FROM client_books WHERE id='{0}' AND MONTH(date)='{1}' AND YEAR(date)='{2}'"},
            { SELECT.BooksByPeriod, "SELECT * FROM client_books WHERE id='{0}' AND (date) >= {1} AND (date) < {2}" },
            { SELECT.Books, "SELECT * FROM client_books"},
            { SELECT.BooksByUser, "SELECT * FROM client_books WHERE id='{0}'"},
            { SELECT.Deadlines, "SELECT * FROM deadlines"},
            { SELECT.DeadlineByClient, "SELECT * FROM deadlines WHERE id='{0}'"},
            { SELECT.Deadline, "SELECT * FROM deadlines WHERE id='{0}' AND MONTH(date)='{1}'"},
            { SELECT.DeadlineByDay, "SELECT * FROM deadlines WHERE DAY(date)='{0}'"},
            { SELECT.Journal, "SELECT * FROM journal"},
            { SELECT.Company, "SELECT * FROM company"},
            { SELECT.Company_bank, "SELECT * FROM company_bank"},
            { SELECT.Layouts, "SELECT * FROM layouts"},
            { SELECT.Records, "SELECT * FROM record"},
            { SELECT.RecordsByClient, "SELECT * FROM record WHERE client_id='{0}'"},
            { SELECT.RecordsByClientAndDate, "SELECT * FROM record WHERE client_id='{0}' AND MONTH(date)='{1}' AND YEAR(date)='{2}'"},
            { SELECT.License, "SELECT * FROM license"},
            { SELECT.Version, "SELECT * FROM version"},
            { SELECT.OfficeCash, "SELECT * FROM office_cash"},
            { SELECT.OfficeCashByDate, "SELECT * FROM office_cash WHERE MONTH(date)='{0}' AND YEAR(date)='{1}'" },
            { SELECT.OfficeByPeriod, "SELECT * FROM office_cash WHERE (date) >= {0} AND (date) < {1}" }
            
        };
        protected AdapterCommandInfo CreateAdapterCommandInfo(SELECT select, params object[] values)
        {
            string commandText = selectCommand[select];
            AdapterCommandInfo info = new AdapterCommandInfo();

            if (values == null || values.Length == 0)
            {
                info.CommandText = commandText;
                return info;
            }

            int parameterIndex = 0;
            commandText = Regex.Replace(commandText, @"'([^']*\{\d+(?::[^}]*)?\}[^']*)'", match =>
            {
                string parameterName = "@p" + parameterIndex++;
                info.Parameters.Add(new AdapterCommandParameter
                {
                    Name = parameterName,
                    Value = string.Format(CultureInfo.InvariantCulture, match.Groups[1].Value, values)
                });
                return parameterName;
            });

            commandText = Regex.Replace(commandText, @"\{(\d+)(?::([^}]+))?\}", match =>
            {
                string parameterName = "@p" + parameterIndex++;
                int valueIndex = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                object value = values[valueIndex];

                if (match.Groups[2].Success)
                    value = string.Format(CultureInfo.InvariantCulture, "{0:" + match.Groups[2].Value + "}", value);

                info.Parameters.Add(new AdapterCommandParameter
                {
                    Name = parameterName,
                    Value = value ?? DBNull.Value
                });
                return parameterName;
            });

            info.CommandText = commandText;
            return info;
        }
        protected static string ValidateSqlIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                throw new ArgumentException("Invalid SQL identifier.", nameof(name));
            return name;
        }
        /// <summary>
        /// Defines the available sELECT values used by the application.
        /// </summary>
        public enum SELECT
        {
            Cash,
            BargeFromMonth,
            BargeByPeriod,
            Hardcash,
            Users,
            Accounts,
            Advisors,
            Clients,
            Client,
            Bank,
            BankByDate,
            BankByPeriod,
            Emploees,
            Assistant,
            Book,
            BooksByPeriod,
            Books,
            BooksByUser,
            Deadlines,
            DeadlineByClient,
            Deadline,
            DeadlineByDay,
            Journal,
            Company,
            Company_bank,
            Layouts,
            Records,
            RecordsByClient,
            RecordsByClientAndDate,
            License,
            Version,
            OfficeCash,
            OfficeCashByDate,
            OfficeByPeriod
        }

        /// <summary>
        /// Set a new user for sql databas object.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal async Task SetCurrentUserAsync(User user)
        {
            //CurrentUser = user ?? throw new ArgumentNullException(nameof(user));

            if (user == null)
                return;

            User = user;

            Printing.UpdateVariable(Printing.VarNames.assistant_name, user.Name);
            Printing.UpdateVariable(Printing.VarNames.assistant_email, user.Email);
            Printing.UpdateVariable(Printing.VarNames.assistant_fax, user.Fax);
            Printing.UpdateVariable(Printing.VarNames.assistant_phone, user.Phone);
            await OnUpdateAsync();
            if (UpdateVersion != null)
                UpdateVersion(this.GetType().Name, Version);
        }

        /// <summary>
        /// Gets the table Name value from the current application state.
        /// </summary>
        public string GetTableName(SELECT select)
        {
            switch (select)
            {
                case SELECT.Cash:
                    return "cash_books";
                case SELECT.Hardcash:
                    return "hard_cash";
                case SELECT.Users:
                    return "users";
                case SELECT.Accounts:
                    return "accounts";
                case SELECT.Advisors:
                    return "advisors";
                case SELECT.Clients:
                    return "clients";
                case SELECT.Bank:
                    return "bank_books";
                case SELECT.Emploees:
                    return "employees";
                case SELECT.Books:
                    return "client_books";
                case SELECT.Deadlines:
                    return "deadlines";
                case SELECT.Journal:
                    return "journal";
                case SELECT.Company:
                    return "company";
                case SELECT.Company_bank:
                    return "company_bank";
                case SELECT.Layouts:
                    return "layouts";
                case SELECT.Records:
                    return "record";
                case SELECT.License:
                    return "license";
                case SELECT.Version:
                    return "version";
                case SELECT.OfficeCash:
                    return "office_cash";
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// Defines the available column Names values used by the application.
        /// </summary>
        public enum ColumnNames
        {
            id,
            title,
            name,
            street,
            phone,
            fax,
            email,
            zipcode,
            city,
            born,
            footer,
            login,
            pw,
            admin,
            access,
            date,
            type,
            account,
            account_id,
            account_transfer,
            amount,
            amount_payout,
            amount_payback,
            amount_payback_type,
            lastbook,
            active,
            info,
            note,
            advisor_id,
            document_id,
            book_to,
            book_cat,
            handsign,
            created_at
        }
        private static Dictionary<ColumnNames, string> columnNames = new Dictionary<ColumnNames, string>()
        {
            { ColumnNames.id, "id"},
            { ColumnNames.title, "title"},
            { ColumnNames.name, "name"},
            { ColumnNames.street, "street"},
            { ColumnNames.phone, "phone"},
            { ColumnNames.fax, "fax"},
            { ColumnNames.email, "email"},
            { ColumnNames.zipcode, "zipcode"},
            { ColumnNames.city, "city"},
            { ColumnNames.born, "born"},
            { ColumnNames.footer, "footer"},
            { ColumnNames.login, "login"},
            { ColumnNames.pw, "pw"},
            { ColumnNames.admin, "admin"},
            { ColumnNames.access, "access"},
            { ColumnNames.date, "date"},
            { ColumnNames.type, "type"},
            { ColumnNames.account_transfer, "account_transfer"},
            { ColumnNames.amount, "amount"},
            { ColumnNames.account, "account"},
            { ColumnNames.account_id, "account_id"},
            { ColumnNames.amount_payout, "amount_payout"},
            { ColumnNames.amount_payback, "amount_payback"},
            { ColumnNames.amount_payback_type, "amount_payback_type"},
            { ColumnNames.lastbook, "lastbook"},
            { ColumnNames.active, "active"},
            { ColumnNames.info, "info"},
            { ColumnNames.note, "note"},
            { ColumnNames.advisor_id, "advisor_id"},
            { ColumnNames.document_id, "document_id"},
            { ColumnNames.book_to, "book_to"},
            { ColumnNames.book_cat, "book_cat"},
            { ColumnNames.handsign, "handsign"},
            { ColumnNames.created_at, "created_at"}
        };
        /// <summary>
        /// Runs the names operation and updates the related application state.
        /// </summary>
        public static string Names(ColumnNames name)
        {
            return columnNames[name];
        }
        /// <summary>
        /// Defines the available book Category values used by the application.
        /// </summary>
        public enum BookCategory
        {
            Einzahlung,
            Auszahlung,
            Storno
        }
        /// <summary>
        /// Defines the available booking To values used by the application.
        /// </summary>
        public enum BookingTo
        {
            Barbestand,
            Bankbestand,
            Altbestand
        }
        /// <summary>
        /// Defines the available client Active values used by the application.
        /// </summary>
        public enum ClientActive
        {
            Inactive,
            Active,
            History
        }
        /// <summary>
        /// Defines the available repayment values used by the application.
        /// </summary>
        public enum Repayment
        {
            None,
            Payout,
            Transfered,
            Direct_Debit
        }
        /// <summary>
        /// Defines the available title values used by the application.
        /// </summary>
        public enum Title
        {
            Ms,
            Mr,
            Miss,
            Family
        }
        /// <summary>
        /// Runs the call Functions operation and updates the related application state.
        /// </summary>
        public abstract Task<object> CallFunctionsAsync(string name, params object[] values);
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        public abstract Task FillAdapterAsync(SELECT select, DataTable table);
        /// <summary>
        /// Fills the adapter data structure with values from the current source.
        /// </summary>
        public abstract Task FillAdapterAsync(SELECT select, DataTable table, params object[] values);
        /// <summary>
        /// Handles the update lifecycle step and applies the related control behavior.
        /// </summary>
        private async Task OnUpdateAsync()
        {
            DataTable table = null;
            try
            {
                table = new DataTable();
                await FillAdapterAsync(SELECT.Version, table);
            }
            catch (Exception)
            {
                throw new Exception(Messages.database_version_table_missing);
            }
            Version = new Version(1, 0, 6, 3);
            try
            {
                Version = System.Version.Parse(table.Rows[table.Rows.Count - 1]["main"].ToString());
            }
            catch (Exception)
            {
                throw new Exception(Messages.database_version_table_missing);
            }
            await UpdateAsync();
        }
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public abstract Task UpdateAsync();
        /// <summary>
        /// Updates the update data and refreshes the related application state.
        /// </summary>
        public abstract Task UpdateAsync(Version version);
        /// <summary>
        /// Creates the user data or user interface element for the current workflow.
        /// </summary>
        public virtual Task CreateUserAsync(string username, string pwd, string database, string host) { return Task.CompletedTask; }
        /// <summary>
        /// Deletes the user data from the current workflow.
        /// </summary>
        public virtual Task DeleteUserAsync(string username, string pwd, string database, string host) { return Task.CompletedTask; }
        /// <summary>
        /// Updates the adapter data and refreshes the related application state.
        /// </summary>
        public abstract Task<bool> UpdateAdapterAsync(SELECT select, DataTable table);
        /// <summary>
        /// Runs the insert Table operation and updates the related application state.
        /// </summary>
        protected abstract Task InsertTableAsync(SELECT select, DataTable to);
        /// <summary>
        /// Gets the view value from the current application state.
        /// </summary>
        public virtual Task<object> GetViewAsync(string name) { throw new NotImplementedException(); }
        public static int MaximumUpdates = 1;
        /// <summary>
        /// Updates the asistance data and refreshes the related application state.
        /// </summary>
        public async Task<bool> UpdateAsistanceAsync(string name, DateTime date, decimal amount, int repayment)
        {
            if (repayment <= 0)
                throw new Exception(Messages.ioan_missing_repayment);
            DataTable table = new DataTable();
            await FillAdapterAsync(SELECT.Emploees, table);
            DataRow row = table.Rows
                .OfType<DataRow>()
                .FirstOrDefault(item => string.Equals(item[Names(ColumnNames.name)].ToString(), name, StringComparison.Ordinal));
            if (row == null)
                return false;
            decimal amountPayout = decimal.Parse(row[Names(ColumnNames.amount_payout)].ToString()) - amount;
            decimal amountPayback = decimal.Parse(row[Names(ColumnNames.amount_payback)].ToString()) + amount;
            row[Names(ColumnNames.amount_payout)] = amountPayout;
            row[Names(ColumnNames.amount_payback)] = amountPayback;
            row[Names(ColumnNames.amount_payback_type)] = repayment;
            if (amountPayout == 0)
            {
                row[Names(ColumnNames.account_transfer)] = 0;
                row[Names(ColumnNames.active)] = false;
            }
            row[Names(ColumnNames.handsign)] = User.Name;
            return await UpdateAdapterAsync(SELECT.Emploees, table);
        }
        public async Task<int> CreateAccountIdAsync(string type, bool active = true)
        {
            DataTable accounts = new DataTable();
            await FillAdapterAsync(SELECT.Accounts, accounts);

            int accountId = accounts.Rows
                .OfType<DataRow>()
                .Where(row => row.RowState != DataRowState.Deleted && row[Names(ColumnNames.id)] != DBNull.Value)
                .Select(row => Convert.ToInt32(row[Names(ColumnNames.id)]))
                .DefaultIfEmpty(-1)
                .Max() + 1;

            DataRow account = accounts.NewRow();
            account[Names(ColumnNames.id)] = accountId;
            account[Names(ColumnNames.type)] = type;
            account[Names(ColumnNames.active)] = active ? 1 : 0;
            account[Names(ColumnNames.created_at)] = DateTime.Now;
            accounts.Rows.Add(account);

            if (!await UpdateAdapterAsync(SELECT.Accounts, accounts))
                throw new Exception(Messages.datatable_update_failed);

            return accountId;
        }

        public async Task EnsureAccountIdsForClientsAndEmployeesAsync()
        {
            DataTable accounts = new DataTable();
            DataTable clients = new DataTable();
            DataTable employees = new DataTable();
            await FillAdapterAsync(SELECT.Accounts, accounts);
            await FillAdapterAsync(SELECT.Clients, clients);
            await FillAdapterAsync(SELECT.Emploees, employees);

            int nextAccountId = accounts.Rows
                .OfType<DataRow>()
                .Where(row => row.RowState != DataRowState.Deleted && row[Names(ColumnNames.id)] != DBNull.Value)
                .Select(row => Convert.ToInt32(row[Names(ColumnNames.id)]))
                .DefaultIfEmpty(1)
                .Max() + 1;

            bool accountsChanged = false;
            bool clientsChanged = EnsureAccountIdsForTable(accounts, clients, "Client", ref nextAccountId, ref accountsChanged);
            bool employeesChanged = EnsureAccountIdsForTable(accounts, employees, "Employee", ref nextAccountId, ref accountsChanged);

            if (accountsChanged && !await UpdateAdapterAsync(SELECT.Accounts, accounts))
                throw new Exception(Messages.datatable_update_failed);
            if (clientsChanged && !await UpdateAdapterAsync(SELECT.Clients, clients))
                throw new Exception(Messages.datatable_update_failed);
            if (employeesChanged && !await UpdateAdapterAsync(SELECT.Emploees, employees))
                throw new Exception(Messages.datatable_update_failed);
        }

        private bool EnsureAccountIdsForTable(DataTable accounts, DataTable table, string type, ref int nextAccountId, ref bool accountsChanged)
        {
            if (table == null || !table.Columns.Contains(Names(ColumnNames.account_id)))
                return false;

            bool tableChanged = false;
            foreach (DataRow row in table.Rows.OfType<DataRow>())
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                int accountId;
                if (row[Names(ColumnNames.account_id)] == DBNull.Value)
                {
                    accountId = nextAccountId++;
                    row[Names(ColumnNames.account_id)] = accountId;
                    tableChanged = true;
                }
                else
                    accountId = Convert.ToInt32(row[Names(ColumnNames.account_id)]);

                if (!ContainsAccountId(accounts, accountId))
                {
                    DataRow account = accounts.NewRow();
                    account[Names(ColumnNames.id)] = accountId;
                    account[Names(ColumnNames.type)] = type;
                    account[Names(ColumnNames.active)] = row.Table.Columns.Contains(Names(ColumnNames.active)) && row[Names(ColumnNames.active)] != DBNull.Value ? row[Names(ColumnNames.active)] : 1;
                    account[Names(ColumnNames.created_at)] = DateTime.Now;
                    accounts.Rows.Add(account);
                    accountsChanged = true;
                }
            }

            return tableChanged;
        }

        private static bool ContainsAccountId(DataTable accounts, int accountId)
        {
            return accounts.Rows
                .OfType<DataRow>()
                .Any(row => row.RowState != DataRowState.Deleted && row["id"] != DBNull.Value && Convert.ToInt32(row["id"]) == accountId);
        }
        /// <summary>
        /// Creates the barge booking data for the current workflow.
        /// </summary>
        public async Task<bool> ToBargeAsync(DateTime date, string note, decimal amount, string account, BookCategory bookingCategory, BookingTo bookTo)
        {
            DataTable cashTable = new DataTable();
            await FillAdapterAsync(SELECT.Cash, cashTable);
            cashTable.Columns["id"].AutoIncrement = true;
            cashTable.Columns["id"].Unique = true;
            DataRow cashRow = cashTable.NewRow();
            int? accountId = await ResolveLegacyAccountIdAsync(account);
            cashRow[Names(ColumnNames.date)] = date.Date;
            cashRow[Names(ColumnNames.note)] = note;
            cashRow[Names(ColumnNames.amount)] = amount;
            cashRow[Names(ColumnNames.account)] = account;
            if (accountId.HasValue && cashTable.Columns.Contains(Names(ColumnNames.account_id)))
                cashRow[Names(ColumnNames.account_id)] = accountId.Value;
            cashRow[Names(ColumnNames.book_to)] = bookTo;
            cashRow[Names(ColumnNames.book_cat)] = bookingCategory;
            cashRow[Names(ColumnNames.handsign)] = User.Name;
            cashTable.Rows.Add(cashRow);
            return await UpdateAdapterAsync(SELECT.Cash, cashTable);
        }
        /// <summary>
        /// Creates the bank booking data for the current workflow.
        /// </summary>
        public async Task<bool> ToBankAsync(DateTime date, string note, decimal amount, string account, BookCategory bookingCategory, BookingTo bookTo)
        {
            DataTable bankTable = new DataTable();
            await FillAdapterAsync(SELECT.Bank, bankTable);
            DataRow bankRow = bankTable.NewRow();
            int? accountId = await ResolveLegacyAccountIdAsync(account);
            bankRow[Names(ColumnNames.date)] = date.Date;
            bankRow[Names(ColumnNames.note)] = note;
            bankRow[Names(ColumnNames.amount)] = amount;
            bankRow[Names(ColumnNames.account)] = account;
            if (accountId.HasValue && bankTable.Columns.Contains(Names(ColumnNames.account_id)))
                bankRow[Names(ColumnNames.account_id)] = accountId.Value;
            bankRow[Names(ColumnNames.book_to)] = bookTo;
            bankRow[Names(ColumnNames.book_cat)] = bookingCategory;
            bankRow[Names(ColumnNames.handsign)] = User.Name;
            bankTable.Rows.Add(bankRow);
            return await UpdateAdapterAsync(SELECT.Bank, bankTable);
        }
        
        /// <summary>
        /// Creates the books booking data for the current workflow.
        /// </summary>
        public async Task<(bool, DataRow)> ToBooksAsync(string clientName, int clientId, DateTime date, string bookText, decimal amount, BookCategory bookingCategory, BookingTo bookTo)
        {
            DataTable bookTable = new DataTable();
            await FillAdapterAsync(SELECT.Book, bookTable, clientId, date.Month, date.Year);
            DataRow row = bookTable.NewRow();
            row[Names(ColumnNames.date)] = date;
            row[Names(ColumnNames.note)] = bookText;
            row[Names(ColumnNames.book_cat)] = bookingCategory;
            row[Names(ColumnNames.amount)] = amount;
            row[Names(ColumnNames.handsign)] = User.Name;
            row[Names(ColumnNames.id)] = clientId;
            row[Names(ColumnNames.document_id)] = 0;
            row[Names(ColumnNames.book_to)] = bookTo;
            bookTable.Rows.Add(row);
            int belegNr = 1;
            DataRow[] rows = bookTable.Select("", Names(ColumnNames.date));
            foreach (DataRow item in rows)
                item[Names(ColumnNames.document_id)] = belegNr++;
            return (await UpdateAdapterAsync(SELECT.Book, bookTable), row);
        }
        /// <summary>
        /// Runs the book2 Cash Office operation and updates the related application state.
        /// </summary>
        public async Task<(bool, DataRow)> Book2CashOfficeAsync(DateTime date, string bookText, decimal amount, BookCategory bookingCategory, int account)
        {
            DataTable cashOfficeTable = new DataTable();
            await FillAdapterAsync(SELECT.OfficeCash, cashOfficeTable);
            amount = Math.Abs(amount);
            DataRow row = cashOfficeTable.NewRow();
            row[Names(ColumnNames.date)] = date;
            row[Names(ColumnNames.note)] = bookText;
            row[Names(ColumnNames.book_cat)] = bookingCategory;
            row[Names(ColumnNames.amount)] = (bookingCategory == BookCategory.Auszahlung) ? -amount : amount;
            row[Names(ColumnNames.handsign)] = User.Name;
            row[Names(ColumnNames.account)] = account;
            cashOfficeTable.Rows.Add(row);
            return (await UpdateAdapterAsync(SELECT.OfficeCash, cashOfficeTable), row);
        }
        /// <summary>
        /// Updates the journal data and refreshes the related application state.
        /// </summary>
        public abstract int UpdateJournal(Enums.UpdateJournal param, DateTime date, string note, string changes = "");
        /// <summary>
        /// Updates the data Base data and refreshes the related application state.
        /// </summary>
        public abstract Task<int> UpdateDataBaseAsync(string command);
        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        public virtual void Dispose()
        {
        }
        /// <summary>
        /// Trims the between value and returns the cleaned result.
        /// </summary>
        public string TrimBetween(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            string[] splittedStr = str.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splittedStr.Length; i++)
            {
                sb.Append(splittedStr[i]);
                if (i < splittedStr.Length - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// Gets the ID value from the current application state.
        /// </summary>
        public int GetID(DataTable table)
        {
            DataRow[] rows = table.Select("", "id");
            Dictionary<int, int> ids = new Dictionary<int, int>();
            foreach (DataRow item in rows)
                ids[Int32.Parse(item["id"].ToString())] = 0;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!ids.ContainsKey(i))
                    return i;
            }
            return -1;
        }
        protected Dictionary<string, int> CreateLegacyAccountIdMap(DataTable clients, DataTable employees)
        {
            Dictionary<string, int> accountsByLegacyId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            AddLegacyAccountIds(accountsByLegacyId, clients, "K");
            AddLegacyAccountIds(accountsByLegacyId, employees, "M");
            return accountsByLegacyId;
        }

        protected async Task RefreshLegacyAccountIdMapAsync()
        {
            DataTable clients = new DataTable();
            DataTable employees = new DataTable();
            await FillAdapterAsync(SELECT.Clients, clients);
            await FillAdapterAsync(SELECT.Emploees, employees);
            LegacyAccountIdsByPreviousCode = CreateLegacyAccountIdMap(clients, employees);
        }

        protected async Task ApplyLegacyAccountIdsToCashAndBankBooksAsync()
        {
            await RefreshLegacyAccountIdMapAsync();
            await ApplyLegacyAccountIdsToBookTableAsync(SELECT.Cash);
            await ApplyLegacyAccountIdsToBookTableAsync(SELECT.Bank);
        }

        private async Task ApplyLegacyAccountIdsToBookTableAsync(SELECT select)
        {
            DataTable table = new DataTable();
            await FillAdapterAsync(select, table);
            if (!ApplyLegacyAccountIdsToBookTable(table))
                return;

            await UpdateAdapterAsync(select, table);
        }

        private bool ApplyLegacyAccountIdsToBookTable(DataTable table)
        {
            if (table == null || !table.Columns.Contains("account") || !table.Columns.Contains("account_id"))
                return false;

            bool changed = false;
            foreach (DataRow row in table.Rows.OfType<DataRow>())
            {
                if (row.RowState == DataRowState.Deleted || row["account_id"] != DBNull.Value)
                    continue;

                string legacyAccount = row["account"] == DBNull.Value ? string.Empty : row["account"].ToString().Trim();
                if (String.IsNullOrEmpty(legacyAccount))
                    continue;

                int accountId;
                if (TryGetAccountIdFromLegacyValue(legacyAccount, out accountId))
                {
                    row["account_id"] = accountId;
                    changed = true;
                }
            }

            return changed;
        }

        private bool TryGetAccountIdFromLegacyValue(string legacyAccount, out int accountId)
        {
            if (LegacyAccountIdsByPreviousCode.TryGetValue(legacyAccount, out accountId))
                return true;

            if (String.Equals(legacyAccount, BookingTo.Barbestand.ToString(), StringComparison.OrdinalIgnoreCase) ||
                String.Equals(legacyAccount, "Cash", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(legacyAccount, "Kasse", StringComparison.OrdinalIgnoreCase))
            {
                accountId = 0;
                return true;
            }

            if (String.Equals(legacyAccount, BookingTo.Bankbestand.ToString(), StringComparison.OrdinalIgnoreCase) ||
                String.Equals(legacyAccount, "Bank", StringComparison.OrdinalIgnoreCase))
            {
                accountId = 1;
                return true;
            }

            accountId = -1;
            return false;
        }

        private async Task<int?> ResolveLegacyAccountIdAsync(string legacyAccount)
        {
            if (String.IsNullOrWhiteSpace(legacyAccount))
                return null;

            int accountId;
            legacyAccount = legacyAccount.Trim();
            if (TryGetAccountIdFromLegacyValue(legacyAccount, out accountId))
                return accountId;

            await RefreshLegacyAccountIdMapAsync();
            if (TryGetAccountIdFromLegacyValue(legacyAccount, out accountId))
                return accountId;

            return null;
        }

        private static void AddLegacyAccountIds(Dictionary<string, int> accountsByLegacyId, DataTable table, string prefix)
        {
            if (table == null || !table.Columns.Contains("id") || !table.Columns.Contains("account_id"))
                return;

            foreach (DataRow row in table.Rows.OfType<DataRow>())
            {
                if (row.RowState == DataRowState.Deleted ||
                    row["id"] == DBNull.Value ||
                    row["account_id"] == DBNull.Value)
                    continue;

                int id = Convert.ToInt32(row["id"]);
                int accountId = Convert.ToInt32(row["account_id"]);
                accountsByLegacyId[prefix + id.ToString("000", CultureInfo.InvariantCulture)] = accountId;
            }
        }
        private const string emailAddressRegex = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + 
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
        /// <summary>
        /// Checks whether the email condition is true for the current value.
        /// </summary>
        public bool IsEmail(string candidate)
        {
            if (String.IsNullOrEmpty(candidate))
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(candidate, emailAddressRegex);
        }
        /// <summary>
        /// Creates a backup for the backup data in the selected target.
        /// </summary>
        public async Task BackupAsync(string filename)
        {
            DataSet dataset = new DataSet("clientFunds");
            DataTable table = new DataTable("Advisors");
            await FillAdapterAsync(SELECT.Advisors, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Clients");
            await FillAdapterAsync(SELECT.Clients, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Assistants");
            await FillAdapterAsync(SELECT.Emploees, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Books");
            await FillAdapterAsync(SELECT.Books, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Bank");
            await FillAdapterAsync(SELECT.Bank, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Barge");
            await FillAdapterAsync(SELECT.Cash, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("OfficeCash");
            await FillAdapterAsync(SELECT.OfficeCash, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Cash");
            await FillAdapterAsync(SELECT.Hardcash, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Receipts");
            await FillAdapterAsync(SELECT.Records, table, "");
            dataset.Tables.Add(table);
            table = new DataTable("Deadlines");
            await FillAdapterAsync(SELECT.Deadlines, table, "");
            dataset.Tables.Add(table);
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(DataSet));
                XmlSerializer.Serialize(fileStream, dataset);
            }
        }
        /// <summary>
        /// Creates the fixed Tables data or user interface element for the current workflow.
        /// </summary>
        protected abstract void CreateFixedTables(StringBuilder sb);
        /// <summary>
        /// Creates the user Tables data or user interface element for the current workflow.
        /// </summary>
        protected abstract Task CreateUserTablesAsync(StringBuilder sb);
        /// <summary>
        /// Creates the trigger data or user interface element for the current workflow.
        /// </summary>
        protected abstract Task CreateTriggerAsync();
        /// <summary>   
        /// Restores the restore data from the selected source.
        /// </summary>
        public virtual async Task RestoreAsync(string filename)
        {
            UpdateProgressText(Messages.sql_load_database + filename);
            DataSet dataset = new DataSet("clientFunds");
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(DataSet));
                dataset = (DataSet)XmlSerializer.Deserialize(fileStream);
            }
            DataTable table = new DataTable();
            int maximumRows = 0;
            for (int i = 0; i < dataset.Tables.Count; ++i)
                maximumRows += dataset.Tables[i].Rows.Count;
            UpdateMaximumProgress(maximumRows);
            UpdateProgressText(Messages.sql_transfer_bookings);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Books, table, "");
            await TransferDataAsync(SELECT.Books, dataset.Tables["Books"], table);
            UpdateProgressText(Messages.sql_transfer_bank);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Bank, table, "");
            await TransferDataAsync(SELECT.Bank, dataset.Tables["Bank"], table);
            UpdateProgressText(Messages.sql_transfer_cash);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Cash, table, "");
            await TransferDataAsync(SELECT.Cash, dataset.Tables["Barge"], table);
            UpdateProgressText(Messages.sql_transfer_officecash);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.OfficeCash, table, "");
            await TransferDataAsync(SELECT.OfficeCash, dataset.Tables["OfficeCash"], table);
            UpdateProgressText(Messages.sql_transfer_advsisors);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Advisors, table, "");
            await TransferDataAsync(SELECT.Advisors, dataset.Tables["Advisors"], table);
            UpdateProgressText(Messages.sql_transfer_clients);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Clients, table, "");
            await TransferDataAsync(SELECT.Clients, dataset.Tables["Clients"], table);
            UpdateProgressText(Messages.sql_transfer_assistants);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Emploees, table, "");
            await TransferDataAsync(SELECT.Emploees, dataset.Tables["Assistants"], table);
            await EnsureAccountIdsForClientsAndEmployeesAsync();
            await ApplyLegacyAccountIdsToCashAndBankBooksAsync();
            UpdateProgressText(Messages.sql_transfer_coins);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Hardcash, table, "");
            await TransferDataAsync(SELECT.Hardcash, dataset.Tables["Cash"], table);
            UpdateProgressText(Messages.sql_transfer_receipts);
            var table_belege = dataset.Tables["Receipts"];
            if (!table_belege.Columns.Contains("client_id"))
            {
                table_belege.Columns["id"].ColumnName = "client_id";
                table_belege.Columns["index"].ColumnName = "id";
                table_belege.Columns["id"].AutoIncrement = true;
                
            }
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Records, table, "");
            await TransferDataAsync(SELECT.Records, table_belege, table);
            UpdateProgressText(Messages.sql_transfer_deadlines);
            table = new DataTable();
            await FillAdapterAsync(SQLBase.SELECT.Deadlines, table, "");
            await TransferDataAsync(SELECT.Deadlines, dataset.Tables["Deadlines"], table);
        }
        /// <summary>
        /// Runs the transfer Data operation and updates the related application state.
        /// </summary>
        private async Task TransferDataAsync(SQLBase.SELECT select, DataTable from, DataTable to)
        {
            foreach (DataRow currentRow in from.Rows)
            {
                DataRow row = to.NewRow();

                foreach (DataColumn column in from.Columns)
                {
                    row[column.ColumnName] = currentRow[column.ColumnName];
                }
                to.Rows.Add(row);
            }
            await InsertTableAsync(select, to);
            UpdateProgress(to.Rows.Count, true);
        }
    }
}
