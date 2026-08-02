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
    /// Represents the Input Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class InputDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        /// <summary>
        /// Creates a new Input Form instance and initializes the required state.
        /// </summary>
        protected InputDialog()
        {
            InitializeComponent();
            inputText.DataBindings.Add("Text", this, "InputTxt");
            outputTextBox.DataBindings.Add("Text", this, "OutputTxt");
        }
        [DefaultValue("")]
        public string InputTxt { get; set; }
        [DefaultValue("")]
        public string OutputTxt { get; set; }
        /// <summary>
        /// Runs the show Input operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowInput(IWin32Window owner, string inputTxt, out string value)
        {
            value = string.Empty;
            using (InputDialog InputForm = new InputDialog(){ InputTxt = inputTxt })
            {
                var dialogResult = InputForm.ShowDialog(owner);
                value = InputForm.OutputTxt;
                return dialogResult;
            }
        }
    }
}
