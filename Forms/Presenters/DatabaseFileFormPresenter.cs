using Pflegehaushaltsbuch.Databases;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DatabaseFileFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        public DatabaseFileFormPresenter(IDatabaseFileFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IDatabaseFileFormContract View { get; private set; }

        public virtual async Task CreateAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                string databaseFileName;
                if (!View.ShowSaveFileDialog("Verwahrgeld", "database|*.db", "db", out databaseFileName))
                    return;

                    SQLITE sql = null;
                    try
                    {
                        sql = new SQLITE();
                        await sql.CreateDataBaseAsync(databaseFileName, string.Empty, View.Password, "Verwahrgeld");
                        await sql.OnLoadAsync();
                        SaveConfig(databaseFileName);
                        session.Replace(sql);
                        sql = null;
                        View.ShowDefaultLoginMessage();
                        View.AcceptDialog();
                    }
                    catch
                    {
                        if (sql != null)
                            sql.Dispose();
                        throw;
                    }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual async Task ConnectAsync()
        {
                XmlConfig config = XmlConfig.LoadXml();
                string databaseFileName;
                if (!View.ShowOpenFileDialog(config.Database, "database|*.db", out databaseFileName))
                    return;

                SQLITE sql = null;
                try
                {
                    sql = new SQLITE();
                    await sql.TestConnectionAsync(string.Empty, databaseFileName, string.Empty, View.Password);
                    await sql.OnLoadAsync();
                    SaveConfig(databaseFileName);
                    session.Replace(sql);
                    sql = null;
                    View.AcceptDialog();
                }
                catch
                {
                    if (sql != null)
                        sql.Dispose();
                    throw;
                }
        }

        private void SaveConfig(string databaseFileName)
        {
            XmlConfig config = XmlConfig.LoadXml();
            config.DBType = XmlConfig.DataBaseTypes.SQLite;
            config.User = string.Empty;
            config.Keyword = View.Password;
            config.Database = databaseFileName;
            config.Save();
        }
    }
}
