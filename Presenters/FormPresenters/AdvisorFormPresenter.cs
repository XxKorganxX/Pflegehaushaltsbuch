using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class AdvisorFormPresenter
    {
        public AdvisorFormPresenter(IAdvisorFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IAdvisorFormContract View { get; private set; }

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
