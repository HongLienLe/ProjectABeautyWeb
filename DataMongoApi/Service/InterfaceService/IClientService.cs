using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IClientService
    {
        public List<Client> Get();
        public Client Get(string id);
        public Client Create(ClientDetails client);
        public void Update(string id, ClientDetails clientIn);
        public void Remove(Client clientIn);
        public void Remove(string id);
        public Client GetByContactNo(string contactNo);
        public void AddAppointment(string clientid, Appointment appointment);

    }
}
