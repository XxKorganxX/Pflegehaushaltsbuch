using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class AdvisorFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private DataTable table;

        public AdvisorFormPresenter(IAdvisorFormContract view, SqlSession session)
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

        protected IAdvisorFormContract View { get; private set; }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            View.ClearAdvisors();
            table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Advisors, table);
            table.PrimaryKey = new DataColumn[] { table.Columns[Columns.Id] };

            if (!string.IsNullOrWhiteSpace(View.CurrentSortColumn))
                table.DefaultView.Sort = View.CurrentSortColumn;
            else
                table.DefaultView.Sort = View.DefaultSortColumn;

            View.BindAdvisors(table.DefaultView);
            View.BindAdvisorDate(table.DefaultView);
        }

        public virtual void DisconnectTable()
        {
            View.ClearAdvisors();
            table?.Clear();
        }

        public virtual async Task ChangeAdvisorAsync()
        {
            if (table == null)
                return;

            int position = View.SelectedAdvisorPosition;
            if (position < 0)
                return;

            if (!View.ShowChangeAdvisorDialog(table, position))
                return;

            if (await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table))
                View.ShowMessage(Messages.advisor_created_changed);
            else
                View.ShowError(Messages.advisor_changed_failed);
        }

        public virtual async Task CreateAccountAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (!View.ShowCreateAdvisorDialog(table))
                    return;

                if (await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table))
                    View.ShowMessage(Messages.advisor_created_changed);
                else
                    View.ShowError(Messages.advisor_changed_failed);
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Back()
        {
            View.ShowMainForm();
        }

        public virtual void Print()
        {
            if (table == null)
                return;

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            DataRow[] rows = table.Select("", "date");
            View.PrintAdvisors(rows);
        }

        public virtual async Task DeleteAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                int? advisorId = View.SelectedAdvisorId;
                if (!advisorId.HasValue)
                    return;

                DataRow row = table.Rows.Find(advisorId.Value);
                if (row == null)
                    return;

                if (!View.ConfirmMessage(Messages.advisor_delete))
                    return;

                View.ClearAdvisors();
                row.Delete();
                bool value = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table);
                if (!value)
                {
                    table.RejectChanges();
                    View.ShowError(Messages.advisor_delete_failed);
                }

                View.BindAdvisors(table.DefaultView);
                View.BindAdvisorDate(table.DefaultView);
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

        public virtual async Task ChangeSelectedAdvisorAsync(int rowIndex)
        {
            if (rowIndex < 0)
                return;

            if (View.ChangeButtonEnabled)
                await ChangeAdvisorAsync();
        }

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Advisors, table);
                if (!valid)
                {
                    table.RejectChanges();
                    View.ShowError(Messages.datatable_update_failed);
                }
                else
                {
                    await ConnectTableToDataBaseAsync();
                    View.ShowMessage(Messages.datatable_updated);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
    }
}
