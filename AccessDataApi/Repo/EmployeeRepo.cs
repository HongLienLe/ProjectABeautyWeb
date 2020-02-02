using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class EmployeeRepo
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

        public Employee GetEmployee(int id)
        {
            return _context.Employees.First(x => x.EmployeeId == id);
        }

        public void AddEmployee(Employee employee)
        {
            using(var context = _context)
            {
                context.Employees.Add(employee);

                context.SaveChanges();
            }
        }

        public void UpdateEmployee(int id, Employee employee)
        {
            using (var context = _context)
            {
                var oldEmployee = _context.Employees.First(x => x.EmployeeId == id);

                oldEmployee.EmployeName = employee.EmployeName;

                context.SaveChanges();
            }
        }

        //Delete
        public void DeleteEmployee(int id)
        {
            using (var context = _context)
            {
                var employee = new Employee() { EmployeeId = id };

                context.Entry(employee).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}
