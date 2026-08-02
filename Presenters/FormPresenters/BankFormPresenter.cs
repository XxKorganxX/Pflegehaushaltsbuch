using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class BankFormPresenter
    {
        public BankFormPresenter(IBankFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IBankFormContract View { get; private set; }

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
