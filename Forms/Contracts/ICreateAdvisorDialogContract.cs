using System.Data;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ICreateAdvisorDialogContract
    {
        string AdvisorIDText { get; set; }
        string AdvisorTitleText { get; }
        int AdvisorTitleIndex { get; set; }
        string AdvisorNameText { get; }
        string AdvisorEmailText { get; }
        string AdvisorCoText { get; }
        string AdvisorStreetText { get; }
        string AdvisorZipcodeText { get; }
        string AdvisorCityText { get; }
        void AddAdvisorTitle(string title);
        BindingSource CreateBindingSource(DataTable table, int position);
        void BindAdvisor(BindingSource bindingSource, ConvertEventHandler parseHandler);
        void RejectChanges(DataTable table);
        void SetDialogResultNone();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
