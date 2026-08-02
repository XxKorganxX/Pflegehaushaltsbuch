using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DatabaseManagerFormService
    {
        public DatabaseManagerFormService(IDatabaseManagerFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDatabaseManagerFormContract Form { get; private set; }

        public virtual void Close()
        {
        }

        public virtual void GetDataabase()
        {
        }

        public virtual void Connect()
        {
        }

        public virtual void CreateDataBase()
        {
        }

        public virtual void SqlUser()
        {
        }

        public virtual void Label5()
        {
        }

        public virtual void Label7()
        {
        }

        public virtual void Label8()
        {
        }

        public virtual void UpdateKeyWord()
        {
        }

        public virtual void ChangeMasterkeywordLabel()
        {
        }
    }
}
