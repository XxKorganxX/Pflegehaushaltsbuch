using System;
using System.Data;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Collections.Generic;
using System.Linq;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Edit Data Table Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EditDataTableDialog : Form, IEditDataTableDialogContract
    {
        private readonly EditDataTableDialogPresenter presenter;
        public DataTable TableDesign;

        /// <summary>
        /// Creates a new EditDataTableDialog view.
        /// </summary>
        public EditDataTableDialog(DataTable table)
        {
            InitializeComponent();
            presenter = new EditDataTableDialogPresenter(this);
            presenter.Initialize(table);
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Provides the table design value for the presenter.
        /// </summary>
        DataTable IEditDataTableDialogContract.TableDesign
        {
            get { return TableDesign; }
            set { TableDesign = value; }
        }

        /// <summary>
        /// Provides the table column data property names value for the presenter.
        /// </summary>
        IEnumerable<string> IEditDataTableDialogContract.TableColumnDataPropertyNames
        {
            get { return view.Columns.Cast<DataGridViewColumn>().Select(column => column.DataPropertyName); }
        }

        /// <summary>
        /// Provides the column align column name value for the presenter.
        /// </summary>
        string IEditDataTableDialogContract.ColumnAlignColumnName
        {
            get { return columnAlignColumn.Name; }
        }

        /// <summary>
        /// Provides the text align column name value for the presenter.
        /// </summary>
        string IEditDataTableDialogContract.TextAlignColumnName
        {
            get { return textAlignColumn.Name; }
        }

        /// <summary>
        /// Provides the width column name value for the presenter.
        /// </summary>
        string IEditDataTableDialogContract.WidthColumnName
        {
            get { return widthColumn.Name; }
        }

        /// <summary>
        /// Runs the set auto generate columns view action for the presenter.
        /// </summary>
        void IEditDataTableDialogContract.SetAutoGenerateColumns(bool autoGenerateColumns)
        {
            view.AutoGenerateColumns = autoGenerateColumns;
        }

        /// <summary>
        /// Runs the bind combo box column view action for the presenter.
        /// </summary>
        void IEditDataTableDialogContract.BindComboBoxColumn(string columnName, object dataSource, string displayMember, string valueMember)
        {
            DataGridViewComboBoxColumn column = view.Columns[columnName] as DataGridViewComboBoxColumn;
            if (column == null)
            {
                return;
            }

            column.DataSource = dataSource;
            column.DisplayMember = displayMember;
            column.ValueMember = valueMember;
        }

        /// <summary>
        /// Runs the set column format view action for the presenter.
        /// </summary>
        void IEditDataTableDialogContract.SetColumnFormat(string columnName, string format)
        {
            view.Columns[columnName].DefaultCellStyle.Format = format;
        }

        /// <summary>
        /// Runs the set data source view action for the presenter.
        /// </summary>
        void IEditDataTableDialogContract.SetDataSource(DataTable table)
        {
            view.DataSource = table;
        }
    }
}
