using NUnit.Framework;
using Moq;
using DataMongoApi.Models;
using MongoDB.Driver;
using DataMongoApi.Service;
using DataMongoApi.Middleware;

namespace DataMongoApiTest
{
    public class Tests
    {
        private ClientService _clientService;

        [SetUp]
        public void Setup()
        {

           
        }

        [Test]
        public void ShouldCreateNewClient()
        {
            var settingsMock = new Mock<ISalonDatabaseSettings>();
            settingsMock.SetupAllProperties();
            settingsMock.Object.ConnectionString = "mongodb+srv://admin:admin@salondb-xznnp.azure.mongodb.net/test?retryWrites=true&w=majority";

            var clientConfigMock = new Mock<IClientConfiguration>();
            clientConfigMock.SetupAllProperties();
            clientConfigMock.Object.MerchantId = "TEST";

            _clientService = new ClientService(settingsMock.Object, clientConfigMock.Object);

            var newClient = new ClientDetails()
            {
                FirstName = "First",
                LastName = "Last",
                Email = "FL@mail.com",
                Phone = "123"
            };

            var processClient = _clientService.Create(newClient);

            Assert.AreEqual(processClient.About.FirstName, "First");
        }
    }
}