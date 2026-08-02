using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Edit Data Table Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EditDataTableDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        public DataTable TableDesign;
        /// <summary>
        /// Creates a new Edit Data Table Form instance and initializes the required state.
        /// </summary>
        public EditDataTableDialog(DataTable table)
        {
            InitializeComponent();
            if (table == null)
            {
                TableDesign = new DataTable();
                foreach (DataGridViewColumn column in view.Columns)
                    TableDesign.Columns.Add(column.DataPropertyName);
                table = TableDesign;
            }
            else if (table.Columns.Count < 5)
            {
                TableDesign = new DataTable("dataTable");
                TableDesign.Columns.Add("name");
                TableDesign.Columns.Add("id");
                TableDesign.Columns.Add("columnAlign", typeof(int));
                TableDesign.Columns.Add("textAlign", typeof(int));//StringAlignment));
                TableDesign.Columns.Add("width", typeof(int));
                TableDesign.Columns["textAlign"].DefaultValue = StringAlignment.Near;
                TableDesign.Columns["columnAlign"].DefaultValue = StringAlignment.Near;
                TableDesign.Columns["width"].DefaultValue = 0;
                
                foreach (DataRow r in table.Rows)
                {
                    DataRow row = TableDesign.NewRow();
                    row["name"] = r["name"];
                    row["id"] = r["id"];
                    row["width"] = (int)(float.Parse(r["weight"].ToString()) * 100.0f);
                    TableDesign.Rows.Add(row);
                }
                table = TableDesign;
            }
            else
            {
                TableDesign = table;
            }
            view.AutoGenerateColumns = false;
            (view.Columns[columnAlignColumn.Name] as DataGridViewComboBoxColumn).DataSource =
                Enum.GetValues(typeof(StringAlignment)).Cast<StringAlignment>()
                .Select(p => new { Key = (int)p, Value = p.ToString() })
                .ToList();
            (view.Columns[columnAlignColumn.Name] as DataGridViewComboBoxColumn).DisplayMember = "Value";
            (view.Columns[columnAlignColumn.Name] as DataGridViewComboBoxColumn).ValueMember = "Key";
            (view.Columns[textAlignColumn.Name] as DataGridViewComboBoxColumn).DataSource =
                Enum.GetValues(typeof(StringAlignment)).Cast<StringAlignment>()
                .Select(p => new { Key = (int)p, Value = p.ToString() })
                .ToList();
            (view.Columns[textAlignColumn.Name] as DataGridViewComboBoxColumn).DisplayMember = "Value";
            (view.Columns[textAlignColumn.Name] as DataGridViewComboBoxColumn).ValueMember = "Key";
            view.Columns[widthColumn.Name].DefaultCellStyle.Format = "0.\\%";
            view.DataSource = TableDesign;
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
        }
    }
}
