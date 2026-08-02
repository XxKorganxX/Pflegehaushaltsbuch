using Pflegehaushaltsbuch.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Design Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DesignForm : Pflegehaushaltsbuch.FormControls.Form, IDesignFormContract
    {
        private readonly DesignFormPresenter presenter;


        bool restartSystem = false;
        /// <summary>
        /// Creates a new Design Form instance and initializes the required state.
        /// </summary>
        public DesignForm()
        {
            InitializeComponent();
            presenter = new DesignFormPresenter(this);
            Closed += DesignForm_Closed;
            Closing += DesignForm_Closing;
            Settings.Default.PropertyChanged += Default_PropertyChanged;
            if (CultureInfo.CurrentCulture.Name.ToLower().Contains("de"))
                languageBox.SelectedIndex = 0;
            else
                languageBox.SelectedIndex = 1;
            var binding = fontSizeBox.DataBindings.Add("Text", Settings.Default, "FontSize");
            binding.Parse += Binding_Parse;
            backColorModeBox.DataBindings.Add("SelectedIndex", Settings.Default, "BackgroundColorMode");
            documentPathBox.DataBindings.Add("Text", Settings.Default, "documentPath");
            languageBox.Validated += LanguageBox_Validated;
        }
        /// <summary>
        /// Handles the closing event for design Form and updates the related state.
        /// </summary>
        private void DesignForm_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default.Save();
            if (restartSystem)
            {
                MessageBox.Show(this, Messages.design_restart_required);
                Application.Restart();
            }
        }
        /// <summary>
        /// Handles the closed event for design Form and updates the related state.
        /// </summary>
        private void DesignForm_Closed(object sender, EventArgs e)
        {
            Settings.Default.PropertyChanged -= Default_PropertyChanged;
        }
        /// <summary>
        /// Handles the property Changed event for default and updates the related state.
        /// </summary>
        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("BackgroundColorMode".Equals(e.PropertyName) || "FontSize".Equals(e.PropertyName) || "language".Equals(e.PropertyName))
                restartSystem = true;
        }
        /// <summary>
        /// Handles the after Select event for view and updates the related state.
        /// </summary>
        private void view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tabControl.SelectedIndex = e.Node.Index;
        }
        /// <summary>
        /// Handles the parse event for binding and updates the related state.
        /// </summary>
        private void Binding_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value == null)
                e.Value = 10;
            else
            {
                float value;
                if (float.TryParse(e.Value.ToString(), out value))
                    e.Value = Math.Max(6, value);
            }
        }
        /// <summary>
        /// Handles the validated event for language Box and updates the related state.
        /// </summary>
        private void LanguageBox_Validated(object sender, EventArgs e)
        {
            if (languageBox.SelectedIndex == 0)
                Settings.Default.language = "de";
            else
                Settings.Default.language = "en";
        }
        /// <summary>
        /// Handles the click event for select Path Button and updates the related state.
        /// </summary>
        private void selectPathButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.documentPath = folderBrowserDialog.SelectedPath;
                }
            }
        }
    }
}
