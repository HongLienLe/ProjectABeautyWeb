using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class OrderDetails : Entity
    {
        public string ClientPhone { get; set; }
        public List<TreatmentOrder> Treatments { get; set; }
        public int MiscPrice { get; set; }
        public int Total { get; set; }
    }

    public class TreatmentOrder
    {
        public string TreatmentId { get; set; }
        public int Price { get; set; }
    }
}
