using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class UserManagerFormPresenter
    {
        public SqlSession session { get; private set; }

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private readonly DataTable table = new DataTable();

        public UserManagerFormPresenter(IUserManagerFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            View = view;
            this.session = session;
        }

        protected IUserManagerFormContract View { get; private set; }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            View.ClearUsers();
            table.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Users, table);
            table.PrimaryKey = new DataColumn[] { table.Columns[Columns.Name] };
            table.CaseSensitive = true;
            View.BindUsers(table);
        }

        public virtual async Task EnterAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual void Back()
        {
            View.ShowAdministrationForm();
        }

        public virtual async Task SaveAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Users, table);
                if (!valid)
                {
                    table.RejectChanges();
                    View.ShowDataTableUpdateFailed();
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task CreateAsync()
        {
            if (!View.ShowCreateUserDialog(session))
                return;

            await ConnectTableToDataBaseAsync();
        }

        private DataRow GetSelectedRow()
        {
            return View.SelectedUserRow;
        }

        public virtual async Task UpdateAsync()
        {
            DataRow row = GetSelectedRow();
            if (row == null)
            {
                View.ShowUsersMissing();
                return;
            }

            if (!View.ShowUpdateUserDialog(session, row))
                return;

            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task DeleteAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                DataRow row = GetSelectedRow();
                if (row == null)
                {
                    View.ShowUsersMissing();
                    return;
                }

                if ((bool)row[Columns.Admin] && table.Select(Columns.Admin + "=true").Count() == 1)
                {
                    if (!View.ConfirmLastAdminDelete())
                        return;
                }

                if (!View.ConfirmUserDelete(row[Columns.Name].ToString()))
                    return;

                row.Delete();
                bool value = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Users, table);
                if (value)
                {
                    table.AcceptChanges();
                    View.ShowUserDeleted();
                    View.BindUsers(table);
                }
                else
                {
                    table.RejectChanges();
                    View.ShowUserNotDeleted();
                }
            }
            catch
            {
                table.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
    }
}
