using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class CashCheckUpFormPresenter
    {
        public CashCheckUpFormPresenter(ICashCheckUpFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ICashCheckUpFormContract View { get; private set; }

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
