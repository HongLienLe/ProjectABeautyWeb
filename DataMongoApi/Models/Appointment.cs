using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class AppointmentDetails
    {
        public ClientDetails Client { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> TreatmentId { get; set; }
        public string Notes { get; set; }
        public string Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class Appointment : Entity
    {
        public AppointmentDetails Info { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string EmployeeId { get; set; }
        public int MiscPrice { get; set; }
        public bool HasBeenProcess { get; set; } = false;
    }
}
