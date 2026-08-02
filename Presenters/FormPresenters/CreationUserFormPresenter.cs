using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class CreationUserFormPresenter
    {
        public CreationUserFormPresenter(ICreationUserFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ICreationUserFormContract View { get; private set; }

        public virtual void BindData()
        {
        }

        public virtual void Ok()
        {
        }
    }
}
