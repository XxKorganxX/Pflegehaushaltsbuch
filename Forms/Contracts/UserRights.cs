using Pflegehaushaltsbuch.Data;

namespace Pflegehaushaltsbuch.Forms
{
    public class UserRights
    {
        public bool CanRead { get; set; }
        public bool CanInsert { get; set; }
        public bool CanModify { get; set; }
        public bool CanDelete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSupervisor { get; set; }

        public static UserRights FromUser(User user)
        {
            if (user == null)
                return new UserRights();

            return new UserRights
            {
                CanRead = true,
                CanInsert = user.CanInsert,
                CanModify = user.CanModify,
                CanDelete = user.CanDelete,
                IsAdmin = user.Admin,
                IsSupervisor = user.Supervisor
            };
        }
    }
}
