using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<Employee> _employee;
        private readonly IMongoCollection<OperatingHours> _operatingHours;
        private readonly IMongoCollection<Treatment> _treatment;

        public EmployeeService(IMongoDbContext context)
        {
            _context = context;

            _employee = _context.GetCollection<Employee>("Employees");
            _treatment = _context.GetCollection<Treatment>("Treatments");
            _operatingHours = _context.GetCollection<OperatingHours>("OperatingHours");

        }

        public List<Employee> Get()
        {
            return _employee.Find(Employee => true).ToList();
        }

        public Employee Get(string id)
        {
            return _employee.Find<Employee>(e => e.ID == id).FirstOrDefault();
        }

        public Employee Create(EmployeeDetails employee)
        {
            Employee employee1 = new Employee()
            {
                Details = employee,
            };

            employee1.ModifiedOn = DateTime.UtcNow;
            _employee.InsertOne(employee1);
            return employee1;
        }

        public void Update(string id, EmployeeDetails employeeIn)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.ID, id);
            var update = Builders<Employee>.Update
                .Set(e => e.Details, employeeIn)
                .CurrentDate(e => e.ModifiedOn);

            _employee.UpdateOne(filter, update);
        }

        public void Remove(Employee employeeIn) =>
            _employee.DeleteOne(e => e.ID == employeeIn.ID);

        public void Remove(string id) =>
            _employee.DeleteOne(e => e.ID == id);

        public void AddTreatmentsSkills(string id, List<string> treatmentIds)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID,id);

            var updateEmployee = Builders<Employee>.Update
                .AddToSetEach("Treatments", treatmentIds)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);

            var treatment = Builders<Treatment>.Filter.In("ID",treatmentIds);
            var updateTreatment = Builders<Treatment>.Update
                .AddToSetEach("Employees", id)
                .CurrentDate(t => t.ModifiedOn);

            _treatment.UpdateMany(treatment, updateTreatment);
        }

        public void AddWorkDays(string id, List<string> operatingDayIds)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID, id);

            var updateEmployee = Builders<Employee>.Update
                .AddToSetEach("WorkDays", operatingDayIds)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);

            var operatingDays = Builders<OperatingHours>.Filter.In("ID", operatingDayIds);
            var updateOperatingHours = Builders<OperatingHours>.Update
                .AddToSet("Employees", id)
                .CurrentDate(t => t.ModifiedOn);

            _operatingHours.UpdateMany(operatingDays, updateOperatingHours);

        }

    }
}
