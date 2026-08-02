using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class AdministrationFormPresenter
    {
        public AdministrationFormPresenter(IAdministrationFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IAdministrationFormContract View { get; private set; }

        public virtual void OnUserRights()
        {
        }

        public virtual void UserRights()
        {
        }

        public virtual void ClientManagement()
        {
        }

        public virtual void Cash()
        {
        }

        public virtual void Credit()
        {
        }

        public virtual void AccountHoldings()
        {
        }

        public virtual void Advisor()
        {
        }

        public virtual void CashOfficeControl()
        {
        }

        public virtual void Banking()
        {
        }

        public virtual void Record()
        {
        }

        public virtual void License()
        {
        }

        public virtual void DatabaseBackup()
        {
        }

        public virtual void Layout()
        {
        }

        public virtual void Restore()
        {
        }

        public virtual void ResetDatabase()
        {
        }

        public virtual void Improved()
        {
        }

        public virtual void DbConnect()
        {
        }

        public virtual void ConnectEmbeddedDatabase()
        {
        }

        public virtual void Company()
        {
        }

        public virtual void Design()
        {
        }
    }
}
