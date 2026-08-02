using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class CreateDeadlineFormService
    {
        public CreateDeadlineFormService(ICreateDeadlineFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected ICreateDeadlineFormContract Form { get; private set; }

        public virtual void Ok()
        {
        }
    }
}
