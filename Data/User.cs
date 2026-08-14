using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the user component used by the application.
    /// </summary>
    public class User
    {
        private bool admin, supervisor;
        private int access;
        private string handsign, login;
        public bool Admin { get { return admin; } }
        internal bool Supervisor { get { return supervisor; } }
        public int Access { get { return access; } }
        public string Handsign { get { return handsign; } }
        public string Login { get { return login; } }
        /// <summary>
        /// Creates a new User instance and initializes the required state.
        /// </summary>
        protected User() { supervisor = false; }
        public bool CanInsert
        {
            get
            {
                return admin | ((access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert);
            }
        }
        public bool CanModify
        {
            get 
            {
                return admin | ((access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change);
            }
        }
        public bool CanDelete
        {
            get
            {
                return admin;
            }
        }
        public bool CanBook
        {
            get
            {
                return admin | HasRight(Enums.UserRightEnum.Book);
            }
        }
        public bool CanCancelBooking
        {
            get
            {
                return admin | HasRight(Enums.UserRightEnum.CancelBooking);
            }
        }
        public bool CanAccessCashBalance
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.CashBalance);
            }
        }
        public bool CanAccessBankBalance
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.BankBalance);
            }
        }
        public bool CanAccessPettyCash
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.PettyCash);
            }
        }
        public bool CanAccessClients
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.Clients);
            }
        }
        public bool CanAccessRepresentatives
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.Representatives);
            }
        }
        public bool CanAccessEmployees
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.Employees);
            }
        }
        public bool CanAccessDocuments
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.Documents);
            }
        }
        public bool CanAccessCashAudit
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.CashAudit);
            }
        }
        public bool CanAccessStatistics
        {
            get
            {
                return admin | CanAccessArea(Enums.UserRightEnum.Statistics);
            }
        }
        /// <summary>
        /// Creates a user object from already authenticated data.
        /// </summary>
        internal User(string handsign, string login, int access, bool admin, bool supervisor)
        {
            this.handsign = handsign;
            this.login = login;
            this.access = access;
            this.admin = admin;
            this.supervisor = supervisor;
        }

        /// <summary>
        /// Updates the login data and refreshes the related application state.
        /// </summary>
        internal static async Task UpdateLogin(SQLBase sql, string oldLogin, string oldKeyword, string username, string login, string keyword)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            CheckUniqueNames(table, oldLogin, login, username);
            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => MatchesIdentity(userRow, username))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);
            DataRow row = rows.First();
            if (!PasswordMatches(oldKeyword, row[Columns.Password].ToString()))
                throw new Exception(Messages.login_invalid_password);
            row[Columns.Login] = login;
            row[Columns.Password] = PasswordHasher.Hash(keyword);
            ResetLoginThrottle(row);
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);
            await UserAuthenticator.LoginAsync(sql, username, keyword);
        }

        /// <summary>
        /// Updates only the password for the selected user.
        /// </summary>
        internal static async Task UpdatePassword(SQLBase sql, string username, string newKeyword, string login)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => MatchesIdentity(userRow, username))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);

            DataRow row = rows.First();
            row[Columns.Password] = PasswordHasher.Hash(newKeyword);
            ResetLoginThrottle(row);
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);

            await UserAuthenticator.LoginAsync(sql, login, newKeyword);
        }

        /// <summary>
        /// Updates the user data and refreshes the related application state.
        /// </summary>
        internal static async Task UpdateUser(SQLBase sql, string oldLogin, string handsign, string login, int access, bool admin)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            CheckUniqueNames(table, oldLogin, handsign, login);
    
            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => MatchesIdentity(userRow, oldLogin))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);
            DataRow row = rows.First();
            row[Columns.HandSign] = handsign;
            row[Columns.Login] = login;
            row[Columns.Access] = access;
            row[Columns.Admin] = admin == true ? 1 : 0;
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);
        }
        /// <summary>
        /// Creates the user data or user interface element for the current workflow.
        /// </summary>
        internal static async Task CreateUser(SQLBase sql, string handsign, string login, string keyword, int access, bool admin)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            CheckExistNames(table, handsign, login);
                
            DataRow row = table.NewRow();
            row[Columns.HandSign] = handsign;
            row[Columns.Login] = login;
            row[Columns.Password] = PasswordHasher.Hash(keyword);
            row[Columns.Access] = access;
            row[Columns.Admin] = admin;
            ResetLoginThrottle(row);
            table.Rows.Add(row);
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);
        }
        /// <summary>
        /// Checks the exist Names state and returns the result to the caller.
        /// </summary>
        internal static void CheckExistNames(DataTable table, params string[] names)
        {
            foreach (string name in names)
            {
                if (table.Rows.OfType<DataRow>().Any(userRow => MatchesUserNameOrLogin(userRow, name)))
                    throw new Exception(Messages.createUserFailed);
            }
        }
        /// <summary>
        /// Checks the unique Names state and returns the result to the caller.
        /// </summary>
        internal static void CheckUniqueNames(DataTable table, params string[] names)
        {
            HashSet<DataRow> uniqueRows = new HashSet<DataRow>();
            foreach (string currentName in names)
            {
                foreach (DataRow rw in table.Rows.OfType<DataRow>().Where(userRow => MatchesUserNameOrLogin(userRow, currentName)))
                    uniqueRows.Add(rw);
            }
            if (uniqueRows.Count > 1)
                throw new Exception(Messages.createUserFailed);
        }
        internal static bool MatchesIdentity(DataRow row, string identity)
        {
            return string.Equals(row[Columns.Login].ToString(), identity, StringComparison.Ordinal);
        }

        private static bool MatchesUserNameOrLogin(DataRow row, string value)
        {
            return string.Equals(row[Columns.HandSign].ToString(), value, StringComparison.Ordinal)
                || string.Equals(row[Columns.Login].ToString(), value, StringComparison.Ordinal);
        }

        private static bool PasswordMatches(string keyword, string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword))
                return string.IsNullOrEmpty(keyword);

            return PasswordHasher.Verify(keyword, storedPassword);
        }

        private static void ResetLoginThrottle(DataRow row)
        {
            if (!row.Table.Columns.Contains(Columns.FailedLoginAttempts)
                || !row.Table.Columns.Contains(Columns.LastFailedLogin)
                || !row.Table.Columns.Contains(Columns.LockedUntil))
                return;

            row[Columns.FailedLoginAttempts] = 0;
            row[Columns.LastFailedLogin] = DBNull.Value;
            row[Columns.LockedUntil] = DBNull.Value;
        }

        private bool HasRight(Enums.UserRightEnum right)
        {
            return (access & (int)right) == (int)right;
        }

        private bool CanAccessArea(Enums.UserRightEnum right)
        {
            return HasRight(right);
        }
    }
}
