using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class ChangeUserFormService
    {
        public ChangeUserFormService(IChangeUserFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IChangeUserFormContract Form { get; private set; }

        public virtual void Ok()
        {
        }
    }
}
