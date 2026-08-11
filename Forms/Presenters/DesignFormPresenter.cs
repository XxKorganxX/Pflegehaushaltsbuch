using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Properties;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DesignFormPresenter
    {
        private readonly SqlSession session;
        private bool restartSystem;

        public DesignFormPresenter(IDesignFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected IDesignFormContract View { get; private set; }

        public virtual void Initialize()
        {
            Settings.Default.PropertyChanged += Default_PropertyChanged;
            View.BindSettings();
        }

        public virtual void Closing()
        {
            Settings.Default.Save();
            if (restartSystem)
            {
                View.ShowRestartRequired();
                View.RestartApplication();
            }
        }

        public virtual void Closed()
        {
            Settings.Default.PropertyChanged -= Default_PropertyChanged;
        }

        public virtual void AfterSelect(int index)
        {
            View.SelectTab(index);
        }

        public virtual void SelectPath()
        {
            string selectedPath;
            if (View.ShowFolderDialog(out selectedPath))
                Settings.Default.documentPath = selectedPath;
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("BackgroundColorMode".Equals(e.PropertyName) || "FontSize".Equals(e.PropertyName) || "language".Equals(e.PropertyName))
                restartSystem = true;
        }

    }
}
