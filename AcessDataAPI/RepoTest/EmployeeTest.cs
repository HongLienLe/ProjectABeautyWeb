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
    public class EmployeeTest : BaseTest
    {
        private EmployeeRepo _employeeRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.Employees.AddRange(GetEmployees());
            _context.SaveChanges();
        }

        // Add employeefromtheform return success

        [Test]
        public void CanAddNewEmployeeWithForm()
        {
            var mockDoes = new Mock<IDoes>();
            _employeeRepo = new EmployeeRepo(_context, mockDoes.Object);

            EmployeeDetails employeeForm = new EmployeeDetails()
            {
                EmployeeName = "UnitTest",
                Email ="UTest@gmail.com"
            };

            var result = _employeeRepo.AddEmployee(CastTo.Employee(employeeForm));
            var doesNewEmployeeExist = _context.Employees.First(x => x.EmployeName == "UnitTest");

            EndConnection();

            Assert.IsTrue(result.Contains("Success"));
            Assert.IsNotNull(doesNewEmployeeExist);

        }

        [Test]
        public void ReturnOkayIfEmployeeListIsPresent()
        {
            var mockDoes = new Mock<IDoes>();
            _employeeRepo = new EmployeeRepo(_context, mockDoes.Object);

            var resultEmployees = _employeeRepo.GetEmployees();
            var expectedEmployees = GetEmployees();

            EndConnection();

            for(int i = 0; i < expectedEmployees.Count; i++)
            {
                Assert.IsTrue(resultEmployees[i].EmployeName == expectedEmployees[i].EmployeName);
            }
        }

        [Test]
        public void ReturnTrueIfChoosenEmployeeById()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.EmployeeExist(1)).Returns(true);
            _employeeRepo = new EmployeeRepo(_context, mockDoes.Object);
            var resultEmployee = _employeeRepo.GetEmployee(1);

            var expectedName = "Employee1";

            EndConnection();

            Assert.IsTrue(resultEmployee.EmployeName == expectedName);
        }

        [Test]
        public void ReturnTrueIfUpdateEmployeeById()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.EmployeeExist(1)).Returns(true);
            _employeeRepo = new EmployeeRepo(_context, mockDoes.Object);

            var newUpdatedInfo = new Employee() { EmployeName = "Updated" };
            var resultReturn = _employeeRepo.UpdateEmployee(1, newUpdatedInfo);
            var resultUpdatedEmployeeName = _employeeRepo.GetEmployee(1).EmployeName;

            var expectedName = "Updated";

            EndConnection();

            Assert.IsTrue(resultReturn.Contains("Updated"));
            Assert.IsTrue(resultUpdatedEmployeeName == expectedName);
        }

        [Test]
        public void ReturnTrueIfEmployee1HasBeenDeleted()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.EmployeeExist(1)).Returns(true);
            _employeeRepo = new EmployeeRepo(_context, mockDoes.Object);

            _employeeRepo.DeleteEmployee(1);

            var resultIfEmployeeExist = _context.Employees.Any(x => x.EmployeeId == 1);

            EndConnection();

            Assert.IsFalse(resultIfEmployeeExist);

        }

    }
}