using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class UserManagerFormPresenterTests
    {
        [TestMethod]
        public async Task DeleteAsync_DoesNotDeleteLastAdmin()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddUser("Admin", "Admin", true);
            UserManagerFormView view = new UserManagerFormView();
            using (SqlSession session = new SqlSession())
            {
                session.Replace(sql);
                UserManagerFormPresenter presenter = new UserManagerFormPresenter(view, session);
                await presenter.ConnectTableToDataBaseAsync();
                view.SelectedUserRowValue = view.Users.Rows[0];

                await presenter.DeleteAsync();
            }

            Assert.AreEqual(1, sql.Users.Rows.Count);
            Assert.AreEqual(Messages.usermanagement_admin_delete, view.LastMessage);
            Assert.AreEqual(0, view.ConfirmUserDeleteCalls);
            Assert.AreEqual(0, view.ConfirmLastAdminDeleteCalls);
        }

        private sealed class UserManagerFormView : IUserManagerFormContract
        {
            public DataTable Users { get; private set; }
            public DataRow SelectedUserRowValue { get; set; }
            public string LastMessage { get; private set; }
            public int ConfirmUserDeleteCalls { get; private set; }
            public int ConfirmLastAdminDeleteCalls { get; private set; }

            public DataRow SelectedUserRow
            {
                get { return SelectedUserRowValue; }
            }

            public void BindUsers(DataTable table)
            {
                Users = table;
            }

            public void ClearUsers()
            {
                Users = null;
            }

            public void ShowAdministrationForm()
            {
            }

            public bool ShowCreateUserDialog(SqlSession session)
            {
                return false;
            }

            public bool ShowUpdateUserDialog(SqlSession session, DataRow row)
            {
                return false;
            }

            public void ShowUsersMissing()
            {
            }

            public bool ConfirmLastAdminDelete()
            {
                ConfirmLastAdminDeleteCalls++;
                return true;
            }

            public bool ConfirmUserDelete(string userName)
            {
                ConfirmUserDeleteCalls++;
                return true;
            }

            public void ShowUserDeleted()
            {
            }

            public void ShowUserNotDeleted()
            {
            }

            public void ShowDataTableUpdateFailed()
            {
            }

            public void ShowMessage(string msg)
            {
                LastMessage = msg;
            }

            public void ShowError(string msg)
            {
                LastMessage = msg;
            }

            public bool ConfirmMessage(string msg)
            {
                return true;
            }
        }

        private sealed class FakeSqlDatabase : SQLBase
        {
            public FakeSqlDatabase()
            {
                Users = new DataTable("users");
                Users.Columns.Add("handsign", typeof(string));
                Users.Columns.Add("login", typeof(string));
                Users.Columns.Add("pw", typeof(string));
                Users.Columns.Add("access", typeof(int));
                Users.Columns.Add("admin", typeof(bool));
            }

            public DataTable Users { get; private set; }

            public void AddUser(string handsign, string login, bool admin)
            {
                DataRow row = Users.NewRow();
                row["handsign"] = handsign;
                row["login"] = login;
                row["pw"] = string.Empty;
                row["access"] = 0;
                row["admin"] = admin;
                Users.Rows.Add(row);
                Users.AcceptChanges();
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table)
            {
                if (select != SELECT.Users)
                    throw new NotSupportedException(select.ToString());

                table.Clear();
                table.Columns.Clear();
                foreach (DataColumn column in Users.Columns)
                    table.Columns.Add(column.ColumnName, column.DataType);

                foreach (DataRow row in Users.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    DataRow copy = table.NewRow();
                    foreach (DataColumn column in Users.Columns)
                        copy[column.ColumnName] = row[column.ColumnName];
                    table.Rows.Add(copy);
                }
                table.AcceptChanges();
                return Task.CompletedTask;
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table, params object[] values)
            {
                return FillAdapterAsync(select, table);
            }

            public override Task<bool> UpdateAdapterAsync(SELECT select, DataTable table)
            {
                if (select != SELECT.Users)
                    throw new NotSupportedException(select.ToString());

                Users = table.Clone();
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    DataRow copy = Users.NewRow();
                    foreach (DataColumn column in Users.Columns)
                        copy[column.ColumnName] = row[column.ColumnName];
                    Users.Rows.Add(copy);
                }
                Users.AcceptChanges();
                return Task.FromResult(true);
            }

            protected override DbTransaction BeginDbTransaction()
            {
                throw new NotSupportedException();
            }

            protected override Task InsertTableAsync(SELECT select, DataTable to)
            {
                throw new NotSupportedException();
            }

            public override Task<bool> TestConnectionAsync(string host, string database, string username, string password)
            {
                return Task.FromResult(true);
            }

            public override Task ConnectAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task DropDatabaseAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task CreateDataBaseAsync(string host, string username, string password, string database)
            {
                return Task.CompletedTask;
            }

            public override Task CreateNewPasswordAsync(string host, string username, string password, string new_password)
            {
                return Task.CompletedTask;
            }

            public override Task UpdateAsync()
            {
                return Task.CompletedTask;
            }

            public override Task UpdateAsync(Version version)
            {
                return Task.CompletedTask;
            }

            public override int UpdateJournal(Enums.UpdateJournal param, DateTime date, string note, string changes = "")
            {
                return 0;
            }

            public override Task<int> UpdateDataBaseAsync(string command)
            {
                return Task.FromResult(0);
            }

            public override Task<object> CallFunctionsAsync(string name, params object[] values)
            {
                return Task.FromResult<object>(null);
            }

            protected override void CreateFixedTables(StringBuilder sb)
            {
            }

            protected override Task CreateUserTablesAsync(StringBuilder sb)
            {
                return Task.CompletedTask;
            }

            protected override Task CreateTriggerAsync()
            {
                return Task.CompletedTask;
            }
        }
    }
}
