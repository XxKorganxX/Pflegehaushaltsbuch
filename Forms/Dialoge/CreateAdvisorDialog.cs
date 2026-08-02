using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Create Advisor window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreateAdvisorDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        private bool update = false;
        private DataTable table;
        private BindingSource bindingSource;
        /// <summary>
        /// Creates a new Create Advisor instance and initializes the required state.
        /// </summary>
        public CreateAdvisorDialog(SQLBase sql, DataTable table)
        {
            InitializeComponent();
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                advisorTitleBox.Items.Add(enumval.GetDisplayName());
            this.sql = sql;
            this.table = table;
            advisorIDBox.Text = sql.GetID(table).ToString();
            advisorTitleBox.SelectedIndex = 0;
            this.update = false;
        }
        /// <summary>
        /// Creates a new Create Advisor instance and initializes the required state.
        /// </summary>
        public CreateAdvisorDialog(SQLBase sql, DataTable table, int position)
        {
            InitializeComponent();
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                advisorTitleBox.Items.Add(enumval.GetDisplayName());
            this.sql = sql;
            this.table = table;
            advisorIDBox.Text = sql.GetID(table).ToString();
            bindingSource = new BindingSource();
            bindingSource.DataSource = table;
            bindingSource.Position = position;
            this.update = true;
        }
        /// <summary>
        /// Handles the shown event for create Client Form and updates the related state.
        /// </summary>
        private void CreateClientForm_Shown(object sender, EventArgs e)
        {
            if (update)
            {
                advisorIDBox.DataBindings.Add("Text", bindingSource, "id");
                //advisorNameBox.DataBindings[0].Parse += binding_Format;
                advisorTitleBox.DataBindings.Add("SelectedItem", bindingSource, "title");
                advisorNameBox.DataBindings.Add("Text", bindingSource, "name");
                advisorNameBox.DataBindings[0].Parse += binding_Format;
                advisorEmailBox.DataBindings.Add("Text", bindingSource, "email");
                advisorEmailBox.DataBindings[0].Parse += binding_Format;
                advisorCoBox.DataBindings.Add("Text", bindingSource, "co");
                advisorCoBox.DataBindings[0].Parse += binding_Format;
                advisorStreetBox.DataBindings.Add("Text", bindingSource, "street");
                advisorStreetBox.DataBindings[0].Parse += binding_Format;
                advisorZipcodeBox.DataBindings.Add("Text", bindingSource, "zipcode");
                advisorZipcodeBox.DataBindings[0].Parse += binding_Format;
                advisorCityBox.DataBindings.Add("Text", bindingSource, "city");
                advisorCityBox.DataBindings[0].Parse += binding_Format;
            }
        }
        
        void binding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = Trim(e.Value.ToString());
        }
        /// <summary>
        /// Trims the trim value and returns the cleaned result.
        /// </summary>
        private string Trim(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            string[] splittedStr = str.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splittedStr.Length; i++)
            {
                sb.Append(splittedStr[i]);
                if (i < splittedStr.Length - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(advisorNameBox.Text))
                    throw new Exception(Messages.advisors_name_missing);
                if (string.IsNullOrWhiteSpace(advisorStreetBox.Text))
                    throw new Exception(Messages.missing_street);
                if (string.IsNullOrWhiteSpace(advisorCityBox.Text))
                    throw new Exception(Messages.missing_city);
                if (string.IsNullOrWhiteSpace(advisorZipcodeBox.Text))
                    throw new Exception(Messages.missing_zip);
                if (!string.IsNullOrWhiteSpace(advisorEmailBox.Text) && !sql.IsEmail(advisorEmailBox.Text))
                    throw new Exception(Messages.invalid_email);
                int id = 0;
                if (!Int32.TryParse(Trim(advisorIDBox.Text), out id) || id == 0)
                    throw new Exception(Messages.invalid_no);
                if (!update)
                {
                    DataRow row = table.NewRow();
                    row["id"] = id;
                    row["title"] = Trim(advisorTitleBox.Text);
                    row["name"] = Trim(advisorNameBox.Text);
                    row["email"] = Trim(advisorEmailBox.Text);
                    row["co"] = Trim(advisorCoBox.Text);
                    row["street"] = Trim(advisorStreetBox.Text);
                    row["zipcode"] = Trim(advisorZipcodeBox.Text);
                    row["city"] = Trim(advisorCityBox.Text);
                    row["date"] = DateTime.Now.Date;
                    row["handsign"] = sql.User.Name;
                    table.Rows.Add(row);
                }
                else
                {
                    bindingSource.EndEdit();
                }               
            }
            catch
            {
                table.RejectChanges();
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for cancel Button and updates the related state.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            table.RejectChanges();
        }
    }
}
