using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IEmployeeTreatmentRepo
    {
        public void AddEmployeeTreatment(EmployeeTreatmentCrud et);
        public List<Treatment> GetTreatmentsByEmployee(int employeeId);
        public List<Employee> GetEmployeesByTreatment(int treatmentId);
        public void RemoveEmployeeTreatment(EmployeeTreatmentCrud et);
    }
}
