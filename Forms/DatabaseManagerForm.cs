using Microsoft.Data.SqlClient;
using MySqlConnector;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database Manager Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseManagerForm : Pflegehaushaltsbuch.FormControls.Form, IDatabaseManagerFormContract
    {
        private readonly DatabaseManagerFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        XmlConfig config = null;
        public SQLBase SQL = null;
        /// <summary>
        /// Creates a new Database Manager Form instance and initializes the required state.
        /// </summary>
        public DatabaseManagerForm(XmlConfig config)
        {
            InitializeComponent();
            presenter = new DatabaseManagerFormPresenter(this);
            this.config = config;
            Shown += DatabaseManagerForm_Shown;
        }
        /// <summary>
        /// Handles the shown event for database Manager Form and updates the related state.
        /// </summary>
        private async void DatabaseManagerForm_Shown(object sender, EventArgs e)
        {
            if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                SQL = new SQL();
            else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                SQL = new MySQL();
            else if (config.DBType == XmlConfig.DataBaseTypes.SQLite)
                SQL = new SQLITE();
            var obj = await SQL.GetAllDatabasesAsync(config.Host, config.User, config.Keyword);
            databasesBox.Items.Clear();
            databasesBox.Items.AddRange(obj);
            databasesBox.DataBindings.Clear();
            var binding = databasesBox.DataBindings.Add("SelectedItem", config, "Database", false, DataSourceUpdateMode.OnPropertyChanged);
            if (databasesBox.SelectedItem == null)
                config.Database = null;
        }
        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// Handles the click event for get Dataabase Button and updates the related state.
        /// </summary>
        private void getDataabaseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    config.Database = openFileDialog.FileName;
            }
        }
        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(config.Database))
            {
                MessageBox.ShowDialog(this, Messages.database_enter_name);
                return;
            }
            SQLBase sql = null;
            try
            {
                if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                    sql = new SQL();
                else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                    sql = new MySQL();
                else if (config.DBType == XmlConfig.DataBaseTypes.SQLite)
                    sql = new SQLITE();
                else
                    throw new Exception(Messages.database_connection_type_missing);
                await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                SQL = sql;
                config.Save();
                Settings.Default.Save();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch
            {
                if (sql != null)
                    sql.Dispose();
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for create Data Base Button and updates the related state.
        /// </summary>
        private async void createDataBaseButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                string database = databaseBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(database))
                {
                    MessageBox.ShowDialog(this, Messages.database_enter_name);
                    return;
                }
                if (MessageBox.ShowDialog(this, Messages.database_creating, Messages.database, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                    return;

                if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                    SQL = new SQL();
                else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                    SQL = new MySQL();
                else
                    throw new Exception(Messages.database_connection_type_missing);
                await SQL.DropDatabaseAsync(config.Host, config.User, config.Keyword, database);
                await SQL.CreateDataBaseAsync(config.Host, config.User, config.Keyword, database);
                config.Save();
                MessageBox.ShowDialog(this, Messages.database_default_login, Messages.database_default_login_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.ShowDialog(this, Messages.database_created);
                if (databasesBox.Items.OfType<string>().Where(a => a.Equals(database, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
                    databasesBox.Items.Add(database);
                databasesBox.SelectedItem = database;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for sql User Button and updates the related state.
        /// </summary>
        private async void sqlUserButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                    SQL = new SQL();
                else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                    SQL = new MySQL();
                else
                {
                    MessageBox.ShowDialog(this, Messages.database_connection_type_missing);
                    return;
                }
                await SQL.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                await SQL.CreateUserAsync(usernameBox.Text, keywordBox.Text, config.Database, fromHostBox.Text);
                MessageBox.ShowDialog(this, Messages.user_created);
                SQL.Dispose();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for label5 and updates the related state.
        /// </summary>
        private void label5_Click(object sender, EventArgs e)
        {
            createdatabasePanel.Visible = !createdatabasePanel.Visible;
        }
        /// <summary>
        /// Handles the click event for label7 and updates the related state.
        /// </summary>
        private void label7_Click(object sender, EventArgs e)
        {
            userPanel.Visible = !userPanel.Visible;
        }
        /// <summary>
        /// Handles the click event for label8 and updates the related state.
        /// </summary>
        private void label8_Click(object sender, EventArgs e)
        {
            connectPanel.Visible = !connectPanel.Visible;
        }
        /// <summary>
        /// Updates the key Word data and refreshes the related application state.
        /// </summary>
        private async void UpdateKeyWord()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                if (!masterKeywordIIBox.Text.Equals(masterKeywordBox.Text))
                    throw new Exception(Messages.keywords_not_equal);
                SQLBase sql;
                if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                    sql = new SQL();
                else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                    sql = new MySQL();
                else
                    throw new Exception(Messages.sql_server_required);
                using (sql)
                {
                    await sql.ConnectAsync(config.Host, config.User, config.Keyword, config.Database);
                    await sql.CreateNewPasswordAsync(config.Host, config.User, config.Keyword, masterKeywordBox.Text);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the 1 event for change Masterkey Button Click and updates the related state.
        /// </summary>
        private void changeMasterkeyButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                UpdateKeyWord();
                MessageBox.Show(this, Messages.master_password_changed_restart);
                Application.Restart();
            }
            catch
            {
                DialogResult = DialogResult.None;
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for change Masterkeyword Label and updates the related state.
        /// </summary>
        private void changeMasterkeywordLabel_Click(object sender, EventArgs e)
        {
            masterkeyPanel.Visible = !masterkeyPanel.Visible;
        }
    }
}
