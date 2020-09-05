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
            var treatments = new List<TreatmentIdName>();

            if (eF.Treatments != null)
            {

                foreach (var treatmentId in eF.Treatments)
                {
                    var treatment = _treatment.Find(x => x.ID == treatmentId).FirstOrDefault();
                    if (treatment == null)
                        return null;

                    var tIdName = new TreatmentIdName()
                    {
                        Id = treatment.ID,
                        TreatmentName = $"{treatment.About.TreatmentType} {treatment.About.TreatmentName}"
                    };

                    treatments.Add(tIdName);
                }
            }

            var employee = new Employee()
            {
                Details = eF.Details,
                Treatments = treatments,
                WorkDays = eF.WorkDays,
                ModifiedOn = DateTime.Now
            };

            _employee.InsertOne(employee);

            if (employee.Treatments != null)
                AddTreatmentsSkills(employee.ID, eF.Treatments);

            if (employee.WorkDays != null)
                AddWorkDays(employee.ID, employee.WorkDays);

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
                RemoveEmployeeFromWorkDays(employee);
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


        private void AddTreatmentsSkills(string id, List<string> treatments)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID,id);
            var updateEmployee = Builders<Employee>.Update
                .Set("Treatments", treatments)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);



            var treatment = Builders<Treatment>.Filter.In("ID",treatments);
            var updateTreatment = Builders<Treatment>.Update
                .AddToSet("Employees", id)
                .CurrentDate(t => t.ModifiedOn);
            _treatment.UpdateMany(treatment, updateTreatment);
        }

        private void AddWorkDays(string id, List<string> days)
        {
            var employee = Builders<Employee>.Filter.Eq(e => e.ID, id);
            var updateEmployee = Builders<Employee>.Update
                .Set("WorkDays", days)
                .CurrentDate(e => e.ModifiedOn);
            _employee.UpdateOne(employee, updateEmployee);

                var employeeIdName = new EmployeeIdName()
                {
                    Id = id,
                    Name = Get(id).Details.Name
                };

            var operatingDays = Builders<OperatingHours>.Filter.In(x => x.About.Day, days);
            var updateOperatingHours = Builders<OperatingHours>.Update
                .AddToSet("Employees", employeeIdName)
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

        private void RemoveEmployeeFromWorkDays(Employee employee)
        {
            var employeeIDName = new EmployeeIdName()
            {
                Id = employee.ID,
                Name = employee.Details.Name
            };

            var dayFilter = Builders<OperatingHours>.Filter.AnyEq(x => x.Employees, employeeIDName);
            var dayUpdate = Builders<OperatingHours>.Update
                .Pull("Employees", employeeIDName);
            _operatingHours.UpdateMany(dayFilter, dayUpdate);
        }

    }
}
