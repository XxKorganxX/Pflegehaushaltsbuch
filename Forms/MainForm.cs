using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the MDI window and coordinates its user interface behavior.
    /// </summary>
    public partial class MainForm : Form, IMainFormContract
    {
        private readonly MainFormPresenter presenter;

        /// <summary>
        /// Creates a new MainForm view.
        /// </summary>
        public MainForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new MainFormPresenter(this, session);
            presenter.Initialize();
        }

        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            presenter.Closed();
        }

        /// <summary>
        /// Handles the load event for MDI and updates the related state.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            IMainFormContract view = this;

            MainMenuForm mainForm = new MainMenuForm(Session);
            ClientsForm clientForm = new ClientsForm(Session);
            ClientBooksForm bookForm = new ClientBooksForm(Session);
            OfficeCashForm officeCash = new OfficeCashForm(Session);
            CashForm cashForm = new CashForm(Session);
            EmployeesForm assistantsForm = new EmployeesForm(Session);
            DeadLinesForm deadLinesForm = new DeadLinesForm(Session);
            StatisticsForm inventoryForm = new StatisticsForm(Session);
            UserManagerForm userRightsForm = new UserManagerForm(Session);
            AdvisorForm advisorForm = new AdvisorForm(Session);
            CashCheckUpForm cashOfficeControlForm = new CashCheckUpForm(Session);
            BankForm bankForm = new BankForm(Session);
            CompanyForm companyForm = new CompanyForm(Session);
            LayoutManager layoutManager = new LayoutManager(Session);
            DocumentsForm recordForm = new DocumentsForm(Session);
            AdministrationForm administrationForm = new AdministrationForm(Session);
            ImprovedForm suggestionboxForm = new ImprovedForm(Session);
            DataExchangeForm dataExchangeForm = new DataExchangeForm(Session);
            AboutForm aboutForm = new AboutForm(Session);

            RegisterNavigation(view, 
            mainForm,
            administrationForm,
            clientForm,
            bookForm,
            officeCash,
            cashForm,
            assistantsForm,
            deadLinesForm,
            inventoryForm,
            userRightsForm,
            advisorForm,
            cashOfficeControlForm,
            bankForm,
            companyForm,
            layoutManager,
            recordForm,
            suggestionboxForm,
            dataExchangeForm,
            aboutForm);

            clientForm.ClientID_Changed += bookForm.OnClientID_Changed;
            clientForm.ClientID_Changed += deadLinesForm.OnClientID_Changed;
            mainForm.FormClosed += MainFormClosed;

            view.ConnectForm(mainForm, Enums.Forms.Main);
            view.ConnectForm(clientForm, Enums.Forms.Clients);
            view.ConnectForm(bookForm, Enums.Forms.Book);
            view.ConnectForm(officeCash, Enums.Forms.OfficeCash);
            view.ConnectForm(cashForm, Enums.Forms.Cash);
            view.ConnectForm(assistantsForm, Enums.Forms.Credits);
            view.ConnectForm(deadLinesForm, Enums.Forms.Calendar);
            view.ConnectForm(inventoryForm, Enums.Forms.Inventory);
            view.ConnectForm(userRightsForm, Enums.Forms.UserRights);
            view.ConnectForm(advisorForm, Enums.Forms.Advisor);
            view.ConnectForm(cashOfficeControlForm, Enums.Forms.CashOfficeControl);
            view.ConnectForm(bankForm, Enums.Forms.Banking);
            view.ConnectForm(companyForm, Enums.Forms.Company);
            view.ConnectForm(layoutManager, Enums.Forms.LayoutManager);
            view.ConnectForm(recordForm, Enums.Forms.Record);
            view.ConnectForm(administrationForm, Enums.Forms.Administration);
            view.ConnectForm(suggestionboxForm, Enums.Forms.Suggestionbox);
            view.ConnectForm(dataExchangeForm, Enums.Forms.DataExchange);
            view.ConnectForm(aboutForm, Enums.Forms.AboutUs);

            view.SelectForm(Enums.Forms.Main);
        }

        /// <summary>
        /// Runs the register navigation action.
        /// </summary>
        private void RegisterNavigation(IMainFormContract view, params Form[] forms)
        {
            foreach (Form form in forms)
                form.ShowForm += view.SelectForm;
        }

        /// <summary>
        /// Runs the main form closed action.
        /// </summary>
        private void MainFormClosed(object sender, FormClosedEventArgs e)
        {
            ((IMainFormContract)this).CloseView();
        }

        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        /// <summary>
        /// Runs the initialize automation view action for the presenter.
        /// </summary>
        void IMainFormContract.InitializeAutomation()
        {
            System.Windows.Automation.AutomationElement.FromHandle(Handle);
        }

        /// <summary>
        /// Runs the connect form view action for the presenter.
        /// </summary>
        void IMainFormContract.ConnectForm(Form form, Enums.Forms page)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            tabControl1.TabPages.Add(page.ToString(), page.ToString());

            TabPage tabPage = tabControl1.TabPages[page.ToString()];
            tabPage.Controls.Add(form);
            form.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Runs the select form view action for the presenter.
        /// </summary>
        void IMainFormContract.SelectForm(Enums.Forms selectForm)
        {
            TabPage tabPage = tabControl1.TabPages[selectForm.ToString()];
            Form form = tabPage.Controls[0] as Form;

            if (!form.Visible)
                form.Visible = true;

            tabControl1.SelectedTab = tabPage;
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IMainFormContract.CloseView()
        {
            Close();
        }
    }
}
