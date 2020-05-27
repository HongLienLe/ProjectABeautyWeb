using System;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DataMongoApiTest.IntergrationTest
{
    public class TestDataContext
    {
        public MongoDbContext mongoDbContext;
        private EmployeeService _employeeService;

        [SetUp]
        public void SetUp()
        {
            var settings = Options.Create<SalonDatabaseSettings>(new SalonDatabaseSettings()
            {
                ConnectionString = "mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass%20Community&ssl=false"
            });

            var clientConfig = new ClientConfiguration()
            {
                MerchantId = "TestDB"
            };
            mongoDbContext = new MongoDbContext(settings, clientConfig);

            _employeeService = new EmployeeService(mongoDbContext);
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
            var employeesCount = _employeeService.Get().Count;
            Assert.NotNull(result);
            Assert.AreEqual(result.Details, details);
            Assert.IsNotEmpty(result.ID);
            Assert.AreEqual(employeesCount, 2);
        }
    }
}
