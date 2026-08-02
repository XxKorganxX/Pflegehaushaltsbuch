using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class PageSettingsFormPresenter
    {
        public PageSettingsFormPresenter(IPageSettingsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IPageSettingsFormContract View { get; private set; }

        public virtual void Ok()
        {
        }

        public virtual void LeftText()
        {
        }

        public virtual void CenterText()
        {
        }

        public virtual void RightText()
        {
        }

        public virtual void TopText()
        {
        }

        public virtual void BottomText()
        {
        }
    }
}
