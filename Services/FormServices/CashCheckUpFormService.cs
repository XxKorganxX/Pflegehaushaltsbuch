using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class CashCheckUpFormService
    {
        public CashCheckUpFormService(ICashCheckUpFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected ICashCheckUpFormContract Form { get; private set; }

        public virtual void Back()
        {
        }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void UpdateCashHolding()
        {
        }

        public virtual void GetHardCashAmount()
        {
        }

        public virtual void Print()
        {
        }
    }
}
