using Pflegehaushaltsbuch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the License Check Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class LicenseCheckDialog : Pflegehaushaltsbuch.FormControls.Form, INotifyPropertyChanged
    {

        private bool outputEnabled = false;
        private string msg = string.Empty;
        /// <summary>
        /// Creates a new License Check Form instance and initializes the required state.
        /// </summary>
        public LicenseCheckDialog()
        {
            InitializeComponent();
            Output = Messages.license_check;
            textBox.DataBindings.Add("Text", this, "Output");
            okButton.DataBindings.Add("Enabled", this, "OutputEnabled");
        }
        public string Output
        {
            get 
            { 
                return msg; 
            }
            set 
            { 
                msg = value; FirePropertyChanged("Output"); 
            }
        }
        public bool OutputEnabled { get { return outputEnabled; } set { outputEnabled = value; FirePropertyChanged("OutputEnabled"); } }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Runs the fire Property Changed operation and updates the related application state.
        /// </summary>
        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Handles the mouse Move event for table Layout Panel1 and updates the related state.
        /// </summary>
        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            WindowMove(null, e);
        }
    }
}
