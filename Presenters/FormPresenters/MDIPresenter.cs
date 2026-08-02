using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class MDIPresenter
    {
        public MDIPresenter(IMDIContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IMDIContract View { get; private set; }

        public virtual void Connect()
        {
        }
    }
}
