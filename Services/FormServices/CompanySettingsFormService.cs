using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class CompanySettingsFormService
    {
        public CompanySettingsFormService(ICompanySettingsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected ICompanySettingsFormContract Form { get; private set; }

        public virtual void CompanySave()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Logo()
        {
        }
    }
}
