using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class DocumentsFormPresenter
    {
        public DocumentsFormPresenter(IDocumentsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IDocumentsFormContract View { get; private set; }

        public virtual void ConnectToClients()
        {
        }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void Insert()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void Change()
        {
        }

        public virtual void Back()
        {
        }
    }
}
