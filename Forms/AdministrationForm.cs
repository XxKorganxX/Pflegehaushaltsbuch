using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Administration Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AdministrationForm : Pflegehaushaltsbuch.FormControls.Form, IAdministrationFormContract
    {
        private readonly AdministrationFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

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
        /// Creates a new Administration Form instance and initializes the required state.
        /// </summary>
        public AdministrationForm()
        {
            InitializeComponent();
            presenter = new AdministrationFormPresenter(this);
            /*
            byte[] fontData = Properties.Resources.ELINA;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.ELINA.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.ELINA.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            label1.Font = new Font(fonts.Families[0], label1.Font.Size);
            */
            if (Program.DesignMode)
                return;
            //this.Enter += MainForm_Enter;
        }
        /// <summary>
        /// Handles the enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (sql != null)
            {
                if (sql.User.Admin || sql.User.Supervisor)
                {
                    layoutButton.Enabled =
                    userRightsButton.Enabled =
                    companyButton.Enabled =
                    databaseBackupButton.Enabled =
                    restoreButton.Enabled =
                    disconnectDatabaseButton.Enabled =
                        true;
                }
                else
                {
                    layoutButton.Enabled =
                   userRightsButton.Enabled =
                   companyButton.Enabled =
                   databaseBackupButton.Enabled =
                   restoreButton.Enabled =
                   disconnectDatabaseButton.Enabled =
                       false;
                }
                improvedButton.Enabled =
                improvedButton.Enabled =
                    true;
            }
            else
            {
                layoutButton.Enabled =
                userRightsButton.Enabled =
                companyButton.Enabled =
                improvedButton.Enabled =
                databaseBackupButton.Enabled =
                restoreButton.Enabled =
                disconnectDatabaseButton.Enabled =
                improvedButton.Enabled =
                    false;
            }
            connectDatabaseButton.Enabled =
                sql == null;
        }
        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnUserRights(int access, bool admin, bool supervisor)
        {
        }
        /// <summary>
        /// Runs the user Rights operation and updates the related application state.
        /// </summary>
        public void UserRights(int access, bool admin, bool supervisor)
        {
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
            ShowForm(Enums.Forms.UserRights, sql);
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
        /// Handles the click event for license Button and updates the related state.
        /// </summary>
        private void licenseButton_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the 1 event for exit Button Click and updates the related state.
        /// </summary>
        private void exitButton_Click_1(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the click event for database Backup Button and updates the related state.
        /// </summary>
        private async void databaseBackupButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (backupFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
                await sql.BackupAsync(backupFileDialog.FileName);
                MessageBox.ShowDialog(this, Messages.database_backup);
            }
            finally 
            {
                databaseOperationLock.Release();
            }
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
        /// Handles the click event for restore Button and updates the related state.
        /// </summary>
        private async void restoreButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (openBackupFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;

                using (ProgressDialog progressForm = new ProgressDialog("Restore database.."))
                {
                    progressForm.Show(this);
                    Enabled = false;

                    try
                    {
                        await RestoreIntoStagingDatabaseAsync(openBackupFileDialog.FileName, progressForm);
                        MessageBox.ShowDialog(this, Messages.database_restore_staging_success);
                        OnEnter(null);
                    }
                    finally
                    {
                        progressForm.Close();
                        Enabled = true;
                    }
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Restores a backup into a new database and switches the configuration only after validation succeeded.
        /// </summary>
        private async Task RestoreIntoStagingDatabaseAsync(string backupFilename, ProgressDialog progressForm)
        {
            XmlConfig currentConfig = XmlConfig.LoadXml();
            XmlConfig stagingConfig = CreateStagingConfig(currentConfig);
            SQLBase restoredSql = CreateSqlProvider(stagingConfig.DBType);
            var currentUser = sql.User;
            bool restoreSucceeded = false;

            restoredSql.UpdateProgress += progressForm.UpdateProgress;
            restoredSql.UpdateMaximumProgress += progressForm.UpdateMaximumProgress;
            restoredSql.UpdateProgressText += progressForm.UpdateText;

            try
            {
                await Task.Run(async () =>
                {
                    await CreateStagingDatabaseAsync(restoredSql, stagingConfig);
                    await restoredSql.RestoreAsync(backupFilename);
                    await ValidateRestoredDatabaseAsync(restoredSql);
                    await restoredSql.OnLoadAsync();
                    await restoredSql.SetCurrentUserAsync(currentUser);
                    await restoredSql.Company.Load(restoredSql);
                });

                stagingConfig.Save();
                SQLBase previousSql = sql;
                sql = restoredSql;
                restoreSucceeded = true;
                previousSql.Dispose();
            }
            catch
            {
                restoredSql.Dispose();
                await CleanupStagingDatabaseAsync(stagingConfig);
                throw;
            }
            finally
            {
                restoredSql.UpdateProgress -= progressForm.UpdateProgress;
                restoredSql.UpdateMaximumProgress -= progressForm.UpdateMaximumProgress;
                restoredSql.UpdateProgressText -= progressForm.UpdateText;

                if (!restoreSucceeded)
                    restoredSql.Dispose();
            }
        }
        /// <summary>
        /// Creates a cloned config that points to a new restore target.
        /// </summary>
        private static XmlConfig CreateStagingConfig(XmlConfig currentConfig)
        {
            XmlConfig stagingConfig = (XmlConfig)currentConfig.Clone();
            string suffix = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

            if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
            {
                string currentDatabase = stagingConfig.Database;
                string directory = Path.GetDirectoryName(currentDatabase);
                if (string.IsNullOrWhiteSpace(directory))
                    directory = Environment.CurrentDirectory;

                string fileName = Path.GetFileNameWithoutExtension(currentDatabase);
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = "Verwahrgeld";

                string extension = Path.GetExtension(currentDatabase);
                if (string.IsNullOrWhiteSpace(extension))
                    extension = ".db";

                stagingConfig.Host = string.Empty;
                stagingConfig.Database = Path.Combine(directory, fileName + "_restore_" + suffix + extension);
                return stagingConfig;
            }

            stagingConfig.Database = CreateStagingDatabaseName(stagingConfig.Database, suffix);
            return stagingConfig;
        }
        /// <summary>
        /// Creates a provider-safe database name for SQL Server and MySQL staging restores.
        /// </summary>
        private static string CreateStagingDatabaseName(string database, string suffix)
        {
            StringBuilder name = new StringBuilder();
            foreach (char c in database ?? string.Empty)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                    name.Append(c);
                else
                    name.Append('_');
            }

            if (name.Length == 0 || char.IsDigit(name[0]))
                name.Insert(0, "Verwahrgeld_");

            return name + "_restore_" + suffix;
        }
        /// <summary>
        /// Creates the database target that receives the restore data.
        /// </summary>
        private static async Task CreateStagingDatabaseAsync(SQLBase restoredSql, XmlConfig stagingConfig)
        {
            if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
            {
                await restoredSql.CreateDataBaseAsync(stagingConfig.Database, stagingConfig.User, stagingConfig.Keyword, stagingConfig.Database);
                return;
            }

            await restoredSql.CreateDataBaseAsync(stagingConfig.Host, stagingConfig.User, stagingConfig.Keyword, stagingConfig.Database);
        }
        /// <summary>
        /// Performs a minimal structural validation before the app switches to the restored database.
        /// </summary>
        private static async Task ValidateRestoredDatabaseAsync(SQLBase restoredSql)
        {
            SQLBase.SELECT[] requiredTables =
            {
                SQLBase.SELECT.Advisors,
                SQLBase.SELECT.Clients,
                SQLBase.SELECT.Assistants,
                SQLBase.SELECT.Books,
                SQLBase.SELECT.Bank,
                SQLBase.SELECT.Barge,
                SQLBase.SELECT.OfficeCash,
                SQLBase.SELECT.Hardcash,
                SQLBase.SELECT.Records,
                SQLBase.SELECT.Deadlines,
                SQLBase.SELECT.Version
            };

            foreach (SQLBase.SELECT table in requiredTables)
            {
                DataTable dataTable = new DataTable();
                await restoredSql.FillAdapterAsync(table, dataTable);

                if (table == SQLBase.SELECT.Hardcash && dataTable.Rows.Count == 0)
                    throw new Exception(Messages.database_restore_missing_hardcash);

                if (table == SQLBase.SELECT.Version && dataTable.Rows.Count == 0)
                    throw new Exception(Messages.database_restore_missing_version);
            }

            await restoredSql.GetViewAsync("bank_total_amount");
            await restoredSql.GetViewAsync("barge_total_amount");
            await restoredSql.GetViewAsync("office_total_amount");
        }
        /// <summary>
        /// Removes a failed staging restore without touching the previously active database.
        /// </summary>
        private static async Task CleanupStagingDatabaseAsync(XmlConfig stagingConfig)
        {
            try
            {
                if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
                {
                    if (File.Exists(stagingConfig.Database))
                        File.Delete(stagingConfig.Database);
                    return;
                }

                using (SQLBase cleanupSql = CreateSqlProvider(stagingConfig.DBType))
                {
                    await cleanupSql.DropDatabaseAsync(stagingConfig.Host, stagingConfig.User, stagingConfig.Keyword, stagingConfig.Database);
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Creates the SQL provider for the selected database type.
        /// </summary>
        private static SQLBase CreateSqlProvider(XmlConfig.DataBaseTypes dbType)
        {
            if (dbType == XmlConfig.DataBaseTypes.SQL)
                return new SQL();
            if (dbType == XmlConfig.DataBaseTypes.MySQL)
                return new MySQL();
            if (dbType == XmlConfig.DataBaseTypes.SQLite)
                return new SQLITE();

            throw new NotSupportedException(Messages.database_connection_type_missing);
        }
        /// <summary>
        /// Handles the click event for reset Database and updates the related state.
        /// </summary>
        private void resetDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.ShowDialog(this, Messages.database_reset, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;
            XmlConfig.Disconnect();
            sql.Dispose();
            sql = null;
            OnEnter(null);
        }
        /// <summary>
        /// Handles the click event for improved Button and updates the related state.
        /// </summary>
        private void improvedButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.AboutUs, sql);
        }
        /// <summary>
        /// Handles the click event for db Connect Button and updates the related state.
        /// </summary>
        private async void dbConnectButton_Click(object sender, EventArgs e)
        {
            XmlConfig config = XmlConfig.LoadXml();
            using (DatabaseServerConnectForm connectForm = new DatabaseServerConnectForm(config))
            {
                if (connectForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
            }
            using (DatabaseManagerForm connectDBForm = new DatabaseManagerForm(config))
            {
                if (connectDBForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
                var sql = connectDBForm.SQL;
                await sql.Printing.LoadDocuments(sql);
                UserLoginForm userLoginForm = new UserLoginForm(sql);
                if (userLoginForm.ShowDialog(this) == DialogResult.OK)
                {
                    this.sql = sql;
                    await sql.Company.Load(sql);
                }
                OnEnter(null);
            }
        }
        /// <summary>
        /// Handles the click event for connect Embedded Database Button and updates the related state.
        /// </summary>
        private void connectEmbeddedDatabaseButton_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for company Button and updates the related state.
        /// </summary>
        private void companyButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Company, sql);
        }
        /// <summary>
        /// Handles the click event for design Button and updates the related state.
        /// </summary>
        private void designButton_Click(object sender, EventArgs e)
        {
            DesignForm SettingsForm = new DesignForm();
            SettingsForm.ShowDialog(this);
        }
    }
}
