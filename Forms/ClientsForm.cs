using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Clients Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ClientsForm : Form, IClientsFormContract
    {
        private readonly ClientsFormPresenter presenter;

        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnClientID_Changed(int clientID);
        public event OnClientID_Changed ClientID_Changed;

        /// <summary>
        /// Creates a new ClientsForm view.
        /// </summary>
        public ClientsForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new ClientsFormPresenter(this, session);

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
            presenter.SelectAccount();
        }

        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            if (rights.IsSupervisor)
            {
                updateButton.Visible = true;
                clientsView.AllowUserToDeleteRows = true;
            }
            insertButton.Enabled = rights.CanInsert;
            changeButton.Enabled = rights.CanModify;
        }

        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;

            presenter.Leave();
        }

        /// <summary>
        /// Handles the clients view selection changed selection change.
        /// </summary>
        void clientsView_SelectionChanged(object sender, EventArgs e)
        {
            presenter.SelectionChanged();
        }

        /// <summary>
        /// Handles the clients view cell paint event.
        /// </summary>
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
                                brush.SurroundColors = new[] { Color.Blue };
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
        /// Handles the click event for create Account Button and updates the related state.
        /// </summary>
        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateAccountAsync();
        }

        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await presenter.ChangeAsync();
        }

        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            await presenter.DeleteAsync();
        }

        /// <summary>
        /// Handles the click event for dead Lines Button and updates the related state.
        /// </summary>
        private void deadLinesButton_Click(object sender, EventArgs e)
        {
            presenter.DeadLines();
        }

        /// <summary>
        /// Handles the click event for select Account Button and updates the related state.
        /// </summary>
        private void selectAccountButton_Click(object sender, EventArgs e)
        {
            presenter.SelectAccount();
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
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
            await presenter.PrintAsync();
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }

        /// <summary>
        /// Handles the selected Index Changed event for active Clients Box and updates the related state.
        /// </summary>
        private async void activeClientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!activeClientsBox.Focused)
                return;

            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the click event for client Books Button and updates the related state.
        /// </summary>
        private void clientBooksButton_Click(object sender, EventArgs e)
        {
            presenter.ClientBooks();
        }

        /// <summary>
        /// Handles the key Up event for clients View and updates the related state.
        /// </summary>
        private void clientsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                presenter.SelectAccount();
            }
        }

        /// <summary>
        /// Handles the drop Down Closed event for client Box and updates the related state.
        /// </summary>
        private void clientBox_DropDownClosed(object sender, EventArgs e)
        {
            clientsView.Focus();
        }

        /// <summary>
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string IClientsFormContract.DefaultSortColumn
        {
            get { return nameColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the currently sorted column name for the presenter.
        /// </summary>
        string IClientsFormContract.CurrentSortColumn
        {
            get { return clientsView.SortedColumn == null ? null : clientsView.SortedColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the active clients filter index for the presenter.
        /// </summary>
        int IClientsFormContract.ActiveClientsFilterIndex
        {
            get { return activeClientsBox.SelectedIndex; }
        }

        /// <summary>
        /// Provides the selected client id for the presenter.
        /// </summary>
        int? IClientsFormContract.SelectedClientId
        {
            get
            {
                if (clientsView.SelectedRows.Count == 0)
                    return null;

                DataRowView rowView = clientsView.SelectedRows[0].DataBoundItem as DataRowView;
                if (rowView == null || rowView.Row[idColumn.DataPropertyName] == DBNull.Value)
                    return null;

                return Convert.ToInt32(rowView.Row[idColumn.DataPropertyName]);
            }
        }

        /// <summary>
        /// Provides the selected client name for the presenter.
        /// </summary>
        string IClientsFormContract.SelectedClientName
        {
            get { return clientBox.Text; }
        }

        /// <summary>
        /// Provides the formatted total amount for the presenter.
        /// </summary>
        string IClientsFormContract.TotalAmountText
        {
            get { return totalAmountBox.Text; }
        }

        /// <summary>
        /// Binds the client list to the view controls.
        /// </summary>
        void IClientsFormContract.BindClients(DataView clients)
        {
            clientsView.DataSource = clients;
            clientBox.DataSource = clients;
            clientBox.DisplayMember = nameColumn.DataPropertyName;
        }

        /// <summary>
        /// Clears client list bindings.
        /// </summary>
        void IClientsFormContract.ClearClients()
        {
            clientBox.DataSource = null;
            clientsView.DataSource = null;
        }

        /// <summary>
        /// Binds selected client date fields.
        /// </summary>
        void IClientsFormContract.BindClientDates(DataView clients)
        {
            bornBox.DataBindings.Clear();
            bornBox.DataBindings.Add("Text", clients, Columns.Born, true, DataSourceUpdateMode.OnValidation, "", "dd/MM/yyyy");
            clientDateBox.DataBindings.Clear();
            clientDateBox.DataBindings.Add("Text", clients, Columns.Date, true, DataSourceUpdateMode.OnValidation, "", "dd/MM/yyyy");
        }

        /// <summary>
        /// Shows the current total client count.
        /// </summary>
        void IClientsFormContract.SetTotalClients(int totalClients)
        {
            totalClientsBox.Text = totalClients.ToString();
        }

        /// <summary>
        /// Shows the current total amount.
        /// </summary>
        void IClientsFormContract.SetTotalAmount(string totalAmount)
        {
            totalAmountBox.Text = totalAmount;
        }

        /// <summary>
        /// Shows the deadline text for the selected client.
        /// </summary>
        void IClientsFormContract.SetDeadlineText(string text)
        {
            deadLineBox.Text = text;
        }

        /// <summary>
        /// Selects the first client with the given name.
        /// </summary>
        void IClientsFormContract.SelectClientByName(string clientName)
        {
            foreach (DataRowView item in clientBox.Items)
            {
                if (item.Row[nameColumn.DataPropertyName].ToString().Equals(clientName))
                {
                    clientBox.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Runs the notify client id changed view action for the presenter.
        /// </summary>
        void IClientsFormContract.NotifyClientIdChanged(int clientID)
        {
            ClientID_Changed?.Invoke(clientID);
        }

        /// <summary>
        /// Runs the show create client dialog view action for the presenter.
        /// </summary>
        bool IClientsFormContract.ShowCreateClientDialog(out ClientAccountInput clientData)
        {
            using (CreateClientDialog dialog = new CreateClientDialog(Session))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    clientData = null;
                    return false;
                }

                clientData = MapClientAccountInput(dialog.Data);
                return true;
            }
        }

        /// <summary>
        /// Runs the show change client dialog view action for the presenter.
        /// </summary>
        bool IClientsFormContract.ShowChangeClientDialog(int clientID, out ClientAccountInput clientData)
        {
            using (CreateClientDialog dialog = new CreateClientDialog(Session, clientID))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    clientData = null;
                    return false;
                }

                clientData = MapClientAccountInput(dialog.Data);
                return true;
            }
        }

        /// <summary>
        /// Runs the show print clients books dialog view action for the presenter.
        /// </summary>
        void IClientsFormContract.ShowPrintClientsBooksDialog()
        {
            using (Dialoge.PrintClientsBooksDialog dialog = new Dialoge.PrintClientsBooksDialog(Session))
            {
                dialog.ShowDialog(this);
            }
        }

        /// <summary>
        /// Prints the currently displayed clients.
        /// </summary>
        void IClientsFormContract.PrintClients(DataRow[] clients)
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.clients);
            printer.Print(Text, Text, this, clients);
        }

        /// <summary>
        /// Runs the map client account input action.
        /// </summary>
        private static ClientAccountInput MapClientAccountInput(CreateClientDialog.ClientData data)
        {
            return new ClientAccountInput
            {
                ClientID = data.ClientID,
                Title = data.Title,
                Name = data.Name,
                Street = data.Street,
                Zipcode = data.Zipcode,
                City = data.City,
                BornDate = data.BornDate,
                Amount = data.Amount,
                AdvisorId = data.AdvisorId
            };
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IClientsFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        /// <summary>
        /// Runs the show main form async view action for the presenter.
        /// </summary>
        Task IClientsFormContract.ShowMainFormAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ShowFormEvent(Enums.Forms.Main);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Runs the show book form view action for the presenter.
        /// </summary>
        void IClientsFormContract.ShowBookForm()
        {
            ShowFormEvent(Enums.Forms.Book);
        }

        /// <summary>
        /// Runs the show calendar form view action for the presenter.
        /// </summary>
        void IClientsFormContract.ShowCalendarForm()
        {
            ShowFormEvent(Enums.Forms.Calendar);
        }
    }
}
