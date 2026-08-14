using Pflegehaushaltsbuch.Databases;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class MainFormPresenter
    {
        private readonly SqlSession session;

        public MainFormPresenter(IMainFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IMainFormContract View { get; private set; }

        public virtual void Initialize()
        {
            View.InitializeAutomation();
        }

        public virtual void Closed()
        {
            session.Dispose();
        }
    }
}
