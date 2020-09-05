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
        public Address Address { get; set; }
    }

    public sealed class Address
    {
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}

