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

        public virtual void CreateControl()
        {
            SQLBase.UpdateVersion += UpdateVersion;
            UpdateVersion(null, null);
        }

        public virtual void Closed()
        {
            session.Dispose();
        }

        private void UpdateVersion(string sqlClass, Version version)
        {
            string title = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title);
            string productVersion = Application.ProductVersion.Remove(Application.ProductVersion.LastIndexOf('.'));

            if (version != null)
                View.SetTitle(string.Format("{0} {1} - {2} {3}", title, productVersion, sqlClass, version));
            else
                View.SetTitle(string.Format("{0} {1} - {2}", title, productVersion, Messages.database_not_available));
        }

        private static string GetAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }
    }
}
