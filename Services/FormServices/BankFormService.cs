using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class BankFormService
    {
        public BankFormService(IBankFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IBankFormContract Form { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void Book()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void AllBooksCheck()
        {
        }

        public virtual void PeriodCheck()
        {
        }

        public virtual void Export()
        {
        }
    }
}
