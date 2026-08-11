using System;
using System.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Advisor window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateAdvisorDialog : Form, ICreateAdvisorDialogContract
    {
        private readonly CreateAdvisorDialogPresenter presenter;

        /// <summary>
        /// Creates a new CreateAdvisorDialog view.
        /// </summary>
        public CreateAdvisorDialog(SqlSession session, DataTable table)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreateAdvisorDialogPresenter(this, session, table, false, -1);
        }
        /// <summary>
        /// Creates a new CreateAdvisorDialog view.
        /// </summary>
        public CreateAdvisorDialog(SqlSession session, DataTable table, int position)
        {
            InitializeComponent();
            Session = session;
            presenter = new CreateAdvisorDialogPresenter(this, session, table, true, position);
        }
        /// <summary>
        /// Handles the shown event for create Client Form and updates the related state.
        /// </summary>
        private void CreateClientForm_Shown(object sender, EventArgs e)
        {
            presenter.Shown();
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Ok();
        }
        /// <summary>
        /// Handles the click event for cancel Button and updates the related state.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            presenter.Cancel();
        }

        /// <summary>
        /// Provides the advisor idtext value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorIDText
        {
            get { return advisorIDBox.Text; }
            set { advisorIDBox.Text = value; }
        }

        /// <summary>
        /// Provides the advisor title text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorTitleText
        {
            get { return advisorTitleBox.Text; }
        }

        /// <summary>
        /// Provides the advisor title index value for the presenter.
        /// </summary>
        int ICreateAdvisorDialogContract.AdvisorTitleIndex
        {
            get { return advisorTitleBox.SelectedIndex; }
            set { advisorTitleBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the advisor name text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorNameText
        {
            get { return advisorNameBox.Text; }
        }

        /// <summary>
        /// Provides the advisor email text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorEmailText
        {
            get { return advisorEmailBox.Text; }
        }

        /// <summary>
        /// Provides the advisor co text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorCoText
        {
            get { return advisorCoBox.Text; }
        }

        /// <summary>
        /// Provides the advisor street text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorStreetText
        {
            get { return advisorStreetBox.Text; }
        }

        /// <summary>
        /// Provides the advisor zipcode text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorZipcodeText
        {
            get { return advisorZipcodeBox.Text; }
        }

        /// <summary>
        /// Provides the advisor city text value for the presenter.
        /// </summary>
        string ICreateAdvisorDialogContract.AdvisorCityText
        {
            get { return advisorCityBox.Text; }
        }

        /// <summary>
        /// Runs the add advisor title view action for the presenter.
        /// </summary>
        void ICreateAdvisorDialogContract.AddAdvisorTitle(string title)
        {
            advisorTitleBox.Items.Add(title);
        }

        /// <summary>
        /// Runs the create binding source view action for the presenter.
        /// </summary>
        BindingSource ICreateAdvisorDialogContract.CreateBindingSource(DataTable table, int position)
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = table;
            bindingSource.Position = position;
            return bindingSource;
        }

        /// <summary>
        /// Runs the bind advisor view action for the presenter.
        /// </summary>
        void ICreateAdvisorDialogContract.BindAdvisor(BindingSource bindingSource, ConvertEventHandler parseHandler)
        {
            advisorIDBox.DataBindings.Clear();
            advisorTitleBox.DataBindings.Clear();
            advisorNameBox.DataBindings.Clear();
            advisorEmailBox.DataBindings.Clear();
            advisorCoBox.DataBindings.Clear();
            advisorStreetBox.DataBindings.Clear();
            advisorZipcodeBox.DataBindings.Clear();
            advisorCityBox.DataBindings.Clear();

            advisorIDBox.DataBindings.Add("Text", bindingSource, "id");
            advisorTitleBox.DataBindings.Add("SelectedItem", bindingSource, "title");
            advisorNameBox.DataBindings.Add("Text", bindingSource, "name");
            advisorNameBox.DataBindings[0].Parse += parseHandler;
            advisorEmailBox.DataBindings.Add("Text", bindingSource, "email");
            advisorEmailBox.DataBindings[0].Parse += parseHandler;
            advisorCoBox.DataBindings.Add("Text", bindingSource, "co");
            advisorCoBox.DataBindings[0].Parse += parseHandler;
            advisorStreetBox.DataBindings.Add("Text", bindingSource, "street");
            advisorStreetBox.DataBindings[0].Parse += parseHandler;
            advisorZipcodeBox.DataBindings.Add("Text", bindingSource, "zipcode");
            advisorZipcodeBox.DataBindings[0].Parse += parseHandler;
            advisorCityBox.DataBindings.Add("Text", bindingSource, "city");
            advisorCityBox.DataBindings[0].Parse += parseHandler;
        }

        /// <summary>
        /// Runs the reject changes view action for the presenter.
        /// </summary>
        void ICreateAdvisorDialogContract.RejectChanges(DataTable table)
        {
            table.RejectChanges();
        }

        /// <summary>
        /// Runs the set dialog result none view action for the presenter.
        /// </summary>
        void ICreateAdvisorDialogContract.SetDialogResultNone()
        {
            DialogResult = DialogResult.None;
        }
    }
}
