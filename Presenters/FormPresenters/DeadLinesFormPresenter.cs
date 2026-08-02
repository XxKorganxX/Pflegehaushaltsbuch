using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DeadLinesFormPresenter
    {
        public DeadLinesFormPresenter(IDeadLinesFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDeadLinesFormContract View { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void ConnectToClient()
        {
        }

        public virtual void Export()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Update()
        {
        }
    }
}
