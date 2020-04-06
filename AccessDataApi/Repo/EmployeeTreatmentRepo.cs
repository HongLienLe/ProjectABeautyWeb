using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class EmployeeTreatmentRepo : IEmployeeTreatmentRepo
    {
        private ApplicationContext _context;
        private IDoes _does;

        public EmployeeTreatmentRepo(ApplicationContext context, IDoes does)
        {
            _context = context;
            _does = does;
        }

        public string AddTreatmentsToEmployee(OneIdToManyIdForm et)
        {

            var treatment = _context.EmployeeTreatment
            .Where(x => et.Ids.Contains(x.TreatmentId) && x.EmployeeId == et.Id)
            .Select(x => x.TreatmentId);

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
            return "Treatments have been added employee credentials";

        }

        public string AddEmployeesToTreatment(OneIdToManyIdForm et)
        {
            var employee = _context.EmployeeTreatment
                .Where(x => et.Ids.Contains(x.EmployeeId) && x.TreatmentId == et.Id)
                .Select(x => x.EmployeeId);

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
            return "Treatment has been added to the employees credentials";
        }

        public List<TreatmentDetails> GetTreatmentsByEmployee(int id)
        {
            return _context.EmployeeTreatment
                .Where(x => x.EmployeeId == id)
                .Select(x => CastTo.TreatmentDetails(x.Treatment)).ToList();
        }

        public List<EmployeeDetails> GetEmployeesByTreatment(int id)
        {
            return _context.EmployeeTreatment
                .Where(x => x.TreatmentId == id)
                .Select(x => CastTo.EmployeeDetails(x.Employee)).ToList();
        }

        public string RemoveEmployeeFromTreatments(OneIdToManyIdForm et)
        {

            var etList = _context.EmployeeTreatment
                .Where(x => x.EmployeeId == et.Id && et.Ids.Contains(x.TreatmentId));

            _context.EmployeeTreatment.RemoveRange(etList);

            _context.SaveChanges();

            return $"Successfully removed treatments from Employee {et.Id} credentials";
            
        }

        public string RemoveTreatmentFromEmployees(OneIdToManyIdForm et)
        {
            var teList = _context.EmployeeTreatment
                .Where(x => x.TreatmentId == et.Id && et.Ids.Contains(x.TreatmentId));

            _context.EmployeeTreatment.RemoveRange(teList);

            _context.SaveChanges();

            return $"Successfully removed treatment {et.Id} from Employees credentials";
        }

    }
}
