using System;
using System.Collections.Generic;
using DataMongoApi.Models;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employee;

        public EmployeeService(ISalonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _employee = database.GetCollection<Employee>(settings.SalonCollectionName);
        }

        public List<Employee> Get() =>
            _employee.Find(Employee => true).ToList();

        public Employee Get(string id) =>
            _employee.Find<Employee>(e => e.Id == id).FirstOrDefault();

        public Employee Create(Employee employee)
        {
            _employee.InsertOne(employee);
            return employee;
        }

        public void Update(string id, Employee employeeIn) =>
            _employee.ReplaceOne(e => e.Id == id, employeeIn);

        public void Remove(Employee employeeIn) =>
            _employee.DeleteOne(e => e.Id == employeeIn.Id);

        public void Remove(string id) =>
            _employee.DeleteOne(e => e.Id == id);

    }
}
