using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace AppointmentApi.Models
{
    public class TreatmentDetails : Entity
    {
        public string TreatmentName { get; set; }
        public string TreatmentType { get; set; }
        public bool IsAddOn { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
    }

    public class Treatment
    {
        public List<ObjectId> Employees { get; set; }
    }
}
