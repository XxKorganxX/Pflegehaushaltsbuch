using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class ImprovedFormService
    {
        public ImprovedFormService(IImprovedFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IImprovedFormContract Form { get; private set; }

        public virtual void OnUserRights()
        {
        }

        public virtual void Send()
        {
        }

        public virtual void Back()
        {
        }
    }
}
