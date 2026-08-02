using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class CreateDeadlineFormPresenter
    {
        public CreateDeadlineFormPresenter(ICreateDeadlineFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ICreateDeadlineFormContract View { get; private set; }

        public virtual void Ok()
        {
        }
    }
}
