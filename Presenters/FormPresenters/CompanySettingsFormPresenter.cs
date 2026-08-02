using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class CompanySettingsFormPresenter
    {
        public CompanySettingsFormPresenter(ICompanySettingsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ICompanySettingsFormContract View { get; private set; }

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
