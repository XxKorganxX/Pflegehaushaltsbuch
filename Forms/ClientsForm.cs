using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.Properties;
using Pflegehaushaltsbuch.WPFControls;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Clients Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ClientsForm : Pflegehaushaltsbuch.FormControls.Form, IClientsFormContract
    {
        private readonly ClientsFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnClientID_Changed(int clientID);
        public event OnClientID_Changed ClientID_Changed;
        string client;
        int clientID = 0;
        UserTextBox deadLineBox = null;
        DataTable table, deadLinesTable = new DataTable();
        /// <summary>
        /// Creates a new Clients Form instance and initializes the required state.
        /// </summary>
        public ClientsForm()
        {
            InitializeComponent();
            presenter = new ClientsFormPresenter(this);
            deadLineBox = new UserTextBox();
            deadlineHost.Child = deadLineBox;
            
            clientsView.AutoGenerateColumns = false;
            foreach (SQLBase.ClientActive enumval in Enum.GetValues(typeof(SQLBase.ClientActive)))
                activeClientsBox.Items.Add(enumval.GetDisplayName());
            
            activeClientsBox.SelectedIndex = 1;
            clientsView.SelectionChanged += clientsView_SelectionChanged;
            clientsView.CellPainting += clientsView_CellPainting;
            this.Enter += clientPanel_Enter;
            this.Leave += clientPanel_Leave;
            clientBox.OnListBoxClosed += ClientBox_OnListBoxClosed;
        }
        /// <summary>
        /// Handles the on List Box Closed event for client Box and updates the related state.
        /// </summary>
        private void ClientBox_OnListBoxClosed()
        {
            SelectAccount();
        }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void OnUserRights(SQLBase sql)
        {
            base.OnUserRights(sql);
            if (sql.User.Supervisor)
            {
                updateButton.Visible = true;
                clientsView.AllowUserToDeleteRows = true;
            }
            insertButton.Enabled = sql.User.CanInsert;
            changeButton.Enabled = sql.User.CanModify;
            importButton.Enabled = sql.User.CanInsert;
        }
        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (clientsView.SelectedRows.Count != 0)
            {
                DataGridViewRow rowView = clientsView.SelectedRows[0];
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                client = row[nameColumn.DataPropertyName].ToString();
                clientID = Int32.Parse(row[idColumn.DataPropertyName].ToString());
                ClientID_Changed?.Invoke(clientID);
            }
            clientBox.DataSource = null;
            table?.Clear();
        }
        void clientsView_SelectionChanged(object sender, EventArgs e)
        {
            deadLineBox.Text = string.Empty;
            if (clientsView.SelectedRows.Count == 0)
                return;
            DataGridViewRow rowView = clientsView.SelectedRows[0];
            DataRow row = (rowView.DataBoundItem as DataRowView).Row;
            var value = row[infoColumn.DataPropertyName];
            string clientID = row[idColumn.DataPropertyName].ToString();
            if (sql.User.CanDelete)
            {
                /*
                DataTable books = new DataTable();
                sql.Adapter(SQLBase.SELECT.BooksByUser, books, clientID);
                if (books.Rows.Count == 0)
                    deleteButton.Visible = true;
                else
                    deleteButton.Visible = false;
                sql.UpdateAdapter(SQLBase.SELECT.BooksByUser, books);
                */
            }
            if (value == DBNull.Value)
                return;
            foreach (DataRow deadlineRow in deadLinesTable.Rows)
            {
                if (clientID.Equals(deadlineRow["id"].ToString()))
                {
                    deadLineBox.Text = deadlineRow["note"].ToString();
                    break;
                }
            }
        }
        void clientsView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (e.ColumnIndex == infoColumn.Index)
            {
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
                if (e.RowIndex >= 0)
                {
                    int isActive = 0;
                    if (e.Value != null && e.Value != DBNull.Value && Int32.TryParse(e.Value.ToString(), out isActive))
                    {
                        Rectangle rect = e.CellBounds;
                        rect.Height -= 10;
                        rect.X += 5;
                        rect.Y += 5;
                        rect.Width = rect.Height;
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        Rectangle bounds = rect;
                        using (var ellipsePath = new GraphicsPath())
                        {
                            ellipsePath.AddEllipse(bounds);
                            using (var brush = new PathGradientBrush(ellipsePath))
                            {
                                brush.CenterPoint = new PointF(bounds.X + bounds.Width / 2f - 1, bounds.Y + bounds.Height / 2f - 1);
                                brush.CenterColor = Color.White;
                                //if (isActive != 0)
                                brush.SurroundColors = new[] { Color.Blue };
                                //else
                                e.Graphics.FillEllipse(brush, rect);
                            }
                        }
                        e.Graphics.DrawEllipse(new Pen(Brushes.Black), rect);
                        e.Graphics.SmoothingMode = SmoothingMode.Default;
                    }
                }
                else
                {
                    Rectangle rect = e.CellBounds;
                    rect.Height -= 10;
                    rect.X += 5;
                    rect.Y += 5;
                    rect.Width = rect.Height;
                    e.Graphics.DrawImage(Resources.kalender, rect);
                }
                e.Handled = true;
            }
            else if (e.ColumnIndex == activeColumn.Index && e.RowIndex >= 0)
            {
                SolidBrush backColor = new SolidBrush(clientsView.BackgroundColor);
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
                int isActive = 0;
                if (e.Value != null && e.Value != DBNull.Value && Int32.TryParse(e.Value.ToString(), out isActive))
                {
                    Rectangle rect = e.CellBounds;
                    rect.Height -= 10;
                    rect.X += 5;
                    rect.Y += 5;
                    rect.Width = rect.Height;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle bounds = rect;
                    using (var ellipsePath = new GraphicsPath())
                    {
                        ellipsePath.AddEllipse(bounds);
                        using (var brush = new PathGradientBrush(ellipsePath))
                        {
                            brush.CenterPoint = new PointF(bounds.X + bounds.Width / 2f - 1, bounds.Y + bounds.Height / 2f - 1);
                            brush.CenterColor = Color.White;
                            if (isActive == 0)
                                brush.SurroundColors = new[] { Color.Red };
                            else if (isActive == 1)
                                brush.SurroundColors = new[] { Color.Green };
                            else if (isActive == 2)
                                brush.SurroundColors = new[] { Color.Black };
                            e.Graphics.FillEllipse(brush, rect);
                        }
                    }
                    e.Graphics.DrawEllipse(new Pen(Brushes.Black), rect);
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            //clientBox.DataSource = null;
            table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, table);
            table.PrimaryKey = new DataColumn[] { table.Columns["id"] };

            DateTime today = DateTime.Now;
            deadLinesTable.Clear();
            await sql.FillAdapterAsync(SQLBase.SELECT.DeadlineByDay, deadLinesTable, today.Day);
            foreach (DataRow deadlineRow in deadLinesTable.Rows)
            {
                DataRow clientRow = table.Rows.Find(deadlineRow["id"]);
                if (clientRow != null)
                    clientRow["info"] = 1;
            }
            string activeColumnName = activeColumn.DataPropertyName;
            bool activeOnly = activeClientsBox.SelectedIndex == 1;
            table.DefaultView.RowFilter = table.Columns[activeColumnName].DataType == typeof(bool)
                ? string.Format("{0} = {1}", activeColumnName, activeOnly)
                : string.Format("{0} = {1}", activeColumnName, activeClientsBox.SelectedIndex);
            UpdateTotalAmount();
            if (clientsView.SortedColumn != null)
                table.DefaultView.Sort = clientsView.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = nameColumn.DataPropertyName;
            clientsView.DataSource = table.DefaultView;
            clientBox.DataSource = table.DefaultView;
            clientBox.DisplayMember = "name";
            bornBox.DataBindings.Clear();
            bornBox.DataBindings.Add("Text", table.DefaultView, "born", true, DataSourceUpdateMode.OnValidation, "", "dd/MM/yyyy");
            clientDateBox.DataBindings.Clear();
            clientDateBox.DataBindings.Add("Text", table.DefaultView, "date", true, DataSourceUpdateMode.OnValidation, "", "dd/MM/yyyy");
            totalClientsBox.Text = table.DefaultView.Count.ToString();
            if (!string.IsNullOrWhiteSpace(client))
            {
                DataRow row = table.Rows.Find(clientID);
                foreach (DataRowView item in clientBox.Items)
                {
                    object str = item.Row["name"].ToString();
                    if (str.Equals(client))
                    {
                        clientBox.SelectedItem = item;
                        break;
                    }
                }
            }
            clientsView_SelectionChanged(null, new EventArgs());
        }
        /// <summary>
        /// Updates the total Amount data and refreshes the related application state.
        /// </summary>
        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRowView rowView in table.DefaultView)
            {
                decimal amount = 0;
                DataRow row = rowView.Row;
                if (decimal.TryParse(row[amountColumn.DataPropertyName].ToString(), out amount))
                    totalAmount += amount;
            }
            totalAmountBox.Text = totalAmount.ToString("C");
        }

        private async Task createAccount(CreateClientDialog.ClientData clientData, bool updateClient)
        {
            bool transactionCommitted = false;
            try
            {
                DataRow row = updateClient ? table.Rows.Find(clientData.ClientID) : table.NewRow();
                if (row == null)
                    throw new Exception(Messages.client_not_found);

                row["id"] = clientData.ClientID;
                row["title"] = clientData.Title;
                row["name"] = clientData.Name;
                row["street"] = clientData.Street;
                row["zipcode"] = clientData.Zipcode;
                row["city"] = clientData.City;
                row["born"] = clientData.BornDate;
                row["advisor_id"] = clientData.AdvisorId.HasValue ? (object)clientData.AdvisorId.Value : DBNull.Value;
                row["handsign"] = sql.User.Name;

                if (!updateClient)
                {
                    row["date"] = DateTime.Now.Date;
                    row["amount"] = clientData.Amount;
                    row["account_transfer"] = clientData.Amount;
                    row["active"] = (int)SQLBase.ClientActive.Active;
                    row["info"] = 0;
                    table.Rows.Add(row);
                }

                using (var transaction = sql.BeginTransaction())
                {
                    try
                    {
                        bool value = await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, table);
                        if (!value)
                            throw new Exception(Messages.clients_changed_failed);

                        if (!updateClient && clientData.Amount != 0)
                        {
                            bool openingBalanceBooked = await sql.ToBankAsync(
                                DateTime.Now.Date,
                                string.Format(Messages.clients_previous_amount, clientData.Name),
                                clientData.Amount,
                                string.Format("K{0:000}", clientData.ClientID),
                                SQLBase.BookCategory.Einzahlung,
                                SQLBase.BookingTo.Altbestand);
                            if (!openingBalanceBooked)
                                throw new Exception(Messages.clients_changed_failed);
                        }

                        transaction.Commit();
                        transactionCommitted = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                await ConnectTableToDataBase();
                MessageBox.ShowDialog(this, Messages.clients_changed);
            }
            catch
            {
                if (!transactionCommitted)
                    table.RejectChanges();
                throw;
            }
        }

        /// <summary>
        /// Handles the click event for create Account Button and updates the related state.
        /// </summary>
        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            using (CreateClientDialog createAccountDialog = new CreateClientDialog(sql))
            {
                if (createAccountDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    await createAccount(createAccountDialog.Data, false);
            }
        }
        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            if (clientsView.SelectedRows.Count <= 0)
                return;
            DataGridViewRow rowView = clientsView.SelectedRows[0];
            DataRow row = (rowView.DataBoundItem as DataRowView).Row;
            int clientID = Int32.Parse(row[idColumn.DataPropertyName].ToString());
            using (CreateClientDialog createAccountDialog = new CreateClientDialog(sql, clientID))
            {
                if (createAccountDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    await createAccount(createAccountDialog.Data, true);
            }
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                if (clientsView.SelectedRows.Count <= 0)
                    return;
                DataGridViewRow rowView = clientsView.SelectedRows[0];
                DataRow rowOfView = (rowView.DataBoundItem as DataRowView).Row;
                int clientID = Int32.Parse(rowOfView[idColumn.DataPropertyName].ToString());
                if (MessageBox.ShowDialog(this, string.Format(Messages.clients_delete, clientBox.Text), Messages.clients_delete_title, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
                DataTable bookTable = new DataTable(), bankTable = new DataTable(), bargeTable = new DataTable(), clientTable = new DataTable();
                await sql.FillAdapterAsync(SQLBase.SELECT.Books, bookTable);
                await sql.FillAdapterAsync(SQLBase.SELECT.Bank, bankTable);
                await sql.FillAdapterAsync(SQLBase.SELECT.Barge, bargeTable);
                string clientIdNumber = string.Format("K{0:000}", clientID);

                foreach (DataRow row in bargeTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row["account"] == DBNull.Value || string.IsNullOrWhiteSpace(row["account"].ToString()))
                        throw new Exception(Messages.clients_delete_cash_not_assignable);
                    if (row["account"].ToString().Equals(clientIdNumber))
                        row.Delete();
                }
                foreach (DataRow row in bookTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row["id"] == DBNull.Value)
                        throw new Exception(Messages.clients_delete_books_not_assignable);
                    if (Int32.Parse(row["id"].ToString()) == clientID)
                        row.Delete();
                }
                foreach (DataRow row in bankTable.Rows.OfType<DataRow>().ToArray())
                {
                    if (row["account"] == DBNull.Value || string.IsNullOrWhiteSpace(row["account"].ToString()))
                        throw new Exception(Messages.clients_delete_bank_not_assignable);
                    if (row["account"].ToString().Equals(clientIdNumber))
                        row.Delete();
                }
                using (var transaction = sql.BeginTransaction())
                {
                    try
                    {
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Books, bookTable))
                            throw new Exception(Messages.clients_changed_failed);
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Barge, bargeTable))
                            throw new Exception(Messages.clients_changed_failed);
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Bank, bankTable))
                            throw new Exception(Messages.clients_changed_failed);
                        await sql.FillAdapterAsync(SQLBase.SELECT.Client, clientTable, clientID);
                        if (clientTable.Rows.Count == 0)
                            throw new Exception(Messages.client_not_found);
                        clientTable.Rows[0].Delete();
                        if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, clientTable))
                            throw new Exception(Messages.clients_changed_failed);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                await ConnectTableToDataBase();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for dead Lines Button and updates the related state.
        /// </summary>
        private void deadLinesButton_Click(object sender, EventArgs e)
        {
            if (clientsView.SelectedRows.Count == 0)
                return;
            
            ShowForm?.Invoke(Enums.Forms.Calendar, sql);
        }
        /// <summary>
        /// Runs the select Account operation and updates the related application state.
        /// </summary>
        private void SelectAccount()
        {
            if (clientsView.SelectedRows.Count == 0)
                return;

            ShowForm?.Invoke(Enums.Forms.Book, sql);
        }
        /// <summary>
        /// Handles the click event for select Account Button and updates the related state.
        /// </summary>
        private void selectAccountButton_Click(object sender, EventArgs e)
        {
            SelectAccount();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm?.Invoke(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private async void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                sql.Printing.UpdateVariable(Data.Printing.VarNames.date, DateTime.Now.ToShortDateString());
                sql.Printing.UpdateVariable(Data.Printing.VarNames.amount_clients, totalAmountBox.Text);
                PrintBase printer = new PrintBase(sql, Data.Printing.LayoutEnum.clients);
                DataTable printTable = table.Clone();
                foreach (DataGridViewRow rowView in clientsView.Rows)
                    printTable.ImportRow((rowView.DataBoundItem as DataRowView).Row);
                printTable.Columns.Add("credit", typeof(decimal));
                printTable.Columns.Add("debit", typeof(decimal));
                foreach (DataRow row in printTable.Rows)
                {
                    DateTime date = DateTime.Now;
                    DataTable books = new DataTable();
                    await sql.FillAdapterAsync(SQLBase.SELECT.Book, books, row[idColumn.DataPropertyName], date.Month, date.Year);
                    decimal credit = 0, debit = 0;
                    foreach (DataRow bookRow in books.Rows)
                    {
                        var value = (decimal)bookRow["amount"];
                        if (value > 0)
                            credit += value;
                        else
                            debit += Math.Abs(value);
                    }
                    row["credit"] = credit;
                    row["debit"] = debit;
                }
                printer.Print(Text, Text, this, printTable.Rows.OfType<DataRow>().ToArray());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, table);
                if (!valid)
                {
                    table.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.datatable_update_failed);
                }
                else
                {
                    await ConnectTableToDataBase();
                    MessageBox.ShowDialog(this, Messages.datatable_updated);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }            
        }
        /// <summary>
        /// Handles the selected Index Changed event for active Clients Box and updates the related state.
        /// </summary>
        private async void activeClientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!activeClientsBox.Focused)
                return;
            await ConnectTableToDataBase();
        }
        /// <summary>
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private async Task importClients(ImportClientsDialog.ImportsClientData importData)
        {
            DataTable importTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, importTable);
            importTable.PrimaryKey = new DataColumn[] { importTable.Columns["id"] };

            foreach (ImportClientsDialog.ImportedClient clientData in importData.Clients)
            {
                DataRow row = importTable.NewRow();
                row["id"] = clientData.Id;
                row["title"] = clientData.Title;
                row["name"] = clientData.Name;
                row["street"] = clientData.Street;
                row["zipcode"] = clientData.Zipcode;
                row["city"] = clientData.City;
                row["born"] = clientData.BornDate;
                row["date"] = clientData.CreatedDate;
                row["amount"] = clientData.OpeningBalance;
                row["account_transfer"] = clientData.OpeningBalance;
                row["active"] = (int)SQLBase.ClientActive.Active;
                row["advisor_id"] = clientData.AdvisorId.HasValue ? (object)clientData.AdvisorId.Value : DBNull.Value;
                row["handsign"] = sql.User.Name;
                importTable.Rows.Add(row);
            }

            using (var transaction = sql.BeginTransaction())
            {
                try
                {
                    bool saved = await sql.UpdateAdapterAsync(SQLBase.SELECT.Clients, importTable);
                    if (!saved)
                        throw new Exception(Messages.clients_changed_failed);

                    foreach (ImportClientsDialog.ImportedClient clientData in importData.Clients)
                    {
                        if (clientData.OpeningBalance == 0)
                            continue;

                        bool booked = await sql.ToBankAsync(
                            clientData.CreatedDate,
                            string.Format(Messages.clients_previous_amount, clientData.Name),
                            clientData.OpeningBalance,
                            string.Format("K{0:000}", clientData.Id),
                            SQLBase.BookCategory.Einzahlung,
                            SQLBase.BookingTo.Altbestand);
                        if (!booked)
                            throw new Exception(Messages.clients_changed_failed);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            await ConnectTableToDataBase();
            MessageBox.Show(this, Messages.clients_imported);
        }        /// <summary>
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private async void importButton_Click(object sender, EventArgs e)
        {
            DataTable importTable = new DataTable();
            DataTable advisorTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, importTable);
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisorTable);
            importTable.PrimaryKey = new DataColumn[] { importTable.Columns["id"] };
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns["name"] };

            using (ImportClientsDialog importDialog = new ImportClientsDialog(sql, table.DefaultView.Count, importTable, advisorTable))
            {
                if (importDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                await importClients(importDialog.Data);
            }
        }
        /// <summary>
        /// Handles the click event for client Books Button and updates the related state.
        /// </summary>
        private void clientBooksButton_Click(object sender, EventArgs e)
        {
            using (PrintClientsBooksDialog printClientsBooksForm = new PrintClientsBooksDialog(sql))
            {
                printClientsBooksForm.ShowDialog(this);
            }
        }
        /// <summary>
        /// Handles the key Up event for clients View and updates the related state.
        /// </summary>
        private void clientsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SelectAccount();
            }
        }
        /// <summary>
        /// Handles the click event for clients View and updates the related state.
        /// </summary>
        private void clientsView_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the cell Enter event for clients View and updates the related state.
        /// </summary>
        private void clientsView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }
        /// <summary>
        /// Handles the cell Content Click event for clients View and updates the related state.
        /// </summary>
        private void clientsView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = Messages.clients_export_filename;
                fileDialog.Filter = "Excel|*.xlsx";
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                var currentTable = table.DefaultView.ToTable();
                currentTable.Columns.Remove("account_transfer");
                currentTable.Columns.Remove("date");
                currentTable.Columns.Remove("lastbook");
                Excel.ExportToExcel(currentTable, fileDialog.FileName);
                MessageBox.ShowDialog(this, string.Format(Messages.export_success, fileDialog.FileName));
            }
        }
        /// <summary>
        /// Handles the cell Click event for clients View and updates the related state.
        /// </summary>
        private void clientsView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || nameColumn.Index != e.ColumnIndex)
                return;
            SelectAccount();
        }
        /// <summary>
        /// Handles the drop Down Closed event for client Box and updates the related state.
        /// </summary>
        private void clientBox_DropDownClosed(object sender, EventArgs e)
        {
            clientsView.Focus();
        }
    }
}
