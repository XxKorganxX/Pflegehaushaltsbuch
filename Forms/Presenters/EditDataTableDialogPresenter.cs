using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class EditDataTableDialogPresenter
    {
        private readonly IEditDataTableDialogContract view;

        public EditDataTableDialogPresenter(IEditDataTableDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(DataTable table)
        {
            view.TableDesign = CreateTableDesign(table);
            view.SetAutoGenerateColumns(false);

            object alignmentItems = Enum.GetValues(typeof(StringAlignment))
                .Cast<StringAlignment>()
                .Select(p => new { Key = (int)p, Value = p.ToString() })
                .ToList();

            view.BindComboBoxColumn(view.ColumnAlignColumnName, alignmentItems, "Value", "Key");
            view.BindComboBoxColumn(view.TextAlignColumnName, alignmentItems, "Value", "Key");
            view.SetColumnFormat(view.WidthColumnName, "0.\\%");
            view.SetDataSource(view.TableDesign);
        }

        private DataTable CreateTableDesign(DataTable table)
        {
            if (table == null)
            {
                DataTable tableDesign = new DataTable();
                foreach (string columnName in view.TableColumnDataPropertyNames)
                {
                    tableDesign.Columns.Add(columnName);
                }

                return tableDesign;
            }

            if (table.Columns.Count >= 5)
            {
                return table;
            }

            return ConvertLegacyTable(table);
        }

        private static DataTable ConvertLegacyTable(DataTable table)
        {
            DataTable tableDesign = new DataTable("dataTable");
            tableDesign.Columns.Add("name");
            tableDesign.Columns.Add("id");
            tableDesign.Columns.Add("columnAlign", typeof(int));
            tableDesign.Columns.Add("textAlign", typeof(int));
            tableDesign.Columns.Add("width", typeof(int));
            tableDesign.Columns["textAlign"].DefaultValue = (int)StringAlignment.Near;
            tableDesign.Columns["columnAlign"].DefaultValue = (int)StringAlignment.Near;
            tableDesign.Columns["width"].DefaultValue = 0;

            foreach (DataRow r in table.Rows)
            {
                DataRow row = tableDesign.NewRow();
                row["name"] = r["name"];
                row["id"] = r["id"];
                row["width"] = (int)(float.Parse(r["weight"].ToString()) * 100.0f);
                tableDesign.Rows.Add(row);
            }

            return tableDesign;
        }
    }
}
