using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ImprovedFormPresenter
    {
        private readonly SqlSession session;

        public ImprovedFormPresenter(IImprovedFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IImprovedFormContract View { get; private set; }

        public virtual void Send()
        {
            if (string.IsNullOrWhiteSpace(View.TextInput))
                throw new Exception(Messages.improved_missing_text);

            View.ShowRemovedLicenseServer();
            View.ShowForm(Enums.Forms.Administration);
        }

        public virtual void Back()
        {
            View.ShowForm(Enums.Forms.Administration);
        }
    }
}
