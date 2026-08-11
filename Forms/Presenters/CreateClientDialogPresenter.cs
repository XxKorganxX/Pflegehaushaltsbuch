using Pflegehaushaltsbuch.Databases;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CreateClientDialogPresenter
    {
        private readonly ICreateClientDialogContract view;
        private readonly SqlSession session;
        private readonly DataTable clientTable = new DataTable();
        private readonly DataTable advisorTable = new DataTable();
        private readonly ClientPersonData client = new ClientPersonData();
        private readonly bool updateClient;

        public CreateClientDialogPresenter(ICreateClientDialogContract view, SqlSession session, bool updateClient, int clientID)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
            this.updateClient = updateClient;

            foreach (SQLBase.Title enumval in Enum.GetValues(typeof(SQLBase.Title)))
                view.AddTitle(enumval.GetDisplayName());

            view.BindSaldo();
            if (updateClient)
            {
                view.SetDebitorEnabled(false);
                view.SetSaldoEnabled(false);
                view.ClientID = clientID;
            }
        }

        public virtual async Task ShownAsync()
        {
            await ConnectTableToDataBaseAsync();
            if (!updateClient)
            {
                view.ClientID = GetNextFreeID();
            }
            else
            {
                DataRow row = clientTable.Rows.Find(view.ClientID);
                if (row == null)
                    throw new Exception(Messages.client_not_found);

                client.Title = GetString(row, Columns.Title);
                client.Name = GetString(row, Columns.Name);
                client.Street = GetString(row, Columns.Street);
                client.Zipcode = GetString(row, Columns.Zipcode);
                client.City = GetString(row, Columns.City);
                bool invalidBornDate;
                view.BornDate = GetDate(row, Columns.Born, DateTime.Now.Date, out invalidBornDate);
                if (invalidBornDate)
                    view.ShowError(Messages.client_invalid_born_date);

                view.Amount = GetDecimal(row, Columns.AccountTransfer);

                if (row[Columns.AdvisorId] != DBNull.Value)
                {
                    view.UseAdvisorChecked = true;
                    DataRow[] advisorRows = advisorTable.Select(string.Format("{0}={1}", Columns.Id, row[Columns.AdvisorId]));
                    if (advisorRows != null && advisorRows.Length > 0)
                    {
                        view.SetAdvisorsEnabled(true);
                        view.SelectAdvisorByName(advisorRows[0][Columns.Name].ToString());
                    }
                }
            }

            view.BindClientID();
            view.BindClient(client);
            UpdateAdvisorBoxes();
        }

        public virtual void Ok()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(client.Name))
                    throw new Exception(Messages.clients_enter_name);
                if (string.IsNullOrWhiteSpace(client.Street))
                    throw new Exception(Messages.invalid_street);
                if (string.IsNullOrWhiteSpace(client.Zipcode))
                    throw new Exception(Messages.invalid_zip);
                if (string.IsNullOrWhiteSpace(client.City))
                    throw new Exception(Messages.invalid_city);
                if (view.BornDate > DateTime.Now)
                    throw new Exception(Messages.invalid_date);

                view.Data = CreateClientData();
            }
            catch
            {
                view.SetDialogResultNone();
                throw;
            }
        }

        public virtual void UseAdvisorChanged()
        {
            view.SetAdvisorsEnabled(view.UseAdvisorChecked);
        }

        public virtual void UpdateAdvisorBoxes()
        {
        }

        private async Task ConnectTableToDataBaseAsync()
        {
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clientTable);
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Advisors, advisorTable);
            clientTable.PrimaryKey = new DataColumn[] { clientTable.Columns[Columns.Id] };
            advisorTable.PrimaryKey = new DataColumn[] { advisorTable.Columns[Columns.Name] };
            view.SetAdvisorsDataSource(advisorTable);
        }

        private int GetNextFreeID()
        {
            DataRow[] rows = clientTable.Select("", Columns.Id);
            System.Collections.Generic.Dictionary<int, int> ids = new System.Collections.Generic.Dictionary<int, int>();
            foreach (DataRow item in rows)
                ids[Convert.ToInt32(item[Columns.Id])] = 0;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!ids.ContainsKey(i))
                    return i;
            }

            return -1;
        }

        private CreateClientDialog.ClientData CreateClientData()
        {
            return new CreateClientDialog.ClientData
            {
                ClientID = view.ClientID,
                Title = session.SQL.TrimBetween(client.Title),
                Name = session.SQL.TrimBetween(client.Name),
                Street = session.SQL.TrimBetween(client.Street),
                Zipcode = session.SQL.TrimBetween(client.Zipcode),
                City = session.SQL.TrimBetween(client.City),
                BornDate = view.BornDate,
                Amount = view.Amount,
                AdvisorId = view.SelectedAdvisorId
            };
        }

        private static string GetString(DataRow row, string columnName)
        {
            object value = row[columnName];
            return value == DBNull.Value ? string.Empty : value.ToString();
        }

        private static DateTime GetDate(DataRow row, string columnName, DateTime defaultValue, out bool invalidValue)
        {
            invalidValue = false;
            object value = row[columnName];
            if (value == DBNull.Value || value == null)
            {
                invalidValue = true;
                return defaultValue;
            }

            if (value is DateTime date)
                return date.Date;

            DateTime parsedDate;
            if (DateTime.TryParse(value.ToString(), out parsedDate))
                return parsedDate.Date;

            invalidValue = true;
            return defaultValue;
        }

        private static decimal GetDecimal(DataRow row, string columnName)
        {
            object value = row[columnName];
            if (value == DBNull.Value || value == null)
                return 0;

            if (value is decimal decimalValue)
                return decimalValue;

            decimal parsedDecimal;
            return decimal.TryParse(value.ToString(), out parsedDecimal) ? parsedDecimal : 0;
        }
    }

    public class ClientPersonData : INotifyPropertyChanged
    {
        private string title = "Frau";
        private string name;
        private string street;
        private string zipcode;
        private string city;

        public string Title { get { return title; } set { title = value; FirePropertyChanged("Title"); } }
        public string Name { get { return name; } set { name = value; FirePropertyChanged("Name"); } }
        public string Street { get { return street; } set { street = value; FirePropertyChanged("Street"); } }
        public string Zipcode { get { return zipcode; } set { zipcode = value; FirePropertyChanged("Zipcode"); } }
        public string City { get { return city; } set { city = value; FirePropertyChanged("City"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
