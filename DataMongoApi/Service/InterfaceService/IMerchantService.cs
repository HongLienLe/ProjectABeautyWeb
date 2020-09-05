using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IMerchantService
    {
        public Merchant Get();
        public void Update(Merchant merchantIn);
    }
}
