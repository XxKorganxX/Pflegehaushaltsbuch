using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class PageSettingsFormService
    {
        public PageSettingsFormService(IPageSettingsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IPageSettingsFormContract Form { get; private set; }

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
