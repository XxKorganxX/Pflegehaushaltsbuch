using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using DataGridView = Pflegehaushaltsbuch.FormControls.DataGridView;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Bank Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class BankForm : Form, IBankFormContract
    {
        private readonly BankFormPresenter presenter;

        /// <summary>
        /// Creates a new BankForm view.
        /// </summary>
        public BankForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new BankFormPresenter(this, session);
            view.AutoGenerateColumns = false;
            Enter += CashForm_Enter;
            Leave += CashForm_Leave;
            view.CellFormatting += CellFormatting;
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
                view.AllowUserToDeleteRows = true;
            }
            bookButton.Enabled = rights.CanInsert | rights.CanModify;
        }

        /// <summary>
        /// Handles the cell format event.
        /// </summary>
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == bookCategoryColumn.Index)
            {
                int index = Int32.Parse(e.Value.ToString());
                if (index == (int)SQLBase.BookCategory.Einzahlung)
                    e.CellStyle.ForeColor = Color.Green;
                else if (index == (int)SQLBase.BookCategory.Auszahlung)
                    e.CellStyle.ForeColor = Color.Red;
                e.Value = ((SQLBase.BookCategory)index).GetDisplayName();
            }
        }

        /// <summary>
        /// Handles the cash form enter enter event.
        /// </summary>
        async void CashForm_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the cash form leave leave event.
        /// </summary>
        void CashForm_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the click event for book Button and updates the related state.
        /// </summary>
        private async void bookButton_Click(object sender, EventArgs e)
        {
            await presenter.BookAsync();
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }

        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private async void printButton_Click(object sender, EventArgs e)
        {
            await presenter.PrintAsync();
        }

        /// <summary>
        /// Handles the click event for all Books Check Box and updates the related state.
        /// </summary>
        private async void allBooksCheckBox_Click(object sender, EventArgs e)
        {
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the value Changed event for date and updates the related state.
        /// </summary>
        private async void date_ValueChanged()
        {
            if (DesignMode)
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
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string IBankFormContract.DefaultSortColumn
        {
            get { return dateColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the currently sorted column name for the presenter.
        /// </summary>
        string IBankFormContract.CurrentSortColumn
        {
            get { return view.SortedColumn == null ? null : view.SortedColumn.DataPropertyName; }
        }
        /// <summary>
        /// Provides the from date value for the presenter.
        /// </summary>
        DateTime IBankFormContract.FromDate
        {
            get { return fromDateBox.Date; }
        }

        /// <summary>
        /// Provides the to date value for the presenter.
        /// </summary>
        DateTime IBankFormContract.ToDate
        {
            get { return toDateBox.Date; }
        }

        /// <summary>
        /// Provides the period checked value for the presenter.
        /// </summary>
        bool IBankFormContract.PeriodChecked
        {
            get { return periodCheckBox.Checked; }
        }

        void IBankFormContract.SetTotalAmount(string totalAmount)
        {
            totalAmountBox.Text = totalAmount;
        }

        /// <summary>
        /// Runs the show bank book dialog view action for the presenter.
        /// </summary>
        bool IBankFormContract.ShowBankBookDialog(out BankBookingInput input)
        {
            using (Dialoge.BankBookDialog dialog = new Dialoge.BankBookDialog(Session))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new BankBookingInput
                {
                    Amount = dialog.Amount,
                    BookText = dialog.BookText,
                    BookingDate = dialog.BookingDate,
                    BookingTarget = dialog.BookingTarget,
                    BookingCategory = dialog.BookingCategory,
                    PrintQuittance = dialog.PrintQuittance,
                    SelectedClients = dialog.SelectedClients
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the set period controls visible view action for the presenter.
        /// </summary>
        void IBankFormContract.SetPeriodControlsVisible(bool visible)
        {
            toDateBox.Visible = fromToLabel.Visible = visible;
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IBankFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        public void BindBank(DataTable table)
        {
            view.DataSource = table;
        }

        public void PrintQuittance(string clientName, IEnumerable<DataRow> currentBooks)
        {
            Quittance quittance = new Quittance(Session);
            quittance.Print(clientName, clientName, this, new List<DataRow>(currentBooks));
        }

        public void PrintBank(DataRow[] rows)
        {
            PrintBase printer = new PrintBase(Session, Printing.LayoutEnum.bank);
            printer.Print(Text, Text, this, rows);
        }
    }
}
