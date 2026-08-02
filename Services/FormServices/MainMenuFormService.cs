using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class MainMenuFormService
    {
        public MainMenuFormService(IMainMenuFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IMainMenuFormContract Form { get; private set; }

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

        public virtual void Exit()
        {
        }

        public virtual void Layout()
        {
        }

        public virtual void OfficeCash()
        {
        }
    }
}
