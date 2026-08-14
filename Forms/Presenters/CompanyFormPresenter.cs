using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CompanyFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        public CompanyFormPresenter(ICompanyFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected ICompanyFormContract View { get; private set; }

        public virtual void CreateControl()
        {
            View.BindCompany(session.Company);
        }

        public virtual void Enter()
        {
            View.BindCompany(session.Company);
            View.ShowCompanyLogo();
        }

        public virtual async Task CompanySaveAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await session.Company.Save(session.SQL);
                View.ShowCompanySaved();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Back()
        {
            session.SQL.Printing.UpdateUserAndCompany(session.Company);
            View.ShowAdministrationForm();
        }

        public virtual void Logo()
        {
            string fileName;
            if (!View.ShowLogoDialog(out fileName))
                return;

            Image previousLogo = session.Company.Logo;
            session.Company.Logo = LoadLogo(fileName);
            View.ShowCompanyLogo();
            session.SQL.Printing.UpdateVariable(Printing.VarNames.company_logo, session.Company.Logo);
            previousLogo?.Dispose();
        }

        public virtual bool IsEmailValid()
        {
            return string.IsNullOrWhiteSpace(View.Email) || Company.IsValidEmail(View.Email);
        }

        private static Image LoadLogo(string fileName)
        {
            using (FileStream stream = File.OpenRead(fileName))
            using (Image image = Image.FromStream(stream))
            {
                return new Bitmap(image);
            }
        }
    }
}
