using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Main Menu Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class MainMenuForm : Pflegehaushaltsbuch.FormControls.Form, IMainMenuFormContract
    {
        private readonly MainMenuFormPresenter presenter;


        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms selectForm, SQLBase sql);
        public event OnShowForm ShowForm;
        //[System.Runtime.InteropServices.DllImport("gdi32.dll")]
        //private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
        //    IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        //private PrivateFontCollection fonts = new PrivateFontCollection();
        /// <summary>
        /// Creates a new Main Menu Form instance and initializes the required state.
        /// </summary>
        public MainMenuForm()
        {
            InitializeComponent();
            presenter = new MainMenuFormPresenter(this);
            if (Program.DesignMode)
                return;
            /*
            byte[] fontData = Properties.Resources.ELINA;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.ELINA.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.ELINA.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            */
            //label1.Font = new Font(fonts.Families[0], label1.Font.Size);
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true; // **** Always accept
                };
        }
        private bool firstRun = true;
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override async void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (Program.DesignMode)
                return;
            if (firstRun)
            {
                firstRun = false;
                try
                {
                    XmlConfig config = XmlConfig.LoadXml();
                    if (config.DBType != XmlConfig.DataBaseTypes.None)// !string.IsNullOrWhiteSpace(config.Host))
                    {
                        SQLBase sql = null;
                        if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                            sql = new MySQL();
                        else if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                            sql = new SQL();
                        else if (config.DBType == XmlConfig.DataBaseTypes.SQLite)
                            sql = new SQLITE();
                        await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                        await sql.Printing.LoadDocuments(sql);
                        UserLoginForm userLoginForm = new UserLoginForm(sql);
                        if (userLoginForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            this.sql = sql;
                            await sql.Company.Load(sql);
                        }
                    }
                }
                catch (Exception err) 
                {
                    sql = null;
                    MessageBox.ShowError(this, err);
                }
            }
            cashPanel.Enabled =
            bankingPanel.Enabled =
            clientsPanel.Enabled =
            advisorPanel.Enabled =
            assistantsPAnel.Enabled =
            cashCheckPanel.Enabled =
            statisticsPanel.Enabled =
            OfficeCashPanel.Enabled =
            recordPanel.Enabled =
            sql != null;
        }
        /// <summary>
        /// Runs the user Rights operation and updates the related application state.
        /// </summary>
        public void UserRights(int access, bool admin, bool supervisor)
        {
            adminPanel.Visible = admin | supervisor;
        }
        /// <summary>
        /// Handles the click event for client Management Button and updates the related state.
        /// </summary>
        private void clientManagementButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Clients, sql);
        }
        /// <summary>
        /// Handles the click event for cash Button and updates the related state.
        /// </summary>
        private void cashButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Cash, sql);
        }
        /// <summary>
        /// Handles the click event for credit Button and updates the related state.
        /// </summary>
        private void creditButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Credits, sql);
        }
        /// <summary>
        /// Handles the click event for account Holdings Button and updates the related state.
        /// </summary>
        private void accountHoldingsButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Inventory, sql);
        }
        /// <summary>
        /// Handles the click event for user Rights Button and updates the related state.
        /// </summary>
        private void userRightsButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Administration, sql);
        }
        /// <summary>
        /// Handles the click event for advisor Button and updates the related state.
        /// </summary>
        private void advisorButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Advisor, sql);
        }
        /// <summary>
        /// Handles the click event for cash Office Controlbutton and updates the related state.
        /// </summary>
        private void cashOfficeControlbutton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.CashOfficeControl, sql);
        }
        /// <summary>
        /// Handles the click event for banking Button and updates the related state.
        /// </summary>
        private void bankingButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Banking, sql);
        }
        /// <summary>
        /// Handles the click event for record Button and updates the related state.
        /// </summary>
        private void recordButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Record, sql);
        }
        /// <summary>
        /// Handles the click event for exit Button and updates the related state.
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Handles the 1 event for exit Button Click and updates the related state.
        /// </summary>
        private void exitButton_Click_1(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Handles the activated event for main Form and updates the related state.
        /// </summary>
        private void MainForm_Activated(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for layout Button and updates the related state.
        /// </summary>
        private void layoutButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.LayoutManager, sql);
        }
        /// <summary>
        /// Handles the click event for office Cash Button and updates the related state.
        /// </summary>
        private void officeCashButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.OfficeCash, sql);
        }     
    }
}
