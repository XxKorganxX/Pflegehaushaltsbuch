using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class InputDialogPresenter
    {
        private readonly IInputDialogContract view;

        public InputDialogPresenter(IInputDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize()
        {
            view.BindTextFields();
        }

        public virtual DialogResult ShowInput(IWin32Window owner, string inputTxt, out string value)
        {
            value = string.Empty;
            view.InputTxt = inputTxt;

            DialogResult dialogResult = view.ShowView(owner);
            value = view.OutputTxt;
            return dialogResult;
        }
    }
}
