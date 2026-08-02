using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class MainMenuFormPresenter
    {
        public MainMenuFormPresenter(IMainMenuFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IMainMenuFormContract View { get; private set; }

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
