using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class LicenseCheckDialogPresenter
    {
        private readonly ILicenseCheckDialogContract view;

        public LicenseCheckDialogPresenter(ILicenseCheckDialogContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;
        }

        public virtual void Initialize()
        {
            view.Output = Messages.license_check;
            view.BindOutput();
        }

        public virtual void Accept()
        {
            view.CloseView();
        }

        public virtual void MoveWindow(MouseEventArgs e)
        {
            view.MoveWindow(e);
        }
    }
}
