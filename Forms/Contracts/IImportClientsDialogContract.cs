using System;
using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IImportClientsDialogContract
    {
        string Seperator { get; }
        IEnumerable<string> ImportMappingItems { get; }
        void SetSeperator(string seperator);
        void SetImportedData(ImportsClientData data);
        void ApplyImportLabels(string[] labels);
        void BindClientTable(DataTable table);
        bool ShowOpenImportFilesDialog(out string[] fileNames);
        void ShowMessage(string message);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
        bool ShowErrorAndContinue(Exception err);
    }

    public struct ImportedClient
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public DateTime BornDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public int? AdvisorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public struct ImportsClientData
    {
        public ImportedClient[] Clients { get; set; }
    }
}
