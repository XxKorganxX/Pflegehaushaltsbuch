using Pflegehaushaltsbuch.Forms.Dialoge;
using System;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ProgressDialogPresenter
    {
        private readonly IProgressDialogContract view;

        public ProgressDialogPresenter(IProgressDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize(string text)
        {
            UpdateText(text);
        }

        public virtual void Close()
        {
            view.CloseView();
        }

        public virtual void UpdateText(string text)
        {
            view.SetText(text);
        }

        public virtual void UpdateProgress(int percent, bool increment = false)
        {
            view.SetProgress(percent, increment);
        }

        public virtual void UpdateMaximumProgress(int percent)
        {
            view.SetMaximumProgress(percent);
        }
    }
}
