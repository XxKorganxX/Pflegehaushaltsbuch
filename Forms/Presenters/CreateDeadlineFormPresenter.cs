using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreateDeadlineFormPresenter
    {
        private readonly SqlSession session;

        public CreateDeadlineFormPresenter(ICreateDeadlineFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected ICreateDeadlineFormContract View { get; private set; }

        public virtual void Initialize(DateTime dateTime, string description)
        {
            View.DeadlineDate = dateTime;
            View.ShowYear = false;
            View.Description = description;
        }

        public virtual void Ok()
        {
            View.AcceptDialog();
        }
    }
}
