using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IEmployeeTreatmentRepo
    {
        public string AddTreatmentsToEmployee(OneIdToManyIdForm et);
        public string AddEmployeesToTreatment(OneIdToManyIdForm et);
        public List<TreatmentDetails> GetTreatmentsByEmployee(int id);
        public List<EmployeeDetails> GetEmployeesByTreatment(int id);
        public string RemoveEmployeeFromTreatments(OneIdToManyIdForm et);
        public string RemoveTreatmentFromEmployees(OneIdToManyIdForm et);
    }
}
