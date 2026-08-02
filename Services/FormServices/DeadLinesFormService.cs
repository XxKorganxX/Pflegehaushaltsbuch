using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DeadLinesFormService
    {
        public DeadLinesFormService(IDeadLinesFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDeadLinesFormContract Form { get; private set; }

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
