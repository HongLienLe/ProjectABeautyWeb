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
        public List<TreatmentSkills> Treatments { get; set; } = new List<TreatmentSkills>();
        public List<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
    }

    public class TreatmentSkills
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string TreatmentId { get; set; }
        public string TreatmentName { get; set; }
    }

    public class WorkDay
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string OperatingHoursId { get; set; }
        public string Day { get; set; }
    }

    public class EmployeeForm
    {
        public EmployeeDetails Details { get; set; }
        public List<TreatmentSkills> Treatments { get; set; }
        public List<WorkDay> WorkDays { get; set; }
    }
    
}
