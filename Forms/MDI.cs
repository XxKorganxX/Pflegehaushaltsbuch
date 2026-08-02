using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System.Globalization;
using System.Drawing.Text;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the MDI window and coordinates its user interface behavior.
    /// </summary>
    public partial class MDI : Pflegehaushaltsbuch.FormControls.Form, IMDIContract
    {
        private readonly MDIPresenter presenter;


        /// <summary>
        /// Creates a new MDI instance and initializes the required state.
        /// </summary>
        public MDI()
        {
            InitializeComponent();
            presenter = new MDIPresenter(this);
            System.Windows.Automation.AutomationElement.FromHandle(this.Handle);
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            SQLBase.UpdateVersion += SQL_UpdateVersion;
            SQL_UpdateVersion(null, null);
        }
        public string GetAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }
        void SQL_UpdateVersion(string sql_class, Version version)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    SQL_UpdateVersion(sql_class, version);
                });
                return;
            }
            if (version != null)
                Text = string.Format("{0} {1} - {2} {3}", GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title), Application.ProductVersion.Remove(Application.ProductVersion.LastIndexOf('.')), sql_class, version);
            else
                Text = string.Format("{0} {1} - {2}", GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title), Application.ProductVersion.Remove(Application.ProductVersion.LastIndexOf('.')), Messages.database_not_available);
        }
        /*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        */
        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (sql != null)
                sql.Dispose();
        }
        /// <summary>
        /// Connects the form data source or control used by the current workflow.
        /// </summary>
        private void ConnectForm(Form form, Enums.Forms page)
        {
            form.TopLevel = false;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            tabControl1.TabPages.Add(page.ToString(), page.ToString());
            TabPage tabPage = tabControl1.TabPages[page.ToString()];
            tabPage.Controls.Add(form);
            form.Dock = DockStyle.Fill;
        }
        void ShowForm(Enums.Forms selectForm, SQLBase sql)
        {
            var tabPage = tabControl1.TabPages[selectForm.ToString()];
            var form = tabPage.Controls[0] as FormControls.Form;
            form.OnUserRights(sql);
            if (!form.Visible)
                form.Visible = true;
            tabControl1.SelectedTab = tabPage;
        }
        /// <summary>
        /// Handles the load event for MDI and updates the related state.
        /// </summary>
        private void MDI_Load(object sender, EventArgs e)
        {
            MainMenuForm MainForm = new MainMenuForm();
            ClientsForm ClientForm = new ClientsForm();
            BookForm BookForm = new BookForm();
            OfficeCashForm officeCash = new OfficeCashForm();
            CashForm CashForm = new CashForm();
            AssistantsForm AssistantsForm = new AssistantsForm();
            DeadLinesForm DeadLinesForm = new DeadLinesForm();
            StatisticsForm InventoryForm = new StatisticsForm();
            UserManagerForm userRightsForm = new UserManagerForm();
            AdvisorForm advisorForm = new AdvisorForm();
            CashCheckUpForm CashOfficeControlForm = new CashCheckUpForm();
            BankForm bankForm = new BankForm();
            CompanySettingsForm companyForm = new CompanySettingsForm();
            LayoutManager layoutManager = new LayoutManager();
            DocumentsForm recordForm = new DocumentsForm();
            AdministrationForm administrationForm = new AdministrationForm();
            ImprovedForm suggestionboxForm = new ImprovedForm();
            AboutForm aboutForm = new AboutForm();
            MainForm.ShowForm += ShowForm;
            administrationForm.ShowForm += ShowForm;
            ClientForm.ShowForm += ShowForm;
            BookForm.ShowForm += ShowForm;
            officeCash.ShowForm += ShowForm;
            CashForm.ShowForm += ShowForm;
            AssistantsForm.ShowForm += ShowForm;
            DeadLinesForm.ShowForm += ShowForm;
            InventoryForm.ShowForm += ShowForm;
            userRightsForm.ShowForm += ShowForm;
            advisorForm.ShowForm += ShowForm;
            CashOfficeControlForm.ShowForm += ShowForm;
            bankForm.ShowForm += ShowForm;
            companyForm.ShowForm += ShowForm;
            layoutManager.ShowForm += ShowForm;
            recordForm.ShowForm += ShowForm;
            suggestionboxForm.ShowForm += ShowForm;
            aboutForm.ShowForm += ShowForm;
            ClientForm.ClientID_Changed += BookForm.OnClientID_Changed;
            ClientForm.ClientID_Changed += DeadLinesForm.OnClientID_Changed;
            MainForm.FormClosed += connectUserForm_FormClosed;
            ConnectForm(MainForm, Enums.Forms.Main);
            ConnectForm(ClientForm, Enums.Forms.Clients);
            ConnectForm(BookForm, Enums.Forms.Book);
            ConnectForm(officeCash, Enums.Forms.OfficeCash);
            ConnectForm(CashForm, Enums.Forms.Cash);
            ConnectForm(AssistantsForm, Enums.Forms.Credits);
            ConnectForm(DeadLinesForm, Enums.Forms.Calendar);
            ConnectForm(InventoryForm, Enums.Forms.Inventory);
            ConnectForm(userRightsForm, Enums.Forms.UserRights);
            ConnectForm(advisorForm, Enums.Forms.Advisor);
            ConnectForm(CashOfficeControlForm, Enums.Forms.CashOfficeControl);
            ConnectForm(bankForm, Enums.Forms.Banking);
            ConnectForm(companyForm, Enums.Forms.Company);
            ConnectForm(layoutManager, Enums.Forms.LayoutManager);
            ConnectForm(recordForm, Enums.Forms.Record);
            ConnectForm(administrationForm, Enums.Forms.Administration);
            ConnectForm(suggestionboxForm, Enums.Forms.Suggestionbox);
            ConnectForm(aboutForm, Enums.Forms.AboutUs);
            
            ShowForm(Enums.Forms.Main, sql);
            /*
            try
            {
                //Update Software
                using (WebClient Client = new WebClient())
                {
                    string version = Client.DownloadString("http://87.106.255.83/software/Pflegehaushaltsbuch/version.txt");
                    Version currentVersion, serverVersion;
                    if (Version.TryParse(version, out serverVersion))
                    {
                        Version.TryParse(Application.ProductVersion, out currentVersion);
                        if (serverVersion > currentVersion)
                        {
                            Application.Run(new UpdateForm(serverVersion, Client));
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            */
        }

        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        void connectUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}
