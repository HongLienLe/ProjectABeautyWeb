using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IEmployeeService
    {
        public List<Employee> Get();
        public Employee Get(string id);
        public Employee Create(EmployeeForm eF);
        public void Update(string id, EmployeeForm eF);
        public void Remove(string id);
    }
}
