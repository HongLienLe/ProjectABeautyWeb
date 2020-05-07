using System;
using System.Collections.Generic;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class Merchant : Entity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
