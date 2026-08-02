using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class StatisticsFormService
    {
        public StatisticsFormService(IStatisticsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IStatisticsFormContract Form { get; private set; }

        public virtual void Back()
        {
        }

        public virtual void UpdateDealings()
        {
        }
    }
}
