using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;
using System.Threading.Tasks;
using Pflegehaushaltsbuch.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Print Clients Books Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PrintClientsBooksDialog : Form, IPrintClientsBooksDialogContract
    {
        private readonly PrintClientsBooksDialogPresenter presenter;

        /// <summary>
        /// Creates a new PrintClientsBooksDialog view.
        /// </summary>
        public PrintClientsBooksDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new PrintClientsBooksDialogPresenter(this, session);
        }

        /// <summary>
        /// Handles the shown event for PDF Books Form and updates the related state.
        /// </summary>
        private async void PDFBooksForm_Shown(object sender, EventArgs e)
        {
            await presenter.ShownAsync();
        }

        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                await presenter.PrintAsync();
            }
            catch
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
                throw;
            }
        }

        /// <summary>
        /// Prints the print output for the current workflow.
        /// </summary>
        private async void Print()
        {
            await presenter.PrintAsync();
        }

        /// <summary>
        /// Provides the selected printer value for the presenter.
        /// </summary>
        string IPrintClientsBooksDialogContract.SelectedPrinter
        {
            get { return printerBox.SelectedItem == null ? string.Empty : printerBox.SelectedItem.ToString(); }
        }

        /// <summary>
        /// Provides the has selected printer value for the presenter.
        /// </summary>
        bool IPrintClientsBooksDialogContract.HasSelectedPrinter
        {
            get { return printerBox.SelectedItem != null; }
        }

        /// <summary>
        /// Provides the has selected clients value for the presenter.
        /// </summary>
        bool IPrintClientsBooksDialogContract.HasSelectedClients
        {
            get { return clientView.SelectedItems.Count > 0; }
        }

        /// <summary>
        /// Provides the selected clients value for the presenter.
        /// </summary>
        IEnumerable<ID_Client_Data> IPrintClientsBooksDialogContract.SelectedClients
        {
            get { return clientView.SelectedItems.Cast<ID_Client_Data>().ToArray(); }
        }

        /// <summary>
        /// Provides the selected date value for the presenter.
        /// </summary>
        DateTime IPrintClientsBooksDialogContract.SelectedDate
        {
            get { return dateTimeBox.Date; }
        }

        /// <summary>
        /// Provides the statement note value for the presenter.
        /// </summary>
        string IPrintClientsBooksDialogContract.StatementNote
        {
            get { return accountText.Text; }
        }

        void IPrintClientsBooksDialogContract.BindPrinters(IEnumerable<string> printerNames, string selectedPrinter)
        {
            printerBox.Items.Clear();
            foreach (string printerName in printerNames)
                printerBox.Items.Add(printerName);
            printerBox.SelectedItem = selectedPrinter;
        }

        void IPrintClientsBooksDialogContract.BindClients(IEnumerable<ID_Client_Data> clients)
        {
            clientView.Items.Clear();
            foreach (ID_Client_Data client in clients)
                clientView.Items.Add(client);
        }

        void IPrintClientsBooksDialogContract.PrintClientBooks(string printerName, string fileName, DataRow[] rows, string email)
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.accounts);
            printer.PrintDirect(printerName, Text + "_" + fileName, this, rows, email);
        }
    }
}
