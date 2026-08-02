using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class AboutFormPresenter
    {
        public AboutFormPresenter(IAboutFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IAboutFormContract View { get; private set; }

        public virtual void Back()
        {
        }
    }
}
