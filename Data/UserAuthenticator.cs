using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Authenticates users and updates the current SQL session after a successful login.
    /// </summary>
    internal static class UserAuthenticator
    {
        private const int MaxFailedLoginAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Authenticates the supplied credentials and stores the authenticated user on the SQL session.
        /// </summary>
        internal static async Task<User> LoginAsync(SQLBase sql, string username, string keyword)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);

            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => User.MatchesIdentity(userRow, username))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);

            DataRow row = rows.First();
            DateTime now = DateTime.Now;
            if (IsLoginThrottleActive(row, now))
                throw new Exception(Messages.login_invalid_password);

            string storedPasswordHash = row[Columns.Password].ToString();
            if (!PasswordMatches(keyword, storedPasswordHash))
            {
                await RegisterFailedLoginAsync(sql, table, row, now);
                throw new Exception(Messages.login_invalid_password);
            }

            if (PasswordHasher.NeedsRehash(storedPasswordHash))
                row[Columns.Password] = PasswordHasher.Hash(keyword);
            ResetLoginThrottle(row);
            if (table.GetChanges() != null && !await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.datatable_update_failed);

            User user = new User(
                row[Columns.HandSign].ToString(),
                row[Columns.Login].ToString(),
                int.Parse(row[Columns.Access].ToString()),
                bool.Parse(row[Columns.Admin].ToString()),
                false);

            await sql.SetCurrentUserAsync(user);
            return user;
        }

        private static bool PasswordMatches(string keyword, string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(storedPasswordHash))
                return string.IsNullOrEmpty(keyword);

            return PasswordHasher.Verify(keyword, storedPasswordHash);
        }

        private static bool HasLoginThrottleColumns(DataTable table)
        {
            return table.Columns.Contains(Columns.FailedLoginAttempts)
                && table.Columns.Contains(Columns.LastFailedLogin)
                && table.Columns.Contains(Columns.LockedUntil);
        }

        private static bool IsLoginThrottleActive(DataRow row, DateTime now)
        {
            if (!HasLoginThrottleColumns(row.Table) || row[Columns.LockedUntil] == DBNull.Value)
                return false;

            DateTime lockedUntil = Convert.ToDateTime(row[Columns.LockedUntil]);
            if (lockedUntil <= now)
                return false;

            return true;
        }

        private static async Task RegisterFailedLoginAsync(SQLBase sql, DataTable table, DataRow row, DateTime now)
        {
            if (!HasLoginThrottleColumns(table))
                return;

            int failedAttempts = row[Columns.FailedLoginAttempts] == DBNull.Value
                ? 0
                : Convert.ToInt32(row[Columns.FailedLoginAttempts]);
            failedAttempts++;

            row[Columns.FailedLoginAttempts] = failedAttempts;
            row[Columns.LastFailedLogin] = now;
            if (failedAttempts >= MaxFailedLoginAttempts)
                row[Columns.LockedUntil] = now.Add(LockoutDuration);

            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                throw new Exception(Messages.datatable_update_failed);
        }

        private static void ResetLoginThrottle(DataRow row)
        {
            if (!HasLoginThrottleColumns(row.Table))
                return;

            row[Columns.FailedLoginAttempts] = 0;
            row[Columns.LastFailedLogin] = DBNull.Value;
            row[Columns.LockedUntil] = DBNull.Value;
        }
    }
}
