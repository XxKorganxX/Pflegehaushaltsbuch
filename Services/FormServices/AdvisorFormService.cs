using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class AdvisorFormService
    {
        public AdvisorFormService(IAdvisorFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IAdvisorFormContract Form { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void ChangeAdvisorAsync()
        {
        }

        public virtual void CreateAccount()
        {
        }

        public virtual void Change()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void Import()
        {
        }

        public virtual void Export()
        {
        }

        public virtual void Update()
        {
        }
    }
}
