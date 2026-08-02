using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DatabaseServerConnectFormPresenter
    {
        public DatabaseServerConnectFormPresenter(IDatabaseServerConnectFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDatabaseServerConnectFormContract View { get; private set; }

        public virtual void ConfigureDatabaseTypeButtons()
        {
        }

        public virtual void UpdateDatabaseTypeButtons()
        {
        }

        public virtual void LoadDatabaseTypeIcon()
        {
        }

        public virtual void CreateStrongIconAttributes()
        {
        }

        public virtual void DatabaseType()
        {
        }

        public virtual void Connect()
        {
        }

        public virtual void Close()
        {
        }
    }
}
