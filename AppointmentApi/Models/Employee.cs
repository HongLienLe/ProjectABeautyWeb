using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace AppointmentApi.Models
{
    public class EmployeeDetails : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; } 
    }

    public class Employee : EmployeeDetails
    {
        public List<ObjectId> Treatments { get; set; } = new List<ObjectId>();
        public List<ObjectId> Workdays { get; set; } = new List<ObjectId>();
    }
}
