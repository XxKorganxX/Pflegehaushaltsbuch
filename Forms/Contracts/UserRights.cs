using Pflegehaushaltsbuch.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public class UserRights
    {
        public bool CanRead { get; set; }
        public bool CanInsert { get; set; }
        public bool CanModify { get; set; }
        public bool CanDelete { get; set; }
        public bool CanBook { get; set; }
        public bool CanCancelBooking { get; set; }
        public bool CanAccessCashBalance { get; set; }
        public bool CanAccessBankBalance { get; set; }
        public bool CanAccessPettyCash { get; set; }
        public bool CanAccessClients { get; set; }
        public bool CanAccessRepresentatives { get; set; }
        public bool CanAccessEmployees { get; set; }
        public bool CanAccessDocuments { get; set; }
        public bool CanAccessCashAudit { get; set; }
        public bool CanAccessStatistics { get; set; }
        public bool IsAdmin { get; set; }

        public static UserRights FromUser(User user)
        {
            if (user == null)
                return new UserRights
                {
                    IsAdmin = true
                };

            return new UserRights
            {
                CanRead = true,
                CanInsert = user.CanInsert,
                CanModify = user.CanModify,
                CanDelete = user.CanDelete,
                CanBook = user.CanBook,
                CanCancelBooking = user.CanCancelBooking,
                CanAccessCashBalance = user.CanAccessCashBalance,
                CanAccessBankBalance = user.CanAccessBankBalance,
                CanAccessPettyCash = user.CanAccessPettyCash,
                CanAccessClients = user.CanAccessClients,
                CanAccessRepresentatives = user.CanAccessRepresentatives,
                CanAccessEmployees = user.CanAccessEmployees,
                CanAccessDocuments = user.CanAccessDocuments,
                CanAccessCashAudit = user.CanAccessCashAudit,
                CanAccessStatistics = user.CanAccessStatistics,
                IsAdmin = user.Admin
            };
        }
    }
}
