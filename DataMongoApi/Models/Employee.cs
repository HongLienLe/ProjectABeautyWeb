using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataMongoApi.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
