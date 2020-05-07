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
    public class ClientServiceTest
    {
        private Mock<IMongoCollection<Client>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private ClientService _clientService;
        private List<Client> _list;
        private Client _client;

        [SetUp]
        public void SetUp()
        {

            _client = new Client()
            {
                About = new ClientDetails()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    Email = "HL@mail.com",
                    Phone = "123"
                }
            };

            _mockCollection = new Mock<IMongoCollection<Client>>();
            _mockCollection.Object.InsertOne(_client);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<Client>();
            _list.Add(_client);

            Mock<IAsyncCursor<Client>> _clientCursor = new Mock<IAsyncCursor<Client>>();
            _clientCursor.Setup(_ => _.Current).Returns(_list);
            _clientCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Client>>(),
            It.IsAny<FindOptions<Client, Client>>(),
             It.IsAny<CancellationToken>())).Returns(_clientCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Client>("Clients")).Returns(_mockCollection.Object);

            _clientService = new ClientService(_mockContext.Object);

        }

        [Test]
        public void Get_Valid_Success()
        {
            var result = _clientService.Get();

            foreach (var client in result)
            {
                Assert.NotNull(client);
                Assert.AreEqual(client.About, client.About);
                break;
            }

            _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<Client>>(),
                It.IsAny<FindOptions<Client>>(),
                 It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Create_Valid_Success()
        {
            var client = new ClientDetails()
            {
                FirstName = "Viet",
                LastName = "Le",
                Email = "VL@mail.com",
                Phone = "321"
            };
            

            var result = _clientService.Create(client);

            Assert.NotNull(result);
            Assert.AreEqual(result.About, client);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Read_Client_Valid_Success()
        {
            var result = _clientService.Get(_client.ID);

            Assert.NotNull(result);
            Assert.AreEqual(result.About, _client.About);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Remove_Client_Valid_Success()
        {
            var client = new ClientDetails()
            {
                FirstName = "Viet",
                LastName = "Le",
                Email = "VL@mail.com",
                Phone = "321"
            };


            var result = _clientService.Create(client);

            _clientService.Remove(result.ID);

            Assert.IsTrue(_clientService.Get().Count == 1);
        }

        [Test]
        public void Invalid_Id_Return_Null()
        {
            var result = _clientService.Get("Invalid");
            Console.WriteLine(result.About.FirstName);
            Assert.IsNull(result);
        }
    }
}
