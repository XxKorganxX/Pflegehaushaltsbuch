using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DatabaseUpdateDialogPresenter
    {
        private readonly IDatabaseUpdateDialogContract view;
        private readonly SqlSession session;

        public DatabaseUpdateDialogPresenter(IDatabaseUpdateDialogContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
        }

        public virtual void Initialize()
        {
            session.SQL.PrintCurrentVersion += UpdateVersionText;
        }

        public virtual async Task ShownAsync()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await session.SQL.UpdateAsync();
                });

                view.CloseView();
            }
            catch (Exception err)
            {
                view.ShowError(err);
                view.CloseOwner();
                view.ExitApplication(1);
            }
        }

        private void UpdateVersionText(Version version)
        {
            view.SetVersion(version);
        }
    }
}
