using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Dialoge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class CashBookDialogPresenter
    {
        private readonly ICashBookDialogContract view;
        private readonly SqlSession session;
        private readonly DataTable clientsTable = new DataTable();
        private readonly Dictionary<string, ID_Client_Data> clientData = new Dictionary<string, ID_Client_Data>();

        public CashBookDialogPresenter(ICashBookDialogContract view, SqlSession session)
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
        }

        public virtual void Initialize()
        {
            view.AddBookingCategory(SQLBase.BookCategory.Einzahlung.GetDisplayName());
            view.AddBookingCategory(SQLBase.BookCategory.Auszahlung.GetDisplayName());
            view.AddBookingTarget(SQLBase.BookingTo.Barbestand.GetDisplayName());
            view.AddBookingTarget(SQLBase.BookingTo.Bankbestand.GetDisplayName());
            view.BindFields();
        }

        public virtual async Task LoadAsync()
        {
            await ConnectTableToDataBaseAsync();
        }

        public virtual async Task ConnectTableToDataBaseAsync()
        {
            view.ClearClients();
            clientData.Clear();
            clientsTable.Clear();
            await session.SQL.FillAdapterAsync(SQLBase.SELECT.Clients, clientsTable, string.Empty);
            clientsTable.PrimaryKey = new DataColumn[]
            {
                clientsTable.Columns[Columns.Id]
            };

            foreach (DataRow row in clientsTable.Select())
            {
                ID_Client_Data data = new ID_Client_Data()
                {
                    Name = row[Columns.Name].ToString(),
                    ID = Int32.Parse(row[Columns.Id].ToString())
                };
                clientData[data.Name] = data;
                view.AddClient(data);
                view.AddClientLookupName(data.Name);
            }
        }

        public virtual void ValidateOk()
        {
            try
            {
                if (view.Amount == 0)
                    throw new Exception(Messages.missing_amount);
                if (string.IsNullOrWhiteSpace(view.BookText))
                    throw new Exception(Messages.missing_bookingtext);
                if (view.BookingDate == DateTime.MinValue || view.BookingDate > DateTime.Now)
                    throw new Exception(Messages.invalid_date);
                if (view.BookingTarget == SQLBase.BookingTo.Barbestand && !view.SelectedClients.Any())
                    throw new Exception(Messages.clients_select_first);
            }
            catch
            {
                view.SetDialogResultNone();
                throw;
            }
        }

        public virtual void BookingTargetChanged()
        {
            bool showClients = view.BookTo != 1;
            view.SetClientSelectionVisible(showClients);
            ChangeButtonState();
        }

        public virtual void ClientActiveChanged()
        {
            ChangeButtonState();
        }

        public virtual void ClientLookupValidated()
        {
            ID_Client_Data data;
            if (!clientData.TryGetValue(view.ClientLookupText, out data))
                return;

            view.SetClientSelection(data);
        }

        public virtual void ClientLookupTextChanged()
        {
            ID_Client_Data data;
            if (!clientData.TryGetValue(view.ClientLookupText, out data))
                return;

            view.SetClientSelection(data);
            view.ToggleSelectedClientChecked();
        }

        private void ChangeButtonState()
        {
            if (view.BookTo != 1)
                view.SetOkEnabled(true);
        }
    }
}
