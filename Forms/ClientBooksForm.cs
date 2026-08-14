using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Book Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ClientBooksForm : Form, IClientBooksFormContract
    {
        private readonly ClientBooksFormPresenter presenter;
        private bool initializingPeriodDateRange;

        /// <summary>
        /// Creates a new BookForm view.
        /// </summary>
        public ClientBooksForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new ClientBooksFormPresenter(this, session);

            commentBox.Validated += CommentBox_Validated;

            bookView.AutoGenerateColumns = false;
            ApplyCurrencyFormat(amountColumn);
            DoubleBuffered = true;
            foreach (SQLBase.ClientActive enumval in Enum.GetValues(typeof(SQLBase.ClientActive)))
                accountStatusBox.Items.Add(enumval.GetDisplayName());
            Enter += bookPanel_Enter;
            Leave += bookPanel_Leave;

            bookView.CellFormatting += bookView_CellFormatting;
        }

        /// <summary>
        /// Handles the validated event for comment Box and updates the related state.
        /// </summary>
        private async void CommentBox_Validated(object sender, EventArgs e)
        {
            presenter.ApplyCommentText(commentBox.Text);
            await presenter.UpdateClientNoteAsync();
        }

        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            bookButton.Enabled = rights.CanBook;
            commentBox.Enabled = accountStatusBox.Enabled = rights.CanModify;
            stornoButton.Enabled = rights.CanCancelBooking;
        }

        /// <summary>
        /// Handles the client ID Changed lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnClientID_Changed(int clientID)
        {
            presenter.SetClientID(clientID);
        }

        /// <summary>
        /// Handles the book view cell format event.
        /// </summary>
        void bookView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (e.ColumnIndex == bookCategoryColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                if (index == (int)SQLBase.BookCategory.Einzahlung)
                    e.CellStyle.ForeColor = Color.Green;
                else if (index == (int)SQLBase.BookCategory.Auszahlung)
                    e.CellStyle.ForeColor = Color.Red;
                e.Value = ((SQLBase.BookCategory)index).GetDisplayName();
            }
            else if (e.ColumnIndex == bookToColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                e.Value = ((SQLBase.BookingTo)index).GetDisplayName();
            }
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the enter event for book Panel and updates the related state.
        /// </summary>
        private async void bookPanel_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            ApplyCurrencyFormat(amountColumn);

            clientNameBox.DataBindings.Clear();
            accountStatusBox.DataBindings.Clear();
            totalAmountBox.DataBindings.Clear();
            lastBookBox.DataBindings.Clear();

            DataTable clientTable = await presenter.EnterAsync();

            clientNameBox.DataBindings.Add("Text", clientTable, "name");
            totalAmountBox.DataBindings.Add("Text", clientTable, "amount", true, DataSourceUpdateMode.OnValidation, 0, "C", CurrencyFormatProvider);
            lastBookBox.DataBindings.Add("Text", clientTable, "lastbook", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
            if (commentBox != null && clientTable.Rows.Count > 0)
                commentBox.Text = clientTable.Rows[0]["note"].ToString();
            accountStatusBox.DataBindings.Add("SelectedIndex", accountBinding, "active");
        }

        /// <summary>
        /// Handles the leave event for book Panel and updates the related state.
        /// </summary>
        private void bookPanel_Leave(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
        }

        /// <summary>
        /// Handles the click event for storno Button and updates the related state.
        /// </summary>
        private async void stornoButton_Click(object sender, EventArgs e)
        {
            if (bookView.SelectedRows.Count == 0)
                throw new Exception(Messages.booking_no_booking_canceled);

            DataGridViewRow rowView = bookView.SelectedRows[0];
            DataRow row = (rowView.DataBoundItem as DataRowView).Row;

            await presenter.StornoAsync(row);
        }

        /// <summary>
        /// Handles the validated event for note Box and updates the related state.
        /// </summary>
        private void noteBox_Validated(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the validated event for account Status Box and updates the related state.
        /// </summary>
        private async void accountStatusBox_Validated(object sender, EventArgs e)
        {
            await presenter.UpdateAccountStatusAsync();
        }

        /// <summary>
        /// Handles the click event for print Account Button and updates the related state.
        /// </summary>
        private void printAccountButton_Click(object sender, EventArgs e)
        {
            presenter.PrintAccount(totalAmountBox.Text);
        }

        /// <summary>
        /// Handles the click event for book Button and updates the related state.
        /// </summary>
        private async void bookButton_Click(object sender, EventArgs e)
        {
            await presenter.BookAsync(clientNameBox.Text);
        }

        /// <summary>
        /// Handles the value Changed event for date and updates the related state.
        /// </summary>
        private async void date_ValueChanged()
        {
            if (DesignMode)
                return;
            if (initializingPeriodDateRange)
                return;

            await presenter.DateChangedAsync();
        }

        /// <summary>
        /// Handles the click event for period Check Box and updates the related state.
        /// </summary>
        private async void periodCheckBox_Click(object sender, EventArgs e)
        {
            await presenter.PeriodCheckAsync();
        }

        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            presenter.Export();
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }

        /// <summary>
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string IClientBooksFormContract.DefaultSortColumn
        {
            get { return dateColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the number column name value for the presenter.
        /// </summary>
        string IClientBooksFormContract.NumberColumnName
        {
            get { return numberColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the date column name value for the presenter.
        /// </summary>
        string IClientBooksFormContract.DateColumnName
        {
            get { return dateColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the book category column name value for the presenter.
        /// </summary>
        string IClientBooksFormContract.BookCategoryColumnName
        {
            get { return bookCategoryColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the amount column name value for the presenter.
        /// </summary>
        string IClientBooksFormContract.AmountColumnName
        {
            get { return amountColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the from date value for the presenter.
        /// </summary>
        DateTime IClientBooksFormContract.FromDate
        {
            get { return fromDateBox.Date; }
        }

        /// <summary>
        /// Provides the to date value for the presenter.
        /// </summary>
        DateTime IClientBooksFormContract.ToDate
        {
            get { return toDateBox.Date; }
        }

        /// <summary>
        /// Provides the period checked value for the presenter.
        /// </summary>
        bool IClientBooksFormContract.PeriodChecked
        {
            get { return periodCheckBox.Checked; }
        }

        void IClientBooksFormContract.SetPeriodDateRange(DateTime fromDate, DateTime toDate)
        {
            initializingPeriodDateRange = true;
            try
            {
                fromDateBox.Date = fromDate;
                toDateBox.Date = toDate;
            }
            finally
            {
                initializingPeriodDateRange = false;
            }
        }

        /// <summary>
        /// Runs the show print books dialog view action for the presenter.
        /// </summary>
        void IClientBooksFormContract.ShowPrintBooksDialog(System.Data.DataTable table, int clientID, string totalAmount, DateTime from, DateTime to)
        {
            using (Dialoge.PrintBooksDialog dialog = new Dialoge.PrintBooksDialog(Session, table, clientID, totalAmount, from, to))
            {
                dialog.ShowDialog(this);
            }
        }

        /// <summary>
        /// Runs the show client book dialog view action for the presenter.
        /// </summary>
        bool IClientBooksFormContract.ShowClientBookDialog(string clientName, string clientID, out ClientBookingInput input)
        {
            using (Dialoge.ClientBookDialog dialog = new Dialoge.ClientBookDialog(Session, clientName, clientID))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new ClientBookingInput
                {
                    Amount = dialog.Amount,
                    BookText = dialog.BookText,
                    BookingDate = dialog.BookingDate,
                    ClientName = dialog.ClientName,
                    ClientID = dialog.ClientID,
                    BookingTarget = dialog.BookingTarget,
                    BookingCategory = dialog.BookingCategory,
                    PrintQuittance = dialog.PrintQuittance
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the set period controls visible view action for the presenter.
        /// </summary>
        void IClientBooksFormContract.SetPeriodControlsVisible(bool visible)
        {
            toDateBox.Visible = fromToLabel.Visible = visible;
        }

        /// <summary>
        /// Runs the show clients form view action for the presenter.
        /// </summary>
        void IClientBooksFormContract.ShowClientsForm()
        {
            ShowFormEvent(Enums.Forms.Clients);
        }
        public void SetBookTable(DataTable table)
        {
            if (bookView.SortedColumn != null)
                table.DefaultView.Sort = bookView.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;

            bookView.DataSource = table;
        }

        public void SetClientTable(DataTable clientTable)
        {
            accountBinding.DataSource = clientTable;
        }

        public void EndEditAccount()
        {
            accountBinding.EndEdit();
        }

        public void PrintQuittance(string clientName, DataRow[] dataRows)
        {
            Quittance quittance = new Quittance(Session);
            quittance.Print(clientName, clientName, this, dataRows);
        }
    }
}
