using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IEmployeeService
    {
        public List<Employee> Get();
        public Employee Get(string id);
        public Employee Create(EmployeeDetails employee);
        public void Update(string id, EmployeeDetails employeeIn);
        public void Remove(string id);
        public void AddTreatmentsSkills(string id, List<string> treatmentIds);
        public void AddWorkDays(string id, List<string> operatingDayIds);
    }
}
