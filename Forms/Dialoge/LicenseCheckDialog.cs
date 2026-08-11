using System;
using System.ComponentModel;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the License Check Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class LicenseCheckDialog : Form, INotifyPropertyChanged, ILicenseCheckDialogContract
    {
        private readonly LicenseCheckDialogPresenter presenter;
        private bool outputEnabled = false;
        private string msg = string.Empty;

        /// <summary>
        /// Creates a new LicenseCheckDialog view.
        /// </summary>
        public LicenseCheckDialog()
        {
            InitializeComponent();
            presenter = new LicenseCheckDialogPresenter(this);
            presenter.Initialize();
        }

        /// <summary>
        /// Provides the output value.
        /// </summary>
        public string Output
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
                FirePropertyChanged("Output");
            }
        }

        public bool OutputEnabled { get { return outputEnabled; } set { outputEnabled = value; FirePropertyChanged("OutputEnabled"); } }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Accept();
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
            presenter.MoveWindow(e);
        }

        /// <summary>
        /// Runs the bind output view action for the presenter.
        /// </summary>
        void ILicenseCheckDialogContract.BindOutput()
        {
            textBox.DataBindings.Add("Text", this, "Output");
            okButton.DataBindings.Add("Enabled", this, "OutputEnabled");
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void ILicenseCheckDialogContract.CloseView()
        {
            Close();
        }

        /// <summary>
        /// Runs the move window view action for the presenter.
        /// </summary>
        void ILicenseCheckDialogContract.MoveWindow(MouseEventArgs e)
        {
            WindowMove(null, e);
        }
    }
}
