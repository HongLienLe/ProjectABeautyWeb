using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IEmployeeRepo
    {
        public List<Employee> GetEmployees();
        public Employee GetEmployee(int employeeId);
        public void AddEmployee(Employee employee);
        public string UpdateEmployee(int employeeId, Employee employee);
        public void DeleteEmployee(int employeeId);

    }
}
