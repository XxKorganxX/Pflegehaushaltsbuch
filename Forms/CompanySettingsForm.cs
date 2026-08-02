using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Company Settings Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CompanySettingsForm : Pflegehaushaltsbuch.FormControls.Form, ICompanySettingsFormContract
    {
        private readonly CompanySettingsFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        BindingSource bs;
        /// <summary>
        /// Creates a new Company Settings Form instance and initializes the required state.
        /// </summary>
        public CompanySettingsForm()
        {
            InitializeComponent();
            presenter = new CompanySettingsFormPresenter(this);
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            bs = new BindingSource();
            bs.DataSource = new Company();
            secretaryBox.DataBindings.Add("Text", bs, "Secretary");
            companyBox.DataBindings.Add("Text", bs, "Name");
            streetBox.DataBindings.Add("Text", bs, "Street");
            zipcodeBox.DataBindings.Add("Text", bs, "Zipcode");
            cityBox.DataBindings.Add("Text", bs, "City");
            emailBox.DataBindings.Add("Text", bs, "Email");
            telBox.DataBindings.Add("Text", bs, "Phone");
            faxBox.DataBindings.Add("Text", bs, "Fax");
            webBox.DataBindings.Add("Text", bs, "Web");
            bankBox.DataBindings.Add("Text", bs, "Bank");
            ibanBox.DataBindings.Add("Text", bs, "Bank_iban");
            bicBox.DataBindings.Add("Text", bs, "Bank_bic");
            accountNoBox.DataBindings.Add("Text", bs, "Bank_account_no");
            bankCodeBox.DataBindings.Add("Text", bs, "Bank_code");
            localCourtBox.DataBindings.Add("Text", bs, "Local_court");
            hrbBox.DataBindings.Add("Text", bs, "Hrb");
            ikBox.DataBindings.Add("Text", bs, "Ik");
            logoAlignmentBox.DataBindings.Add("SelectedIndex", bs, "LogoAlignment");
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (Program.DesignMode)
                return;
            bs.DataSource = sql.Company;
            var value = XmlConfig.LoadXml();
            logoBox.BackgroundImage = sql.Company.Logo;
            logoBox.Invalidate();
        }
        /// <summary>
        /// Handles the click event for company Save Button and updates the related state.
        /// </summary>
        private async void companySaveButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await sql.Company.Save(sql);
                MessageBox.ShowDialog(this, Messages.company_saved);
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            sql.Printing.UpdateUserAndCompany(sql);
            ShowForm(Enums.Forms.Administration, sql);
        }
        /// <summary>
        /// Handles the click event for logo Box and updates the related state.
        /// </summary>
        private void logoBox_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            sql.Company.Logo = Image.FromFile(openFileDialog.FileName);
            logoBox.BackgroundImage = sql.Company.Logo;
            logoBox.Invalidate();
            sql.Printing.UpdateVariable(Printing.VarNames.company_logo, sql.Company.Logo);
        }
        /// <summary>
        /// Handles the validating event for email Box and updates the related state.
        /// </summary>
        private void emailBox_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(emailBox.Text) && !Company.IsValidEmail(emailBox.Text))
                e.Cancel = true;
        }
    }
}
