using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace AppointmentApi.Models
{
    public class OperatingHoursDetails : Entity
    {
        public string Day { get; set; }
        public string OpeningHr { get; set; }
        public string ClosingHr { get; set; }
        public bool isOpen { get; set; }
    }

    public class OperatingHours : OperatingHoursDetails
    {
        public List<ObjectId> Employees { get; set; }
    }
}
