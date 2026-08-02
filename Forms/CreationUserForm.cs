using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Creation User Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class CreationUserForm : Pflegehaushaltsbuch.FormControls.Form, ICreationUserFormContract
    {
        private readonly CreationUserFormPresenter presenter;


        private DataRow row = null;
        public string UserName { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int Access { get; set; }
        public bool Admin { get; set; }
        private string oldUsername;
        /// <summary>
        /// Creates a new Creation User Form instance and initializes the required state.
        /// </summary>
        public CreationUserForm(SQLBase sql)
        {
            InitializeComponent();
            presenter = new CreationUserFormPresenter(this);
            if (Program.DesignMode)
                return;
            this.sql = sql;
            Phone = sql.Company.Phone;
            Fax = sql.Company.Fax;
            insertBox.Checked = true;
            changeBox.Checked = true;
            BindData();
        }
        /// <summary>
        /// Creates a new Creation User Form instance and initializes the required state.
        /// </summary>
        public CreationUserForm(SQLBase sql, DataRow row)
        {
            InitializeComponent();
            presenter = new CreationUserFormPresenter(this);
            if (Program.DesignMode)
                return;
            this.sql = sql;
            this.row = row;
            UserName = oldUsername = row["name"].ToString();
            Login = row["login"].ToString();
            Phone = row["phone"].ToString();
            Fax = row["fax"].ToString();
            Email = row["email"].ToString();
            int access = Int32.Parse(row["access"].ToString());
            Admin = bool.Parse(row["admin"].ToString());
            insertBox.Checked = (access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert;
            changeBox.Checked = (access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change;
            deleteBox.Checked = (access & (int)Enums.UserRightEnum.Delete) == (int)Enums.UserRightEnum.Delete;
            BindData();
        }
        /// <summary>
        /// Runs the bind Data operation and updates the related application state.
        /// </summary>
        private void BindData()
        {
            nameBox.DataBindings.Add("Text", this, "UserName");
            loginBox.DataBindings.Add("Text", this, "Login");
            phoneBox.DataBindings.Add("Text", this, "Phone");
            faxBox.DataBindings.Add("Text", this, "Fax");
            emailBox.DataBindings.Add("Text", this, "Email");
            adminBox.DataBindings.Add("Checked", this, "Admin");
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new Exception(Messages.name);
            if (string.IsNullOrWhiteSpace(Email))
                throw new Exception(Messages.email);
            //Update User
            if (row != null)
            {
                await User.UpdateUser(sql,
                        oldUsername,
                        UserName,
                        Login,
                        Phone,
                        Fax,
                        Email,
                        Access,
                        Admin);
                MessageBox.ShowDialog(this, Messages.usermanagement_user_changed);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Login))
                    Login = UserName;
                await User.CreateUser(sql,
                    UserName,
                    Login,
                    string.Empty,
                    Phone,
                    Fax,
                    Email,
                    Access,
                    Admin);
                MessageBox.ShowDialog(this, Messages.usermanagement_user_created);
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        /// <summary>
        /// Handles the checked Changed event for access and updates the related state.
        /// </summary>
        private void access_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;
           // if (!box.Focused)
           //     return;
            Enums.UserRightEnum userRight = Enums.UserRightEnum.None;
            if (insertBox.Checked)
                userRight |= Enums.UserRightEnum.Insert;
            if (changeBox.Checked)
                userRight |= Enums.UserRightEnum.Change;
            if (deleteBox.Checked)
                userRight |= Enums.UserRightEnum.Delete;
            Access = (int)userRight;
        }
        /// <summary>
        /// Handles the validating event for email Box and updates the related state.
        /// </summary>
        private void emailBox_Validating(object sender, CancelEventArgs e)
        {
            if (!Company.IsValidEmail(emailBox.Text))
                e.Cancel = true;
        }
    }
}
