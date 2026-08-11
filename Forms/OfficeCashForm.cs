using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataGridView = Pflegehaushaltsbuch.FormControls.DataGridView;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Office Cash Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class OfficeCashForm : Form, IOfficeCashFormContract
    {
        private readonly OfficeCashFormPresenter presenter;

        /// <summary>
        /// Creates a new OfficeCashForm view.
        /// </summary>
        public OfficeCashForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new OfficeCashFormPresenter(this, session);
            view.AutoGenerateColumns = false;
            view.CellFormatting += CellFormatting;
        }
        /// <summary>
        /// Handles the format event for cash Form and updates the related state.
        /// </summary>
        private void CashForm_Format(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
        }
        /// <summary>
        /// Handles the cell format event.
        /// </summary>
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == bookCat.Index)
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
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            presenter.ApplyUserRights(rights);
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }
        /// <summary>
        /// Handles the click event for book Button and updates the related state.
        /// </summary>
        private async void bookButton_Click(object sender, EventArgs e)
        {
            await presenter.BookAsync();
        }
        /// <summary>
        /// Handles the enter event for cash Office Form and updates the related state.
        /// </summary>
        private async void CashOfficeForm_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            await presenter.EnterAsync();
        }
        /// <summary>
        /// Handles the selection Change Committed event for account Box and updates the related state.
        /// </summary>
        private void accountBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private async void printButton_Click(object sender, EventArgs e)
        {
            await presenter.PrintAsync();
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
        /// Runs the show cash office book dialog action.
        /// </summary>
        public bool ShowCashOfficeBookDialog(out CashOfficeBookingInput input)
        {
            input = new CashOfficeBookingInput();

            using (CashOfficeBookDialog cashOfficeForm = new CashOfficeBookDialog(Session))
            {
                if (cashOfficeForm.ShowDialog(this) != DialogResult.OK)
                    return false;

                input = new CashOfficeBookingInput
                {
                    BookingDate = cashOfficeForm.BookingDate,
                    BookText = cashOfficeForm.BookText,
                    Amount = cashOfficeForm.Amount,
                    BookingCategory = cashOfficeForm.BookingCategory,
                    Account = cashOfficeForm.Account
                };

                return true;
            }
        }

        /// <summary>
        /// Provides the from date value for the presenter.
        /// </summary>
        DateTime IOfficeCashFormContract.FromDate
        {
            get { return fromDateBox.Date; }
        }

        /// <summary>
        /// Provides the to date value for the presenter.
        /// </summary>
        DateTime IOfficeCashFormContract.ToDate
        {
            get { return toDateBox.Date; }
        }

        /// <summary>
        /// Provides the period checked value for the presenter.
        /// </summary>
        bool IOfficeCashFormContract.PeriodChecked
        {
            get { return periodCheckBox.Checked; }
        }

        IEnumerable<DataRow> IOfficeCashFormContract.OfficeCashRows
        {
            get
            {
                List<DataRow> rows = new List<DataRow>();
                foreach (DataGridViewRow rowView in view.Rows)
                {
                    DataRowView dataRowView = rowView.DataBoundItem as DataRowView;
                    if (dataRowView != null)
                        rows.Add(dataRowView.Row);
                }

                return rows;
            }
        }

        void IOfficeCashFormContract.SetTotalAmount(string totalAmount)
        {
            totalAmountBox.Text = totalAmount;
        }

        /// <summary>
        /// Runs the set supervisor rights view action for the presenter.
        /// </summary>
        void IOfficeCashFormContract.SetSupervisorRights(bool enabled)
        {
            updateButton.Visible = enabled;
            view.AllowUserToDeleteRows = enabled;
        }

        /// <summary>
        /// Runs the set book buttons enabled view action for the presenter.
        /// </summary>
        void IOfficeCashFormContract.SetBookButtonsEnabled(bool enabled)
        {
            bookButton.Enabled = stornoButton.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set period controls visible view action for the presenter.
        /// </summary>
        void IOfficeCashFormContract.SetPeriodControlsVisible(bool visible)
        {
            toDateBox.Visible = fromToLabel.Visible = visible;
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IOfficeCashFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        void IOfficeCashFormContract.PrintOfficeCash(IEnumerable<DataRow> rows)
        {
            PrintBase cashPrinting = new PrintBase(Session, Data.Printing.LayoutEnum.officecash);
            cashPrinting.Print(Text, Text, this, rows.ToList());
        }

        public void BindOfficeCash(DataTable table)
        {
            if (view.SortedColumn != null)
                table.DefaultView.Sort = view.SortedColumn.DataPropertyName;
            else
                table.DefaultView.Sort = dateColumn.DataPropertyName;

            view.DataSource = table;
        }
    }
}
