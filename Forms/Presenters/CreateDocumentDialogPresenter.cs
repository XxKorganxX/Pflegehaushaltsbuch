using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Data;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreateDocumentDialogPresenter
    {
        private readonly ICreateDocumentDialogContract view;
        private readonly SqlSession session;
        private readonly int clientID;

        public CreateDocumentDialogPresenter(ICreateDocumentDialogContract view, SqlSession session, int clientID, string filename, DataTable table)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
            this.clientID = clientID;

            view.FilePath = filename;
            view.BindClients(table, clientID);
        }

        public virtual void Ok()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(view.FilePath))
                    throw new Exception(Messages.document_missing_filename);
                if (string.IsNullOrWhiteSpace(view.Description))
                    throw new Exception(Messages.document_missing_description);
            }
            catch
            {
                view.SetDialogResultNone();
                throw;
            }
        }

        public virtual void Import()
        {
        }
    }
}
