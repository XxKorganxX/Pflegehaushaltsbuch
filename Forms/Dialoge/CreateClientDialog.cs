using Microsoft.Office.Interop.Outlook;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Client Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateClientDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private DataTable clientTable = new DataTable(), advisorTable = new DataTable();
        private int clientID = -1;
        public int ClientID { get { return clientID; } set { clientID = value; } }
        /// <summary>
        /// Represents the Person Data window and coordinates its user interface behavior.
        /// </summary>
        private class PersonData : INotifyPropertyChanged
        {
            private string title = "Frau", name, co, street, zipcode, city;
            public string Title { get { return title; } set { title = value; FirePropertyChanged("Title"); } }
            public string Name { get { return name; } set { name = value; FirePropertyChanged("Name"); } }
            public string Co { get { return co; } set { co = value; FirePropertyChanged("Co"); } }
            public string Street { get { return street; } set { street = value; FirePropertyChanged("Street"); } }
            public string Zipcode { get { return zipcode; } set { zipcode = value; FirePropertyChanged("Zipcode"); } }
            public string City { get { return city; } set { city = value; FirePropertyChanged("City"); } }
            public event PropertyChangedEventHandler PropertyChanged;
            /// <summary>
            /// Runs the fire Property Changed operation and updates the related application state.
            /// </summary>
            protected void FirePropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private PersonData client = new PersonData();
        private bool updateClient = false;
        /// <summary>
        /// Creates a new Create Client Form instance and initializes the required state.
        /// </summary>
        public CreateClientDialog(SQLBase sql)
        {
            InitializeComponent();
            this.sql = sql;
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                titleBox.Items.Add(enumval.GetDisplayName());
            saldoBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0, "c");
        }
        /// <summary>
        /// Creates a new Create Client Form instance and initializes the required state.
        /// </summary>
        public CreateClientDialog(SQLBase sql, int clientID)
        {
            InitializeComponent();
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                titleBox.Items.Add(enumval.GetDisplayName());
            debitorNrBox.Enabled = false;
            saldoBox.Enabled = false;
            saldoBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0, "c");
            this.sql = sql;
            this.ClientID = clientID;
            updateClient = true;
        }
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets the next Free ID value from the current application state.
        /// </summary>
        private int GetNextFreeID()
        {
            DataRow[] rows = clientTable.Select("", "id");
            Dictionary<int, int> ids = new Dictionary<int, int>();
            foreach (DataRow item in rows)
                ids[Int32.Parse(item["id"].ToString())] = 0;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!ids.ContainsKey(i))
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Handles the shown event for create Client Form and updates the related state.
        /// </summary>
        private async void CreateClientForm_Shown(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            await ConnectTableToDataBase();
            if (!updateClient)
                clientID = GetNextFreeID();
            else
            {
                DataRow row = clientTable.Rows.Find(clientID);
                if (row == null)
                    throw new Exception(Messages.client_not_found);
                client.Title = row["title"].ToString();
                client.Name = row["name"].ToString();
                client.Street = row["street"].ToString();
                client.Zipcode = row["zipcode"].ToString();
                client.City = row["city"].ToString();
                bornBox.Date = DateTime.Parse(row["born"].ToString());
                Amount = decimal.Parse(row["account_transfer"].ToString());
                saldoBox.Text = Amount.ToString("C");

                if (row["advisor_id"] != DBNull.Value)
                {
                    useAdvisor.Checked = true;
                    DataRow[] advisorRow = advisorTable.Select(string.Format("id={0}", row["advisor_id"]));
                    if (advisorRow != null && advisorRow.Length > 0)
                    {
                        advisorsBox.Enabled = true;
                        string advisorName = advisorRow[0]["name"].ToString();
                        foreach (DataRowView item in advisorsBox.Items)
                        {
                            if (item["name"].Equals(advisorName))
                            {
                                advisorsBox.SelectedItem = item;
                                advisorsBox.Invalidate();
                                break;
                            }
                        }
                        //advisorsBox.SelectedItem = advisorRow[0];//["name"].ToString();
                        //advisorsBox.SelectedItem = advisorsBox.Items[
                    }
                }
            }
            debitorNrBox.DataBindings.Add("Text", this, "ClientID");
            titleBox.DataBindings.Add("Text", client, "Title");
            nameBox.DataBindings.Add("Text", client, "Name");
            streetBox.DataBindings.Add("Text", client, "Street");
            zipcodeBox.DataBindings.Add("Text", client, "Zipcode");
            cityBox.DataBindings.Add("Text", client, "City");
            UpdateAdvisorBoxes();
        }
        public struct ClientData
        {
            public int ClientID { get; set; }
            public string Title { get; set; }
            public string Name { get; set; }
            public string Street { get; set; }
            public string Zipcode { get; set; }
            public string City { get; set; }
            public DateTime BornDate { get; set; }
            public decimal Amount { get; set; }
            public int? AdvisorId { get; set; }
        }
        public ClientData Data { get; private set; }
        private ClientData CreateClientData()
        {
            return new ClientData
            {
                ClientID = ClientID,
                Title = sql.TrimBetween(client.Title),
                Name = sql.TrimBetween(client.Name),
                Street = sql.TrimBetween(client.Street),
                Zipcode = sql.TrimBetween(client.Zipcode),
                City = sql.TrimBetween(client.City),
                BornDate = bornBox.Date.Date,
                Amount = Amount,
                AdvisorId = GetSelectedAdvisorId()
            };
        }
        private int? GetSelectedAdvisorId()
        {
            if (!useAdvisor.Checked || advisorsBox.SelectedItem == null)
                return null;

            DataRowView advisor = advisorsBox.SelectedItem as DataRowView;
            if (advisor == null || advisor["id"] == DBNull.Value)
                return null;

            return Convert.ToInt32(advisor["id"]);
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            advisorsBox.DataSource = null;
            await sql.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            await sql.FillAdapterAsync(SQLBase.SELECT.Advisors, advisorTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns["id"] };
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns["name"] };
            advisorsBox.DataSource = advisorTable;
            advisorsBox.DisplayMember = "name";
        }
        /// <summary>
        /// Handles the click event for create Advisor Button and updates the related state.
        /// </summary>
        private void createAdvisorButton_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for advisor Change Button and updates the related state.
        /// </summary>
        private void advisorChangeButton_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for delete Advisor Button and updates the related state.
        /// </summary>
        private void deleteAdvisorButton_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(client.Name))
                    throw new Exception(Messages.clients_enter_name);
                if (string.IsNullOrWhiteSpace(streetBox.Text))
                    throw new Exception(Messages.invalid_street);
                if (string.IsNullOrWhiteSpace(zipcodeBox.Text))
                    throw new Exception(Messages.invalid_zip);
                if (string.IsNullOrWhiteSpace(cityBox.Text))
                    throw new Exception(Messages.invalid_city);
                if (bornBox.Date > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
                Data = CreateClientData();
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the checked Changed event for use Advisor and updates the related state.
        /// </summary>
        private void useAdvisor_CheckedChanged(object sender, EventArgs e)
        {
           advisorsBox.Enabled = useAdvisor.Checked;
        }
        /// <summary>
        /// Handles the selected Index Changed event for advisors Box and updates the related state.
        /// </summary>
        private void advisorsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the selection Change Committed event for advisors Box and updates the related state.
        /// </summary>
        private void advisorsBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Updates the advisor Boxes data and refreshes the related application state.
        /// </summary>
        private void UpdateAdvisorBoxes()
        {
        }
    }
}
