using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class AdvisorFormPresenterTests
    {
        [TestMethod]
        public async Task DeleteAsync_DoesNotDeleteAdvisor_WhenClientReferencesAdvisor()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddAdvisor(1, "Referenced Advisor");
            sql.AddClient(10, 1);

            FakeAdvisorView view = new FakeAdvisorView { SelectedAdvisorId = 1 };
            AdvisorFormPresenter presenter = CreatePresenter(view, sql);

            await presenter.ConnectTableToDataBaseAsync();
            await presenter.DeleteAsync();

            Assert.AreEqual(1, sql.Advisors.Rows.Count);
            Assert.AreEqual(0, sql.RepresentativesUpdateCount);
            Assert.AreEqual(0, view.ConfirmMessageCount);
            Assert.AreEqual(Messages.advisor_delete_used_by_client, view.LastError);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesAdvisor_WhenNoClientReferencesAdvisor()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddAdvisor(1, "Unused Advisor");
            sql.AddClient(10, null);

            FakeAdvisorView view = new FakeAdvisorView { SelectedAdvisorId = 1, ConfirmResult = true };
            AdvisorFormPresenter presenter = CreatePresenter(view, sql);

            await presenter.ConnectTableToDataBaseAsync();
            await presenter.DeleteAsync();

            Assert.AreEqual(0, sql.Advisors.Rows.Count);
            Assert.AreEqual(1, sql.RepresentativesUpdateCount);
            Assert.AreEqual(1, view.ConfirmMessageCount);
            Assert.IsNull(view.LastError);
        }

        private static AdvisorFormPresenter CreatePresenter(FakeAdvisorView view, FakeSqlDatabase sql)
        {
            SqlSession session = new SqlSession();
            session.Replace(sql);
            return new AdvisorFormPresenter(view, session);
        }

        private sealed class FakeAdvisorView : IAdvisorFormContract
        {
            public string DefaultSortColumn { get { return Columns.Id; } }
            public string CurrentSortColumn { get { return null; } }
            public bool ChangeButtonEnabled { get { return true; } }
            public int SelectedAdvisorPosition { get { return 0; } }
            public int? SelectedAdvisorId { get; set; }
            public bool ConfirmResult { get; set; }
            public int ConfirmMessageCount { get; private set; }
            public string LastError { get; private set; }

            public void BindAdvisors(DataView advisors) { }
            public void ClearAdvisors() { }
            public void BindAdvisorDate(DataView advisors) { }
            public bool ShowCreateAdvisorDialog(DataTable table) { return false; }
            public bool ShowChangeAdvisorDialog(DataTable table, int position) { return false; }
            public void PrintAdvisors(DataRow[] advisors) { }
            public void ShowMainForm() { }
            public void ShowMessage(string msg) { }
            public void ShowError(string msg) { LastError = msg; }
            public bool ConfirmMessage(string msg)
            {
                ConfirmMessageCount++;
                return ConfirmResult;
            }
        }

        private sealed class FakeSqlDatabase : SQLBase
        {
            public FakeSqlDatabase()
            {
                Advisors = CreateAdvisorsTable();
                Clients = CreateClientsTable();
            }

            public DataTable Advisors { get; private set; }
            public DataTable Clients { get; private set; }
            public int RepresentativesUpdateCount { get; private set; }

            public void AddAdvisor(int id, string name)
            {
                DataRow row = Advisors.NewRow();
                row[Columns.Id] = id;
                row[Columns.Name] = name;
                Advisors.Rows.Add(row);
                Advisors.AcceptChanges();
            }

            public void AddClient(int id, int? advisorId)
            {
                DataRow row = Clients.NewRow();
                row[Columns.Id] = id;
                row[Columns.AdvisorId] = advisorId.HasValue ? (object)advisorId.Value : DBNull.Value;
                Clients.Rows.Add(row);
                Clients.AcceptChanges();
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table)
            {
                FillTable(table, SourceTable(select));
                return Task.CompletedTask;
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table, params object[] values)
            {
                FillTable(table, SourceTable(select));
                return Task.CompletedTask;
            }

            public override Task<bool> UpdateAdapterAsync(SELECT select, DataTable table)
            {
                if (select == SELECT.Representatives)
                {
                    RepresentativesUpdateCount++;
                    Advisors = CopyWithoutDeletedRows(table);
                    return Task.FromResult(true);
                }

                if (select == SELECT.Clients)
                {
                    Clients = CopyWithoutDeletedRows(table);
                    return Task.FromResult(true);
                }

                return Task.FromResult(true);
            }

            private DataTable SourceTable(SELECT select)
            {
                if (select == SELECT.Representatives)
                    return Advisors;
                if (select == SELECT.Clients)
                    return Clients;
                return new DataTable();
            }

            private static DataTable CreateAdvisorsTable()
            {
                DataTable table = new DataTable("advisors");
                table.Columns.Add(Columns.Id, typeof(int));
                table.Columns.Add(Columns.Name, typeof(string));
                table.PrimaryKey = new[] { table.Columns[Columns.Id] };
                return table;
            }

            private static DataTable CreateClientsTable()
            {
                DataTable table = new DataTable("clients");
                table.Columns.Add(Columns.Id, typeof(int));
                table.Columns.Add(Columns.AdvisorId, typeof(int));
                table.PrimaryKey = new[] { table.Columns[Columns.Id] };
                return table;
            }

            private static void FillTable(DataTable target, DataTable source)
            {
                target.Clear();
                target.Columns.Clear();

                foreach (DataColumn column in source.Columns)
                    target.Columns.Add(column.ColumnName, column.DataType);

                foreach (DataRow row in source.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    target.Rows.Add(row.ItemArray);
                }

                target.AcceptChanges();
            }

            private static DataTable CopyWithoutDeletedRows(DataTable source)
            {
                DataTable copy = source.Clone();
                foreach (DataRow row in source.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    copy.Rows.Add(row.ItemArray);
                }

                copy.AcceptChanges();
                return copy;
            }

            protected override DbTransaction BeginDbTransaction() { throw new NotSupportedException(); }
            public override Task<bool> TestConnectionAsync(string host, string database, string username, string password) { return Task.FromResult(true); }
            public override Task ConnectAsync(string host, string username, string password, string database) { return Task.CompletedTask; }
            public override Task DropDatabaseAsync(string host, string username, string password, string database) { return Task.CompletedTask; }
            public override Task CreateDataBaseAsync(string host, string username, string password, string database) { return Task.CompletedTask; }
            public override Task CreateNewPasswordAsync(string host, string username, string password, string newPassword) { return Task.CompletedTask; }
            public override Task<object> CallFunctionsAsync(string name, params object[] values) { return Task.FromResult<object>(null); }
            public override Task UpdateAsync() { return Task.CompletedTask; }
            public override Task UpdateAsync(Version version) { return Task.CompletedTask; }
            protected override Task InsertTableAsync(SELECT select, DataTable to) { return Task.CompletedTask; }
            public override int UpdateJournal(Enums.UpdateJournal param, DateTime date, string note, string changes = "") { return 0; }
            public override Task<int> UpdateDataBaseAsync(string command) { return Task.FromResult(0); }
            protected override void CreateFixedTables(StringBuilder sb) { }
            protected override Task CreateUserTablesAsync(StringBuilder sb) { return Task.CompletedTask; }
            protected override Task CreateTriggerAsync() { return Task.CompletedTask; }
        }
    }
}
