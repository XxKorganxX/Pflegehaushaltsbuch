using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class UserLoginFormPresenter
    {
        public SqlSession session { get; private set; }

        public UserLoginFormPresenter(IUserLoginFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            View = view;
            this.session = session;
        }

        protected IUserLoginFormContract View { get; private set; }

        public virtual async Task LoginAsync()
        {
            string user = View.UserName;
            string keyword = View.Password;

            if (string.IsNullOrWhiteSpace(user))
                throw new Exception(Messages.login_insert_username);

            await UserAuthenticator.LoginAsync(session.SQL, user, keyword);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                DataTable users = new DataTable();
                await session.SQL.FillAdapterAsync(SQLBase.SELECT.Users, users);

                if (!View.ShowChangeUserDialog(session, user, keyword, users))
                    throw new Exception(Messages.login_keyword_unchanged);
            }
        }

        public virtual async Task ResetUserAsync()
        {
            string user = View.UserName;
            string keyword = View.Password;

            if (string.IsNullOrWhiteSpace(user))
                throw new Exception(Messages.login_insert_username);
            if (user.ToLower().StartsWith(Messages.login_guest))
                throw new Exception(Messages.login_guest_access_proteced);

            await UserAuthenticator.LoginAsync(session.SQL, user, keyword);

            DataTable users = new DataTable();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Users, users);

            if (!View.ShowChangeUserDialog(session, user, keyword, users))
                throw new Exception(Messages.login_keyword_unchanged);

            View.ShowUserDataChanged();
        }

        public virtual async Task ConnectAsync()
        {
            await LoginAsync();
            View.SetAccepted();
        }

        public virtual void Cancel()
        {
            View.CloseView();
        }

        public virtual async Task ResetAndAcceptAsync()
        {
            await ResetUserAsync();
            View.SetAccepted();
        }

        public virtual void PasswordEnter()
        {
            View.ClearPassword();
        }

        public virtual void Close()
        {
            View.CloseView();
        }
    }
}
