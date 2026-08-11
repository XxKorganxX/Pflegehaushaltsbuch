using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class MonthCalendarDialogPresenter
    {
        private readonly IMonthCalendarDialogContract view;

        public MonthCalendarDialogPresenter(IMonthCalendarDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(DateTime date)
        {
            view.SelectedDate = date;
            view.SetCalendarDate(date);
        }

        public virtual void SelectDate(DateTime date)
        {
            view.SelectedDate = date;
            view.CloseView();
        }
    }
}
