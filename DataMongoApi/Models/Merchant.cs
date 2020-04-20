using System;
using System.Collections.Generic;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class Merchant : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
    }
}
