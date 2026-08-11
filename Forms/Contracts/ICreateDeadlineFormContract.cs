using System;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreateDeadlineFormContract
    {
        DateTime DeadlineDate { get; set; }
        bool ShowYear { set; }
        bool ForAllMonths { get; }
        string Description { get; set; }
        void AcceptDialog();
        void Ok();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
