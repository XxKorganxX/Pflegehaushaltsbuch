using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class UserLoginFormService
    {
        public UserLoginFormService(IUserLoginFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IUserLoginFormContract Form { get; private set; }

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
