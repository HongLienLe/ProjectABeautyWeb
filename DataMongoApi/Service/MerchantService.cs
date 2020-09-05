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

        public Merchant Get() =>
            _merchant.Find(Merchant => true).FirstOrDefault();

        public Merchant Create(Merchant merchant)
        {
            merchant.ModifiedOn = DateTime.UtcNow;
            _merchant.InsertOne(merchant);
            return merchant;
        }

        public void Update(Merchant merchantIn)
        {
            var merchant = Get();
            _merchant.ReplaceOne(t => t.ID == merchant.ID, merchantIn);
        }
    }
}
