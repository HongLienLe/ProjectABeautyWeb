using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private ApplicationContext _context;

        public EmployeeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();

        }

        public Employee GetEmployee(int employeeId)
        {
            if (!doesEmployeeExist(employeeId))
            {
                return null;
            }

            return _context.Employees.First(x => x.EmployeeId == employeeId);    
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);

            _context.SaveChanges();
        }

        public string UpdateEmployee(int employeeId, Employee employee)
        {
            if(!doesEmployeeExist(employeeId))
            {
                return "Does not exist, make a new one";
            }
            var oldEmployee = _context.Employees.First(x => x.EmployeeId == employeeId);

            oldEmployee.EmployeName = employee.EmployeName;

            _context.SaveChanges();

            return "Updated Employee Details"; 
        }

        public void DeleteEmployee(int id)
        {
            _context.Remove(_context.Employees.Single(x => x.EmployeeId == id));

            _context.SaveChanges();
            
        }

        public bool doesEmployeeExist(int employeeId)
        {
            return _context.Employees.Any(x => x.EmployeeId == employeeId);
        }
    }
}
