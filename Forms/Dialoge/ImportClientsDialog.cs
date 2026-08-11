using System;
using System.Data;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Import Clients Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ImportClientsDialog : Form, IImportClientsDialogContract
    {

        private readonly ImportClientsDialogPresenter presenter;
        public string Seperator { get; set; }
        public ImportsClientData Data { get; set; }

        /// <summary>
        /// Creates a new ImportClientsDialog view.
        /// </summary>
        public ImportClientsDialog(SqlSession session, int clients, DataTable clientTable, DataTable advisorTable)
        {
            InitializeComponent();
            Session = session;
            presenter = new ImportClientsDialogPresenter(this, session);
            presenter.Initialize(clientTable, advisorTable);
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.AcceptImport();
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the cell Content Click event for data Grid View1 and updates the related state.
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for load Button and updates the related state.
        /// </summary>
        private void loadButton_Click(object sender, EventArgs e)
        {
            presenter.LoadFilesFromDialog();
        }
        /// <summary>
        /// Handles the drag Drop event for import View and updates the related state.
        /// </summary>
        private void importView_DragDrop(object sender, DragEventArgs e)
        {
            if (MoveImportMappingItem(e))
                presenter.UpdateImportMapping();
        }
        /// <summary>
        /// Handles the item Drag event for import View and updates the related state.
        /// </summary>
        private void importView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            importView.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        /// <summary>
        /// Handles the drag Enter event for import View and updates the related state.
        /// </summary>
        private void importView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        /// <summary>
        /// Handles the drag Over event for import View and updates the related state.
        /// </summary>
        private void importView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Provides the import mapping items value for the presenter.
        /// </summary>
        IEnumerable<string> IImportClientsDialogContract.ImportMappingItems
        {
            get { return importView.Items.Cast<ListViewItem>().Select(item => item.Text).ToArray(); }
        }

        /// <summary>
        /// Sets the import separator.
        /// </summary>
        void IImportClientsDialogContract.SetSeperator(string seperator)
        {
            Seperator = seperator;
            seperatorBox.DataBindings.Clear();
            seperatorBox.DataBindings.Add("Text", this, "Seperator");
        }

        /// <summary>
        /// Stores the imported client data for the caller.
        /// </summary>
        void IImportClientsDialogContract.SetImportedData(ImportsClientData data)
        {
            Data = data;
        }

        /// <summary>
        /// Runs the show message view action for the presenter.
        /// </summary>
        void IImportClientsDialogContract.ShowMessage(string message)
        {
            MessageBox.ShowDialog(this, message);
        }

        /// <summary>
        /// Runs the show error and continue view action for the presenter.
        /// </summary>
        bool IImportClientsDialogContract.ShowErrorAndContinue(System.Exception err)
        {
            return MessageBox.ShowErrorDialog(this, err, MessageBoxButtons.OKCancel) != DialogResult.Cancel;
        }

        /// <summary>
        /// Runs the apply import labels view action for the presenter.
        /// </summary>
        void IImportClientsDialogContract.ApplyImportLabels(string[] labels)
        {
            for (int i = 0; i < labels.Length && i < importView.Items.Count; i++)
            {
                importView.Items[i].Text = labels[i];
            }

            for (int i = 0; i < labels.Length && i < view.Columns.Count; i++)
            {
                view.Columns[i].HeaderText = labels[i];
            }
        }

        /// <summary>
        /// Runs the bind client table view action for the presenter.
        /// </summary>
        void IImportClientsDialogContract.BindClientTable(DataTable table)
        {
            view.AutoGenerateColumns = false;
            view.DataSource = table;
        }

        /// <summary>
        /// Runs the show open import files dialog view action for the presenter.
        /// </summary>
        bool IImportClientsDialogContract.ShowOpenImportFilesDialog(out string[] fileNames)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Text Dateien|*.txt|CSV|*.csv";
                fileDialog.Multiselect = true;

                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    fileNames = new string[0];
                    return false;
                }

                fileNames = fileDialog.FileNames;
                return true;
            }
        }

        /// <summary>
        /// Runs the move import mapping item view action for the presenter.
        /// </summary>
        private bool MoveImportMappingItem(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                return false;
            }

            Point pos = importView.PointToClient(new Point(e.X, e.Y));
            ListViewHitTestInfo hit = importView.HitTest(pos);
            if (hit.Item == null)
            {
                return false;
            }

            ListViewItem item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            int index = importView.Items.IndexOf(hit.Item);
            importView.Items.Remove(item);
            importView.Items.Insert(index, item);
            return true;
        }

    }
}
