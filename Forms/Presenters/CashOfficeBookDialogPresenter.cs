using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CashOfficeBookDialogPresenter
    {
        private readonly ICashOfficeBookDialogContract view;
        private readonly SqlSession session;

        public CashOfficeBookDialogPresenter(ICashOfficeBookDialogContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
        }

        public virtual void Initialize()
        {
            view.Amount = 0;
            view.Account = 0;
            view.SetBookingCategoryIndex(0);
            view.BindFields();
        }

        public virtual void ValidateOk()
        {
            try
            {
                if (view.Amount == 0)
                    throw new Exception(Messages.missing_amount);
                if (string.IsNullOrWhiteSpace(view.BookText))
                    throw new Exception(Messages.missing_bookingtext);
                if (view.BookingDate == DateTime.MinValue || view.BookingDate > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
            }
            catch
            {
                view.SetDialogResultNone();
                throw;
            }
        }
    }
}
