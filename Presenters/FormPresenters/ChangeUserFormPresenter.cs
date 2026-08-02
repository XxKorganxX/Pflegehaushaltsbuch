using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class ChangeUserFormPresenter
    {
        public ChangeUserFormPresenter(IChangeUserFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IChangeUserFormContract View { get; private set; }

        public virtual void Ok()
        {
        }
    }
}
