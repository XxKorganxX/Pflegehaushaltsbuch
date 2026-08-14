using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class EmployeesFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private DataTable table;
        private string totalAmountText;

        public EmployeesFormPresenter(IEmployeesFormContract view, SqlSession session)
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

        protected IEmployeesFormContract View { get; private set; }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            View.ClearEmployees();
            table = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, table);
            table.PrimaryKey = new DataColumn[] { table.Columns[Columns.Id] };
            UpdateTotalAmount();

            if (!string.IsNullOrWhiteSpace(View.CurrentSortColumn))
                table.DefaultView.Sort = View.CurrentSortColumn;
            else
                table.DefaultView.Sort = View.DefaultSortColumn;

            View.BindEmployees(table.DefaultView);
            View.BindEmployeeDate(table.DefaultView);
        }

        public virtual void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row[Columns.AmountPayout] != DBNull.Value)
                        totalAmount += Convert.ToDecimal(row[Columns.AmountPayout]);
                }
            }

            totalAmountText = totalAmount.ToString("C", session.Company.Currencies);
            View.SetTotalAmount(totalAmountText);
        }

        public virtual async Task CreateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                AssistantInput input;
                if (!View.ShowCreateAssistantDialog(session.SQL.GetID(table), out input))
                    return;

                    try
                    {
                        DataRow row = table.NewRow();
                        row[Columns.Id] = input.ID;
                        row[Columns.Name] = input.AssistantName;
                        row[Columns.AmountPayout] = input.Amount;
                        row[Columns.AccountTransfer] = 0;
                        row[Columns.AmountPayback] = 0;
                        row[Columns.AmountPaybackType] = 0;
                        row[Columns.Date] = input.Date.Date;
                        row[Columns.Active] = true;
                        row[Columns.HandSign] = session.SQL.User.Handsign;
                        table.Rows.Add(row);

                        bool valid = false;
                        using (var transaction = session.SQL.BeginTransaction())
                        {
                            try
                            {
                                int accountId = -1;
                                if (table.Columns.Contains(Columns.AccountId))
                                {
                                    accountId = await session.SQL.CreateAccountIdAsync("Employee");
                                    row[Columns.AccountId] = accountId;
                                }

                                valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Emploees, table);
                                if (valid && input.Amount != 0)
                                    valid = await session.SQL.ToBargeAsync(input.Date, string.Format(Messages.ioan_to, input.AssistantName), -Math.Abs(input.Amount), accountId, SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand);
                                if (!valid)
                                    throw new Exception(Messages.assistants_created_failed);
                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }

                        if (valid)
                        {
                            await ConnectTableToDataBaseAsync();
                            View.ShowMessage(Messages.assistants_created);
                        }
                        else
                        {
                            table?.RejectChanges();
                            View.ShowError(Messages.assistants_created_failed);
                        }
                    }
                    catch
                    {
                        table?.RejectChanges();
                        throw;
                    }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task DeleteAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                DataRow rowOfView = GetSelectedRow();
                if (rowOfView == null)
                    return;

                int id = Int32.Parse(rowOfView[Columns.Id].ToString());
                if ((decimal)rowOfView[Columns.AmountPayout] != 0)
                {
                    View.ShowMessage(string.Format(Messages.asistants_not_deleteable, View.SelectedAssistantName));
                    return;
                }

                if (!View.ConfirmMessage(string.Format(Messages.assistants_delete, View.SelectedAssistantName)))
                    return;

                DataTable bankTable = new DataTable(), bargeTable = new DataTable(), assistantsTable = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Bank, bankTable);
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Cash, bargeTable);
                int accountId = Convert.ToInt32(rowOfView[Columns.AccountId]);

                foreach (DataRow row in bargeTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row[Columns.AccountId] == DBNull.Value)
                        throw new Exception(Messages.assistants_not_deleteable_book);
                    if (Convert.ToInt32(row[Columns.AccountId]) == accountId)
                        row.Delete();
                }

                foreach (DataRow row in bankTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row[Columns.AccountId] == DBNull.Value)
                        throw new Exception(Messages.assistants_not_deleteable_book);
                    if (Convert.ToInt32(row[Columns.AccountId]) == accountId)
                        row.Delete();
                }

                using (var transaction = session.SQL.BeginTransaction())
                {
                    try
                    {
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Cash, bargeTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Bank, bankTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        await session.SQL.FillAdapterAsync(SQLBase.SELECT.Assistant, assistantsTable, id);
                        if (assistantsTable.Rows.Count == 0)
                            throw new Exception(Messages.assistant_not_found);
                        assistantsTable.Rows[0].Delete();
                        if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Assistant, assistantsTable))
                            throw new Exception(Messages.assistants_changed_failed);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                if (rowOfView.Table == table)
                {
                    table.Rows.Remove(rowOfView);
                    UpdateTotalAmount();
                }
                else
                {
                    await ConnectTableToDataBaseAsync();
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task PayOutAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                DataRow row = GetSelectedRow();
                if (row == null)
                    return;

                if (Convert.ToDecimal(row[Columns.AmountPayout]) == 0)
                    throw new Exception(Messages.ioan_repaid_needless);

                AssistantPaybackInput input;
                if (!View.ShowIoanPaybackDialog(row[Columns.Name].ToString(), Int32.Parse(row[Columns.Id].ToString()),
                    Convert.ToDecimal(row[Columns.AmountPayout]), out input))
                    return;

                    int accountId = Convert.ToInt32(row[Columns.AccountId]);
                    using (var transaction = session.SQL.BeginTransaction())
                    {
                        try
                        {
                            bool valid = await session.SQL.UpdateAsistanceAsync(input.AssistantName, input.PaybackDate, input.Amount, input.RepaymentIndex);
                            if (!valid)
                                throw new Exception(Messages.ioan_repaid_failed);
                            switch (input.Repayment)
                            {
                                case SQLBase.Repayment.Payout:
                                    valid = await session.SQL.ToBargeAsync(input.PaybackDate,
                                        string.Format(Messages.ioan_repaid_by, input.AssistantName), input.Amount, accountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Barbestand);
                                    break;
                                case SQLBase.Repayment.Transfered:
                                case SQLBase.Repayment.Direct_Debit:
                                    valid = await session.SQL.ToBankAsync(input.PaybackDate, string.Format(Messages.ioan_repaid_by, input.AssistantName), input.Amount, accountId, SQLBase.BookCategory.Einzahlung, SQLBase.BookingTo.Bankbestand);
                                    break;
                            }

                            if (!valid)
                                throw new Exception(Messages.ioan_repaid_failed);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    View.ShowMessage(Messages.ioan_repaid);
                    await ConnectTableToDataBaseAsync();
            }
            catch
            {
                table?.RejectChanges();
                throw;
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

        public virtual async Task UpdateAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                bool valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Emploees, table);
                if (!valid)
                {
                    table?.RejectChanges();
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

        public virtual void Print()
        {
            if (table == null)
                return;

            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
            session.SQL.Printing.UpdateVariable(Data.Printing.VarNames.amount_assistants, totalAmountText);
            DataRow[] rows = table.Select("", "date");
            View.PrintEmployees(rows);
        }

        public virtual async Task ChangeSelectedAssistantAsync(int rowIndex)
        {
            if (rowIndex < 0)
                return;

            if (View.ChangeButtonEnabled)
                await ChangeAssistantAsync();
        }

        public virtual async Task ChangeAssistantAsync()
        {
            if (table == null || !await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                DataRow row = GetSelectedRow();
                if (row == null)
                    return;

                AssistantInput input;
                if (!View.ShowChangeAssistantDialog(
                    Int32.Parse(row[Columns.Id].ToString()),
                    row[Columns.Name].ToString(),
                    DateTime.Parse(row[Columns.Date].ToString()),
                    Convert.ToDecimal(row[Columns.AmountPayout]),
                    out input))
                    return;

                    row[Columns.Id] = input.ID;
                    row[Columns.Name] = input.AssistantName;
                    row[Columns.Date] = input.Date;
                    row[Columns.HandSign] = session.SQL.User.Handsign;
                    row[Columns.Active] = true;
                    bool bookAssistant = Convert.ToDecimal(row[Columns.AmountPayout]) == 0;
                    if (bookAssistant)
                    {
                        row[Columns.AmountPayout] = input.Amount;
                        row[Columns.AmountPayback] = 0;
                        row[Columns.AmountPaybackType] = 0;
                    }

                    bool valid = false;
                    using (var transaction = session.SQL.BeginTransaction())
                    {
                        try
                        {
                            int accountId = Convert.ToInt32(row[Columns.AccountId]);
                            valid = await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Emploees, table);
                            if (valid && bookAssistant)
                                valid = await session.SQL.ToBargeAsync(input.Date, string.Format(Messages.ioan_to, input.AssistantName), -Math.Abs(input.Amount), accountId, SQLBase.BookCategory.Auszahlung, SQLBase.BookingTo.Barbestand);
                            if (!valid)
                                throw new Exception(Messages.assistants_changed_failed);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    if (valid)
                    {
                        if (bookAssistant)
                            await ConnectTableToDataBaseAsync();
                        View.ShowMessage(Messages.assistants_changed);
                    }
                    else
                    {
                        table?.RejectChanges();
                        View.ShowError(Messages.assistants_changed_failed);
                    }
            }
            catch
            {
                table?.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Export()
        {
            if (table == null)
                return;

            string fileName;
            if (!View.ShowSaveFileDialog(Messages.assistants_export_filename, "Excel|*.xlsx", string.Empty, out fileName))
                return;

                DataTable currentTable = table.DefaultView.ToTable();
                currentTable.Columns.Remove(Columns.AccountTransfer);
                currentTable.Columns.Remove(Columns.AmountPayback);
                currentTable.Columns.Remove(Columns.AmountPaybackType);
                currentTable.Columns.Remove(Columns.Date);
                Excel.ExportToExcel(currentTable, fileName, session.Company.CurrencyCode);
                View.ShowMessage(string.Format(Messages.export_success, fileName));
        }

        public virtual async Task ImportAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                string fileName;
                if (!View.ShowOpenFileDialog(Messages.assistants_export_filename, "Excel|*.xlsx", out fileName))
                    return;

                    DataTable importTable = new DataTable();
                    await session.SQL.FillAdapterAsync(SQLBase.SELECT.Emploees, importTable);

                    Excel.Import(fileName, importTable, int.MaxValue, new HashSet<string>() { Columns.HandSign });
                    string[] ids = importTable.Rows
                        .OfType<DataRow>()
                        .Where(a => a.RowState == DataRowState.Added)
                        .Select(a => a[Columns.Id].ToString())
                        .ToArray();

                    foreach (DataRow row in importTable.Rows)
                    {
                        if (row.RowState != DataRowState.Added)
                            continue;
                        row[Columns.Date] = DateTime.Now.Date;
                        row[Columns.HandSign] = session.SQL.User.Handsign;
                        row[Columns.AmountPayback] = 0;
                        row[Columns.AmountPaybackType] = 0;
                    }

                    using (var transaction = session.SQL.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow row in importTable.Rows)
                            {
                                if (row.RowState == DataRowState.Added && importTable.Columns.Contains(Columns.AccountId))
                                    row[Columns.AccountId] = await session.SQL.CreateAccountIdAsync("Employee");
                            }

                            if (!await session.SQL.UpdateAdapterAsync(SQLBase.SELECT.Emploees, importTable))
                            {
                                View.ShowError(Messages.assistants_import_failed);
                                return;
                            }

                            foreach (string addedID in ids)
                            {
                                DataRow row = importTable.Select(Columns.Id + "=" + addedID)[0];
                                string name = row[Columns.Name].ToString();
                                decimal payout = Convert.ToDecimal(row[Columns.AmountPayout]);
                                int accountId = Convert.ToInt32(row[Columns.AccountId]);

                                if (payout != 0 && !await session.SQL.ToBargeAsync(DateTime.Now.Date, string.Format(Messages.ioan_to, name),
                                    -Math.Abs(payout), accountId, SQLBase.BookCategory.Auszahlung,
                                    SQLBase.BookingTo.Barbestand))
                                    throw new Exception(Messages.assistants_import_failed);
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    View.ShowMessage(Messages.assistants_created);
                    await ConnectTableToDataBaseAsync();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        private DataRow GetSelectedRow()
        {
            int? selectedAssistantId = View.SelectedAssistantId;
            if (!selectedAssistantId.HasValue || table == null)
                return null;

            return table.Rows.Find(selectedAssistantId.Value);
        }
    }
}
