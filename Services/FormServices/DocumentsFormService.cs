using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DocumentsFormService
    {
        public DocumentsFormService(IDocumentsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDocumentsFormContract Form { get; private set; }

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
