using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class AssistantsFormService
    {
        public AssistantsFormService(IAssistantsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IAssistantsFormContract Form { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void UpdateTotalAmount()
        {
        }

        public virtual void Create()
        {
        }

        public virtual void Change()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void PayOut()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void ChangeAssistant()
        {
        }

        public virtual void Export()
        {
        }

        public virtual void ButtonImport()
        {
        }
    }
}
