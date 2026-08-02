using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DatabaseFileFormService
    {
        public DatabaseFileFormService(IDatabaseFileFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDatabaseFileFormContract Form { get; private set; }

        public virtual void Create()
        {
        }

        public virtual void Connect()
        {
        }
    }
}
