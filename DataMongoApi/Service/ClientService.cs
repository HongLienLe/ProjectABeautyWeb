using System;
using System.Collections.Generic;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;
using MongoDB.Entities;

namespace DataMongoApi.Service
{
    public class ClientService : IClientService
    {
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoDbContext _context;

        public ClientService(IMongoDbContext context)
        {
            _context = context;

            _client = _context.GetCollection<Client>("Clients");

        }

        public List<Client> Get() =>
            _client.Find(Client => true).ToList();

        public Client Get(string id) =>
            _client.Find(c => c.ID == id).FirstOrDefault();

        public Client GetByContactNo(string phone)
        {
            return _client.Find(c => c.About.Phone == phone).FirstOrDefault();
        }

        public Client Create(ClientDetails client)
        {
            var newClient = new Client()
            {
                About = client,
                ModifiedOn = DateTime.UtcNow
            };

            _client.InsertOne(newClient);
            return newClient;
        }

        public void Update(string id, ClientDetails clientIn)
        {
            var filter = Builders<Client>.Filter.Eq(x => x.ID, id);
            var update = Builders<Client>.Update
                .Set(x => x.About, clientIn)
                .CurrentDate(x => x.ModifiedOn);
            _client.UpdateOne(filter, update);
        }

        public void Remove(Client clientIn) =>
            _client.DeleteOne(c => c.ID == clientIn.ID);

        public void Remove(string id) =>
            _client.DeleteOne(e => e.ID == id);

        public void AddAppointment(string clientid, Appointment appointment)
        {
            var client = Builders<Client>.Filter.Eq(x => x.ID, clientid);
            var update = Builders<Client>.Update
                .AddToSet("Appointments", appointment.ID);

            _client.UpdateOne(client, update);
        }

        public void RemoveAppointment(string clientNo, string appointmentid)
        {
            var clientId = GetByContactNo(clientNo).ID;
            var filter = Builders<Client>.Filter.Eq(x => x.ID, clientId);
            var update = Builders<Client>.Update
                .Pull("Appointments", appointmentid);

            _client.FindOneAndUpdate(filter, update);
        }

        //Delete Payment 
    }
}
