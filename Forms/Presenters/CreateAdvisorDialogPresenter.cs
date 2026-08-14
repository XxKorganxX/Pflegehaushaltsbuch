using Pflegehaushaltsbuch.Databases;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreateAdvisorDialogPresenter
    {
        private readonly ICreateAdvisorDialogContract view;
        private readonly SqlSession session;
        private readonly DataTable table;
        private readonly bool update;
        private BindingSource bindingSource;

        public CreateAdvisorDialogPresenter(ICreateAdvisorDialogContract view, SqlSession session, DataTable table, bool update, int position)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            this.view = view;
            this.session = session;
            this.table = table;
            this.update = update;

            InitializeTitles();
            view.AdvisorIDText = session.SQL.GetID(table).ToString();
            if (update)
                bindingSource = view.CreateBindingSource(table, position);
            else
                view.AdvisorTitleIndex = 0;
        }

        public virtual void Shown()
        {
            if (update)
                view.BindAdvisor(bindingSource, BindingParse);
        }

        public virtual void Ok()
        {
            try
            {
                Validate();

                int id = Int32.Parse(Trim(view.AdvisorIDText));
                if (!update)
                {
                    DataRow row = table.NewRow();
                    row[Columns.Id] = id;
                    row[Columns.Title] = Trim(view.AdvisorTitleText);
                    row[Columns.Name] = Trim(view.AdvisorNameText);
                    row[Columns.Email] = Trim(view.AdvisorEmailText);
                    row[Columns.Co] = Trim(view.AdvisorCoText);
                    row[Columns.Street] = Trim(view.AdvisorStreetText);
                    row[Columns.Zipcode] = Trim(view.AdvisorZipcodeText);
                    row[Columns.City] = Trim(view.AdvisorCityText);
                    row[Columns.Date] = DateTime.Now.Date;
                    row[Columns.HandSign] = session.SQL.User.Handsign;
                    table.Rows.Add(row);
                }
                else
                {
                    bindingSource.EndEdit();
                }
            }
            catch
            {
                view.RejectChanges(table);
                view.SetDialogResultNone();
                throw;
            }
        }

        public virtual void Cancel()
        {
            view.RejectChanges(table);
        }

        private void InitializeTitles()
        {
            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                view.AddAdvisorTitle(enumval.GetDisplayName());
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(view.AdvisorNameText))
                throw new Exception(Messages.advisors_name_missing);
            if (string.IsNullOrWhiteSpace(view.AdvisorStreetText))
                throw new Exception(Messages.missing_street);
            if (string.IsNullOrWhiteSpace(view.AdvisorCityText))
                throw new Exception(Messages.missing_city);
            if (string.IsNullOrWhiteSpace(view.AdvisorZipcodeText))
                throw new Exception(Messages.missing_zip);
            if (!string.IsNullOrWhiteSpace(view.AdvisorEmailText) && !session.SQL.IsEmail(view.AdvisorEmailText))
                throw new Exception(Messages.invalid_email);

            int id;
            if (!Int32.TryParse(Trim(view.AdvisorIDText), out id) || id == 0)
                throw new Exception(Messages.invalid_no);
        }

        private void BindingParse(object sender, ConvertEventArgs e)
        {
            e.Value = Trim(e.Value == null ? string.Empty : e.Value.ToString());
        }

        private static string Trim(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            string[] splittedStr = str.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splittedStr.Length; i++)
            {
                sb.Append(splittedStr[i]);
                if (i < splittedStr.Length - 1)
                    sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
