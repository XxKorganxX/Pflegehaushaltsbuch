using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DesignFormPresenter
    {
        public DesignFormPresenter(IDesignFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDesignFormContract View { get; private set; }

        public virtual void SelectPath()
        {
        }
    }
}
