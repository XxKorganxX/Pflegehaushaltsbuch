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
    /// Represents the Change User Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ChangeUserForm : Pflegehaushaltsbuch.FormControls.Form, IChangeUserFormContract
    {
        private readonly ChangeUserFormPresenter presenter;


        private string oldkeyword;
        /// <summary>
        /// Creates a new Change User Form instance and initializes the required state.
        /// </summary>
        public ChangeUserForm(SQLBase sql, string username, string keyword, DataTable users )
        {
            InitializeComponent();
            presenter = new ChangeUserFormPresenter(this);
            this.sql = sql;
            DataRow[] rows = users.Rows
                .OfType<DataRow>()
                .Where(userRow => User.MatchesIdentity(userRow, username))
                .ToArray();
            usernameBox.Text = rows[0]["name"].ToString();
            loginBox.Text = rows[0]["login"].ToString();
            oldkeyword = keyword;
            //usernameBox.Text = username;
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                string username = usernameBox.Text.Trim();
                string login = loginBox.Text.Trim();
                string keyword = pw0Box.Text.Trim();
                string keywordAgain = pw1Box.Text.Trim();
                if (string.IsNullOrWhiteSpace(username))
                    throw new Exception( Messages.login_name);
                if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(keywordAgain))
                    throw new Exception( Messages.login_enter_passwords);
                if (!keyword.Equals(keywordAgain))
                    throw new Exception(Messages.login_passwords_not_match);
                await User.UpdateLogin(sql, username, oldkeyword, username, login, keyword);
                DialogResult = DialogResult.OK;
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
    }
}
