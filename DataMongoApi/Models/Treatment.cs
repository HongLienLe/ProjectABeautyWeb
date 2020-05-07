using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class TreatmentDetails
    {
        public string TreatmentName { get; set; }
        public string TreatmentType { get; set; }
        public bool IsAddOn { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
    }

    public class Treatment : Entity
    {
        public TreatmentDetails About { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Employees { get; set; } = new List<string>();
        public double Name { get; set; }
    }
}
