using System.Collections.Generic;
using System.Data;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    public interface IEditDataTableDialogContract
    {
        DataTable TableDesign { get; set; }
        IEnumerable<string> TableColumnDataPropertyNames { get; }
        string ColumnAlignColumnName { get; }
        string TextAlignColumnName { get; }
        string WidthColumnName { get; }

        void SetAutoGenerateColumns(bool autoGenerateColumns);
        void BindComboBoxColumn(string columnName, object dataSource, string displayMember, string valueMember);
        void SetColumnFormat(string columnName, string format);
        void SetDataSource(DataTable table);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
