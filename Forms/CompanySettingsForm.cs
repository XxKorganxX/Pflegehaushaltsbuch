using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Company Settings Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CompanySettingsForm : Form, ICompanySettingsFormContract
    {
        private readonly CompanySettingsFormPresenter presenter;
        private BindingSource companyBindingSource;
        private bool controlsInitialized;

        /// <summary>
        /// Creates a new CompanySettingsForm view.
        /// </summary>
        public CompanySettingsForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CompanySettingsFormPresenter(this, session);
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

            companyBindingSource = new BindingSource();
            companyBindingSource.DataSource = new Company();
            BindCompanyControls();
            presenter.CreateControl();
        }

        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            presenter.Enter();
        }

        /// <summary>
        /// Handles the click event for company Save Button and updates the related state.
        /// </summary>
        private async void companySaveButton_Click(object sender, EventArgs e)
        {
            await presenter.CompanySaveAsync();
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the click event for logo Box and updates the related state.
        /// </summary>
        private void logoBox_Click(object sender, EventArgs e)
        {
            presenter.Logo();
        }

        /// <summary>
        /// Handles the validating event for email Box and updates the related state.
        /// </summary>
        private void emailBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !presenter.IsEmailValid();
        }

        /// <summary>
        /// Provides the email value for the presenter.
        /// </summary>
        string ICompanySettingsFormContract.Email
        {
            get { return emailBox.Text; }
        }

        /// <summary>
        /// Binds company data to the view controls.
        /// </summary>
        void ICompanySettingsFormContract.BindCompany(Company company)
        {
            companyBindingSource.DataSource = company;
        }

        /// <summary>
        /// Sets the displayed company logo.
        /// </summary>
        void ICompanySettingsFormContract.ShowCompanyLogo()
        {
            logoBox.BackgroundImage = Session.SQL.Company.Logo;
            logoBox.Invalidate();
        }

        /// <summary>
        /// Runs the show logo dialog view action for the presenter.
        /// </summary>
        bool ICompanySettingsFormContract.ShowLogoDialog(out string fileName)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                fileName = string.Empty;
                return false;
            }

            fileName = openFileDialog.FileName;
            return true;
        }

        /// <summary>
        /// Runs the show company saved view action for the presenter.
        /// </summary>
        void ICompanySettingsFormContract.ShowCompanySaved()
        {
            MessageBox.ShowDialog(this, Messages.company_saved);
        }

        /// <summary>
        /// Runs the show administration form view action for the presenter.
        /// </summary>
        void ICompanySettingsFormContract.ShowAdministrationForm()
        {
            ShowFormEvent(Enums.Forms.Administration);
        }

        private void BindCompanyControls()
        {
            secretaryBox.DataBindings.Add("Text", companyBindingSource, "Secretary");
            companyBox.DataBindings.Add("Text", companyBindingSource, "Name");
            streetBox.DataBindings.Add("Text", companyBindingSource, "Street");
            zipcodeBox.DataBindings.Add("Text", companyBindingSource, "Zipcode");
            cityBox.DataBindings.Add("Text", companyBindingSource, "City");
            emailBox.DataBindings.Add("Text", companyBindingSource, "Email");
            telBox.DataBindings.Add("Text", companyBindingSource, "Phone");
            faxBox.DataBindings.Add("Text", companyBindingSource, "Fax");
            webBox.DataBindings.Add("Text", companyBindingSource, "Web");
            bankBox.DataBindings.Add("Text", companyBindingSource, "Bank");
            ibanBox.DataBindings.Add("Text", companyBindingSource, "Bank_iban");
            bicBox.DataBindings.Add("Text", companyBindingSource, "Bank_bic");
            accountNoBox.DataBindings.Add("Text", companyBindingSource, "Bank_account_no");
            bankCodeBox.DataBindings.Add("Text", companyBindingSource, "Bank_code");
            localCourtBox.DataBindings.Add("Text", companyBindingSource, "Local_court");
            hrbBox.DataBindings.Add("Text", companyBindingSource, "Hrb");
            ikBox.DataBindings.Add("Text", companyBindingSource, "Ik");
            logoAlignmentBox.DataBindings.Add("SelectedIndex", companyBindingSource, "LogoAlignment");
        }
    }
}
