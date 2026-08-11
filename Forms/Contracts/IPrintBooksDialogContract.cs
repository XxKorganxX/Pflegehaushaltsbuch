using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IPrintBooksDialogContract
    {
        AdvisorPrintContact AdvisorContact { get; }
        string StatementNote { get; }
        void BindTitles(IEnumerable<string> titles, int selectedIndex);
        void ShowAdvisorContact(AdvisorPrintContact contact);
        void PrintBooks(string documentTitle, string fileName, DataRow[] rows, string email);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public class AdvisorPrintContact
    {
        public string Title { get; set; }
        public string Co { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }
}
