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
        public EmployeeDetails Details { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<TreatmentIdName> Treatments { get; set; } = new List<TreatmentIdName>();
        public List<string> WorkDays { get; set; } = new List<string>();
    }

    public class EmployeeForm
    {
        public EmployeeDetails Details { get; set; }

        public List<string> Treatments { get; set; }
        public List<string> WorkDays { get; set; }
    }

    public class TreatmentIdName
    {
        public string Id { get; set; }
        public string TreatmentName { get; set;}
    }
}
