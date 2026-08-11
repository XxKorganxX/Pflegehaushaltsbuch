using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Cash Check Up Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CashCheckUpForm : Form, ICashCheckUpFormContract
    {
        private readonly CashCheckUpFormPresenter presenter;

        /// <summary>
        /// Creates a new CashCheckUpForm view.
        /// </summary>
        public CashCheckUpForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CashCheckUpFormPresenter(this, session);
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the enter event for cash Office Control Form and updates the related state.
        /// </summary>
        private async void CashOfficeControlForm_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            presenter.Print();
        }

        /// <summary>
        /// Shows the calculated cash audit values.
        /// </summary>
        void ICashCheckUpFormContract.ShowCashAudit(CashCheckUpSummary summary)
        {
            clientsActiveBox.Text = summary.ClientsActive.ToString("C");
            clientsInActiveBox.Text = summary.ClientsInactive.ToString("C");
            clientsHistoryBox.Text = summary.ClientsHistory.ToString("C");
            clientsBox.Text = summary.ClientsTotal.ToString("C");
            amountEmployeesBox.Text = summary.AssistantsAmount.ToString("C");
            bankSaldoBox.Text = summary.BankSaldo.ToString("C");
            calculatedSaldoBox.Text = summary.CalculatedSaldo.ToString("C");
            differenceAmountBox.Text = summary.DifferenceAmount.ToString("C");
            cashHoldingBox.Text = summary.CashHolding.ToString("C");
            hardCashAmountBox.Text = summary.HardCashAmount.ToString("C");
        }

        /// <summary>
        /// Prints the current cash audit report.
        /// </summary>
        void ICashCheckUpFormContract.PrintCashAudit()
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.cashaudit);
            printer.Print(Text, Text, this);
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void ICashCheckUpFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }
    }
}
