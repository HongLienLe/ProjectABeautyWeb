using System;
using System.Collections.Generic;
using System.Threading;
using DataMongoApi.DbContext;
using DataMongoApi.Models;
using DataMongoApi.Service;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace DataMongoApiTest.ServiceTest
{
    public class EmployeeServiceTest
    {
        private Mock<IMongoCollection<Employee>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private EmployeeService _employeeService;
        private List<Employee> _list;
        private Employee _employee;


        [SetUp]
        public void SetUp()
        {

            _employee = new Employee()
            {
                Details = new EmployeeDetails()
                {
                    Name = "Hong",
                    Email = "H@gmail.com"
                }
            };

            _mockCollection = new Mock<IMongoCollection<Employee>>();
            _mockCollection.Object.InsertOne(_employee);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<Employee>();
            _list.Add(_employee);

            Mock<IAsyncCursor<Employee>> _employeetCursor = new Mock<IAsyncCursor<Employee>>();
            _employeetCursor.Setup(_ => _.Current).Returns(_list);
            _employeetCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Employee>>(),
            It.IsAny<FindOptions<Employee, Employee>>(),
             It.IsAny<CancellationToken>())).Returns(_employeetCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Employee>("Employees")).Returns(_mockCollection.Object);

            _employeeService = new EmployeeService(_mockContext.Object);

        }

        [Test]
        public void Get_Valid_Sucess()
        {
            var result = _employeeService.Get();

            foreach (var employee in result)
            {
                Assert.NotNull(employee);
                Assert.AreEqual(employee.Details.Name, _employee.Details.Name);
            }

            _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<Employee>>(),
                It.IsAny<FindOptions<Employee>>(),
                 It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Get_By_Id_Employee_Success()
        {
            var result = _employeeService.Get(_employee.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, _employee);

        }

        [Test]
        public void Create_Employee_Entry_Valid_Success()
        {
            var details = new EmployeeDetails()
            {
                Name = "Test",
                Email = "Test@mail.com"
            };

            var result = _employeeService.Create(details);
            Assert.NotNull(result);
            Assert.AreEqual(result.Details, details);
            Assert.IsNotEmpty(result.ID);
        }

        //[Test]
        //public void Remove_Employee_ByID_Valid_Success()
        //{
        //    var details = new EmployeeDetails()
        //    {
        //        Name = "Test",
        //        Email = "Test@mail.com"
        //    };

        //    var result = _employeeService.Create(details);

        //    _employeeService.Remove(result.ID);

        //    Assert.IsTrue(_employeeService.Get().Count == 1);
        //}

        //[Test]
        //public void Update_Id_Return_Success()
        //{

        //    var details = new EmployeeDetails()
        //    {
        //        Name = "Test",
        //        Email = "Test@mail.com"
        //    };

        //    var result = _employeeService.Create(details);
        //    result.ID = "UID";

        //    var updated = new EmployeeDetails()
        //    {
        //        Name = "Hong",
        //        Email = "Updated@mail.com"
        //    };

        //    _employeeService.Update(result.ID, updated);
        //    var updatedResult = _employeeService.Get(result.ID);

        //    Console.WriteLine(updatedResult.Details.Email);
        //    Assert.AreEqual(updated.Email, updatedResult.Details.Email);
        //    Assert.IsNotNull(updatedResult);

        //}

        [Test]
        public void Update_Work_Days_To_Employee_Successful()
        {

        }

        [Test]
        public void Update_Work_Days_To_Employee_Fail()
        {

        }

        [Test]
        public void Update_Treatments_To_Employee_Successful()
        {

        }

        [Test]
        public void Update_Treatments_To_Employee_Fail()
        {

        }

    }
}
