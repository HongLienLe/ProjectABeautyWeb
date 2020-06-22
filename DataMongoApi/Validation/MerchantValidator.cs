using System;
using DataMongoApi.DbContext;
using DataMongoApi.Models;
using System.Collections.Generic;
using MongoDB.Driver;

namespace DataMongoApi.Validation
{
    public class MerchantValidator : IMerchantValidator
    {
        private IMongoDbContext _context;

        private Dictionary<string, List<string>> AllMerchantDb { get; set; }

        public MerchantValidator(IMongoDbContext context)
        {
            _context = context;
            AllMerchantDb = _context.GetDatabasesAndCollections();
        }

        public bool DoesMerchantExist(string merchantId)
        {
            return AllMerchantDb.ContainsKey(merchantId);
        }
    }

    public interface IMerchantValidator
    {
        public bool DoesMerchantExist(string merchantId);
    }
}
