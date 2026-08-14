using DocumentFormat.OpenXml.ExtendedProperties;
using Pflegehaushaltsbuch.Databases;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the company component used by the application.
    /// </summary>
    public class Company : INotifyPropertyChanged
    {
        Image logo;
        int logoAlignment;
        string name;
        string secretary;
        string street;
        string zipcode;
        string city;
        string phone;
        string fax;
        string email;
        string web;
        string localCourt;
        string hrb;
        string ik;
        string bank;
        string bankAccountNo;
        string bankCode;
        string bankIban;
        string bankBic;
        int licenseCountValue;
        string currencyCode = null;
        CultureInfo cultureCurrencyCode = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Image Logo
        {
            get { return logo; }
            set { SetProperty(ref logo, value); }
        }

        public int LogoAlignment
        {
            get { return logoAlignment; }
            set { SetProperty(ref logoAlignment, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Secretary
        {
            get { return secretary; }
            set { SetProperty(ref secretary, value); }
        }

        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }

        public string Zipcode
        {
            get { return zipcode; }
            set { SetProperty(ref zipcode, value); }
        }

        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        public string Fax
        {
            get { return fax; }
            set { SetProperty(ref fax, value); }
        }

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Web
        {
            get { return web; }
            set { SetProperty(ref web, value); }
        }

        public string Local_court
        {
            get { return localCourt; }
            set { SetProperty(ref localCourt, value); }
        }

        public string Hrb
        {
            get { return hrb; }
            set { SetProperty(ref hrb, value); }
        }

        public string Ik
        {
            get { return ik; }
            set { SetProperty(ref ik, value); }
        }

        public string Bank
        {
            get { return bank; }
            set { SetProperty(ref bank, value); }
        }

        public string Bank_account_no
        {
            get { return bankAccountNo; }
            set { SetProperty(ref bankAccountNo, value); }
        }

        public string Bank_code
        {
            get { return bankCode; }
            set { SetProperty(ref bankCode, value); }
        }

        public string Bank_iban
        {
            get { return bankIban; }
            set { SetProperty(ref bankIban, value); }
        }

        public string Bank_bic
        {
            get { return bankBic; }
            set { SetProperty(ref bankBic, value); }
        }

        public int licenseCount
        {
            get { return licenseCountValue; }
            set { SetProperty(ref licenseCountValue, value); }
        }

        /// <summary>
        /// Gets or sets the currency code used for monetary values.
        /// </summary>
        public string CurrencyCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(currencyCode))
                    return "EUR";

                return currencyCode;
            }
            set
            {
                string normalizedCurrencyCode = string.IsNullOrWhiteSpace(value) ? "EUR" : value.Trim().ToUpperInvariant();
                if (currencyCode == normalizedCurrencyCode)
                    return;

                currencyCode = normalizedCurrencyCode;
                cultureCurrencyCode = null;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Currencies));
            }
        }

        /// <summary>
        /// Gets the culture used to format monetary values.
        /// </summary>
        public CultureInfo Currencies
        {
            get
            {
                if (cultureCurrencyCode == null)
                    cultureCurrencyCode = MoneyFormat.GetCulture(CurrencyCode);

                return cultureCurrencyCode;
            }
        }
        /// <summary>
        /// Checks whether the email Valid condition is true for the current value.
        /// </summary>
        public bool IsEmailValid()
        {
            try
            {
                EmailAddressAttribute emailAttr = new EmailAddressAttribute();
                return emailAttr.IsValid(Email);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Checks whether the valid Email condition is true for the current value.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;
                EmailAddressAttribute emailAttr = new EmailAddressAttribute();
                return emailAttr.IsValid(email);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Creates a new Company instance and initializes the required state.
        /// </summary>
        public Company()
        {
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void Clear()
        {
            Logo = null;
            LogoAlignment = 0;
            Name = null;
            Secretary = null;
            Street = null;
            Zipcode = null;
            City = null;
            Phone = null;
            Fax = null;
            Email = null;
            Web = null;
            Local_court = null;
            Hrb = null;
            Ik = null;
            Bank = null;
            Bank_account_no = null;
            Bank_code = null;
            Bank_iban = null;
            Bank_bic = null;
            licenseCount = 0;
            CurrencyCode = "EUR";
        }

        /// <summary>
        /// Loads the load data required for the current workflow.
        /// </summary>
        public async Task Load(SQLBase sql)
        {
            Clear();

            DataTable companyTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Company, companyTable);
            if (companyTable.Rows.Count == 0)
                companyTable.Rows.Add(companyTable.NewRow());
            DataRow row = companyTable.Rows[0];
            Name = row[Columns.Name].ToString();
            Phone = row[Columns.Phone].ToString();
            Fax = row[Columns.Fax].ToString();
            Email = row[Columns.Email].ToString();
            Street = row[Columns.Street].ToString();
            Zipcode = row[Columns.Zipcode].ToString();
            City = row[Columns.City].ToString();
            if (row["logo"] != DBNull.Value)
            {
                MemoryStream ms = new MemoryStream(row["logo"] as byte[]);
                Logo = Image.FromStream(ms);
            }
            if (row["logo_alignment"] != DBNull.Value)
                LogoAlignment = Int32.Parse(row["logo_alignment"].ToString());

            Web = row["web"] as string;
            Local_court = row["local_court"] as string;
            Secretary = row["secretary"] as string;
            Hrb = row["hrb"] as string;
            Ik = row["ik"] as string;
            if (companyTable.Columns.Contains("currency_code") && row["currency_code"] != DBNull.Value)
                CurrencyCode = row["currency_code"] as string;

            DataTable bankTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Company_bank, bankTable);

            if (bankTable.Rows.Count > 0)
            {
                row = bankTable.Rows[0];
                Bank = row["name"] as string;
                Bank_account_no = row["account_no"] as string;
                Bank_code = row["code"] as string;
                Bank_iban = row["iban"] as string;
                Bank_bic = row["bic"] as string;
            }

            sql.Printing.UpdateUserAndCompany(this);
        }
        /// <summary>
        /// Saves the save data for the current workflow.
        /// </summary>
        public async Task<bool> Save(SQLBase sql)
        {
            DataTable companyTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Company, companyTable);
            DataRow row = null;
            if (companyTable.Rows.Count == 0)
                row = companyTable.NewRow();
            else
                row = companyTable.Rows[0];
            row[Columns.Name] = Name;
            row[Columns.Phone] = Phone;
            row[Columns.Fax] = Fax;
            row[Columns.Email] = Email;
            row[Columns.Street] = Street;
            row[Columns.Zipcode] = Zipcode;
            row[Columns.City] = City;
            if (Logo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Logo.Save(ms, Logo.RawFormat);
                    row["logo"] = ms.ToArray();
                }
                row["logo_alignment"] = (int)LogoAlignment;
            }

            row["web"] = Web;
            row["local_court"] = Local_court;
            row["secretary"] = Secretary;
            row["hrb"] = Hrb;
            row["ik"] = Ik;
            if (companyTable.Columns.Contains("currency_code"))
                row["currency_code"] = string.IsNullOrWhiteSpace(CurrencyCode) ? "EUR" : CurrencyCode.Trim().ToUpperInvariant();

            if (companyTable.Rows.Count == 0)
                companyTable.Rows.Add(row);

            using (var transaction = sql.BeginTransaction())
            {
                try
                {
                    if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Company, companyTable))
                        return false;
                    DataTable bankTable = new DataTable();
                    await sql.FillAdapterAsync(SQLBase.SELECT.Company_bank, bankTable);
                    if (bankTable.Rows.Count == 0)
                        row = bankTable.NewRow();
                    else
                        row = bankTable.Rows[0];
                    row["name"] = Bank;
                    row["account_no"] = Bank_account_no;
                    row["code"] = Bank_code;
                    row["iban"] = Bank_iban;
                    row["bic"] = Bank_bic;
                    if (bankTable.Rows.Count == 0)
                        bankTable.Rows.Add(row);
                    if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Company_bank, bankTable))
                        return false;
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return true;
        }
    }
}
