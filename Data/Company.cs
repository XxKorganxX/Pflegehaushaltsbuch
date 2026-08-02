using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the company component used by the application.
    /// </summary>
    public class Company
    {
        public Image Logo { get; set; }
        public int LogoAlignment { get; set; }
        public string Name { get; set; }
        public string Secretary { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Web { get; set; }
        public string Local_court { get; set; }
        public string Hrb { get; set; }
        public string Ik { get; set; }
        public string Bank { get; set; }
        public string Bank_account_no { get; set; }
        public string Bank_code { get; set; }
        public string Bank_iban { get; set; }
        public string Bank_bic { get; set; }
        public string SMTP_Host { get; set; }
        public string SMTP_User { get; set; }
        public string SMTP_Password { get; set; }
        public int licenseCount { get; set; }
        public bool IsSMTPValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SMTP_Host) &
                        !string.IsNullOrWhiteSpace(SMTP_User) &
                        !string.IsNullOrWhiteSpace(SMTP_Password);
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
        /// <summary>
        /// Loads the load data required for the current workflow.
        /// </summary>
        public async Task Load(SQLBase sql)
        {
            DataTable companyTable = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Company, companyTable);
            if (companyTable.Rows.Count == 0)
                companyTable.Rows.Add(companyTable.NewRow());
            DataRow row = companyTable.Rows[0];
            Name = row[SQLBase.Names(SQLBase.ColumnNames.name)].ToString();
            Phone = row[SQLBase.Names(SQLBase.ColumnNames.phone)].ToString();
            Fax = row[SQLBase.Names(SQLBase.ColumnNames.fax)].ToString();
            Email = row[SQLBase.Names(SQLBase.ColumnNames.email)].ToString();
            Street = row[SQLBase.Names(SQLBase.ColumnNames.street)].ToString();
            Zipcode = row[SQLBase.Names(SQLBase.ColumnNames.zipcode)].ToString();
            City = row[SQLBase.Names(SQLBase.ColumnNames.city)].ToString();
            if (row["logo"] != DBNull.Value)
            {
                MemoryStream ms = new MemoryStream(row["logo"] as byte[]);
                Logo = Image.FromStream(ms);
            }
            if (row["logo_alignment"] != DBNull.Value)
                LogoAlignment = Int32.Parse(row["logo_alignment"].ToString());
            Language = row["language"] as string;
            if (Language == null)
            {
                Language = CultureInfo.CurrentCulture.Name;
                await Save(sql);
            }

            Web = row["web"] as string;
            Local_court = row["local_court"] as string;
            Secretary = row["secretary"] as string;
            Hrb = row["hrb"] as string;
            Ik = row["ik"] as string;
            SMTP_Host = row["smtp_host"] as string;
            SMTP_User = row["smtp_user"] as string;
            SMTP_Password = row["smtp_key"] as string;
            if (!string.IsNullOrWhiteSpace(SMTP_Host))
                SMTP_Host = CredentialProtector.Unprotect(SMTP_Host);
            if (!string.IsNullOrWhiteSpace(SMTP_User))
                SMTP_User = CredentialProtector.Unprotect(SMTP_User);
            if (!string.IsNullOrWhiteSpace(SMTP_Password))
                SMTP_Password = CredentialProtector.Unprotect(SMTP_Password);

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

            sql.Printing.UpdateUserAndCompany(sql);
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
            row[SQLBase.Names(SQLBase.ColumnNames.name)] = Name;
            row[SQLBase.Names(SQLBase.ColumnNames.phone)] = Phone;
            row[SQLBase.Names(SQLBase.ColumnNames.fax)] = Fax;
            row[SQLBase.Names(SQLBase.ColumnNames.email)] = Email;
            row[SQLBase.Names(SQLBase.ColumnNames.street)] = Street;
            row[SQLBase.Names(SQLBase.ColumnNames.zipcode)] = Zipcode;
            row[SQLBase.Names(SQLBase.ColumnNames.city)] = City;
            if (Logo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Logo.Save(ms, Logo.RawFormat);
                    row["logo"] = ms.ToArray();
                }
                row["logo_alignment"] = (int)LogoAlignment;
            }
            row["language"] = CultureInfo.CurrentCulture.Name;
            row["web"] = Web;
            row["local_court"] = Local_court;
            row["secretary"] = Secretary;
            row["hrb"] = Hrb;
            row["ik"] = Ik;
            if (!string.IsNullOrWhiteSpace(SMTP_Host) )
                row["smtp_host"] = CredentialProtector.Protect(SMTP_Host);
            if (!string.IsNullOrWhiteSpace(SMTP_User))
                row["smtp_user"] = CredentialProtector.Protect(SMTP_User);
            if (!string.IsNullOrWhiteSpace(SMTP_Password))
                row["smtp_key"] = CredentialProtector.Protect(SMTP_Password);
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
