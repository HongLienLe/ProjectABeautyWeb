using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using NUnit.Framework;

namespace AcessDataAPITest.RepoTest
{ 
    public class EmployeeTreatmentTest : BaseTest
    {
        private EmployeeTreatmentRepo _employeeTreatmentRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.Treatments.AddRange(GetTreatments());
            _context.Employees.AddRange(GetEmployees());
            _context.SaveChanges();
            _context.EmployeeTreatment.AddRange(GetEmployeeTreatments());
            _context.SaveChanges();
            _employeeTreatmentRepo = new EmployeeTreatmentRepo(_context);
        }

        [Test]
        public void ReturnEmployeesByTreatmentId()
        {
            var resultGetEmployeeByTreatment = _employeeTreatmentRepo.GetEmployeesByTreatment(1).Select(x => x.EmployeeId);
            var expectedEmployeeId = new List<int>() { 1, 2,3};
            EndConnection();

            Assert.IsTrue(resultGetEmployeeByTreatment.Count() == expectedEmployeeId.Count());
            Assert.IsTrue(expectedEmployeeId.All(x => resultGetEmployeeByTreatment.Contains(x)));
        }

        [Test]
        public void ReturnTreatmentsByEmployeeId()
        {
            var resultGetTreatmentByEmployee = _employeeTreatmentRepo.GetTreatmentsByEmployee(1).Select(x => x.TreatmentId);
            var exceptedTreatmentId = new List<int>() { 1, 2, 3, 4,5};

            EndConnection();

            Assert.IsTrue(resultGetTreatmentByEmployee.Count() == exceptedTreatmentId.Count());
            Assert.IsTrue(exceptedTreatmentId.All(x => resultGetTreatmentByEmployee.Contains(x)));
        }

        [Test]
        public void AddByTreatmentIdToEmployeeIds()
        {
            EmployeeTreatmentCrud et = new EmployeeTreatmentCrud() {
                Id = 6,
                Ids = new List<int>() { 3 },
                isEmployeeId = false };

            _employeeTreatmentRepo.AddEmployeeTreatment(et);

            var resultTreatment6Employee = _employeeTreatmentRepo.GetEmployeesByTreatment(6);

            var expectedEmployeeId = 3;

            EndConnection();

            Assert.IsTrue(resultTreatment6Employee.All(x => x.EmployeeId == expectedEmployeeId));
            Assert.IsTrue(resultTreatment6Employee.Count() == 1);
        }

        [Test]
        public void AddEmployeeIdToTreatmentIds()
        {
            EmployeeTreatmentCrud et = new EmployeeTreatmentCrud()
            {
                Id = 3,
                Ids = new List<int>(){6},
                isEmployeeId = true
            };

            _employeeTreatmentRepo.AddEmployeeTreatment(et);

            var resultTreatment6Employee = _employeeTreatmentRepo.GetEmployeesByTreatment(6);

            var expectedEmployeeId = 3;

            EndConnection();

            Assert.IsTrue(resultTreatment6Employee.All(x => x.EmployeeId == expectedEmployeeId));
        }

        [Test]
        public void ReturnTrueIfRemoved()
        {
            EmployeeTreatmentCrud et = new EmployeeTreatmentCrud()
            {
                Id = 1,
                Ids = new List<int>() { 5 },
                isEmployeeId = true
            };
            _employeeTreatmentRepo.RemoveEmployeeTreatment(et);

            EndConnection();
        }

       
    }
}
