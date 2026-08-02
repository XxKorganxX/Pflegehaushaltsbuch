using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class DesignFormService
    {
        public DesignFormService(IDesignFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IDesignFormContract Form { get; private set; }

        public virtual void SelectPath()
        {
        }
    }
}
