using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class EmployeeTreatmentRepo
    {
        private ApplicationContext _context;
        public EmployeeTreatmentRepo(ApplicationContext context)
        {
            _context = context;
        }
        //Create
        public void AddTreatmentToEmployee(int employeeId, List<int> treatmentIds)
        {
            using(var context = _context)
            {
               var employee = context.Employees.First(x => x.EmployeeId == employeeId);

                var treatments = context.Treatments.Where(x => treatmentIds.Contains(x.TreatmentId)).ToList();

                foreach(var treatment in treatments)
                {
                    EmployeeTreatment employeeTreatment = new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment
                    };

                    employee.EmployeeTreatments.Add(employeeTreatment);

                }

                context.SaveChanges();
            }
        }
        //Add MultipleTreatments

        //Read

        public List<Treatment> GetTreatmentsByEmployee(int employeeId)
        {
            using(var context = _context)
            {
                return context.EmployeeTreatment.Where(x => x.EmployeeId == employeeId).Select(x => x.Treatment).ToList();
                
            }
        }
        //Update
        //Delete
    }
}
