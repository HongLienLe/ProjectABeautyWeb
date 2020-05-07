using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IMerchantService
    {
        public Merchant Get(string id);
        public Merchant Create(Merchant merchant);
        public void Update(string id, Merchant merchantIn);
        public void Remove(Merchant merchantIn);
        public void Remove(string id);

    }
}
