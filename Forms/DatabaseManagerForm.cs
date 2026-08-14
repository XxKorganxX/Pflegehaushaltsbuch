using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database Manager Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseManagerForm : Form, IDatabaseManagerFormContract
    {
        private readonly DatabaseManagerFormPresenter presenter;

        /// <summary>
        /// Creates a new DatabaseManagerForm view.
        /// </summary>
        public DatabaseManagerForm(SqlSession session, XmlConfig config)
        {
            InitializeComponent();
            Session = session;
            presenter = new DatabaseManagerFormPresenter(this, session, config);
            Shown += DatabaseManagerForm_Shown;
            Disposed += DatabaseManagerForm_Disposed;
        }

        /// <summary>
        /// Runs the detach sql action.
        /// </summary>
        public SQLBase DetachSql()
        {
            return presenter.DetachSql();
        }

        /// <summary>
        /// Runs the database manager form_disposed action.
        /// </summary>
        private void DatabaseManagerForm_Disposed(object sender, EventArgs e)
        {
            presenter.DisposeConnectedSql();
        }

        /// <summary>
        /// Handles the shown event for database Manager Form and updates the related state.
        /// </summary>
        private async void DatabaseManagerForm_Shown(object sender, EventArgs e)
        {
            await presenter.ShownAsync();
        }

        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            presenter.Close();
        }

        /// <summary>
        /// Handles the click event for get Dataabase Button and updates the related state.
        /// </summary>
        private void getDataabaseButton_Click(object sender, EventArgs e)
        {
            OpenDatabaseFile();
        }

        /// <summary>
        /// Handles the click event for opening an existing SQLite database file.
        /// </summary>
        private void openDatabaseButton_Click(object sender, EventArgs e)
        {
            OpenDatabaseFile();
        }

        private void OpenDatabaseFile()
        {
            presenter.GetDataabase();
        }

        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            await presenter.ConnectAsync();
        }

        /// <summary>
        /// Handles the click event for create Data Base Button and updates the related state.
        /// </summary>
        private async void createDataBaseButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateDataBaseAsync();
        }

        /// <summary>
        /// Handles the click event for sql User Button and updates the related state.
        /// </summary>
        private async void sqlUserButton_Click(object sender, EventArgs e)
        {
            await presenter.SqlUserAsync();
        }

        /// <summary>
        /// Handles the click event for label5 and updates the related state.
        /// </summary>
        private void label5_Click(object sender, EventArgs e)
        {
            presenter.Label5();
        }

        /// <summary>
        /// Handles the click event for label7 and updates the related state.
        /// </summary>
        private void label7_Click(object sender, EventArgs e)
        {
            presenter.Label7();
        }

        /// <summary>
        /// Handles the click event for label8 and updates the related state.
        /// </summary>
        private void label8_Click(object sender, EventArgs e)
        {
            presenter.Label8();
        }

        /// <summary>
        /// Handles the 1 event for change Masterkey Button Click and updates the related state.
        /// </summary>
        private async void changeMasterkeyButton_Click_1(object sender, EventArgs e)
        {
            await presenter.ChangeMasterkeyAsync();
        }

        /// <summary>
        /// Handles the click event for change Masterkeyword Label and updates the related state.
        /// </summary>
        private void changeMasterkeywordLabel_Click(object sender, EventArgs e)
        {
            presenter.ChangeMasterkeywordLabel();
        }

        /// <summary>
        /// Provides the create database name value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.CreateDatabaseName
        {
            get { return databaseBox.Text.Trim(); }
        }

        /// <summary>
        /// Provides the selected database value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.SelectedDatabase
        {
            get { return databasesBox.SelectedItem == null ? string.Empty : databasesBox.SelectedItem.ToString(); }
        }

        /// <summary>
        /// Provides the user name value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.UserName
        {
            get { return usernameBox.Text; }
        }

        /// <summary>
        /// Provides the keyword value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.Keyword
        {
            get { return keywordBox.Text; }
        }

        /// <summary>
        /// Provides the from host value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.FromHost
        {
            get { return fromHostBox.Text; }
        }

        /// <summary>
        /// Provides the master keyword value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.MasterKeyword
        {
            get { return masterKeywordBox.Text; }
        }

        /// <summary>
        /// Provides the master keyword again value for the presenter.
        /// </summary>
        string IDatabaseManagerFormContract.MasterKeywordAgain
        {
            get { return masterKeywordIIBox.Text; }
        }

        /// <summary>
        /// Runs the set databases view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowDatabases(IEnumerable<string> databases, string selectedDatabase)
        {
            databasesBox.Items.Clear();
            databasesBox.Items.AddRange(databases.Cast<object>().ToArray());
            databasesBox.SelectedItem = selectedDatabase;
        }

        /// <summary>
        /// Runs the show open database dialog view action for the presenter.
        /// </summary>
        bool IDatabaseManagerFormContract.ShowOpenDatabaseDialog(out string databaseFileName)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQLite database (*.db)|*.db|All files (*.*)|*.*";
                openFileDialog.DefaultExt = "db";
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;

                string currentDatabase = GetCurrentDatabaseFile();
                if (!string.IsNullOrWhiteSpace(currentDatabase))
                {
                    if (File.Exists(currentDatabase))
                    {
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(currentDatabase);
                        openFileDialog.FileName = Path.GetFileName(currentDatabase);
                    }
                    else if (Directory.Exists(currentDatabase))
                    {
                        openFileDialog.InitialDirectory = currentDatabase;
                    }
                }

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    databaseFileName = openFileDialog.FileName;
                    SelectDatabaseFile(databaseFileName);
                    return true;
                }
            }

            databaseFileName = null;
            return false;
        }

        private string GetCurrentDatabaseFile()
        {
            if (databasesBox.SelectedItem != null)
                return databasesBox.SelectedItem.ToString();

            return databaseBox.Text.Trim();
        }

        private void SelectDatabaseFile(string databaseFileName)
        {
            foreach (object item in databasesBox.Items)
            {
                if (string.Equals(item.ToString(), databaseFileName, StringComparison.CurrentCultureIgnoreCase))
                {
                    databasesBox.SelectedItem = item;
                    return;
                }
            }

            databasesBox.Items.Add(databaseFileName);
            databasesBox.SelectedItem = databaseFileName;
        }

        /// <summary>
        /// Runs the show progress dialog view action for the presenter.
        /// </summary>
        IAdministrationProgress IDatabaseManagerFormContract.ShowProgressDialog(string text)
        {
            ProgressDialog progressDialog = new ProgressDialog(text);
            progressDialog.Show(this);
            return progressDialog;
        }

        /// <summary>
        /// Runs the confirm database creating view action for the presenter.
        /// </summary>
        bool IDatabaseManagerFormContract.ConfirmDatabaseCreating()
        {
            return MessageBox.ShowDialog(this, Messages.database_creating, Messages.database, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;
        }

        /// <summary>
        /// Runs the show enter database name view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowEnterDatabaseName()
        {
            MessageBox.ShowDialog(this, Messages.database_enter_name);
        }

        /// <summary>
        /// Runs the show default login message view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowDefaultLoginMessage()
        {
            MessageBox.ShowDialog(this, Messages.database_default_login, Messages.database_default_login_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Runs the show database created view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowDatabaseCreated()
        {
            MessageBox.ShowDialog(this, Messages.database_created);
        }

        /// <summary>
        /// Runs the show user created view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowUserCreated()
        {
            MessageBox.ShowDialog(this, Messages.user_created);
        }

        /// <summary>
        /// Runs the show creation user dialog view action for the presenter.
        /// </summary>
        bool IDatabaseManagerFormContract.ShowCreationUserDialog(SqlSession session)
        {
            using (CreationUserForm userForm = new CreationUserForm(session, true))
            {
                return userForm.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show master password changed restart view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ShowMasterPasswordChangedRestart()
        {
            MessageBox.Show(this, Messages.master_password_changed_restart);
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.AcceptDialog()
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Runs the cancel dialog view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.CancelDialog()
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Runs the keep dialog open view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.KeepDialogOpen()
        {
            DialogResult = DialogResult.None;
        }

        /// <summary>
        /// Runs the toggle create database panel view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ToggleCreateDatabasePanel()
        {
            createdatabasePanel.Visible = !createdatabasePanel.Visible;
        }

        /// <summary>
        /// Runs the toggle user panel view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ToggleUserPanel()
        {
            userPanel.Visible = !userPanel.Visible;
        }

        /// <summary>
        /// Runs the toggle connect panel view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ToggleConnectPanel()
        {
            connectPanel.Visible = !connectPanel.Visible;
        }

        /// <summary>
        /// Runs the toggle masterkey panel view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.ToggleMasterkeyPanel()
        {
            masterkeyPanel.Visible = !masterkeyPanel.Visible;
        }

        /// <summary>
        /// Runs the restart application view action for the presenter.
        /// </summary>
        void IDatabaseManagerFormContract.RestartApplication()
        {
            Application.Restart();
        }
    }
}
