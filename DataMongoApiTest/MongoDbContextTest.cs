using System;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace DataMongoApiTest
{
    public class MongoDbContextTest
    {
        private Mock<IOptions<SalonDatabaseSettings>> _mockOptions;
        private Mock<IMongoDatabase> _mockDB;
        private Mock<IMongoClient> _mockClient;

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<SalonDatabaseSettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
        }


        [Test]
        public void MongoSalonDBContext_Constructor_Success()
        {
            var context = CreateContext();

            Assert.NotNull(context);
        }

        [Test]
        public void MongoSalonDBContext_GetCollection_NameEmpty_Failure()
        {
            var context = CreateContext();
            var myCollection = context.GetCollection<Client>("");

            Assert.Null(myCollection);
        }

        public MongoDbContext CreateContext()
        {
            var settings = new SalonDatabaseSettings()
            {
                ConnectionString = "mongodb+srv://admin:admin@salondb-xznnp.azure.mongodb.net/test?retryWrites=true&w=majority",
                DatabaseName = "TestDB"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c
            .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);
            var mock = new Mock<IClientConfiguration>();
            mock.Setup(x => x.MerchantId).Returns("TestDb");

            return new MongoDbContext(_mockOptions.Object, mock.Object);
        }
    }
}
