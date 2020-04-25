using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class ClientDetails 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class Client : Entity
    {
        public ClientDetails About { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Appointments { get; set; } = new List<string>();
        public List<string> Orders { get; set; } = new List<string>();
    }
}
