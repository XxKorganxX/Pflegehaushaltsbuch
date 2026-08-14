using System;
using System.Linq;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Represents the enums component used by the application.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Defines the available update Journal values used by the application.
        /// </summary>
        public enum UpdateJournal
        {
            Create,
            Update,
            Delete
        }
        /// <summary>
        /// Defines the available state values used by the application.
        /// </summary>
        public enum State
        {
            InActive,
            Active,
            Dead
        }
        /// <summary>
        /// Defines the available forms values used by the application.
        /// </summary>
        public enum Forms
        {
            Main,
            Clients,
            Book,
            Calendar,
            OfficeCash,
            Cash,
            Credits,
            Inventory,
            UserRights,
            FirstLogin,
            Login,
            Advisor,
            CashOfficeControl,
            Banking,
            Company,
            LayoutManager,
            Record,
            Administration,
            Suggestionbox,
            DataExchange,
            AboutUs
        }
        /// <summary>
        /// Defines the available user Right Enum values used by the application.
        /// </summary>
        [Flags]
        public enum UserRightEnum
        {
            None = 0,
            Insert = 1,
            Change = 2,
            Book = 4,
            CancelBooking = 8,
            CashBalance = 16,
            BankBalance = 32,
            PettyCash = 64,
            Clients = 128,
            Representatives = 256,
            Employees = 512,
            Documents = 1024,
            CashAudit = 2048,
            Statistics = 4096
        }
    }
}
