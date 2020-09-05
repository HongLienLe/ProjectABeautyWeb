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
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class Appointment : Entity
    {
        public AppointmentDetails Info { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientID { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<string> TreatmentNames { get; set; }
        public bool HasBeenProcess { get; set; } = false;
        public int MiscPrice { get; set; }
        public int TotalPrice { get; set; }
    }

    public class ReadAppointment : Entity
    {
        public ClientDetails Clients { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Notes { get; set; }
        public string Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<string> TreatmentId { get; set; }
        public List<string> TreatmentNames { get; set; }
        public int Price { get; set; }
        public int MiscPrice { get; set; }
        public int TotalPrice { get; set; }
        public bool HasBeenProcess { get; set; } = false;
    }
}
