using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class EmployeeDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Employee : Entity
    {
        public EmployeeDetails About { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Treatments { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> WorkDays { get; set; } = new List<string>();
    }
}
