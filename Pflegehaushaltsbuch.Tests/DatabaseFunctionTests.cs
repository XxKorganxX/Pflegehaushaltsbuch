using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class DatabaseFunctionTests
    {
        [TestMethod]
        public async Task ToBargeAsyncAddsCashBooking()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();

            bool saved = await sql.ToBargeAsync(new DateTime(2026, 8, 1), "Cash in", 12.50m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand);

            Assert.IsTrue(saved);
            DataRow row = sql.Table(SQLBase.SELECT.Barge).Rows[0];
            Assert.AreEqual(new DateTime(2026, 8, 1), row["date"]);
            Assert.AreEqual("Cash in", row["note"]);
            Assert.AreEqual(12.50m, row["amount"]);
            Assert.AreEqual("K001", row["account"]);
            Assert.AreEqual(SQLBase.BookCategory.Einzahlung, row["book_cat"]);
            Assert.AreEqual(SQLBase.BookingTo.Barbestand, row["book_to"]);
            Assert.AreEqual("Test User", row["handsign"]);
        }

        [TestMethod]
        public async Task ToBankAsyncAddsBankBooking()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();

            bool saved = await sql.ToBankAsync(new DateTime(2026, 8, 1), "Bank in", 99m, "K002", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);

            Assert.IsTrue(saved);
            DataRow row = sql.Table(SQLBase.SELECT.Bank).Rows[0];
            Assert.AreEqual("Bank in", row["note"]);
            Assert.AreEqual(99m, row["amount"]);
            Assert.AreEqual("K002", row["account"]);
            Assert.AreEqual(SQLBase.BookingTo.Bankbestand, row["book_to"]);
        }

        [TestMethod]
        public async Task ToBooksAsyncAddsClientBookingAndRenumbersDocuments()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddBook(7, new DateTime(2026, 8, 3), "Existing", 4m, 1);

            var result = await sql.ToBooksAsync("Client", 7, new DateTime(2026, 8, 1), "New", 10m, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);

            Assert.IsTrue(result.Item1);
            DataRow[] rows = sql.Table(SQLBase.SELECT.Books).Select("id = 7", "date");
            Assert.AreEqual(2, rows.Length);
            Assert.AreEqual("New", rows[0]["note"]);
            Assert.AreEqual(1, rows[0]["document_id"]);
            Assert.AreEqual("Existing", rows[1]["note"]);
            Assert.AreEqual(2, rows[1]["document_id"]);
        }

        [TestMethod]
        public async Task Book2CashOfficeAsyncStoresSignedAmountByCategory()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();

            var payout = await sql.Book2CashOfficeAsync(new DateTime(2026, 8, 1), "Office payout", 25m, SQLBase.BookCategory.Auszahlung, 3);
            var deposit = await sql.Book2CashOfficeAsync(new DateTime(2026, 8, 2), "Office deposit", 30m, SQLBase.BookCategory.Einzahlung, 3);

            Assert.IsTrue(payout.Item1);
            Assert.IsTrue(deposit.Item1);
            DataRow[] rows = sql.Table(SQLBase.SELECT.OfficeCash).Select("", "date");
            Assert.AreEqual(-25m, rows[0]["amount"]);
            Assert.AreEqual(30m, rows[1]["amount"]);
        }

        [TestMethod]
        public async Task UpdateAsistanceAsyncUpdatesRepaymentAndClosesPaidLoan()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.AddAssistant(12, "Assistant", 100m, 0m, 0, true);

            bool saved = await sql.UpdateAsistanceAsync("Assistant", new DateTime(2026, 8, 1), 100m, 2);

            Assert.IsTrue(saved);
            DataRow row = sql.Table(SQLBase.SELECT.Assistants).Rows[0];
            Assert.AreEqual(0m, row["amount_payout"]);
            Assert.AreEqual(100m, row["amount_payback"]);
            Assert.AreEqual(2, row["amount_payback_type"]);
            Assert.AreEqual(false, row["active"]);
            Assert.AreEqual(0m, row["account_transfer"]);
        }

        [TestMethod]
        public async Task TransactionCommitKeepsAllTableChanges()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();

            using (var transaction = sql.BeginTransaction())
            {
                Assert.IsTrue(await sql.ToBankAsync(new DateTime(2026, 8, 1), "Bank", 10m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand));
                Assert.IsTrue(await sql.ToBargeAsync(new DateTime(2026, 8, 1), "Cash", 20m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand));
                transaction.Commit();
            }

            Assert.AreEqual(1, sql.Table(SQLBase.SELECT.Bank).Rows.Count);
            Assert.AreEqual(1, sql.Table(SQLBase.SELECT.Barge).Rows.Count);
        }

        [TestMethod]
        public async Task TransactionRollbackDiscardsAllTableChangesWhenSecondUpdateFails()
        {
            FakeSqlDatabase sql = new FakeSqlDatabase();
            sql.FailUpdatesFor(SQLBase.SELECT.Barge);

            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                using (var transaction = sql.BeginTransaction())
                {
                    Assert.IsTrue(sql.ToBankAsync(new DateTime(2026, 8, 1), "Bank", 10m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand).GetAwaiter().GetResult());
                    if (!sql.ToBargeAsync(new DateTime(2026, 8, 1), "Cash", 20m, "K001", SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Simulated write failure.");
                    transaction.Commit();
                }
            });

            Assert.AreEqual(0, sql.Table(SQLBase.SELECT.Bank).Rows.Count);
            Assert.AreEqual(0, sql.Table(SQLBase.SELECT.Barge).Rows.Count);
        }

        private sealed class FakeSqlDatabase : SQLBase
        {
            private readonly Dictionary<SELECT, DataTable> tables = new Dictionary<SELECT, DataTable>();
            private readonly HashSet<SELECT> failingUpdates = new HashSet<SELECT>();
            private FakeTransaction activeTransaction;

            public FakeSqlDatabase()
            {
                tables[SELECT.Barge] = CreateBookingTable("barge", includeAccount: true);
                tables[SELECT.Bank] = CreateBookingTable("bank", includeAccount: true);
                tables[SELECT.Books] = CreateBookingTable("books", includeAccount: false);
                tables[SELECT.OfficeCash] = CreateBookingTable("office_cash", includeAccount: true);
                tables[SELECT.Assistants] = CreateAssistantsTable();
                SetTestUser();
            }

            public DataTable Table(SELECT select)
            {
                return tables[Normalize(select)];
            }

            public void FailUpdatesFor(SELECT select)
            {
                failingUpdates.Add(Normalize(select));
            }

            public void AddBook(int clientId, DateTime date, string note, decimal amount, int documentId)
            {
                DataRow row = tables[SELECT.Books].NewRow();
                row["id"] = clientId;
                row["date"] = date;
                row["note"] = note;
                row["amount"] = amount;
                row["document_id"] = documentId;
                row["book_cat"] = SQLBase.BookCategory.Einzahlung;
                row["book_to"] = SQLBase.BookingTo.Bankbestand;
                row["handsign"] = "Initial";
                tables[SELECT.Books].Rows.Add(row);
                tables[SELECT.Books].AcceptChanges();
            }

            public void AddAssistant(int id, string name, decimal amountPayout, decimal amountPayback, int repaymentType, bool active)
            {
                DataRow row = tables[SELECT.Assistants].NewRow();
                row["id"] = id;
                row["name"] = name;
                row["amount_payout"] = amountPayout;
                row["amount_payback"] = amountPayback;
                row["amount_payback_type"] = repaymentType;
                row["account_transfer"] = amountPayout;
                row["active"] = active;
                row["handsign"] = "Initial";
                tables[SELECT.Assistants].Rows.Add(row);
                tables[SELECT.Assistants].AcceptChanges();
            }

            protected override DbTransaction BeginDbTransaction()
            {
                activeTransaction = new FakeTransaction(this, CloneTables(tables));
                return activeTransaction;
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table)
            {
                FillTable(table, CurrentTables()[Normalize(select)]);
                return Task.CompletedTask;
            }

            public override Task FillAdapterAsync(SELECT select, DataTable table, params object[] values)
            {
                DataTable source = CurrentTables()[Normalize(select)];
                DataTable filtered = source.Clone();

                if ((select == SELECT.Book || select == SELECT.BooksByPeriod) && values.Length > 0)
                    CopyRows(source.Select("id = " + Convert.ToInt32(values[0], CultureInfo.InvariantCulture)), filtered);
                else if (select == SELECT.Assistant && values.Length > 0)
                    CopyRows(source.Select("id = " + Convert.ToInt32(values[0], CultureInfo.InvariantCulture)), filtered);
                else
                    CopyRows(source.Select(), filtered);

                FillTable(table, filtered);
                return Task.CompletedTask;
            }

            public override Task<bool> UpdateAdapterAsync(SELECT select, DataTable table)
            {
                SELECT normalized = Normalize(select);
                if (failingUpdates.Contains(normalized))
                    return Task.FromResult(false);

                if (table.GetChanges() == null)
                    return Task.FromResult(true);

                CurrentTables()[normalized] = CopyWithoutDeletedRows(table);
                return Task.FromResult(true);
            }

            protected override Task InsertTableAsync(SELECT select, DataTable to)
            {
                CurrentTables()[Normalize(select)] = CopyWithoutDeletedRows(to);
                return Task.CompletedTask;
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

            private Dictionary<SELECT, DataTable> CurrentTables()
            {
                return activeTransaction == null ? tables : activeTransaction.WorkingTables;
            }

            private void Commit(FakeTransaction transaction)
            {
                foreach (var item in transaction.WorkingTables)
                    tables[item.Key] = item.Value;
                activeTransaction = null;
            }

            private void Rollback(FakeTransaction transaction)
            {
                if (activeTransaction == transaction)
                    activeTransaction = null;
            }

            private void SetTestUser()
            {
                Type userType = typeof(SQLBase).Assembly.GetType("Pflegehaushaltsbuch.Data.User");
                object user = Activator.CreateInstance(
                    userType,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    binder: null,
                    args: new object[] { "Test User", string.Empty, string.Empty, "test@example.invalid", 0, true, true },
                    culture: CultureInfo.InvariantCulture);

                typeof(SQLBase)
                    .GetProperty("User", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(this, user, null);
            }

            private static SELECT Normalize(SELECT select)
            {
                if (select == SELECT.Book || select == SELECT.BooksByPeriod || select == SELECT.BooksByUser)
                    return SELECT.Books;
                if (select == SELECT.Assistant)
                    return SELECT.Assistants;
                if (select == SELECT.BargeFromMonth || select == SELECT.BargeByPeriod)
                    return SELECT.Barge;
                if (select == SELECT.BankByDate || select == SELECT.BankByPeriod)
                    return SELECT.Bank;
                if (select == SELECT.OfficeCashByDate || select == SELECT.OfficeByPeriod)
                    return SELECT.OfficeCash;
                return select;
            }

            private static DataTable CreateBookingTable(string name, bool includeAccount)
            {
                DataTable table = new DataTable(name);
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("date", typeof(DateTime));
                table.Columns.Add("note", typeof(string));
                table.Columns.Add("book_cat", typeof(object));
                table.Columns.Add("amount", typeof(decimal));
                table.Columns.Add("handsign", typeof(string));
                table.Columns.Add("document_id", typeof(int));
                table.Columns.Add("book_to", typeof(object));
                if (includeAccount)
                    table.Columns.Add("account", typeof(object));
                return table;
            }

            private static DataTable CreateAssistantsTable()
            {
                DataTable table = new DataTable("assistants");
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("name", typeof(string));
                table.Columns.Add("amount_payout", typeof(decimal));
                table.Columns.Add("amount_payback", typeof(decimal));
                table.Columns.Add("amount_payback_type", typeof(int));
                table.Columns.Add("account_transfer", typeof(decimal));
                table.Columns.Add("active", typeof(bool));
                table.Columns.Add("handsign", typeof(string));
                return table;
            }

            private static Dictionary<SELECT, DataTable> CloneTables(Dictionary<SELECT, DataTable> source)
            {
                return source.ToDictionary(item => item.Key, item => item.Value.Copy());
            }

            private static void FillTable(DataTable target, DataTable source)
            {
                target.Clear();
                target.Columns.Clear();
                foreach (DataColumn column in source.Columns)
                    target.Columns.Add(column.ColumnName, column.DataType);
                CopyRows(source.Select(), target);
                target.AcceptChanges();
            }

            private static void CopyRows(IEnumerable<DataRow> sourceRows, DataTable target)
            {
                foreach (DataRow row in sourceRows)
                {
                    DataRow newRow = target.NewRow();
                    foreach (DataColumn column in target.Columns)
                        newRow[column.ColumnName] = row[column.ColumnName, DataRowVersion.Current];
                    target.Rows.Add(newRow);
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

                    DataRow newRow = copy.NewRow();
                    foreach (DataColumn column in copy.Columns)
                        newRow[column.ColumnName] = row[column.ColumnName];
                    copy.Rows.Add(newRow);
                }
                copy.AcceptChanges();
                return copy;
            }

            private sealed class FakeTransaction : DbTransaction
            {
                private readonly FakeSqlDatabase owner;
                private bool completed;

                public FakeTransaction(FakeSqlDatabase owner, Dictionary<SELECT, DataTable> workingTables)
                {
                    this.owner = owner;
                    WorkingTables = workingTables;
                }

                public Dictionary<SELECT, DataTable> WorkingTables { get; }

                public override IsolationLevel IsolationLevel
                {
                    get { return IsolationLevel.Unspecified; }
                }

                protected override DbConnection DbConnection
                {
                    get { return null; }
                }

                public override void Commit()
                {
                    completed = true;
                    owner.Commit(this);
                }

                public override void Rollback()
                {
                    completed = true;
                    owner.Rollback(this);
                }

                protected override void Dispose(bool disposing)
                {
                    if (!completed)
                        Rollback();
                    base.Dispose(disposing);
                }
            }
        }
    }
}
