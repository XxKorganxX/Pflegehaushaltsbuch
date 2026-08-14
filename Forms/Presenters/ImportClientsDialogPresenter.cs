using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class ImportClientsDialogPresenter
    {
        public static readonly string[] EnglishImportColumns = new string[]
        {
            "Debitor No.",
            "Title",
            "Name",
            "Born",
            "Street",
            "Zip",
            "City",
            "Advisor",
            "Previous balance"
        };

        private readonly IImportClientsDialogContract view;
        private SqlSession Session;
        private DataTable clientTable;
        private DataTable advisorTable;
        private int debitorNr;
        private int title;
        private int clientName;
        private int born;
        private int street;
        private int zopCode;
        private int city;
        private int advisor;
        private int cash;

        public ImportClientsDialogPresenter(IImportClientsDialogContract view, SqlSession session)
        {
            Session = session;

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }
            if (Session.SQL == null)
            {
                throw new ArgumentNullException(nameof(Session.SQL));
            }

            this.view = view;
        }

        public void Initialize(DataTable clientTable, DataTable advisorTable)
        {
            this.clientTable = clientTable;
            this.advisorTable = advisorTable;
            view.SetSeperator(";;");
            view.ApplyImportLabels(EnglishImportColumns);
            UpdateImportMapping();
            view.BindClientTable(clientTable);
        }

        public void AcceptImport()
        {
            DataTable addedRows = clientTable.GetChanges(DataRowState.Added);
            view.SetImportedData(new ImportsClientData
            {
                Clients = addedRows == null
                    ? new ImportedClient[0]
                    : addedRows.Rows.OfType<DataRow>().Select(CreateImportedClient).ToArray()
            });
        }

        public void LoadFiles(IEnumerable<string> filenames)
        {
            if (filenames == null)
            {
                return;
            }

            string[] seperator = new string[] { view.Seperator };
            foreach (string filename in filenames)
            {
                LoadFile(filename, seperator);
            }
        }

        public void LoadFilesFromDialog()
        {
            string[] fileNames;
            if (!view.ShowOpenImportFilesDialog(out fileNames))
            {
                return;
            }

            LoadFiles(fileNames);
        }

        public void UpdateImportMapping()
        {
            int index = 0;
            foreach (string itemText in view.ImportMappingItems)
            {
                UpdateImportMapping(itemText, index);
                index++;
            }
        }

        private void UpdateImportMapping(string itemText, int index)
        {
            switch (itemText)
            {
                case "Debitor No.":
                    debitorNr = index;
                    break;
                case "Title":
                    title = index;
                    break;
                case "Name":
                    clientName = index;
                    break;
                case "Born":
                    born = index;
                    break;
                case "Street":
                    street = index;
                    break;
                case "Zip":
                    zopCode = index;
                    break;
                case "City":
                    city = index;
                    break;
                case "Advisor":
                    advisor = index;
                    break;
                case "Previous balance":
                    cash = index;
                    break;
            }
        }

        private void LoadFile(string filename, string[] seperator)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    try
                    {
                        ImportLine(line, seperator);
                    }
                    catch (Exception err)
                    {
                        clientTable.RejectChanges();
                        if (!view.ShowErrorAndContinue(err))
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void ImportLine(string line, string[] seperator)
        {
            string[] data = line.Split(seperator, StringSplitOptions.None);
            int id = Int32.Parse(data[debitorNr]);
            string title = data[this.title];
            string clientName = data[this.clientName];
            DateTime born = DateTime.Parse(data[this.born]);
            string street = data[this.street];
            string zipCode = data[zopCode];
            string city = data[this.city];
            string advisor = Session.SQL.TrimBetween(data[this.advisor]);
            decimal cash = 0;
            decimal.TryParse(data[this.cash], out cash);

            if (!title.Equals("Frau") && !title.Equals("Herr"))
            {
                view.ShowMessage(string.Format(Messages.import_clients_invalid_title, clientName));
                return;
            }

            if (id < 1)
            {
                view.ShowMessage(string.Format(Messages.import_clients_invalid_debtor_no, clientName));
                return;
            }

            DataRow row = clientTable.NewRow();
            row[Columns.Date] = DateTime.Now.Date;
            row[Columns.Amount] = cash;
            row[Columns.AccountTransfer] = cash;
            row[Columns.Active] = 1;
            row[Columns.Id] = id;
            row[Columns.Title] = Session.SQL.TrimBetween(title);
            row[Columns.Name] = Session.SQL.TrimBetween(clientName);
            row[Columns.Street] = Session.SQL.TrimBetween(street);
            row[Columns.Zipcode] = zipCode.Trim();
            row[Columns.City] = Session.SQL.TrimBetween(city);
            row[Columns.Born] = born.Date;
            row[Columns.HandSign] = Session.SQL.User.Handsign;

            DataRow advisorRow = advisorTable.Rows.Find(advisor);
            row[Columns.AdvisorId] = advisorRow != null ? advisorRow[Columns.Id] : DBNull.Value;

            clientTable.Rows.Add(row);
        }

        private ImportedClient CreateImportedClient(DataRow row)
        {
            return new ImportedClient
            {
                Id = Convert.ToInt32(row[Columns.Id]),
                Title = row[Columns.Title].ToString(),
                Name = row[Columns.Name].ToString(),
                Street = row[Columns.Street].ToString(),
                Zipcode = row[Columns.Zipcode].ToString(),
                City = row[Columns.City].ToString(),
                BornDate = Convert.ToDateTime(row[Columns.Born]),
                OpeningBalance = Convert.ToDecimal(row[Columns.AccountTransfer]),
                AdvisorId = row[Columns.AdvisorId] == DBNull.Value ? null : (int?)Convert.ToInt32(row[Columns.AdvisorId]),
                CreatedDate = Convert.ToDateTime(row[Columns.Date])
            };
        }
    }
}
