using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    public class OperatingHoursDetails
    {
        public string Day { get; set; }
        public string OpeningHr { get; set; }
        public string ClosingHr { get; set; }
        public bool isOpen { get; set; }
    }

    public class OperatingHours : Entity
    {
        public OperatingHoursDetails About { get; set; }

        public List<EmployeeIdName> Employees { get; set; } = new List<EmployeeIdName>();
    }

    public class EmployeeIdName
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
