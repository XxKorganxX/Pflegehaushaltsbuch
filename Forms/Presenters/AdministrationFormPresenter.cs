using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class AdministrationFormPresenter
    {
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        public SqlSession session { get; private set; }

        public AdministrationFormPresenter(IAdministrationFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IAdministrationFormContract View { get; private set; }

        public virtual void Enter()
        {
            if (View.IsDatabaseConnected)
            {
                View.SetAdministrationButtonsEnabled(View.CanAdministrateDatabase);
                View.SetImprovedEnabled(true);
            }
            else
            {
                View.SetAdministrationButtonsEnabled(false);
                View.SetImprovedEnabled(false);
            }

            View.SetConnectDatabaseEnabled(!View.IsDatabaseConnected);
        }

        public virtual void UserRights()
        {
            View.ShowForm(Enums.Forms.UserRights);
        }

        public virtual void ClientManagement()
        {
            View.ShowForm(Enums.Forms.Clients);
        }

        public virtual void Cash()
        {
            View.ShowForm(Enums.Forms.Cash);
        }

        public virtual void Credit()
        {
            View.ShowForm(Enums.Forms.Credits);
        }

        public virtual void AccountHoldings()
        {
            View.ShowForm(Enums.Forms.Inventory);
        }

        public virtual void Advisor()
        {
            View.ShowForm(Enums.Forms.Advisor);
        }

        public virtual void CashOfficeControl()
        {
            View.ShowForm(Enums.Forms.CashOfficeControl);
        }

        public virtual void Banking()
        {
            View.ShowForm(Enums.Forms.Banking);
        }

        public virtual void Record()
        {
            View.ShowForm(Enums.Forms.Record);
        }

        public virtual void License()
        {
        }

        public virtual async Task DatabaseBackupAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
            {
                return;
            }

            try
            {
                string fileName;
                if (!View.ShowBackupFileDialog(out fileName))
                {
                    return;
                }

                await session.SQL.BackupAsync(fileName);
                View.ShowDatabaseBackupSuccess();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Layout()
        {
            View.ShowForm(Enums.Forms.LayoutManager);
        }

        public virtual async Task RestoreAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
            {
                return;
            }

            try
            {
                string fileName;
                if (!View.ShowRestoreFileDialog(out fileName))
                {
                    return;
                }

                using (IAdministrationProgress progressDialog = View.ShowProgressDialog("Restore database.."))
                {
                    View.SetViewEnabled(false);
                    try
                    {
                        await RestoreIntoStagingDatabaseAsync(fileName, progressDialog);
                        View.ShowDatabaseRestoreSuccess();
                        View.RefreshAccessState();
                    }
                    finally
                    {
                        progressDialog.Close();
                        View.SetViewEnabled(true);
                    }
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void ResetDatabase()
        {
            if (!View.ConfirmDatabaseReset())
            {
                return;
            }

            XmlConfig.Disconnect();
            session.Disconnect();
            View.RefreshAccessState();
        }

        public virtual async Task DbConnectAsync()
        {
            XmlConfig config = XmlConfig.LoadXml();
            if (!View.ShowDatabaseServerConnectDialog(session, config))
            {
                return;
            }

            SQLBase sql;
            if (!View.ShowDatabaseManagerDialog(session, config, out sql))
            {
                return;
            }

            await sql.Printing.LoadDocuments(sql);
            SqlSession loginSession = new SqlSession();
            loginSession.Replace(sql);
            sql = null;

            try
            {
                SQLBase authenticatedSql;
                if (View.ShowUserLoginDialog(loginSession, out authenticatedSql))
                {
                    try
                    {
                        await authenticatedSql.Company.Load(authenticatedSql);
                        session.Replace(authenticatedSql);
                        authenticatedSql = null;
                    }
                    finally
                    {
                        if (authenticatedSql != null)
                        {
                            authenticatedSql.Dispose();
                        }
                    }
                }
            }
            finally
            {
                loginSession.Dispose();
                if (sql != null)
                {
                    sql.Dispose();
                }
            }

            View.RefreshAccessState();
        }

        public virtual void ConnectEmbeddedDatabase()
        {
        }

        public virtual void Company()
        {
            View.ShowForm(Enums.Forms.Company);
        }

        public virtual void Design()
        {
            View.ShowDesignDialog(session);
        }

        public virtual void DataExchange()
        {
            View.ShowForm(Enums.Forms.DataExchange);
        }

        public virtual void Main()
        {
            View.ShowForm(Enums.Forms.Main);
        }

        private async Task RestoreIntoStagingDatabaseAsync(string backupFilename, IAdministrationProgress progressDialog)
        {
            XmlConfig currentConfig = XmlConfig.LoadXml();
            XmlConfig stagingConfig = CreateStagingConfig(currentConfig);
            SQLBase restoredSql = CreateSqlProvider(stagingConfig.DBType);
            var currentUser = session.SQL.User;
            bool restoreSucceeded = false;

            restoredSql.UpdateProgress += progressDialog.UpdateProgress;
            restoredSql.UpdateMaximumProgress += progressDialog.UpdateMaximumProgress;
            restoredSql.UpdateProgressText += progressDialog.UpdateText;

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
                session.Replace(restoredSql);
                restoreSucceeded = true;
            }
            catch
            {
                restoredSql.Dispose();
                await CleanupStagingDatabaseAsync(stagingConfig);
                throw;
            }
            finally
            {
                restoredSql.UpdateProgress -= progressDialog.UpdateProgress;
                restoredSql.UpdateMaximumProgress -= progressDialog.UpdateMaximumProgress;
                restoredSql.UpdateProgressText -= progressDialog.UpdateText;

                if (!restoreSucceeded)
                {
                    restoredSql.Dispose();
                }
            }
        }

        private static XmlConfig CreateStagingConfig(XmlConfig currentConfig)
        {
            XmlConfig stagingConfig = (XmlConfig)currentConfig.Clone();
            string suffix = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

            if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
            {
                string currentDatabase = stagingConfig.Database;
                string directory = Path.GetDirectoryName(currentDatabase);
                if (string.IsNullOrWhiteSpace(directory))
                {
                    directory = Environment.CurrentDirectory;
                }

                string fileName = Path.GetFileNameWithoutExtension(currentDatabase);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "Verwahrgeld";
                }

                string extension = Path.GetExtension(currentDatabase);
                if (string.IsNullOrWhiteSpace(extension))
                {
                    extension = ".db";
                }

                stagingConfig.Host = string.Empty;
                stagingConfig.Database = Path.Combine(directory, fileName + "_restore_" + suffix + extension);
                return stagingConfig;
            }

            stagingConfig.Database = CreateStagingDatabaseName(stagingConfig.Database, suffix);
            return stagingConfig;
        }

        private static string CreateStagingDatabaseName(string database, string suffix)
        {
            StringBuilder name = new StringBuilder();
            foreach (char c in database ?? string.Empty)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    name.Append(c);
                }
                else
                {
                    name.Append('_');
                }
            }

            if (name.Length == 0 || char.IsDigit(name[0]))
            {
                name.Insert(0, "Verwahrgeld_");
            }

            return name + "_restore_" + suffix;
        }

        private static async Task CreateStagingDatabaseAsync(SQLBase restoredSql, XmlConfig stagingConfig)
        {
            if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
            {
                await restoredSql.CreateDataBaseAsync(stagingConfig.Database, stagingConfig.User, stagingConfig.Keyword, stagingConfig.Database);
                return;
            }

            await restoredSql.CreateDataBaseAsync(stagingConfig.Host, stagingConfig.User, stagingConfig.Keyword, stagingConfig.Database);
        }

        private static async Task ValidateRestoredDatabaseAsync(SQLBase restoredSql)
        {
            SQLBase.SELECT[] requiredTables =
            {
                SQLBase.SELECT.Advisors,
                SQLBase.SELECT.Clients,
                SQLBase.SELECT.Emploees,
                SQLBase.SELECT.Books,
                SQLBase.SELECT.Bank,
                SQLBase.SELECT.Cash,
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
                {
                    throw new Exception(Messages.database_restore_missing_hardcash);
                }

                if (table == SQLBase.SELECT.Version && dataTable.Rows.Count == 0)
                {
                    throw new Exception(Messages.database_restore_missing_version);
                }
            }

            await restoredSql.GetViewAsync("bank_total_amount");
            await restoredSql.GetViewAsync("cash_total_amount");
            await restoredSql.GetViewAsync("office_total_amount");
        }

        private static async Task CleanupStagingDatabaseAsync(XmlConfig stagingConfig)
        {
            try
            {
                if (stagingConfig.DBType == XmlConfig.DataBaseTypes.SQLite)
                {
                    if (File.Exists(stagingConfig.Database))
                    {
                        File.Delete(stagingConfig.Database);
                    }

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

        private static SQLBase CreateSqlProvider(XmlConfig.DataBaseTypes dbType)
        {
            if (dbType == XmlConfig.DataBaseTypes.SQL)
            {
                return new SQL();
            }

            if (dbType == XmlConfig.DataBaseTypes.MySQL)
            {
                return new MySQL();
            }

            if (dbType == XmlConfig.DataBaseTypes.SQLite)
            {
                return new SQLITE();
            }

            throw new NotSupportedException(Messages.database_connection_type_missing);
        }
    }
}
