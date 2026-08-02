using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Services.FormServices
{
    public class ClientsFormService
    {
        public ClientsFormService(IClientsFormContract form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form = form;
        }

        protected IClientsFormContract Form { get; private set; }

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
