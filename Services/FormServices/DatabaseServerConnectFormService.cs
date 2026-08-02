using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DatabaseServerConnectFormService
    {
        public DatabaseServerConnectFormService(IDatabaseServerConnectFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDatabaseServerConnectFormContract Form { get; private set; }

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
