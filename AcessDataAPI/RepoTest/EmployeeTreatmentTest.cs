using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Moq;
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
        }

        [Test]
        public void ReturnEmployeesByTreatmentId()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.TreatmentExist(1)).Returns(true);

            _employeeTreatmentRepo = new EmployeeTreatmentRepo(_context, mockDoes.Object);
            var resultGetEmployeeByTreatment = _employeeTreatmentRepo.GetEmployeesByTreatment(1).Select(x => x.Id);
            var expectedEmployeeId = new List<int>() { 1, 2,3};
            EndConnection();

            Assert.IsTrue(resultGetEmployeeByTreatment.Count() == expectedEmployeeId.Count());
            Assert.IsTrue(expectedEmployeeId.All(x => resultGetEmployeeByTreatment.Contains(x)));
        }

        [Test]
        public void ReturnTreatmentsByEmployeeId()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.EmployeeExist(1)).Returns(true);

            _employeeTreatmentRepo = new EmployeeTreatmentRepo(_context, mockDoes.Object);
                
            var resultGetTreatmentByEmployee = _employeeTreatmentRepo.GetTreatmentsByEmployee(1).Select(x => x.Id);
            var exceptedTreatmentId = new List<int>() { 1, 2, 3, 4,5};

            EndConnection();

            Assert.IsTrue(resultGetTreatmentByEmployee.Count() == exceptedTreatmentId.Count());
            Assert.IsTrue(exceptedTreatmentId.All(x => resultGetTreatmentByEmployee.Contains(x)));
        }

        [Test]
        public void AddByTreatmentIdToEmployeeIds()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.TreatmentExist(6)).Returns(true);

            _employeeTreatmentRepo = new EmployeeTreatmentRepo(_context, mockDoes.Object);

            OneIdToManyIdForm et = new OneIdToManyIdForm()
            {
                Id = 6,
                Ids = new List<int>() { 3 },
            };

            _employeeTreatmentRepo.AddEmployeesToTreatment(et);

            var resultTreatment6Employee = _employeeTreatmentRepo.GetEmployeesByTreatment(6);

            var expectedEmployeeId = 3;

            EndConnection();

            Assert.IsTrue(resultTreatment6Employee.All(x => x.Id == expectedEmployeeId));
            Assert.IsTrue(resultTreatment6Employee.Count() == 1);
        }

        [Test]
        public void AddEmployeeIdToTreatmentIds()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.EmployeeExist(3)).Returns(true);
            mockDoes.Setup(x => x.TreatmentExist(6)).Returns(true);

            _employeeTreatmentRepo = new EmployeeTreatmentRepo(_context, mockDoes.Object);

            OneIdToManyIdForm et = new OneIdToManyIdForm()
            {
                Id = 3,
                Ids = new List<int>(){6},
            };

            _employeeTreatmentRepo.AddTreatmentsToEmployee(et);

            var resultTreatment6Employee = _employeeTreatmentRepo.GetEmployeesByTreatment(6);

            var expectedEmployeeId = 3;

            EndConnection();

            Assert.IsTrue(resultTreatment6Employee.All(x => x.Id == expectedEmployeeId));
        }

        [Test]
        public void ReturnTrueIfRemoved()
        {
            OneIdToManyIdForm et = new OneIdToManyIdForm()
            {
                Id = 1,
                Ids = new List<int>() { 5 },
            };
            _employeeTreatmentRepo.RemoveEmployeeFromTreatments(et);

            EndConnection();
        }

       
    }
}
