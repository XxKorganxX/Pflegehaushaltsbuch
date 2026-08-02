using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the user component used by the application.
    /// </summary>
    internal class User
    {
        private bool admin, supervisor;
        private int access;
        private string name, phone, fax, email;
        public bool Admin { get { return admin; } }
        internal bool Supervisor { get { return supervisor; } }
        public int Access { get { return access; } }
        public string Name { get { return name; } }
        public string Phone { get { return phone; } }
        public string Fax { get { return fax; } }
        public string Email { get { return email; } }
        /// <summary>
        /// Creates a new User instance and initializes the required state.
        /// </summary>
        protected User() { supervisor = false; }
        public bool CanInsert
        {
            get
            {
                return admin | Supervisor | ((access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert);
            }
        }
        public bool CanModify
        {
            get 
            {
                return admin | Supervisor | ((access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change);
            }
        }
        public bool CanDelete
        {
            get
            {
                return admin | Supervisor | ((access & (int)Enums.UserRightEnum.Delete) == (int)Enums.UserRightEnum.Delete);
            }
        }
        /// <summary>
        /// Creates a user object from already authenticated data.
        /// </summary>
        internal User(string name, string phone, string fax, string email, int access, bool admin, bool supervisor)
        {
            this.name = name;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
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
            if (!PasswordHasher.Verify(oldKeyword, row[SQLBase.Names(SQLBase.ColumnNames.pw)].ToString()))
                throw new Exception(Messages.login_invalid_password);
            row[SQLBase.Names(SQLBase.ColumnNames.login)] = login;
            row[SQLBase.Names(SQLBase.ColumnNames.pw)] = PasswordHasher.Hash(keyword);
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);
            await UserAuthenticator.LoginAsync(sql, username, keyword);
        }
        /// <summary>
        /// Updates the user data and refreshes the related application state.
        /// </summary>
        internal static async Task UpdateUser(SQLBase sql, string oldLogin, string name, string login, string phone, string fax, string email, int access, bool admin)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            CheckUniqueNames(table, oldLogin, name, email);
    
            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => MatchesIdentity(userRow, oldLogin))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);
            DataRow row = rows.First();
            row[SQLBase.Names(SQLBase.ColumnNames.name)] = name;
            row[SQLBase.Names(SQLBase.ColumnNames.login)] = login;
            row[SQLBase.Names(SQLBase.ColumnNames.phone)] = phone;
            row[SQLBase.Names(SQLBase.ColumnNames.fax)] = fax;
            row[SQLBase.Names(SQLBase.ColumnNames.email)] = email;
            row[SQLBase.Names(SQLBase.ColumnNames.access)] = access;
            row[SQLBase.Names(SQLBase.ColumnNames.admin)] = admin == true ? 1 : 0;
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.createUserFailed);
        }
        /// <summary>
        /// Creates the user data or user interface element for the current workflow.
        /// </summary>
        internal static async Task CreateUser(SQLBase sql, string name, string login, string keyword, string phone, string fax, string email, int access, bool admin)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            //CheckExistNames(table, name, login, email);
                
            DataRow row = table.NewRow();
            row[SQLBase.Names(SQLBase.ColumnNames.name)] = name;
            row[SQLBase.Names(SQLBase.ColumnNames.login)] = login;
            row[SQLBase.Names(SQLBase.ColumnNames.pw)] = PasswordHasher.Hash(keyword);
            row[SQLBase.Names(SQLBase.ColumnNames.phone)] = phone;
            row[SQLBase.Names(SQLBase.ColumnNames.fax)] = fax;
            row[SQLBase.Names(SQLBase.ColumnNames.email)] = email;
            row[SQLBase.Names(SQLBase.ColumnNames.access)] = access;
            row[SQLBase.Names(SQLBase.ColumnNames.admin)] = admin;
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
                if (table.Rows.OfType<DataRow>().Any(userRow => MatchesIdentity(userRow, name)))
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
                foreach (DataRow rw in table.Rows.OfType<DataRow>().Where(userRow => MatchesIdentity(userRow, currentName)))
                    uniqueRows.Add(rw);
            }
            if (uniqueRows.Count > 1)
                throw new Exception(Messages.createUserFailed);
        }
        internal static bool MatchesIdentity(DataRow row, string identity)
        {
            return string.Equals(row[SQLBase.Names(SQLBase.ColumnNames.name)].ToString(), identity, StringComparison.Ordinal)
                || string.Equals(row[SQLBase.Names(SQLBase.ColumnNames.login)].ToString(), identity, StringComparison.Ordinal)
                || string.Equals(row[SQLBase.Names(SQLBase.ColumnNames.email)].ToString(), identity, StringComparison.Ordinal);
        }
    }
}
