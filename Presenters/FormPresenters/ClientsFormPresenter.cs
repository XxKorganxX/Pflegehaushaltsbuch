using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class ClientsFormPresenter
    {
        public ClientsFormPresenter(IClientsFormContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected IClientsFormContract View { get; private set; }

        public virtual void ConnectTableToDataBase()
        {
        }

        public virtual void UpdateTotalAmount()
        {
        }

        public virtual void CreateAccount()
        {
        }

        public virtual void Change()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void DeadLines()
        {
        }

        public virtual void SelectAccount()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void ImportClients()
        {
        }

        public virtual void Import()
        {
        }

        public virtual void ClientBooks()
        {
        }

        public virtual void Clients()
        {
        }

        public virtual void Export()
        {
        }
    }
}
