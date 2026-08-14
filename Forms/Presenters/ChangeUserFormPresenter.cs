using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ChangeUserFormPresenter
    {
        public ChangeUserFormPresenter(IChangeUserFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IChangeUserFormContract View { get; private set; }

        public virtual void Ok()
        {
            try
            {
                string keyword = View.Keyword;
                string keywordAgain = View.KeywordAgain;

                if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(keywordAgain))
                    throw new Exception(Messages.login_enter_passwords);
                if (!keyword.Equals(keywordAgain))
                    throw new Exception(Messages.login_passwords_not_match);

                View.AcceptDialog();
            }
            catch
            {
                View.KeepDialogOpen();
                throw;
            }
        }
    }
}
