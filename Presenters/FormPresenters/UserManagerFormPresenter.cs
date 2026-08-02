using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class UserManagerFormPresenter
    {
        public UserManagerFormPresenter(IUserManagerFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IUserManagerFormContract View { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Save()
        {
        }

        public virtual void Create()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Delete()
        {
        }
    }
}
