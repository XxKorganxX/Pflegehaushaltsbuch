using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class BookFormService
    {
        public BookFormService(IBookFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IBookFormContract Form { get; private set; }

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
