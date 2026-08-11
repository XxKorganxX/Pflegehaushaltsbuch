using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ResetLayoutsDialogPresenter
    {
        private readonly IResetLayoutsDialogContract view;
        private readonly SqlSession session;

        public ResetLayoutsDialogPresenter(IResetLayoutsDialogContract view, SqlSession session)
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

        public virtual void ResetSelectedLayouts()
        {
            if (view.AllChecked)
            {
                session.SQL.Printing.ResetDocuments();
                return;
            }

            if (view.ClientsChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.clients);
            }

            if (view.AdvisorsChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.advisors);
            }

            if (view.EmployeeChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.employees);
            }

            if (view.CashChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.cash);
            }

            if (view.BankChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.bank);
            }

            if (view.BillChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.accounts);
            }

            if (view.CashCheckChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.cashaudit);
            }

            if (view.QuittanceChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.quittance);
            }

            if (view.OfficeCashChecked)
            {
                session.SQL.Printing.ResetDocument(Data.Printing.LayoutEnum.officecash);
            }
        }

        public virtual void UpdateAllSelectionFromLayouts()
        {
            view.AllChecked = view.ClientsChecked &&
                view.AdvisorsChecked &&
                view.EmployeeChecked &&
                view.CashChecked &&
                view.BankChecked &&
                view.BillChecked &&
                view.CashCheckChecked &&
                view.QuittanceChecked &&
                view.OfficeCashChecked;
        }

        public virtual void ApplyAllSelection()
        {
            bool isChecked = view.AllChecked;
            view.ClientsChecked = isChecked;
            view.AdvisorsChecked = isChecked;
            view.EmployeeChecked = isChecked;
            view.CashChecked = isChecked;
            view.BankChecked = isChecked;
            view.BillChecked = isChecked;
            view.CashCheckChecked = isChecked;
            view.QuittanceChecked = isChecked;
            view.OfficeCashChecked = isChecked;
        }
    }
}
