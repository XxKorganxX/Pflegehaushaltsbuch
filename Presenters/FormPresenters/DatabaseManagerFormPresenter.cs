using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DatabaseManagerFormPresenter
    {
        public DatabaseManagerFormPresenter(IDatabaseManagerFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDatabaseManagerFormContract View { get; private set; }

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
