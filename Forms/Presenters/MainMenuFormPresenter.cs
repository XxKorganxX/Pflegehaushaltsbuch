using Pflegehaushaltsbuch.Databases;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class MainMenuFormPresenter
    {
        public SqlSession session { get; private set; }

        private bool firstRun = true;

        public MainMenuFormPresenter(IMainMenuFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IMainMenuFormContract View { get; private set; }

        public virtual void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback += AcceptServerCertificate;
        }

        public virtual async Task EnterAsync()
        {
            if (firstRun)
            {
                firstRun = false;
                try
                {
                    SQLBase sql = await LoadSql();
                    if (sql != null)
                    {
                        session.Replace(sql);
                        if (View.ShowUserLoginDialog(session))
                            await LoadCompany(sql);
                        else
                            session.Disconnect();
                    }
                }
                catch (Exception err)
                {
                    session.Disconnect();
                    View.ShowError(err);
                }
            }

            View.SetWorkPanelsEnabled(session.SQL != null);
        }

        public virtual void UserRights(int access, bool admin, bool supervisor)
        {
            View.SetAdminVisible(admin | supervisor);
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

        public virtual void UserRights()
        {
            View.ShowForm(Enums.Forms.Administration);
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

        public virtual void Exit()
        {
        }

        public virtual void Layout()
        {
            View.ShowForm(Enums.Forms.LayoutManager);
        }

        public virtual void OfficeCash()
        {
            View.ShowForm(Enums.Forms.OfficeCash);
        }

        internal async Task<SQLBase> LoadSql()
        {
            XmlConfig config = XmlConfig.LoadXml();
            if (config.DBType != XmlConfig.DataBaseTypes.None)
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

                return sql;
            }

            return null;
        }

        internal async Task LoadCompany(SQLBase sql)
        {
            await sql.Company.Load(sql);
        }

        private static bool AcceptServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
