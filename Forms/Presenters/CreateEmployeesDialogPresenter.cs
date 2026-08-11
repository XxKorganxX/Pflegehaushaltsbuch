using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreateEmployeesDialogPresenter
    {
        private readonly ICreateEmployeesDialogContract view;

        public CreateEmployeesDialogPresenter(ICreateEmployeesDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(int id)
        {
            InitializeCommon();
            view.Date = DateTime.Now;
            view.ID = id;
            view.BindFields();
        }

        public virtual void Initialize(int id, string name, DateTime date, decimal amount)
        {
            InitializeCommon();
            view.ID = id;
            view.AssistantName = name;
            view.Date = date;
            view.Amount = amount;
            view.BindFields();

            if (amount != 0)
                view.SetAmountEnabled(false);
        }

        public virtual void Ok()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(view.AssistantName))
                    throw new Exception(Messages.assistants_name_missing);
                if (view.Date > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
            }
            catch
            {
                view.SetDialogResultNone();
                throw;
            }
        }

        private void InitializeCommon()
        {
            view.AddBookAccount(SQLBase.BookingTo.Barbestand.GetDisplayName());
            view.SetBookAccountIndex(0);
        }
    }
}
