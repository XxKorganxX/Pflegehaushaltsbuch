using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database Server Connect Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseConnectionForm : Form, IDatabaseConnectionFormContract
    {
        private readonly DatabaseConnectionFormPresenter presenter;

        /// <summary>
        /// Creates a new DatabaseServerConnectForm view.
        /// </summary>
        public DatabaseConnectionForm(SqlSession session, XmlConfig config)
        {
            InitializeComponent();
            Session = session;
            presenter = new DatabaseConnectionFormPresenter(this, session, config);
            presenter.Initialize();
        }

        /// <summary>
        /// Handles the click event for database Type Button and updates the related state.
        /// </summary>
        private void databaseTypeButton_Click(object sender, EventArgs e)
        {
            XmlConfig.DataBaseTypes type = (XmlConfig.DataBaseTypes)((Control)sender).Tag;
            presenter.DatabaseType(type);
        }

        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            await presenter.ConnectAsync();
        }

        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            presenter.Close();
        }

        /// <summary>
        /// Runs the bind config view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.BindConfig(XmlConfig config)
        {
            hostBox.DataBindings.Clear();
            userNameBox.DataBindings.Clear();
            passwordBox.DataBindings.Clear();
            trustServerCertificateCheckBox.DataBindings.Clear();

            hostBox.DataBindings.Add("Text", config, "Host");
            userNameBox.DataBindings.Add("Text", config, "User");
            passwordBox.DataBindings.Add("Text", config, "Keyword");
            trustServerCertificateCheckBox.DataBindings.Add(
                "Checked",
                config,
                "TrustServerCertificate",
                false,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// Runs the set database type icons view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.SetDatabaseTypeIcons(Image sqlIcon, Image mySqlIcon, Image sqliteIcon)
        {
            sqlButton.Image = sqlIcon;
            mySqlButton.Image = mySqlIcon;
            sqliteButton.Image = sqliteIcon;
        }

        /// <summary>
        /// Runs the set database type buttons view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.SetDatabaseTypeButtons(bool sqlChecked, bool mySqlChecked, bool sqliteChecked)
        {
            sqlButton.Checked = sqlChecked;
            mySqlButton.Checked = mySqlChecked;
            sqliteButton.Checked = sqliteChecked;
        }

        /// <summary>
        /// Runs the set host visible view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.SetHostVisible(bool visible)
        {
            hostLabel.Visible = visible;
            hostBox.Visible = visible;
        }

        void IDatabaseConnectionFormContract.SetTrustServerCertificateVisible(bool visible)
        {
            trustServerCertificateCheckBox.Visible = visible;
        }

        /// <summary>
        /// Runs the accept dialog view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.AcceptDialog()
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Runs the cancel dialog view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.CancelDialog()
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Runs the show connection failed view action for the presenter.
        /// </summary>
        void IDatabaseConnectionFormContract.ShowConnectionFailed()
        {
            MessageBox.ShowDialog(this, Messages.database_connected_failed);
        }
    }
}
