using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Globalization;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class EditTextDialogPresenter
    {
        private readonly IEditTextDialogContract view;

        public EditTextDialogPresenter(IEditTextDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(string text)
        {
            view.SetVariableDataSource(Enum.GetValues(typeof(Printing.VarNames)));
            view.EditorText = text;
            view.Content = text;
        }

        public virtual void InsertUnicode()
        {
            if (string.IsNullOrWhiteSpace(view.UnicodeText))
            {
                return;
            }

            int value = int.Parse(view.UnicodeText, NumberStyles.HexNumber);
            view.ReplaceSelection(char.ConvertFromUtf32(value).ToString());
        }

        public virtual void InsertSelectedVariable()
        {
            if (view.SelectedVariableText == null)
            {
                return;
            }

            Printing.VarNames variable = (Printing.VarNames)Enum.Parse(typeof(Printing.VarNames), view.SelectedVariableText);
            view.ReplaceSelection(EnumHelper.ToDescription(variable));
        }

        public virtual void Accept()
        {
            view.Content = view.EditorText;
        }
    }
}
