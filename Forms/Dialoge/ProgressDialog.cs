using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Progress Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ProgressDialog : Form, IProgressDialogContract, IAdministrationProgress
    {
        private readonly ProgressDialogPresenter presenter;

        /// <summary>
        /// Creates a new ProgressDialog view.
        /// </summary>
        public ProgressDialog(string text)
        {
            InitializeComponent();
            presenter = new ProgressDialogPresenter(this);
            presenter.Initialize(text);
        }

        new public void Close()
        {
            presenter.Close();
        }

        /// <summary>
        /// Releases resources used by this instance and performs the required cleanup.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    Dispose(disposing);
                });
                return;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Updates the text data and refreshes the related application state.
        /// </summary>
        public void UpdateText(string text)
        {
            presenter.UpdateText(text);
        }

        /// <summary>
        /// Updates the progress data and refreshes the related application state.
        /// </summary>
        public void UpdateProgress(int percent, bool increment = false)
        {
            presenter.UpdateProgress(percent, increment);
        }

        /// <summary>
        /// Updates the maximum Progress data and refreshes the related application state.
        /// </summary>
        public void UpdateMaximumProgress(int percent)
        {
            presenter.UpdateMaximumProgress(percent);
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IProgressDialogContract.CloseView()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    ((IProgressDialogContract)this).CloseView();
                });
                return;
            }

            base.Close();
        }

        /// <summary>
        /// Runs the set text view action for the presenter.
        /// </summary>
        void IProgressDialogContract.SetText(string text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    ((IProgressDialogContract)this).SetText(text);
                });
                return;
            }

            currentLabel.Text = text;
        }

        /// <summary>
        /// Runs the set progress view action for the presenter.
        /// </summary>
        void IProgressDialogContract.SetProgress(int percent, bool increment)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    ((IProgressDialogContract)this).SetProgress(percent, increment);
                });
                return;
            }

            if (increment)
            {
                progressBar.Value += percent;
            }
            else
            {
                progressBar.Value = percent;
            }
        }

        /// <summary>
        /// Runs the set maximum progress view action for the presenter.
        /// </summary>
        void IProgressDialogContract.SetMaximumProgress(int percent)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    ((IProgressDialogContract)this).SetMaximumProgress(percent);
                });
                return;
            }

            progressBar.Maximum = percent;
            progressBar.Value = 0;
        }
    }
}
