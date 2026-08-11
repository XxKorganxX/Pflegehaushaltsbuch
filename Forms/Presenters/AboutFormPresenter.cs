using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class AboutFormPresenter
    {
        private SqlSession session;

        public AboutFormPresenter(IAboutFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IAboutFormContract View { get; private set; }

        public virtual void Back()
        {
        }
    }
}
