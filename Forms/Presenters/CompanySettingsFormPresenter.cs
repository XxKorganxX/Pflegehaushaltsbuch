using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CompanySettingsFormPresenter
    {
        private readonly SqlSession session;
        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        public CompanySettingsFormPresenter(ICompanySettingsFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected ICompanySettingsFormContract View { get; private set; }

        public virtual void CreateControl()
        {
            View.BindCompany(new Company());
        }

        public virtual void Enter()
        {
            View.BindCompany(session.SQL.Company);
            View.ShowCompanyLogo();
        }

        public virtual async Task CompanySaveAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await session.SQL.Company.Save(session.SQL);
                View.ShowCompanySaved();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Back()
        {
            session.SQL.Printing.UpdateUserAndCompany(session.SQL);
            View.ShowAdministrationForm();
        }

        public virtual void Logo()
        {
            string fileName;
            if (!View.ShowLogoDialog(out fileName))
                return;

            session.SQL.Company.Logo = Image.FromFile(fileName);
            View.ShowCompanyLogo();
            session.SQL.Printing.UpdateVariable(Printing.VarNames.company_logo, session.SQL.Company.Logo);
        }

        public virtual bool IsEmailValid()
        {
            return string.IsNullOrWhiteSpace(View.Email) || Company.IsValidEmail(View.Email);
        }
    }
}
