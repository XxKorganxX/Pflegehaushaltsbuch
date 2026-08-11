using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the printing component used by the application.
    /// </summary>
    public class Printing
    {
        /// <summary>
        /// Defines the available layout Enum values used by the application.
        /// </summary>
        public enum LayoutEnum
        {
            cash,
            bank, 
            clients,
            accounts,
            advisors,
            employees,
            cashaudit,
            quittance,
            officecash
        }
        public Dictionary<LayoutEnum, DocumentLayer> Layouts { get; set;}
        public Dictionary<string, object> Variables;
        /// <summary>
        /// Defines the available var Names values used by the application.
        /// </summary>
        public enum VarNames
        {
            [Description("<company_logo>")]
            company_logo,
            [Description("<company_logo_alignment>")]
            company_logo_alignment,
            [Description("<company_name>")]
            company_name,
            [Description("<company_addr>")]
            company_addr,
            [Description("<company_zip>")]
            company_zip,
            [Description("<company_city>")]
            company_city,
            [Description("<company_phone>")]
            company_phone,
            [Description("<company_fax>")]
            company_fax,
            [Description("<company_email>")]
            company_email,
            [Description("<company_web>")]
            company_web,
            [Description("<company_secretary>")]
            company_secretary,
            [Description("<company_bank>")]
            company_bank,
            [Description("<company_bank_account_no>")]
            company_bank_account_no,
            [Description("<company_bank_code>")]
            company_bank_code,
            [Description("<company_bank_iban>")]
            company_bank_iban,
            [Description("<company_bank_bic>")]
            company_bank_bic,
            [Description("<company_local_court>")]
            company_local_court,
            [Description("<company_hrb>")]
            company_hrb,
            [Description("<company_ik>")]
            company_ik,
            [Description("<advisor_name>")]
            advisor_name,
            [Description("<advisor_title>")]
            advisor_title,
            [Description("<advisor_co>")]
            advisor_co,
            [Description("<advisor_addr>")]
            advisor_addr,
            [Description("<advisor_zip>")]
            advisor_zip,
            [Description("<advisor_city>")]
            advisor_city,
            [Description("<assistant_name>")]
            assistant_name,
            [Description("<assistant_phone>")]
            assistant_phone,
            [Description("<assistant_fax>")]
            assistant_fax,
            [Description("<assistant_email>")]
            assistant_email,
            [Description("<date>")]
            date,
            [Description("<date_long>")]
            date_long,
            [Description("<date_of_paper>")]
            date_of_paper,
            [Description("<date_long_of_paper>")]
            date_long_of_paper,
            [Description("<client>")]
            client,
            [Description("<cash_outflow>")]
            cash_outflow,
            [Description("<cash_inflow>")]
            cash_inflow,
            [Description("<statement_note>")]
            statement_note,
            [Description("<amount>")]
            amount,
            [Description("<amount_previous_month>")]
            amount_previous_month,
            [Description("<amount_cash>")]
            amount_cash,
            [Description("<amount_officecash>")]
            amount_officecash,
            [Description("<amount_bank>")]
            amount_bank,
            [Description("<amount_assistants>")]
            amount_assistants,
            [Description("<amount_clients>")]
            amount_clients,
            [Description("<amount_clients_active>")]
            amount_clients_active,
            [Description("<amount_clients_inactive>")]
            amount_clients_inactive,
            [Description("<amount_clients_history>")]
            amount_clients_history,
            [Description("<amount_hardmoney_calculated>")]
            amount_hardmoney_calculated,
            [Description("<amount_hardmoney_actually>")]
            amount_hardmoney_actually,
            [Description("<document_id>")]
            document_id,
            [Description("<booking_kind>")]
            booking_kind,
            [Description("<credit>")]
            credit,
            [Description("<debit>")]
            debit,
            [Description("<advisor_email>")]
            advisor_email
        }
        /// <summary>
        /// Creates a new Printing instance and initializes the required state.
        /// </summary>
        public Printing()
        {
            Layouts = new Dictionary<LayoutEnum, DocumentLayer>();
            Variables = new Dictionary<string, object>();
        }
        /// <summary>
        /// Updates the variable data and refreshes the related application state.
        /// </summary>
        public void UpdateVariable(VarNames variable, object text)
        {
            string s = EnumHelper.ToDescription(variable);
            Variables[s] = text;
        }
        /// <summary>
        /// Updates the user And Company data and refreshes the related application state.
        /// </summary>
        public void UpdateUserAndCompany(SQLBase sql)
        {
            Company company = sql.Company;
            UpdateVariable(VarNames.company_name, company.Name);
            UpdateVariable(VarNames.company_addr, company.Street);
            UpdateVariable(VarNames.company_city, company.City);
            UpdateVariable(VarNames.company_zip, company.Zipcode);
            UpdateVariable(VarNames.company_phone, company.Phone);
            UpdateVariable(VarNames.company_fax, company.Fax);
            UpdateVariable(VarNames.company_email, company.Email);
            UpdateVariable(VarNames.company_logo, company.Logo);
            UpdateVariable(VarNames.company_logo_alignment, company.LogoAlignment);
            if (company.Web != null)
                UpdateVariable(VarNames.company_web, company.Web);
            if (company.Local_court != null)
                UpdateVariable(VarNames.company_local_court, company.Local_court);
            if (company.Secretary != null)
                UpdateVariable(VarNames.company_secretary, company.Secretary);
            if (company.Hrb != null)
                UpdateVariable(VarNames.company_hrb, company.Hrb);
            if (company.Ik != null)
                UpdateVariable(VarNames.company_ik, company.Ik);
            if (company.Bank != null)
                UpdateVariable(VarNames.company_bank, company.Bank);
            if (company.Bank_account_no != null)
                UpdateVariable(VarNames.company_bank_account_no, company.Bank_account_no);
            if (company.Bank_code != null)
                UpdateVariable(VarNames.company_bank_code, company.Bank_code);
            if (company.Bank_iban != null)
                UpdateVariable(VarNames.company_bank_iban, company.Bank_iban);
            if (company.Bank_bic != null)
                UpdateVariable(VarNames.company_bank_bic, company.Bank_bic);
            User user = sql.User;
        }
        /// <summary>
        /// Loads the documents data required for the current workflow.
        /// </summary>
        public async Task LoadDocuments(SQLBase sql)
        {
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Layouts, table);
            if (table.Rows.Count == 0)
            {
                ResetDocuments();
                await SaveDocuments(sql);
            }
            else
            {
                DataRow row = table.Rows[0];
                bool saveLayouts = false;
                foreach (Printing.LayoutEnum layout in Enum.GetValues(typeof(Data.Printing.LayoutEnum)))
                {
                    try
                    {
                        string columnName = GetLayoutColumnName(table, layout);
                        using (MemoryStream ms = new MemoryStream(row[columnName] as byte[]))
                        {
                            Layouts[layout] = LoadDoument(ms);
                        }
                    }
                    catch
                    {
                        ResetDocument(layout);
                        saveLayouts = true;
                    }
                }
                if(saveLayouts)
                    await SaveDocuments(sql);
            }
        }
            
        /// <summary>
        /// Runs the import Documents operation and updates the related application state.
        /// </summary>
        public void ImportDocuments(string path)
        {
            foreach (Printing.LayoutEnum layout in Enum.GetValues(typeof(Data.Printing.LayoutEnum)))
            {
                string filename = Path.Combine(path, layout.ToString() + ".xml");
                if (!File.Exists(filename))
                    continue;
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    Layouts[layout] = LoadDoument(fs);
            }
        }
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <summary>
        /// Loads the doument data required for the current workflow.
        /// </summary>
        private DocumentLayer LoadDoument(Stream stream)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DocumentLayer),
                new Type[]
                {
                    typeof(GraphicsItem),
                    typeof(DataTableItem),
                    typeof(FontItem),
                    typeof(ImageItem),
                    typeof(LineItem),
                    typeof(RectangleItem),
                    typeof(TextItem),
                }
            );
            var document = xmlSerializer.Deserialize(stream) as DocumentLayer;
            var lookup = document.Items.ToLookup(a => a.ID);
            foreach (var item in document.Items)
            {
                if (item.XmlParent != 0)
                    item.Parent = lookup[item.XmlParent].First();
                item.Items = new List<GraphicsItem>();
                foreach (var id in item.XmlItems)
                    item.Items.Add(lookup[id].First());
            }
            return document;
        }
        /// <summary>
        /// Saves the documents data for the current workflow.
        /// </summary>
        public async Task SaveDocuments(SQLBase sql)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DocumentLayer),
                new Type[]
                {
                    typeof(GraphicsItem),
                    typeof(DataTableItem),
                    typeof(FontItem),
                    typeof(ImageItem),
                    typeof(LineItem),
                    typeof(RectangleItem),
                    typeof(TextItem),
                }
            );
            DataTable table = new DataTable();
            await sql.FillAdapterAsync(SQLBase.SELECT.Layouts, table);
            DataRow row = null;
            if (table.Rows.Count == 0)
            {
                row = table.NewRow();
                table.Rows.Add(row);
            }
            else
                row = table.Rows[0];
            foreach (Printing.LayoutEnum layout in Enum.GetValues(typeof(Data.Printing.LayoutEnum)))
            {
                var document = Layouts[layout];
                int id = 1;
                foreach (var item in document.Items)
                    item.ID = id++;
                foreach (var item in document.Items)
                {
                    if (item.Parent != null)
                        item.XmlParent = item.Parent.ID;
                    item.XmlItems = new List<int>();
                    foreach (var subItem in item.Items)
                        item.XmlItems.Add(subItem.ID);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    xmlSerializer.Serialize(ms, document);
                    row[GetLayoutColumnName(table, layout)] = ms.ToArray();
                }
            }
            if (!await sql.UpdateAdapterAsync(SQLBase.SELECT.Layouts, table))
                throw new Exception(Messages.datatable_update_failed);
        }

        private static string GetLayoutColumnName(DataTable table, Printing.LayoutEnum layout)
        {
            string columnName = layout.ToString();
            if (table.Columns.Contains(columnName))
                return columnName;

            if (layout == Printing.LayoutEnum.employees && table.Columns.Contains("assistants"))
                return "assistants";

            return columnName;
        }
        /// <param name="path"></param>
        /// <summary>
        /// Runs the export Documents operation and updates the related application state.
        /// </summary>
        public void ExportDocuments(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DocumentLayer),
                new Type[]
                {
                    typeof(GraphicsItem),
                    typeof(DataTableItem),
                    typeof(FontItem),
                    typeof(ImageItem),
                    typeof(LineItem),
                    typeof(RectangleItem),
                    typeof(TextItem),
                }
            );
            FileStream fs = null;
            foreach (Printing.LayoutEnum layout in Enum.GetValues(typeof(Data.Printing.LayoutEnum)))
            {
                var document = Layouts[layout];
                int id = 1;
                foreach (var item in document.Items)
                    item.ID = id++;
                foreach (var item in document.Items)
                {
                    if (item.Parent != null)
                        item.XmlParent = item.Parent.ID;
                    item.XmlItems = new List<int>();
                    foreach (var subItem in item.Items)
                        item.XmlItems.Add(subItem.ID);
                }

                using (fs = new FileStream(Path.Combine(path, layout.ToString() + ".xml"), FileMode.Create))
                {
                    xmlSerializer.Serialize(fs, document);
                }
            }
        }
        /// <summary>
        /// Runs the reset Documents operation and updates the related application state.
        /// </summary>
        public void ResetDocuments()
        {
            foreach (Printing.LayoutEnum layout in Enum.GetValues(typeof(Data.Printing.LayoutEnum)))
                ResetDocument(layout);
        }
        /// <param name="layout"></param>
        /// <returns></returns>
        /// <summary>
        /// Runs the reset Document operation and updates the related application state.
        /// </summary>
        public DocumentLayer ResetDocument(Printing.LayoutEnum layout)
        {
            if (CultureInfo.CurrentCulture.Name.ToLower().Contains("de"))
            {
                using (FileStream fs = new FileStream(Path.Combine("layouts", "de", layout.ToString() + ".xml"), FileMode.Open, FileAccess.Read))
                    return Layouts[layout] = LoadDoument(fs);
            }
            else
            {
                using (FileStream fs = new FileStream(Path.Combine("layouts", "en", layout.ToString() + ".xml"), FileMode.Open, FileAccess.Read))
                    return Layouts[layout] = LoadDoument(fs);
            }
        }
    }
}
