using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class OfficeCashFormPresenter
    {
        public OfficeCashFormPresenter(IOfficeCashFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IOfficeCashFormContract View { get; private set; }

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
