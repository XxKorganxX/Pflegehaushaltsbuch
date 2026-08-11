using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ChangeUserFormPresenter
    {
        private readonly SqlSession session;
        private string oldkeyword;

        public ChangeUserFormPresenter(IChangeUserFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IChangeUserFormContract View { get; private set; }

        public virtual void Initialize(string username, string keyword, DataTable users)
        {
            DataRow[] rows = users.Rows
                .OfType<DataRow>()
                .Where(userRow => User.MatchesIdentity(userRow, username))
                .ToArray();

            View.UserName = rows[0]["name"].ToString();
            View.Login = rows[0]["login"].ToString();
            oldkeyword = keyword;
        }

        public virtual async Task OkAsync()
        {
            try
            {
                string username = View.UserName;
                string login = View.Login;
                string keyword = View.Keyword;
                string keywordAgain = View.KeywordAgain;

                if (string.IsNullOrWhiteSpace(username))
                    throw new Exception(Messages.login_name);
                if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(keywordAgain))
                    throw new Exception(Messages.login_enter_passwords);
                if (!keyword.Equals(keywordAgain))
                    throw new Exception(Messages.login_passwords_not_match);

                await User.UpdateLogin(session.SQL, username, oldkeyword, username, login, keyword);
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
