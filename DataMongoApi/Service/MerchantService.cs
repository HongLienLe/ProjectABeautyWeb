using System;
using System.Collections.Generic;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class MerchantService : IMerchantService
    {
        private readonly IMongoCollection<Merchant> _merchant;

        public MerchantService(ISalonDatabaseSettings settings, IClientConfiguration clientConfiguration)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId); ;

            _merchant = database.GetCollection<Merchant>("Merchant");
        }

        public List<Merchant> Get() =>
            _merchant.Find(Merchant => true).ToList();

        public Merchant Get(string id) =>
            _merchant.Find<Merchant>(t => t.Id == id).FirstOrDefault();

        public Merchant Create(Merchant merchant)
        {
            merchant.ModifiedOn = DateTime.UtcNow;
            _merchant.InsertOne(merchant);
            return merchant;
        }

        public void Update(string id, Merchant merchantIn) =>
            _merchant.ReplaceOne(t => t.Id == id, merchantIn);

        public void Remove(Merchant merchantIn) =>
            _merchant.DeleteOne(t => t.Id == merchantIn.Id);

        public void Remove(string id) =>
            _merchant.DeleteOne(t => t.Id == id);
    }
}
