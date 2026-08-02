using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Database Update Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseUpdateDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        /// <summary>
        /// Creates a new Database Update Form instance and initializes the required state.
        /// </summary>
        public DatabaseUpdateDialog(SQLBase sql)
        {
            InitializeComponent();
            this.sql = sql;
            sql.PrintCurrentVersion += updateVersionText;
        }
        void updateVersionText(Version version)
        {
            if (versionBox.InvokeRequired)
            {
                versionBox.Invoke((MethodInvoker)delegate
                {
                    updateVersionText(version);
                });
                return;
            }
            versionBox.Text = version.ToString();
        }
        /// <summary>
        /// Handles the shown event for database Update Form and updates the related state.
        /// </summary>
        private async void DatabaseUpdateForm_Shown(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(async () =>
                {
                    await sql.UpdateAsync();
                });

                Close();
            }
            catch (Exception err)
            {
                MessageBox.ShowError(this, err);
                Owner?.Close();
                Environment.Exit(1);
            }
        }
    }
}
