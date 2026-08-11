using Pflegehaushaltsbuch.Databases;
using System;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IIoanPaybackDialogContract
    {
        decimal Amount { get; set; }
        decimal MaximumAmount { get; set; }
        int AssistantId { get; set; }
        string AssistantName { get; set; }
        DateTime PaybackDate { get; }
        int RepaymentIndex { get; }
        SQLBase.Repayment Repayment { get; }

        void AddRepayment(string repayment);
        void BindAmount();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
