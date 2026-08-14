using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Client Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateClientDialog : Form, ICreateClientDialogContract
    {
        private readonly CreateClientDialogPresenter presenter;

        private int clientID = -1;
        public int ClientID { get { return clientID; } set { clientID = value; } }
        /// <summary>
        /// Creates a new CreateClientDialog view.
        /// </summary>
        public CreateClientDialog(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreateClientDialogPresenter(this, session, false, -1);
        }
        /// <summary>
        /// Creates a new CreateClientDialog view.
        /// </summary>
        public CreateClientDialog(SqlSession session, int clientID)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreateClientDialogPresenter(this, session, true, clientID);
        }
        public decimal Amount { get; set; }
        /// <summary>
        /// Handles the shown event for create Client Form and updates the related state.
        /// </summary>
        private async void CreateClientForm_Shown(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            await presenter.ShownAsync();
        }
        /// <summary>
        /// Provides the client data value.
        /// </summary>
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
        public ClientData Data { get; set; }
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
            presenter.Ok();
        }
        /// <summary>
        /// Handles the checked Changed event for use Advisor and updates the related state.
        /// </summary>
        private void useAdvisor_CheckedChanged(object sender, EventArgs e)
        {
            presenter.UseAdvisorChanged();
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
            presenter.UpdateAdvisorBoxes();
        }

        /// <summary>
        /// Provides the client id value for the presenter.
        /// </summary>
        int ICreateClientDialogContract.ClientID
        {
            get { return ClientID; }
            set { ClientID = value; }
        }

        /// <summary>
        /// Provides the amount value for the presenter.
        /// </summary>
        decimal ICreateClientDialogContract.Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Provides the born date value for the presenter.
        /// </summary>
        DateTime ICreateClientDialogContract.BornDate
        {
            get { return bornBox.Date.Date; }
            set { bornBox.Date = value; }
        }

        /// <summary>
        /// Provides the use advisor checked value for the presenter.
        /// </summary>
        bool ICreateClientDialogContract.UseAdvisorChecked
        {
            get { return useAdvisor.Checked; }
            set { useAdvisor.Checked = value; }
        }

        /// <summary>
        /// Provides the data value for the presenter.
        /// </summary>
        CreateClientDialog.ClientData ICreateClientDialogContract.Data
        {
            get { return Data; }
            set { Data = value; }
        }

        /// <summary>
        /// Provides the selected advisor id value for the presenter.
        /// </summary>
        int? ICreateClientDialogContract.SelectedAdvisorId
        {
            get
            {
                if (!useAdvisor.Checked || advisorsBox.SelectedItem == null)
                    return null;

                DataRowView advisor = advisorsBox.SelectedItem as DataRowView;
                if (advisor == null || advisor["id"] == DBNull.Value)
                    return null;

                return Convert.ToInt32(advisor["id"]);
            }
        }

        /// <summary>
        /// Runs the add title view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.AddTitle(string title)
        {
            titleBox.Items.Add(title);
        }

        /// <summary>
        /// Runs the bind saldo view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.BindSaldo()
        {
            saldoBox.DataBindings.Clear();
            saldoBox.DataBindings.Add("Text", this, "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0, "C", Session.Company.Currencies);
        }

        /// <summary>
        /// Runs the bind client id view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.BindClientID()
        {
            debitorNrBox.DataBindings.Clear();
            debitorNrBox.DataBindings.Add("Text", this, "ClientID");
        }

        /// <summary>
        /// Runs the bind client view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.BindClient(object client)
        {
            titleBox.DataBindings.Clear();
            nameBox.DataBindings.Clear();
            streetBox.DataBindings.Clear();
            zipcodeBox.DataBindings.Clear();
            cityBox.DataBindings.Clear();

            titleBox.DataBindings.Add("Text", client, "Title");
            nameBox.DataBindings.Add("Text", client, "Name");
            streetBox.DataBindings.Add("Text", client, "Street");
            zipcodeBox.DataBindings.Add("Text", client, "Zipcode");
            cityBox.DataBindings.Add("Text", client, "City");
        }

        /// <summary>
        /// Runs the set debitor enabled view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SetDebitorEnabled(bool enabled)
        {
            debitorNrBox.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set saldo enabled view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SetSaldoEnabled(bool enabled)
        {
            saldoBox.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set advisors enabled view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SetAdvisorsEnabled(bool enabled)
        {
            advisorsBox.Enabled = enabled;
        }

        /// <summary>
        /// Runs the set advisors data source view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SetAdvisorsDataSource(DataTable advisorTable)
        {
            advisorsBox.DataSource = null;
            advisorsBox.DataSource = advisorTable;
            advisorsBox.DisplayMember = Columns.Name;
        }

        /// <summary>
        /// Runs the select advisor by name view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SelectAdvisorByName(string advisorName)
        {
            foreach (DataRowView item in advisorsBox.Items)
            {
                if (item["name"].Equals(advisorName))
                {
                    advisorsBox.SelectedItem = item;
                    advisorsBox.Invalidate();
                    break;
                }
            }
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICreateClientDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
