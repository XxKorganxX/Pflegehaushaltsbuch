using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class IoanPaybackDialogPresenter
    {
        private readonly IIoanPaybackDialogContract view;

        public IoanPaybackDialogPresenter(IIoanPaybackDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(string name, int id, decimal amount)
        {
            if (Program.DesignMode)
            {
                return;
            }

            foreach (SQLBase.Repayment enumval in Enum.GetValues(typeof(SQLBase.Repayment)))
            {
                view.AddRepayment(enumval.GetDisplayName());
            }

            view.AssistantId = id;
            view.MaximumAmount = amount;
            view.Amount = amount;
            view.AssistantName = name;
            view.BindAmount();
        }

        public virtual void Accept()
        {
            if (view.Amount <= 0)
            {
                throw new Exception(Messages.ioan_invalid_amount);
            }

            if (view.Amount > view.MaximumAmount)
            {
                throw new Exception(Messages.ioan_invalid_amount);
            }

            if (view.PaybackDate == DateTime.MinValue || view.PaybackDate > DateTime.Now)
            {
                throw new Exception(Messages.invalid_date);
            }
        }
    }
}
