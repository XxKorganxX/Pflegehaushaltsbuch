using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
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
                await ChangePasswordForCurrentUserAsync();
        }

        public virtual async Task ResetUserAsync()
        {
            User currentUser = session.User;
            string user = currentUser == null ? View.UserName : currentUser.Login;
            string keyword = currentUser == null ? View.Password : string.Empty;

            if (string.IsNullOrWhiteSpace(user))
                throw new Exception(Messages.login_insert_username);
            if (user.ToLower().StartsWith(Messages.login_guest))
                throw new Exception(Messages.login_guest_access_proteced);

            if (currentUser == null)
                await UserAuthenticator.LoginAsync(session.SQL, user, keyword);

            await ChangePasswordForCurrentUserAsync();

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

        private async Task ChangePasswordForCurrentUserAsync()
        {
            User currentUser = session.User;
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.Login))
                throw new Exception(Messages.login_insert_username);

            if (!View.ShowChangePasswordDialog(out string keyword))
                throw new Exception(Messages.login_keyword_unchanged);

            await User.UpdatePassword(session.SQL, currentUser.Login, keyword, currentUser.Login);
        }
    }
}
