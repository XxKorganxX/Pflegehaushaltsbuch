using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ClientBookDialogPresenter
    {
        private readonly IClientBookDialogContract view;
        private readonly SqlSession session;

        public ClientBookDialogPresenter(IClientBookDialogContract view, SqlSession session)
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

        public virtual void Initialize(string clientName, string clientID)
        {
            view.BindFields();
            view.AddBookingCategory(SQLBase.BookCategory.Einzahlung.GetDisplayName());
            view.AddBookingCategory(SQLBase.BookCategory.Auszahlung.GetDisplayName());
            view.AddBookingTarget(SQLBase.BookingTo.Barbestand.GetDisplayName());
            view.AddBookingTarget(SQLBase.BookingTo.Bankbestand.GetDisplayName());
            view.AddClient(clientName, clientID);
            view.SelectClient(clientName, clientID);
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
