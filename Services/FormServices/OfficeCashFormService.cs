using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class OfficeCashFormService
    {
        public OfficeCashFormService(IOfficeCashFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IOfficeCashFormContract Form { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Book()
        {
        }

        public virtual void Print()
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
