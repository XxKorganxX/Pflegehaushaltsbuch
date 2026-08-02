using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    /// <summary>
    /// Represents the Edit Text Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EditTextDialog : Pflegehaushaltsbuch.FormControls.Form
    {

        public string Content { get; set; }
        /// <summary>
        /// Creates a new Edit Text Form instance and initializes the required state.
        /// </summary>
        public EditTextDialog(SQLBase sql, string text)
        {
            InitializeComponent();
            this.sql = sql;
            listBox.DataSource = Enum.GetValues(typeof(Printing.VarNames));// new BindingSource(sql.Printing.varNames, null); 
            //listBox.DisplayMember = "Value";
            //listBox.ValueMember = "Key";
            /*
            listBox.Items.Clear();
            foreach (var value in Enum.GetValues(typeof(Printing.VarNames)))//.Values)
                listBox.Items.Add(value);
            */
            //varNames
            richTextBox.Text = text;
            Content = text;
        }
        /// <summary>
        /// Handles the click event for unicode Button and updates the related state.
        /// </summary>
        private void unicodeButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(unicodeTextbox.Text))
                return;
            string text = unicodeTextbox.Text;

            int value = int.Parse(text, NumberStyles.HexNumber);
            text = char.ConvertFromUtf32(value).ToString();
            var selectionIndex = richTextBox.SelectionStart;

            string newText = richTextBox.Text;
            newText = newText.Remove(selectionIndex, richTextBox.SelectionLength);
            richTextBox.Text = newText.Insert(selectionIndex, text);
            richTextBox.SelectionStart = selectionIndex + text.Length;
        }
        /// <summary>
        /// Handles the double Click event for list Box and updates the related state.
        /// </summary>
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null)
                return;
            string text = listBox.SelectedItem.ToString();
            Printing.VarNames variable = (Printing.VarNames)Enum.Parse(typeof(Printing.VarNames), text);
            text = EnumHelper.ToDescription(variable);
            var selectionIndex = richTextBox.SelectionStart;
            if (richTextBox.SelectionLength > 0)
                richTextBox.Text = richTextBox.Text.Remove(selectionIndex, richTextBox.SelectionLength);
            richTextBox.Text = richTextBox.Text.Insert(selectionIndex, text);
            richTextBox.SelectionStart = selectionIndex + text.Length;
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Content = richTextBox.Text;
        }
    }
}
