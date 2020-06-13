//using System;
//using DataMongoApi.DbContext;
//using DataMongoApi.Models;
//using System.Collections.Generic;
//using MongoDB.Driver;

//namespace DataMongoApi.Validation
//{
//    public class MerchantValidator : IMerchantValidator
//    {
//        private IMongoDbContext _context;

//        public MerchantValidator(IMongoDbContext context)
//        {
//            _context = context;
//        }

//        public bool DoesMerchantExist(string merchantId)
//        {
//            _context.AssignDb("PaperAndPen");

//            var merchantIds = _context.GetCollection<Merchant>("Merchant");

//            var merchant = merchantIds.Find(x => x.MerchantId == merchantId);

//            if (merchant == null)
//                return false;

//            _context.AssignDb(merchantId);

//            return true;
//        }
//    }

//    public interface IMerchantValidator
//    {
//        public bool DoesMerchantExist(string merchantId);
//    }
//}
