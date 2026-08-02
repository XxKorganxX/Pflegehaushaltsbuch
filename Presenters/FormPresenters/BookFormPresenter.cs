using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class BookFormPresenter
    {
        public BookFormPresenter(IBookFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IBookFormContract View { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void GetClientInfo()
        {
        }

        public virtual void UpdateDocumentNumbers()
        {
        }

        public virtual void Storno()
        {
        }

        public virtual void UpdateClientNote()
        {
        }

        public virtual void PrintAccount()
        {
        }

        public virtual void Book()
        {
        }

        public virtual void PeriodCheck()
        {
        }

        public virtual void Export()
        {
        }

        public virtual void Update()
        {
        }
    }
}
