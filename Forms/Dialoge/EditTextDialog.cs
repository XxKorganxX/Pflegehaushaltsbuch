using System;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Edit Text Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EditTextDialog : Form, IEditTextDialogContract
    {
        private readonly EditTextDialogPresenter presenter;
        public string Content { get; set; }

        /// <summary>
        /// Creates a new EditTextDialog view.
        /// </summary>
        public EditTextDialog(SqlSession session, string text)
        {
            InitializeComponent();
            Session = session;
            presenter = new EditTextDialogPresenter(this);
            presenter.Initialize(text);
        }

        /// <summary>
        /// Handles the click event for unicode Button and updates the related state.
        /// </summary>
        private void unicodeButton_Click(object sender, EventArgs e)
        {
            presenter.InsertUnicode();
        }

        /// <summary>
        /// Handles the double Click event for list Box and updates the related state.
        /// </summary>
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            presenter.InsertSelectedVariable();
        }

        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Accept();
        }

        /// <summary>
        /// Provides the editor text value for the presenter.
        /// </summary>
        string IEditTextDialogContract.EditorText
        {
            get { return richTextBox.Text; }
            set { richTextBox.Text = value; }
        }

        /// <summary>
        /// Provides the unicode text value for the presenter.
        /// </summary>
        string IEditTextDialogContract.UnicodeText
        {
            get { return unicodeTextbox.Text; }
        }

        /// <summary>
        /// Provides the selected variable text value for the presenter.
        /// </summary>
        string IEditTextDialogContract.SelectedVariableText
        {
            get { return listBox.SelectedItem == null ? null : listBox.SelectedItem.ToString(); }
        }

        /// <summary>
        /// Runs the set variable data source view action for the presenter.
        /// </summary>
        void IEditTextDialogContract.SetVariableDataSource(object dataSource)
        {
            listBox.DataSource = dataSource;
        }

        /// <summary>
        /// Runs the replace selection view action for the presenter.
        /// </summary>
        void IEditTextDialogContract.ReplaceSelection(string text)
        {
            int selectionIndex = richTextBox.SelectionStart;
            string newText = richTextBox.Text;

            if (richTextBox.SelectionLength > 0)
            {
                newText = newText.Remove(selectionIndex, richTextBox.SelectionLength);
            }

            richTextBox.Text = newText.Insert(selectionIndex, text);
            richTextBox.SelectionStart = selectionIndex + text.Length;
        }
    }
}
