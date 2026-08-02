using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class CashFormService
    {
        public CashFormService(ICashFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected ICashFormContract Form { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void UpdateHardCashAmount()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Save()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Undo()
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

        public virtual void Automatic()
        {
        }
    }
}
