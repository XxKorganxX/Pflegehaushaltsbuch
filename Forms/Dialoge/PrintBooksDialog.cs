using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Print Books Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PrintBooksDialog : Form, IPrintBooksDialogContract
    {
        private readonly PrintBooksDialogPresenter presenter;

        /// <summary>
        /// Creates a new PrintBooksDialog view.
        /// </summary>
        public PrintBooksDialog(SqlSession session, DataTable bookTable, int clientID, string clientAmount, DateTime from, DateTime to)
        {
            InitializeComponent();
            Session = session;
            presenter = new PrintBooksDialogPresenter(this, session, bookTable, clientID, from, to);
            presenter.Initialize();
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
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.PreparePrint();
        }

        /// <summary>
        /// Runs the prepare Print operation and updates the related application state.
        /// </summary>
        private void PreparePrint()
        {
            presenter.PreparePrint();
        }

        AdvisorPrintContact IPrintBooksDialogContract.AdvisorContact
        {
            get
            {
                return new AdvisorPrintContact
                {
                    Title = titleBox.Text,
                    Co = coBox.Text,
                    Name = clientNameBox.Text,
                    Street = clientStreetBox.Text,
                    Zipcode = clientZipcodeBox.Text,
                    City = clientCityBox.Text,
                    Email = emailBox.Text
                };
            }
        }

        /// <summary>
        /// Provides the statement note value for the presenter.
        /// </summary>
        string IPrintBooksDialogContract.StatementNote
        {
            get { return accountText.Text; }
        }

        void IPrintBooksDialogContract.BindTitles(IEnumerable<string> titles, int selectedIndex)
        {
            titleBox.Items.Clear();
            foreach (string title in titles)
                titleBox.Items.Add(title);
            titleBox.SelectedIndex = selectedIndex;
        }

        void IPrintBooksDialogContract.ShowAdvisorContact(AdvisorPrintContact contact)
        {
            titleBox.Text = contact.Title;
            coBox.Text = contact.Co;
            clientNameBox.Text = contact.Name;
            clientStreetBox.Text = contact.Street;
            clientZipcodeBox.Text = contact.Zipcode;
            clientCityBox.Text = contact.City;
            emailBox.Text = contact.Email;
        }

        void IPrintBooksDialogContract.PrintBooks(string documentTitle, string fileName, DataRow[] rows, string email)
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.accounts);
            printer.Print(documentTitle, Text + "_" + fileName, this, rows, email);
        }
    }
}
