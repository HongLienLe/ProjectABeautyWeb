using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DataMongoApi.Models
{
    public class OperatingHours
    {
        [BsonId]
        public string Day { get; set; }
        public TimeSpan OpeningHr { get; set; }
        public TimeSpan ClosingHr { get; set; }
    }
}
