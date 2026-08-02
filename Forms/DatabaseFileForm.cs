using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using Pflegehaushaltsbuch.WPFControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database File Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseFileForm : Pflegehaushaltsbuch.FormControls.Form, IDatabaseFileFormContract
    {
        private readonly DatabaseFileFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        public SQLBase sqlBase;
        UserTextBox userTextBox;
        /// <summary>
        /// Creates a new Database File Form instance and initializes the required state.
        /// </summary>
        public DatabaseFileForm()
        {
            InitializeComponent();
            presenter = new DatabaseFileFormPresenter(this);
        }
        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = "Verwahrgeld";
                    saveFileDialog.Filter = "database|*.db";
                    saveFileDialog.DefaultExt = "db";
                    if (saveFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                        return;
                    try
                    {
                        sqlBase = new Databases.SQLITE();
                        await sqlBase.CreateDataBaseAsync(saveFileDialog.FileName, "", passwordBox.Text, "Verwahrgeld");
                        await sqlBase.OnLoadAsync();
                        XmlConfig config = XmlConfig.LoadXml();
                        config.DBType = XmlConfig.DataBaseTypes.SQLite;
                        config.User = "";
                        config.Keyword = passwordBox.Text;
                        config.Database = saveFileDialog.FileName;
                        config.Save();
                        MessageBox.ShowDialog(this, Messages.database_default_login, Messages.database_default_login_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    catch
                    {
                        if (sqlBase != null)
                            sqlBase.Dispose();
                        throw;
                    }
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                XmlConfig config = XmlConfig.LoadXml();
                openFileDialog.FileName = config.Database;// "Pflegehaushaltsbuch.db";
                openFileDialog.Filter = "database|*.db";
                if (openFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
                try
                {
                    sqlBase = new Databases.SQLITE();
                    await sqlBase.TestConnectionAsync("", openFileDialog.FileName, "", passwordBox.Text);
                    await sqlBase.OnLoadAsync();
                    config.DBType = XmlConfig.DataBaseTypes.SQLite;
                    config.User = "";
                    config.Keyword = passwordBox.Text;
                    config.Database = openFileDialog.FileName;
                    config.Save();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                catch
                {
                    if (sqlBase != null)
                        sqlBase.Dispose();
                    throw;
                }
            }
        }
    }
}
