using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DataGridView = Pflegehaushaltsbuch.FormControls.DataGridView;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Cash Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashForm : Form, ICashFormContract
    {
        private readonly CashFormPresenter presenter;
        private BindingSource hardCashBindingSource;
        private bool controlsInitialized;

        /// <summary>
        /// Creates a new CashForm view.
        /// </summary>
        public CashForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CashFormPresenter(this, session);
            view.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            if (controlsInitialized)
                return;

            controlsInitialized = true;

            view.CellFormatting += CellFormatting;
            Enter += CashForm_Enter;
            Leave += CashForm_Leave;
            hardCashBindingSource = new BindingSource();
            DataTable table = new DataTable();
            table.Columns.Add("001"); table.Columns.Add("002"); table.Columns.Add("005");
            table.Columns.Add("010"); table.Columns.Add("020"); table.Columns.Add("050");
            table.Columns.Add("1"); table.Columns.Add("2"); table.Columns.Add("5");
            table.Columns.Add("10"); table.Columns.Add("20"); table.Columns.Add("50");
            table.Columns.Add("100"); table.Columns.Add("200"); table.Columns.Add("500");

            hardCashBindingSource.DataSource = table;
            _1centBox.DataBindings.Add("Value", hardCashBindingSource, "001", false, DataSourceUpdateMode.OnPropertyChanged);
            _2centBox.DataBindings.Add("Value", hardCashBindingSource, "002", false, DataSourceUpdateMode.OnPropertyChanged);
            _5centBox.DataBindings.Add("Value", hardCashBindingSource, "005", false, DataSourceUpdateMode.OnPropertyChanged);
            _10centBox.DataBindings.Add("Value", hardCashBindingSource, "010", false, DataSourceUpdateMode.OnPropertyChanged);
            _20centBox.DataBindings.Add("Value", hardCashBindingSource, "020", false, DataSourceUpdateMode.OnPropertyChanged);
            _50centBox.DataBindings.Add("Value", hardCashBindingSource, "050", false, DataSourceUpdateMode.OnPropertyChanged);
            _1EuroBox.DataBindings.Add("Value", hardCashBindingSource, "1", false, DataSourceUpdateMode.OnPropertyChanged);
            _2EuroBox.DataBindings.Add("Value", hardCashBindingSource, "2", false, DataSourceUpdateMode.OnPropertyChanged);
            _5EuroBox.DataBindings.Add("Value", hardCashBindingSource, "5", false, DataSourceUpdateMode.OnPropertyChanged);
            _10EuroBox.DataBindings.Add("Value", hardCashBindingSource, "10", false, DataSourceUpdateMode.OnPropertyChanged);
            _20EuroBox.DataBindings.Add("Value", hardCashBindingSource, "20", false, DataSourceUpdateMode.OnPropertyChanged);
            _50EuroBox.DataBindings.Add("Value", hardCashBindingSource, "50", false, DataSourceUpdateMode.OnPropertyChanged);
            _100EuroBox.DataBindings.Add("Value", hardCashBindingSource, "100", false, DataSourceUpdateMode.OnPropertyChanged);
            _200EuroBox.DataBindings.Add("Value", hardCashBindingSource, "200", false, DataSourceUpdateMode.OnPropertyChanged);
            _500EuroBox.DataBindings.Add("Value", hardCashBindingSource, "500", false, DataSourceUpdateMode.OnPropertyChanged);
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
        /// Handles the hard cash binding list changed list change.
        /// </summary>
        void hard_cash_binding_ListChanged(object sender, ListChangedEventArgs e)
        {
            presenter.UpdateHardCashAmount();
        }

        /// <summary>
        /// Handles the value Changed event for hard cash and updates the related state.
        /// </summary>
        private void hard_cash_ValueChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            presenter.EndHardCashBindingEdit();
        }

        /// <summary>
        /// Handles the cell format event.
        /// </summary>
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == categoryColumn.Index)
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
            if (DesignMode)
                return;

            ApplyCurrentUserRights();
            hardCashBindingSource.ListChanged -= hard_cash_binding_ListChanged;
            hardCashBindingSource.ListChanged += hard_cash_binding_ListChanged;
            await presenter.EnterAsync();
        }

        /// <summary>
        /// Handles the cash form leave leave event.
        /// </summary>
        void CashForm_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the click event for save Button and updates the related state.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            await presenter.SaveAsync();
        }

        /// <summary>
        /// Handles the validated event for hardcash and updates the related state.
        /// </summary>
        private void hardcash_Validated(object sender, EventArgs e)
        {
            presenter.EndHardCashEdit();
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
        /// Handles the click event for undo Button and updates the related state.
        /// </summary>
        private void undoButton_Click(object sender, EventArgs e)
        {
            presenter.Undo();
        }

        /// <summary>
        /// Handles the click event for book Button and updates the related state.
        /// </summary>
        private async void bookButton_Click(object sender, EventArgs e)
        {
            await presenter.BookAsync();
        }

        /// <summary>
        /// Handles the click event for period Check Box and updates the related state.
        /// </summary>
        private async void periodCheckBox_Click(object sender, EventArgs e)
        {
            await presenter.PeriodCheckAsync();
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
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            presenter.Export();
        }

        /// <summary>
        /// Handles the click event for automatic Button and updates the related state.
        /// </summary>
        private void automaticButton_Click(object sender, EventArgs e)
        {
            presenter.Automatic();
        }

        /// <summary>
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string ICashFormContract.DefaultSortColumn
        {
            get { return dateColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the currently sorted column name for the presenter.
        /// </summary>
        string ICashFormContract.CurrentSortColumn
        {
            get { return view.SortedColumn == null ? null : view.SortedColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the from date value for the presenter.
        /// </summary>
        DateTime ICashFormContract.FromDate
        {
            get { return fromDateBox.Date; }
        }

        /// <summary>
        /// Provides the to date value for the presenter.
        /// </summary>
        DateTime ICashFormContract.ToDate
        {
            get { return toDateBox.Date; }
        }

        /// <summary>
        /// Provides the period checked value for the presenter.
        /// </summary>
        bool ICashFormContract.PeriodChecked
        {
            get { return periodCheckBox.Checked; }
        }

        /// <summary>
        /// Provides the total amount text value for the presenter.
        /// </summary>
        string ICashFormContract.TotalAmountText
        {
            get { return totalAmountBox.Text; }
            set { totalAmountBox.Text = value; }
        }

        /// <summary>
        /// Provides the hard cash amount text value for the presenter.
        /// </summary>
        string ICashFormContract.HardCashAmountText
        {
            get { return hardCashAmountBox.Text; }
            set { hardCashAmountBox.Text = value; }
        }

        /// <summary>
        /// Runs the show cash book dialog view action for the presenter.
        /// </summary>
        bool ICashFormContract.ShowCashBookDialog(out CashBookingInput input)
        {
            using (Dialoge.CashBookDialog dialog = new Dialoge.CashBookDialog(Session))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new CashBookingInput
                {
                    BookText = dialog.BookText,
                    Amount = dialog.Amount,
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
        void ICashFormContract.SetPeriodControlsVisible(bool visible)
        {
            toDateBox.Visible = fromToLabel.Visible = visible;
        }

        /// <summary>
        /// Runs the set hard cash amount warning view action for the presenter.
        /// </summary>
        void ICashFormContract.SetHardCashAmountWarning(bool warning)
        {
            if (warning)
            {
                hardCashAmountBox.BackColor = Color.FromArgb(255, 74, 74);
                hardCashAmountBox.ForeColor = Color.White;
            }
            else
            {
                hardCashAmountBox.BackColor = Color.White;
                hardCashAmountBox.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void ICashFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        public void SetTable(DataTable hardCashTable)
        {
            hardCashBindingSource.DataSource = hardCashTable;
        }

        public void SetCashViewTable(DataTable table)
        {
            view.DataSource = table;
        }

        public void EndEditHardCash()
        {
            hardCashBindingSource.EndEdit();
        }

        public void SuspendBindingHardCash()
        {
            hardCashBindingSource.SuspendBinding();
        }

        public void ResumeBindingHardCash()
        {
            hardCashBindingSource.ResumeBinding();
        }

        public void Print(DataRow[] rows)
        {
            PrintBase cashPrinting = new PrintBase(Session, Data.Printing.LayoutEnum.cash);
            cashPrinting.Print(Text, Text, this, rows);
        }

        public void PrintQuittance(string clientName, List<DataRow> currentBooks)
        {
            Quittance quittance = new Quittance(Session);
            quittance.Print(clientName, clientName, this, currentBooks);
        }
    }
}
