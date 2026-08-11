using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Properties;
using System.Globalization;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Design Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DesignForm : Form, IDesignFormContract
    {
        private readonly DesignFormPresenter presenter;
        private bool settingsBound;

        /// <summary>
        /// Creates a new DesignForm view.
        /// </summary>
        public DesignForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new DesignFormPresenter(this, session);
            Closed += DesignForm_Closed;
            Closing += DesignForm_Closing;
            presenter.Initialize();
        }

        /// <summary>
        /// Handles the closing event for design Form and updates the related state.
        /// </summary>
        private void DesignForm_Closing(object sender, CancelEventArgs e)
        {
            presenter.Closing();
        }

        /// <summary>
        /// Handles the closed event for design Form and updates the related state.
        /// </summary>
        private void DesignForm_Closed(object sender, EventArgs e)
        {
            presenter.Closed();
        }

        /// <summary>
        /// Handles the after Select event for view and updates the related state.
        /// </summary>
        private void view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            presenter.AfterSelect(e.Node.Index);
        }

        /// <summary>
        /// Handles the click event for select Path Button and updates the related state.
        /// </summary>
        private void selectPathButton_Click(object sender, EventArgs e)
        {
            presenter.SelectPath();
        }

        /// <summary>
        /// Runs the bind settings view action for the presenter.
        /// </summary>
        void IDesignFormContract.BindSettings()
        {
            if (settingsBound)
                return;

            settingsBound = true;
            var binding = fontSizeBox.DataBindings.Add("Text", Settings.Default, "FontSize");
            binding.Parse += FontSizeBinding_Parse;
            backColorModeBox.DataBindings.Add("SelectedIndex", Settings.Default, "BackgroundColorMode");
            documentPathBox.DataBindings.Add("Text", Settings.Default, "documentPath");
        }

        /// <summary>
        /// Runs the select tab view action for the presenter.
        /// </summary>
        void IDesignFormContract.SelectTab(int index)
        {
            tabControl.SelectedIndex = index;
        }

        /// <summary>
        /// Runs the show folder dialog view action for the presenter.
        /// </summary>
        bool IDesignFormContract.ShowFolderDialog(out string selectedPath)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    selectedPath = folderBrowserDialog.SelectedPath;
                    return true;
                }
            }

            selectedPath = null;
            return false;
        }

        /// <summary>
        /// Runs the show restart required view action for the presenter.
        /// </summary>
        void IDesignFormContract.ShowRestartRequired()
        {
            MessageBox.Show(this, Messages.design_restart_required);
        }

        /// <summary>
        /// Runs the restart application view action for the presenter.
        /// </summary>
        void IDesignFormContract.RestartApplication()
        {
            Application.Restart();
        }

        private static void FontSizeBinding_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = 10;
                return;
            }

            float value;
            if (float.TryParse(e.Value.ToString(), NumberStyles.Float, CultureInfo.CurrentCulture, out value))
                e.Value = Math.Max(6, value);
        }
    }
}
