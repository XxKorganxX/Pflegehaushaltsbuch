using System.ComponentModel;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Input Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class InputDialog : Form, IInputDialogContract
    {
        private readonly InputDialogPresenter presenter;

        /// <summary>
        /// Creates a new Input Form instance and initializes the required state.
        /// </summary>
        protected InputDialog()
        {
            InitializeComponent();
            presenter = new InputDialogPresenter(this);
            presenter.Initialize();
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
            using (InputDialog inputForm = new InputDialog())
            {
                return inputForm.presenter.ShowInput(owner, inputTxt, out value);
            }
        }

        /// <summary>
        /// Runs the bind text fields view action for the presenter.
        /// </summary>
        void IInputDialogContract.BindTextFields()
        {
            inputText.DataBindings.Add("Text", this, "InputTxt");
            outputTextBox.DataBindings.Add("Text", this, "OutputTxt");
        }

        /// <summary>
        /// Runs the show view action for the presenter.
        /// </summary>
        DialogResult IInputDialogContract.ShowView(IWin32Window owner)
        {
            return ShowDialog(owner);
        }
    }
}
