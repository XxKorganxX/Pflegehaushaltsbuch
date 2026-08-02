using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class AssistantsFormPresenter
    {
        public AssistantsFormPresenter(IAssistantsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IAssistantsFormContract View { get; private set; }

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
