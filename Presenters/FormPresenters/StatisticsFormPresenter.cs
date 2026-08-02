using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class StatisticsFormPresenter
    {
        public StatisticsFormPresenter(IStatisticsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IStatisticsFormContract View { get; private set; }

        public virtual void Back()
        {
        }

        public virtual void UpdateDealings()
        {
        }
    }
}
