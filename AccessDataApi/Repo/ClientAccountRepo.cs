using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class ClientAccountRepo : IClientAccountRepo
    {
        private ApplicationContext _context;
        private IDoes _does;

        public ClientAccountRepo(ApplicationContext context, IDoes does)
        {
            _context = context;
            _does = does;
        }

        public string CreateClientAccount(ClientForm clientAccountForm)
        {
            var castClient = CastTo.ClientAccount(clientAccountForm);

            if (_context.ClientAccounts.Any(x => x.ContactNumber == castClient.ContactNumber))
                return "Account already exist with this Contact Number";

            _context.ClientAccounts.Add(castClient);
            _context.SaveChanges();

            return "Client Account has been successfully been created!";
        }

        public ClientAccount GetClientAccount(int clientAccountId)
        {
            if (!_does.ClientExist(clientAccountId)) 
                return null;
         
            return _context.ClientAccounts.First(x => x.ClientAccountId == clientAccountId);
        }

        public string UpdateClientAccount(int clientAccountId, ClientForm clientAccountForm)
        {

            if (!_does.ClientExist(clientAccountId))
                return "Client Acc does not exist";

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
            if (!_does.ClientExist(clientAccountId))
                return "Client Acc does not exist";

            _context.Remove(_context.ClientAccounts.Single( x=> x.ClientAccountId == clientAccountId));
            _context.SaveChanges();
                return "Client Acc has been successfully deleted";
        }

        public List<ClientAccount> GetAllClients()
        {
            return _context.ClientAccounts.ToList();
        }
    }
}
