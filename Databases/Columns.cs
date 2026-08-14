using Pflegehaushaltsbuch.Databases;

namespace Pflegehaushaltsbuch.Databases
{
    internal static class Columns
    {
        public static string Id => SQLBase.Names(SQLBase.ColumnNames.id);
        public static string Title => SQLBase.Names(SQLBase.ColumnNames.title);
        public static string Name => SQLBase.Names(SQLBase.ColumnNames.name);
        public static string Street => SQLBase.Names(SQLBase.ColumnNames.street);
        public static string Phone => SQLBase.Names(SQLBase.ColumnNames.phone);
        public static string Fax => SQLBase.Names(SQLBase.ColumnNames.fax);
        public static string Email => SQLBase.Names(SQLBase.ColumnNames.email);
        public static string Zipcode => SQLBase.Names(SQLBase.ColumnNames.zipcode);
        public static string City => SQLBase.Names(SQLBase.ColumnNames.city);
        public static string Born => SQLBase.Names(SQLBase.ColumnNames.born);
        public static string Login => SQLBase.Names(SQLBase.ColumnNames.login);
        public static string Password => SQLBase.Names(SQLBase.ColumnNames.pw);
        public static string Access => SQLBase.Names(SQLBase.ColumnNames.access);
        public static string Date => SQLBase.Names(SQLBase.ColumnNames.date);
        public static string Type => SQLBase.Names(SQLBase.ColumnNames.type);
        public static string AccountId => SQLBase.Names(SQLBase.ColumnNames.account_id);
        public static string AccountTransfer => SQLBase.Names(SQLBase.ColumnNames.account_transfer);
        public static string Amount => SQLBase.Names(SQLBase.ColumnNames.amount);
        public static string AmountPayout => SQLBase.Names(SQLBase.ColumnNames.amount_payout);
        public static string AmountPayback => SQLBase.Names(SQLBase.ColumnNames.amount_payback);
        public static string AmountPaybackType => SQLBase.Names(SQLBase.ColumnNames.amount_payback_type);
        public static string LastBook => SQLBase.Names(SQLBase.ColumnNames.lastbook);
        public static string Active => SQLBase.Names(SQLBase.ColumnNames.active);
        public static string Info => SQLBase.Names(SQLBase.ColumnNames.info);
        public static string Note => SQLBase.Names(SQLBase.ColumnNames.note);
        public static string AdvisorId => SQLBase.Names(SQLBase.ColumnNames.advisor_id);
        public static string DocumentId => SQLBase.Names(SQLBase.ColumnNames.document_id);
        public static string BookTo => SQLBase.Names(SQLBase.ColumnNames.book_to);
        public static string BookCategory => SQLBase.Names(SQLBase.ColumnNames.book_cat);
        public static string HandSign => SQLBase.Names(SQLBase.ColumnNames.handsign);
        public static string CreatedAt => SQLBase.Names(SQLBase.ColumnNames.created_at);
        public static string FailedLoginAttempts => SQLBase.Names(SQLBase.ColumnNames.failed_login_attempts);
        public static string LastFailedLogin => SQLBase.Names(SQLBase.ColumnNames.last_failed_login);
        public static string LockedUntil => SQLBase.Names(SQLBase.ColumnNames.locked_until);
        public static string Admin => SQLBase.Names(SQLBase.ColumnNames.admin);
        public const string Co = "co";

        public const string Credit = "credit";
        public const string Debit = "debit";

        public const string ExportDebitorNumber = "Debitor No.";
        public const string ExportTitle = "Title";
        public const string ExportName = "Name";
        public const string ExportBorn = "Born";
        public const string ExportStreet = "Street";
        public const string ExportZip = "Zip";
        public const string ExportCity = "City";
        public const string ExportAdvisor = "Advisor";
        public const string ExportPreviousBalance = "Previous balance";
    }
}
