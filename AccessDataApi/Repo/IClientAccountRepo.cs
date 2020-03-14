using System;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IClientAccountRepo
    {
        public string CreateClientAccount(ClientAccount clientAccount);
        public ClientAccount GetClientAccount(int clientAccountId, string email);
        public string UpdateClientAccount(int clientAccountId, ClientAccount clientAccount);
        public string DeleteClientAccount(int clientAccountId);

    }
}
