using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Progress Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class ProgressDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        /// <summary>
        /// Creates a new Progress Form instance and initializes the required state.
        /// </summary>
        public ProgressDialog(string text)
        {
            InitializeComponent();
            UpdateText(text);
        }
        new public void Close()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    Close();
                });
                return;
            }
            base.Close();
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
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    UpdateText(text);
                });
                return;
            }
            currentLabel.Text = text;
        }
        /// <summary>
        /// Updates the progress data and refreshes the related application state.
        /// </summary>
        public void UpdateProgress(int percent, bool increment = false)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    UpdateProgress(percent, increment);
                });
                return;
            }
            if (increment)
                progressBar.Value += percent;
            else
                progressBar.Value = percent;
        }
        /// <summary>
        /// Updates the maximum Progress data and refreshes the related application state.
        /// </summary>
        public void UpdateMaximumProgress(int percent)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    UpdateMaximumProgress(percent);
                });
                return;
            }
            progressBar.Maximum = percent;
            progressBar.Value = 0;
        }
    }
}
