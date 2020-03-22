using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class ClientAccountRepo : IClientAccountRepo
    {
        private ApplicationContext _context;

        public ClientAccountRepo(ApplicationContext context)
        {
            _context = context;
        }

        public string CreateClientAccount(ClientForm clientAccountForm)
        {

            var castClient = CastClientFormToClientAccount(clientAccountForm);


            if (castClient == null)
                return "Account already exist with this Contact Number";

            _context.ClientAccounts.Add(castClient);
            _context.SaveChanges();

            return "Client Account has been successfully been created!";
        }

        public ClientAccount GetClientAccount(int clientAccountId, string email)
        {
            if (!_context.ClientAccounts.Any(x => x.ClientAccountId == clientAccountId))
                return null;
            

            var clientAcc = _context.ClientAccounts.First(x => x.ClientAccountId == clientAccountId);

            if (clientAcc.Email != email)
                return null;

            return clientAcc;
        }

        public string UpdateClientAccount(int clientAccountId, ClientForm clientAccountForm)
        {

            if (!_context.ClientAccounts.Any(x => x.ClientAccountId == clientAccountId))
                return null;

            var clientAcc = _context.ClientAccounts.First(x => x.ClientAccountId == clientAccountId);
            clientAcc.FirstName = clientAccountForm.FirstName;
            clientAcc.LastName = clientAccountForm.LastName;
            clientAcc.ContactNumber = clientAccountForm.ContactNumber;
            clientAcc.Email = clientAccountForm.Email;

            _context.SaveChanges();

            return "Update was Successful";
        }

        public string DeleteClientAccount(int clientAccountId)
        {
            var client = new ClientAccount() { ClientAccountId = clientAccountId };

            _context.Entry(client).State = EntityState.Deleted;

            return "Client Acc has been successfully deleted";
        }

        public List<ClientAccount> GetAllClients()
        {
            return _context.ClientAccounts.ToList();
        }

        private ClientAccount CastClientFormToClientAccount(ClientForm clientForm)
        {
            if (_context.ClientAccounts.Any(x => x.ContactNumber == clientForm.ContactNumber))
                return null;

            return new ClientAccount()
            {
                FirstName = clientForm.FirstName,
                LastName = clientForm.LastName,
                Email = clientForm.Email,
                ContactNumber = clientForm.ContactNumber
            };
        }
    }
}
