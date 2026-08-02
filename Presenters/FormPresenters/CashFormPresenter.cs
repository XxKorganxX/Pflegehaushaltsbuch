using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class CashFormPresenter
    {
        public CashFormPresenter(ICashFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ICashFormContract View { get; private set; }

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
