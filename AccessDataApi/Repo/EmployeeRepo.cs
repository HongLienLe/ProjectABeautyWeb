using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private ApplicationContext _context;
        private IDoes _does;

        public EmployeeRepo(ApplicationContext context, IDoes does)
        {
            _context = context;
            _does = does;
        }

        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();

        }

        public Employee GetEmployee(int employeeId)
        {
            if (!_does.EmployeeExist(employeeId))
                return null;

            return _context.Employees.First(x => x.EmployeeId == employeeId);    
        }

        public string AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);

            _context.SaveChanges();

            return "Successfully added new employee";
        }

        public string UpdateEmployee(int employeeId, Employee employee)
        {
            if(!_does.EmployeeExist(employeeId))
                return null ;

            var oldEmployee = _context.Employees.First(x => x.EmployeeId == employeeId);

            oldEmployee.EmployeName = employee.EmployeName;

            _context.SaveChanges();

            return "Updated employee details"; 
        }

        public string DeleteEmployee(int id)
        {
            if (!_does.EmployeeExist(id))
                return null;

            _context.Remove(_context.Employees.Single(x => x.EmployeeId == id));

            _context.SaveChanges();

            return "Successfully removed employee";
            
        }
    }
}
