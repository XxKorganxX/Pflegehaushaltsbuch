using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreationUserFormPresenter
    {
        private readonly SqlSession session;
        private bool editMode;
        private string oldUsername;

        public CreationUserFormPresenter(ICreationUserFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected ICreationUserFormContract View { get; private set; }

        public virtual void InitializeNew()
        {
            View.Phone = session.SQL.Company.Phone;
            View.Fax = session.SQL.Company.Fax;
            View.InsertAllowed = true;
            View.ChangeAllowed = true;
            View.BindData();
        }

        public virtual void InitializeExisting(DataRow row)
        {
            oldUsername = row["name"].ToString();
            View.UserName = oldUsername;
            View.Login = row["login"].ToString();
            View.Phone = row["phone"].ToString();
            View.Fax = row["fax"].ToString();
            View.Email = row["email"].ToString();

            int access = Int32.Parse(row["access"].ToString());
            View.Admin = bool.Parse(row["admin"].ToString());
            View.InsertAllowed = (access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert;
            View.ChangeAllowed = (access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change;
            View.DeleteAllowed = (access & (int)Enums.UserRightEnum.Delete) == (int)Enums.UserRightEnum.Delete;
            editMode = true;
            View.BindData();
        }

        public virtual async Task OkAsync()
        {
            if (string.IsNullOrWhiteSpace(View.UserName))
                throw new Exception(Messages.name);
            if (string.IsNullOrWhiteSpace(View.Email))
                throw new Exception(Messages.email);

            int access = GetAccess();
            if (editMode)
            {
                await User.UpdateUser(session.SQL,
                    oldUsername,
                    View.UserName,
                    View.Login,
                    View.Phone,
                    View.Fax,
                    View.Email,
                    access,
                    View.Admin);
                View.ShowUserChanged();
            }
            else
            {
                string login = View.Login;
                if (string.IsNullOrWhiteSpace(login))
                {
                    login = View.UserName;
                    View.Login = login;
                }

                await User.CreateUser(session.SQL,
                    View.UserName,
                    login,
                    string.Empty,
                    View.Phone,
                    View.Fax,
                    View.Email,
                    access,
                    View.Admin);
                View.ShowUserCreated();
            }

            View.AcceptDialog();
        }

        public virtual bool IsEmailValid()
        {
            return Company.IsValidEmail(View.Email);
        }

        private int GetAccess()
        {
            Enums.UserRightEnum userRight = Enums.UserRightEnum.None;
            if (View.InsertAllowed)
                userRight |= Enums.UserRightEnum.Insert;
            if (View.ChangeAllowed)
                userRight |= Enums.UserRightEnum.Change;
            if (View.DeleteAllowed)
                userRight |= Enums.UserRightEnum.Delete;
            return (int)userRight;
        }
    }
}
