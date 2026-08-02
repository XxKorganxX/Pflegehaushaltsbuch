using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class CreationUserFormService
    {
        public CreationUserFormService(ICreationUserFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected ICreationUserFormContract Form { get; private set; }

        public virtual void BindData()
        {
        }

        public virtual void Ok()
        {
        }
    }
}
