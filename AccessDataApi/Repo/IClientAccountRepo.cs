using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IClientAccountRepo
    {
        public string CreateClientAccount(ClientForm clientAccount);
        public ClientAccount GetClientAccount(int clientAccountId);
        public string UpdateClientAccount(int clientAccountId, ClientForm clientAccount);
        public string DeleteClientAccount(int clientAccountId);
        public List<ClientAccount> GetAllClients();

    }
}
