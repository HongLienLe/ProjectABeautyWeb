using System;
using System.Collections.Generic;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson;
using DataMongoApi.Service.InterfaceService;

namespace DataMongoApi.Service
{
    public class TreatmentService : ITreatmentService
    {
        private readonly IMongoCollection<Treatment> _treatment;
        //private readonly IMongoCollection<Employee> _employee;


        public TreatmentService(ISalonDatabaseSettings settings, IClientConfiguration clientConfiguration)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId);

            _treatment = database.GetCollection<Treatment>("Treatments");
            //_employee = database.GetCollection<Employee>("Employee");

        }

        public List<Treatment> Get()
        {
           return _treatment.Find(Treatment => true).ToList();
        }

        public Treatment Get(string id) =>
            _treatment.Find<Treatment>(t => t.ID == id).FirstOrDefault();

        public Treatment Create(Treatment treatment)
        {
            treatment.ModifiedOn = DateTime.UtcNow;
            _treatment.InsertOne(treatment);
            return treatment;
        }

        public void Update(string id, TreatmentDetails treatmentIn)
        {

            var filter = Builders<Treatment>.Filter.Eq(t => t.ID, id);
            var update = Builders<Treatment>.Update
                .Set(t => t.About, treatmentIn)
                .CurrentDate(t => t.ModifiedOn);
            _treatment.UpdateOne(filter, update);
        }

        public void Remove(Treatment treatmentIn) =>
            _treatment.DeleteOne(t => t.ID == treatmentIn.ID);

        public void Remove(string id) =>
            _treatment.DeleteOne(t => t.ID == id);

        public void UpdateEmployee(string id, List<string> employeeids)
        {
            var treatment = Get(id);
            treatment.Employees.AddRange(employeeids);

            _treatment.ReplaceOne(t => t.ID == id, treatment);
        }

        //public void UpdateEmployees(string id, List<string> employeeIds)
        //{
        //    var treatment = Builders<Treatment>.Filter.Eq(t => t.ID, id);

        //    var updateTreatment = Builders<Treatment>.Update
        //        .AddToSetEach("Employees", employeeIds)
        //        .CurrentDate(t => t.ModifiedOn);
        //    _treatment.UpdateOne(treatment, updateTreatment);

        //    var employees = Builders<Employee>.Filter.In("ID", employeeIds);
        //    var updateEmployees = Builders<Employee>.Update
        //        .AddToSet("Treatments", id)
        //        .CurrentDate(t => t.ModifiedOn);
        //    _employee.UpdateMany(employees, updateEmployees);

        //}
    }
}
