using System;
using System.Collections.Generic;
using DataMongoApi.Models;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class ClientService
    {
        private readonly IMongoCollection<Client> _client;

        public ClientService(ISalonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _client = database.GetCollection<Client>(settings.SalonCollectionName);

        }

        public List<Client> Get() =>
            _client.Find(Client => true).ToList();

        public Client Get(string id) =>
            _client.Find<Client>(c => c.Id == id).FirstOrDefault();

        public Client Create(Client client)
        {
            _client.InsertOne(client);
            return client;
        }

        public void Update(string id, Client clientIn) =>
            _client.ReplaceOne(c => c.Id == id, clientIn);

        public void Remove(Client clientIn) =>
            _client.DeleteOne(c => c.Id == clientIn.Id);

        public void Remove(string id) =>
            _client.DeleteOne(e => e.Id == id);
    }
}
