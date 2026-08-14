using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Properties;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DatabaseManagerFormPresenter
    {
        private readonly SqlSession session;
        private readonly XmlConfig config;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private SQLBase connectedSql;
        private string[] databases = new string[0];

        public DatabaseManagerFormPresenter(IDatabaseManagerFormContract view, SqlSession session, XmlConfig config)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
            this.config = config;
        }

        protected IDatabaseManagerFormContract View { get; private set; }

        public virtual SQLBase DetachSql()
        {
            SQLBase detachedSql = connectedSql;
            connectedSql = null;
            return detachedSql;
        }

        public virtual void DisposeConnectedSql()
        {
            if (connectedSql != null)
            {
                connectedSql.Dispose();
                connectedSql = null;
            }
        }

        public virtual async Task ShownAsync()
        {
            using (SQLBase sql = CreateSqlProvider())
            {
                databases = await sql.GetAllDatabasesAsync(config.Host, config.User, config.Keyword);
            }

            View.ShowDatabases(databases, config.Database);
        }

        public virtual void Close()
        {
            View.CancelDialog();
        }

        public virtual void GetDataabase()
        {
            string databaseFileName;
            if (View.ShowOpenDatabaseDialog(out databaseFileName))
                config.Database = databaseFileName;
        }

        public virtual async Task ConnectAsync()
        {
            if (!string.IsNullOrWhiteSpace(View.SelectedDatabase))
                config.Database = View.SelectedDatabase;

            if (string.IsNullOrWhiteSpace(config.Database))
            {
                View.ShowEnterDatabaseName();
                return;
            }

            SQLBase sql = null;
            try
            {
                sql = CreateSqlProvider();
                await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                DisposeConnectedSql();
                connectedSql = sql;
                sql = null;
                config.Save();
                Settings.Default.Save();
                View.AcceptDialog();
            }
            catch
            {
                if (sql != null)
                    sql.Dispose();
                throw;
            }
        }

        public virtual async Task CreateDataBaseAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                string database = GetCreateDatabaseName();
                if (string.IsNullOrWhiteSpace(database))
                {
                    View.ShowEnterDatabaseName();
                    return;
                }

                if (!View.ConfirmDatabaseCreating())
                    return;

                SQLBase sql = null;
                using (IAdministrationProgress progressDialog = View.ShowProgressDialog(Messages.database + "..."))
                {
                    try
                    {
                        sql = CreateSqlProvider();
                        await sql.DropDatabaseAsync(config.Host, config.User, config.Keyword, database);
                        await sql.CreateDataBaseAsync(config.Host, config.User, config.Keyword, database);
                    }
                    finally
                    {
                        progressDialog.Close();
                    }
                }

                DisposeConnectedSql();
                connectedSql = sql;
                sql = null;

                SqlSession creationSession = new SqlSession();
                creationSession.Replace(connectedSql);
                try
                {
                    if (!View.ShowCreationUserDialog(creationSession))
                        return;
                }
                finally
                {
                    creationSession.Detach();
                }

                View.ShowDatabaseCreated();
                AddDatabase(database);
                config.Database = database;
                config.Save();
                View.ShowDatabases(databases, database);
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        private string GetCreateDatabaseName()
        {
            if (config.DBType != XmlConfig.DataBaseTypes.SQLite)
                return View.CreateDatabaseName;

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Verwahrgeld.db");
        }

        private void AddDatabase(string database)
        {
            foreach (string existingDatabase in databases)
            {
                if (existingDatabase.Equals(database, StringComparison.CurrentCultureIgnoreCase))
                    return;
            }

            string[] updatedDatabases = new string[databases.Length + 1];
            Array.Copy(databases, updatedDatabases, databases.Length);
            updatedDatabases[updatedDatabases.Length - 1] = database;
            databases = updatedDatabases;
        }

        public virtual async Task SqlUserAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                using (SQLBase sql = CreateSqlProvider())
                {
                    await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                    await sql.CreateUserAsync(View.UserName, View.Keyword, config.Database, View.FromHost);
                }

                View.ShowUserCreated();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Label5()
        {
            View.ToggleCreateDatabasePanel();
        }

        public virtual void Label7()
        {
            View.ToggleUserPanel();
        }

        public virtual void Label8()
        {
            View.ToggleConnectPanel();
        }

        public virtual async Task UpdateKeyWordAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (!View.MasterKeywordAgain.Equals(View.MasterKeyword))
                    throw new Exception(Messages.keywords_not_equal);

                SQLBase sql;
                if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                    sql = new SQL { TrustServerCertificate = config.TrustServerCertificate };
                else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                    sql = new MySQL();
                else
                    throw new Exception(Messages.sql_server_required);

                using (sql)
                {
                    await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                    await sql.CreateNewPasswordAsync(config.Host, config.User, config.Keyword, View.MasterKeyword);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task ChangeMasterkeyAsync()
        {
            try
            {
                await UpdateKeyWordAsync();
                View.ShowMasterPasswordChangedRestart();
                View.RestartApplication();
            }
            catch
            {
                View.KeepDialogOpen();
                throw;
            }
        }

        public virtual void ChangeMasterkeywordLabel()
        {
            View.ToggleMasterkeyPanel();
        }

        private SQLBase CreateSqlProvider()
        {
            XmlConfig.DataBaseTypes dbType = config.DBType;
            if (dbType == XmlConfig.DataBaseTypes.SQL)
                return new SQL { TrustServerCertificate = config.TrustServerCertificate };
            if (dbType == XmlConfig.DataBaseTypes.MySQL)
                return new MySQL();
            if (dbType == XmlConfig.DataBaseTypes.SQLite)
                return new SQLITE();

            throw new Exception(Messages.database_connection_type_missing);
        }
    }
}
