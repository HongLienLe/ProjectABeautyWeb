using System;
using System.Collections.Generic;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class MerchantService : IMerchantService
    {
        private readonly IMongoCollection<Merchant> _merchant;
        private readonly IMongoDbContext _context;


        public MerchantService(IMongoDbContext context)
        {
            _context = context;
            _merchant = _context.GetCollection<Merchant>("Merchant");
        }

        public Merchant Get(string id) =>
            _merchant.Find<Merchant>(t => t.ID == id).FirstOrDefault();

        public Merchant Create(Merchant merchant)
        {
            merchant.ModifiedOn = DateTime.UtcNow;
            _merchant.InsertOne(merchant);
            return merchant;
        }

        public void Update(string id, Merchant merchantIn) =>
            _merchant.ReplaceOne(t => t.ID == id, merchantIn);

        public void Remove(Merchant merchantIn) =>
            _merchant.DeleteOne(t => t.ID == merchantIn.ID);

        public void Remove(string id) =>
            _merchant.DeleteOne(t => t.ID == id);
    }
}
