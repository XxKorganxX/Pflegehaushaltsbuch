using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class AboutFormService
    {
        public AboutFormService(IAboutFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IAboutFormContract Form { get; private set; }

        public virtual void Back()
        {
        }
    }
}
