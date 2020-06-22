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


        public Employee Create(EmployeeForm eF)
        {
            var employee = new Employee()
            {
                Details = eF.Details,
                Treatments = eF.Treatments,
                WorkDays = eF.WorkDays,
                ModifiedOn = DateTime.Now
            };

            _employee.InsertOne(employee);
            return employee;
        }

        public void Update(string id, EmployeeForm eF)
        {
            UpdateEmployeeDetails(id, eF.Details);

            if(eF.Treatments != null)
            AddTreatmentsSkills(id, eF.Treatments);
            if(eF.WorkDays != null)
            AddWorkDays(id, eF.WorkDays);
        }

        public void Remove(string id)
        {
            var employee = Get(id);
            if(employee.Treatments !=  null)
                RemoveEmployeeFromTreatment(id);
            if(employee.WorkDays != null)
                RemoveEmployeeFromWorkDays(id);
            _employee.DeleteOne(e => e.ID == id);
        }

        private void UpdateEmployeeDetails(string id, EmployeeDetails employeeIn)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.ID, id);
            var update = Builders<Employee>.Update
                .Set(e => e.Details, employeeIn)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(filter, update);
        }


        private void AddTreatmentsSkills(string id, List<TreatmentSkills> treatments)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID,id);
            var updateEmployee = Builders<Employee>.Update
                .Set("Treatments", treatments)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);



            var treatment = Builders<Treatment>.Filter.In("ID",treatments.Select(x => x.TreatmentId).ToList());
            var updateTreatment = Builders<Treatment>.Update
                .AddToSet("Employees", id)
                .CurrentDate(t => t.ModifiedOn);
            _treatment.UpdateMany(treatment, updateTreatment);
        }

        private void AddWorkDays(string id, List<WorkDay> day)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID, id);
            var updateEmployee = Builders<Employee>.Update
                .Set("WorkDays", day)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);

            var dayId = day.Select(x => x.OperatingHoursId);

            var operatingDays = Builders<OperatingHours>.Filter.In("ID", dayId);
            var updateOperatingHours = Builders<OperatingHours>.Update
                .AddToSet("Employees", id)
                .CurrentDate(t => t.ModifiedOn);
            _operatingHours.UpdateMany(operatingDays, updateOperatingHours);
        }

        private void RemoveEmployeeFromTreatment(string id)
        {
            var treatmentfilter = Builders<Treatment>.Filter.AnyEq("Employees", id);
            var treatmentupdate = Builders<Treatment>.Update
                .Pull("Employees", id);
            _treatment.UpdateMany(treatmentfilter, treatmentupdate);
        }

        private void RemoveEmployeeFromWorkDays(string id)
        {
            var dayFilter = Builders<OperatingHours>.Filter.AnyEq("Employees", id);
            var dayUpdate = Builders<OperatingHours>.Update
                .Pull("Employees", id);
            _operatingHours.UpdateMany(dayFilter, dayUpdate);
        }

    }
}
