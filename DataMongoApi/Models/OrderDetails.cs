using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class OrderEntry
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AppointmentID { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> TreatmentIds { get; set; }
        public int MiscPrice { get; set; }
    }

    public class OrderDetails : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        public List<TreatmentOrder> TreatmentOrders { get; set; }
        public int MiscPrice { get; set; }
        public int Total { get; set; }
    }

    public class TreatmentOrder
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string TreatmentId { get; set; }
        public string TreatmentName { get; set; }
        public int Price { get; set; }
    }
}
