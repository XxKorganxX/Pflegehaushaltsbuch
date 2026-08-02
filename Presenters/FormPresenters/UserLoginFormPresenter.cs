using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class UserLoginFormPresenter
    {
        public UserLoginFormPresenter(IUserLoginFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IUserLoginFormContract View { get; private set; }

        public virtual void Login()
        {
        }

        public virtual void ResetUser()
        {
        }

        public virtual void Connect()
        {
        }

        public virtual void Cancel()
        {
        }

        public virtual void Close()
        {
        }
    }
}
