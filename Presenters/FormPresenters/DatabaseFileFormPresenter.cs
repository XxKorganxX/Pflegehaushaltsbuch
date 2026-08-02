using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DatabaseFileFormPresenter
    {
        public DatabaseFileFormPresenter(IDatabaseFileFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDatabaseFileFormContract View { get; private set; }

        public virtual void Create()
        {
        }

        public virtual void Connect()
        {
        }
    }
}
