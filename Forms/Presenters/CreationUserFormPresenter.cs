using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreationUserFormPresenter
    {
        private readonly SqlSession session;
        private bool editMode;
        private string oldUsername;

        public CreationUserFormPresenter(ICreationUserFormContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
        }

        protected ICreationUserFormContract View { get; private set; }

        public virtual void InitializeNew()
        {
            View.InsertAllowed = true;
            View.ChangeAllowed = true;
            View.BookAllowed = true;
            View.CancelBookingAllowed = true;
            View.CashBalanceAllowed = true;
            View.BankBalanceAllowed = true;
            View.PettyCashAllowed = true;
            View.ClientsAllowed = true;
            View.RepresentativesAllowed = true;
            View.EmployeesAllowed = true;
            View.DocumentsAllowed = true;
            View.CashAuditAllowed = true;
            View.StatisticsAllowed = true;
            View.BindData();
        }

        public virtual void InitializeExisting(DataRow row)
        {
            View.Handsign = row[Columns.HandSign].ToString();
            View.Login = row["login"].ToString();
            oldUsername = View.Login;

            int access = Int32.Parse(row["access"].ToString());
            View.Admin = bool.Parse(row["admin"].ToString());
            View.InsertAllowed = (access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert;
            View.ChangeAllowed = (access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change;
            View.BookAllowed = (access & (int)Enums.UserRightEnum.Book) == (int)Enums.UserRightEnum.Book;
            View.CancelBookingAllowed = (access & (int)Enums.UserRightEnum.CancelBooking) == (int)Enums.UserRightEnum.CancelBooking;
            View.CashBalanceAllowed = (access & (int)Enums.UserRightEnum.CashBalance) == (int)Enums.UserRightEnum.CashBalance;
            View.BankBalanceAllowed = (access & (int)Enums.UserRightEnum.BankBalance) == (int)Enums.UserRightEnum.BankBalance;
            View.PettyCashAllowed = (access & (int)Enums.UserRightEnum.PettyCash) == (int)Enums.UserRightEnum.PettyCash;
            View.ClientsAllowed = (access & (int)Enums.UserRightEnum.Clients) == (int)Enums.UserRightEnum.Clients;
            View.RepresentativesAllowed = (access & (int)Enums.UserRightEnum.Representatives) == (int)Enums.UserRightEnum.Representatives;
            View.EmployeesAllowed = (access & (int)Enums.UserRightEnum.Employees) == (int)Enums.UserRightEnum.Employees;
            View.DocumentsAllowed = (access & (int)Enums.UserRightEnum.Documents) == (int)Enums.UserRightEnum.Documents;
            View.CashAuditAllowed = (access & (int)Enums.UserRightEnum.CashAudit) == (int)Enums.UserRightEnum.CashAudit;
            View.StatisticsAllowed = (access & (int)Enums.UserRightEnum.Statistics) == (int)Enums.UserRightEnum.Statistics;
            editMode = true;
            View.BindData();
        }

        public virtual async Task OkAsync()
        {
            if (string.IsNullOrWhiteSpace(View.Handsign))
                throw new Exception(Messages.name);

            int access = GetAccess();
            if (editMode)
            {
                await User.UpdateUser(session.SQL,
                    oldUsername,
                    View.Handsign,
                    View.Login,
                    access,
                    View.Admin);
                View.ShowUserChanged();
            }
            else
            {
                string login = View.Login;
                if (string.IsNullOrWhiteSpace(login))
                {
                    login = View.Handsign;
                    View.Login = login;
                }

                await User.CreateUser(session.SQL,
                    View.Handsign,
                    login,
                    string.Empty,
                    access,
                    View.Admin);
                View.ShowUserCreated();
            }

            View.AcceptDialog();
        }

        private int GetAccess()
        {
            Enums.UserRightEnum userRight = Enums.UserRightEnum.None;
            if (View.InsertAllowed)
                userRight |= Enums.UserRightEnum.Insert;
            if (View.ChangeAllowed)
                userRight |= Enums.UserRightEnum.Change;
            if (View.BookAllowed)
                userRight |= Enums.UserRightEnum.Book;
            if (View.CancelBookingAllowed)
                userRight |= Enums.UserRightEnum.CancelBooking;
            userRight |= GetAreaAccess();
            return (int)userRight;
        }

        private Enums.UserRightEnum GetAreaAccess()
        {
            Enums.UserRightEnum userRight = Enums.UserRightEnum.None;
            if (View.CashBalanceAllowed)
                userRight |= Enums.UserRightEnum.CashBalance;
            if (View.BankBalanceAllowed)
                userRight |= Enums.UserRightEnum.BankBalance;
            if (View.PettyCashAllowed)
                userRight |= Enums.UserRightEnum.PettyCash;
            if (View.ClientsAllowed)
                userRight |= Enums.UserRightEnum.Clients;
            if (View.RepresentativesAllowed)
                userRight |= Enums.UserRightEnum.Representatives;
            if (View.EmployeesAllowed)
                userRight |= Enums.UserRightEnum.Employees;
            if (View.DocumentsAllowed)
                userRight |= Enums.UserRightEnum.Documents;
            if (View.CashAuditAllowed)
                userRight |= Enums.UserRightEnum.CashAudit;
            if (View.StatisticsAllowed)
                userRight |= Enums.UserRightEnum.Statistics;

            return userRight;
        }
    }
}
