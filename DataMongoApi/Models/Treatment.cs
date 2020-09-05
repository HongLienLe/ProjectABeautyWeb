using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace DataMongoApi.Models
{
    //public class TreatmentDetails
    //{
    //    public string TreatmentName { get; set; }
    //    public string TreatmentType { get; set; }
    //    public bool isAddOn { get; set; }
    //    public int Price { get; set; }
    //    public int Duration { get; set; }
    //}

    //public class Treatment : Entity
    //{
    //    public TreatmentDetails About { get; set; }

    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public List<string> Employees { get; set; } = new List<string>();
    //}

    //public class TreatmentType : Entity
    //{
    //    public string Type { get; set; }
    //}

    public class TreatmentTypeEntry
    {
        public string Type { get; set; }
    }

    public class TreatmentForm
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool isAddOn { get; set; }
        public int Price { get; set; }
    }

    public class Treatment : Entity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool isAddOn { get; set; }
        public int Price { get; set; }

        public List<EmployeeTreatment> Employees { get; set; }
    }

    public class EmployeeTreatment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> WorkDays { get; set; }
    }
}
