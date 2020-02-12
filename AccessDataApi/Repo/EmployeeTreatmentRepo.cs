using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class EmployeeTreatmentRepo : IEmployeeTreatmentRepo
    {
        private ApplicationContext _context;

        public EmployeeTreatmentRepo(ApplicationContext context)
        {
            _context = context;
        }

        public void AddEmployeeTreatment(EmployeeTreatmentCrud et)
        {

                if (et.isEmployeeId)
                {
                     var treatment = _context.EmployeeTreatment.Where(x => et.Ids.Contains(x.TreatmentId) && x.EmployeeId == et.Id).Select(x => x.TreatmentId);

                     var notAddedTreatmentsToEmployee = et.Ids.Except(treatment);


                     foreach(var treatmentId in notAddedTreatmentsToEmployee)
                        {

                         EmployeeTreatment employeeTreatment = new EmployeeTreatment()
                            {
                                TreatmentId = treatmentId,
                                EmployeeId = et.Id
                            };

                          _context.EmployeeTreatment.Add(employeeTreatment);
                        }


                    _context.SaveChanges();
                    return;

                }

            var employee = _context.EmployeeTreatment.Where(x => et.Ids.Contains(x.EmployeeId) && x.TreatmentId == et.Id).Select(x => x.EmployeeId);
            var notAddedEmployeesToTreatment = et.Ids.Except(employee);

            foreach (var employeeId in notAddedEmployeesToTreatment)
            {

                EmployeeTreatment employeeTreatment = new EmployeeTreatment()
                {
                    TreatmentId = et.Id,
                    EmployeeId = employeeId
                };

                _context.EmployeeTreatment.Add(employeeTreatment);
            }

            _context.SaveChanges();
            return;
        }

        public List<Treatment> GetTreatmentsByEmployee(int employeeId)
        {
                return _context.EmployeeTreatment.Where(x => x.EmployeeId == employeeId).Select(x => x.Treatment).ToList();
        }

        public List<Employee> GetEmployeesByTreatment(int treatmentId)
        {
                return _context.EmployeeTreatment.Where(x => x.TreatmentId == treatmentId).Select(x => x.Employee).ToList();

        }

        public void RemoveEmployeeTreatment(EmployeeTreatmentCrud et)
        {

                if (et.isEmployeeId)
                {
                    var etList = _context.EmployeeTreatment.Where(x => x.EmployeeId == et.Id && et.Ids.Contains(x.TreatmentId));

                _context.EmployeeTreatment.RemoveRange(etList);

                _context.SaveChanges();

                    return;
                }

                var teList = _context.EmployeeTreatment.Where(x => x.TreatmentId == et.Id && et.Ids.Contains(x.TreatmentId));

            _context.EmployeeTreatment.RemoveRange(teList);

            _context.SaveChanges();
        }
    }
}
