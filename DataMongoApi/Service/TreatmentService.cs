using System;
using System.Collections.Generic;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson;
using DataMongoApi.Service.InterfaceService;
using DataMongoApi.DbContext;

namespace DataMongoApi.Service
{
    public class TreatmentService : ITreatmentService
    {
        private readonly IMongoCollection<Treatment> _treatment;
        private readonly IMongoCollection<Employee> _employee;

        private readonly IMongoDbContext _context;


        public TreatmentService(IMongoDbContext context)
        {
            _context = context;
            _treatment = _context.GetCollection<Treatment>("Treatments");
            _employee = _context.GetCollection<Employee>("Employees");
        }

        public List<Treatment> Get()
        {
           return _treatment.Find(Treatment => true).ToList();
        }

        public Treatment Get(string id)
        {
            return _treatment.Find<Treatment>(t => t.ID == id).FirstOrDefault();

          //  var filter = Builders<Treatment>.Filter.Exists(x => x.ID == id);
        }

        public Treatment Create(Treatment treatment)
        {
            treatment.ModifiedOn = DateTime.UtcNow;
            _treatment.InsertOne(treatment);
            return treatment;
        }

        public void Update(string id, TreatmentForm treatmentIn)
        {
            var filter = Builders<Treatment>.Filter.Eq(t => t.ID, id);
            var update = Builders<Treatment>.Update
                .Set(t => t.Name, treatmentIn.Name)
                .Set(t => t.Price, treatmentIn.Price)
                .Set(t => t.isAddOn, treatmentIn.isAddOn)
                .CurrentDate(t => t.ModifiedOn);
            _treatment.UpdateOne(filter, update);
        }

        public void Remove(string id)
        {
            var treatment = Get(id);
            if (treatment.Employees.Count > 0)
                RemoveTreatmentFromEmployee(id);

            _treatment.DeleteOne(t => t.ID == id);
        }

        public void UpdateEmployee(string id, List<string> employeeids)
        {
            var treatment = Get(id);
            treatment.Employees.AddRange(employeeids);

            _treatment.ReplaceOne(t => t.ID == id, treatment);
        }

        public void RemoveTreatmentFromEmployee(string id)
        {
            var employeeFilter = Builders<Employee>.Filter.AnyEq("Treatments", id);

            var employeeUpdate = Builders<Employee>.Update
                .Pull("Treatments", id);

            _employee.UpdateMany(employeeFilter, employeeUpdate);
        }
    }
}
