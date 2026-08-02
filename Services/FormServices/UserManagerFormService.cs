using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class UserManagerFormService
    {
        public UserManagerFormService(IUserManagerFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IUserManagerFormContract Form { get; private set; }

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
