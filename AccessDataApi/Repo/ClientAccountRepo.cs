using System;
using System.Linq;
using AccessDataApi.Data;
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

        public string CreateClientAccount(ClientAccount clientAccount)
        {
            //check if email exist

            var doesEmailExist = _context.ClientAccounts.Any(x => x.Email == clientAccount.Email);

            if (doesEmailExist)
            {
                return "Email already exist";
            }

            _context.ClientAccounts.Add(clientAccount);
            _context.SaveChanges();

            return "Client Account has been successfully been created!";
        }

        public ClientAccount GetClientAccount(int clientAccountId, string email)
        {
            var doesClientAccExist = _context.ClientAccounts.Any(x => x.ClientAccountId == clientAccountId);

            if (!doesClientAccExist)
            {
                return null;
            }

            var clientAcc = _context.ClientAccounts.First(x => x.ClientAccountId == clientAccountId);

            if (clientAcc.Email != email)
            {
                return null;
            }

            return clientAcc;
        }

        public string UpdateClientAccount(int clientAccountId, ClientAccount clientAccount)
        {
            var doesClientAccExist = _context.ClientAccounts.Any(x => x.ClientAccountId == clientAccountId);

            if (!doesClientAccExist)
            {
                return null;
            }

            var clientAcc = _context.ClientAccounts.First(x => x.ClientAccountId == clientAccountId);

            clientAcc = clientAccount;

            _context.SaveChanges();

            return "Update was Successful";
        }

        public string DeleteClientAccount(int clientAccountId)
        {
            var client = new ClientAccount() { ClientAccountId = clientAccountId };

            _context.Entry(client).State = EntityState.Deleted;

            return "Client Acc has been successfully deleted";
        }
    }
}
