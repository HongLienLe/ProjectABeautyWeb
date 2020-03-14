using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
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
            _employeeRepo = new EmployeeRepo(_context);
        }

        [Test]
        public void ReturnOkayIfEmployeeListIsPresent()
        {
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
            var resultEmployee = _employeeRepo.GetEmployee(1);

            var expectedName = "Employee1";

            EndConnection();

            Assert.IsTrue(resultEmployee.EmployeName == expectedName);
        }

        [Test]
        public void ReturnTrueIfUpdateEmployeeById()
        {
            var newUpdatedInfo = new Employee() { EmployeName = "Updated" };
            var resultReturn = _employeeRepo.UpdateEmployee(1, newUpdatedInfo);
            var resultUpdatedEmployeeName = _employeeRepo.GetEmployee(1).EmployeName;

            var expectedReturn = "Updated Employee Details";
            var expectedName = "Updated";

            EndConnection();

            Assert.IsTrue(resultReturn == expectedReturn);
            Assert.IsTrue(resultUpdatedEmployeeName == expectedName);
        }

        [Test]
        public void ReturnTrueIfEmployee1HasBeenDeleted()
        {
            _employeeRepo.DeleteEmployee(1);

            var resultIfEmployeeExist = _employeeRepo.GetEmployee(1);

            EndConnection();

            Assert.IsTrue(resultIfEmployeeExist == null);

        }

    }
}