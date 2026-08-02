using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class ImprovedFormPresenter
    {
        public ImprovedFormPresenter(IImprovedFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IImprovedFormContract View { get; private set; }

        public virtual void OnUserRights()
        {
        }

        public virtual void Send()
        {
        }

        public virtual void Back()
        {
        }
    }
}
