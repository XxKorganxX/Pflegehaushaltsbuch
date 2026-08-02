using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Import Clients Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ImportClientsDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        DataTable clientTable, advisorTable;
        public string Seperator { get; set; }
        public int DebitorNr { get; set; }
        public int Title { get; set; }
        public int ClientName { get; set; }
        public int Born { get; set; }
        public int Street { get; set; }
        public int ZopCode { get; set; }
        public int City { get; set; }
        public int Advisor { get; set; }
        public int Cash { get; set; }
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
        public ImportsClientData Data { get; private set; }
        /// <summary>
        /// Creates a new Import Clients Form instance and initializes the required state.
        /// </summary>
        public ImportClientsDialog(SQLBase sql, int clients, DataTable clientTable, DataTable advisorTable)
        {
            InitializeComponent();
            this.sql = sql;
            this.clientTable = clientTable;
            this.advisorTable = advisorTable;
            Seperator = ";;";
            UpdateImportTree();
            /*
            clientTable = new DataTable();
            sql.FillAdapter(SQLBase.SELECT.Client, clientTable, 0);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns["id"] };
            */
            view.AutoGenerateColumns = false;
            view.DataSource = clientTable;
            /*
            advisorTable = new DataTable();
            sql.FillAdapter(SQLBase.SELECT.Advisors, advisorTable);
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns["name"] };
            */
            seperatorBox.DataBindings.Add("Text", this, "Seperator");
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable addedRows = clientTable.GetChanges(DataRowState.Added);
                Data = new ImportsClientData
                {
                    Clients = addedRows == null
                        ? new ImportedClient[0]
                        : addedRows.Rows.OfType<DataRow>().Select(CreateImportedClient).ToArray()
                };
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        private ImportedClient CreateImportedClient(DataRow row)
        {
            return new ImportedClient
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Name = row["name"].ToString(),
                Street = row["street"].ToString(),
                Zipcode = row["zipcode"].ToString(),
                City = row["city"].ToString(),
                BornDate = DateTime.Parse(row["born"].ToString()),
                OpeningBalance = Convert.ToDecimal(row["account_transfer"]),
                AdvisorId = row["advisor_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["advisor_id"]),
                CreatedDate = DateTime.Parse(row["date"].ToString())
            };
        }        /// <summary>
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
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Text Dateien|*.txt|CSV|*.csv";
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
                string[] filenames = fileDialog.FileNames;

                string[] seperator = new string[] { Seperator };
                foreach (string filename in filenames)
                {
                    using (StreamReader reader = new StreamReader(filename, Encoding.Default))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            try
                            {
                                string[] data = line.Split(seperator, StringSplitOptions.None);
                                int id = Int32.Parse(data[DebitorNr]);
                                string title = data[Title];
                                string clientName = data[ClientName];
                                DateTime born = DateTime.Parse(data[Born]);
                                string street = data[Street];
                                string zipCode = data[ZopCode];
                                string city = data[City];
                                string advisor = sql.TrimBetween(data[Advisor]);
                                decimal cash = 0;
                                decimal.TryParse(data[Cash], out cash);
                                if (!title.Equals("Frau") && !title.Equals("Herr"))
                                {
                                    MessageBox.ShowDialog(this, string.Format(Messages.import_clients_invalid_title, clientName));
                                    continue;
                                }
                                if (id < 1)
                                {
                                    MessageBox.ShowDialog(this, string.Format(Messages.import_clients_invalid_debtor_no, clientName));
                                    continue;
                                }
                                DataRow row = row = clientTable.NewRow();
                                row["date"] = DateTime.Now.Date;
                                row["amount"] = cash;
                                row["account_transfer"] = cash;
                                row["active"] = 1;
                                row["id"] = id;
                                row["title"] = sql.TrimBetween(title);
                                row["name"] = sql.TrimBetween(clientName);
                                row["street"] = sql.TrimBetween(street);
                                row["zipcode"] = zipCode.Trim();
                                row["city"] = sql.TrimBetween(city);
                                row["born"] = born.Date;
                                row["handsign"] = sql.User.Name;
                                DataRow advisorRow = advisorTable.Rows.Find(advisor);
                                if (advisorRow != null)
                                    row["advisor_id"] = advisorRow["id"];
                                else
                                    row["advisor_id"] = DBNull.Value;
                                clientTable.Rows.Add(row);
                            }
                            catch (Exception err)
                            {
                                clientTable.RejectChanges();
                                if (MessageBox.ShowErrorDialog(this, err, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    return;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Handles the drag Drop event for import View and updates the related state.
        /// </summary>
        private void importView_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                var pos = importView.PointToClient(new Point(e.X, e.Y));
                var hit = importView.HitTest(pos);
                if (hit.Item == null)
                    return;
                ListViewItem item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                int index = importView.Items.IndexOf(hit.Item);
                importView.Items.Remove(item);
                importView.Items.Insert(index, item);
                UpdateImportTree();
            }
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
        /// Updates the import Tree data and refreshes the related application state.
        /// </summary>
        private void UpdateImportTree()
        {
            var items = importView.Items;
            for (int i = 0; i < items.Count; i++)
            {
                switch (items[i].Text)
                { 
                    case "Debitor Nr":
                        DebitorNr = i;
                        break;
                    case "Anrede":
                        Title = i ;
                        break;
                    case "Name":
                        ClientName = i ;
                        break;
                    case "Geboren":
                        Born = i;
                        break;
                    case "Straße":
                        Street = i;
                        break;
                    case "Postleitzahl":
                        ZopCode = i;
                        break;
                    case "Ort":
                        City = i;
                        break;
                    case "Betreuer":
                        Advisor = i;
                        break;
                    case "Altbestand":
                        Cash = i;
                        break;
                }
            }
        }
    }
}
