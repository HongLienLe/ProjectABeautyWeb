using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using Moq;
using NUnit.Framework;

namespace AcessDataAPITest.RepoTest
{
    public class ClientAccountTest : BaseTest
    {
        private ClientAccountRepo _clientAccountRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.ClientAccounts.AddRange(GetClientAccount());
            _context.SaveChanges();
        }

        [Test]
        public void CanCreateNewAccount()
        {
            var mockDoes = new Mock<IDoes>();
            

            _clientAccountRepo = new ClientAccountRepo(_context, mockDoes.Object);

            var clientForm = new ClientForm()
            {
                FirstName = "Unit",
                LastName = "Test",
                Email = "UT@mail.com",
                ContactNumber = "123321"
            };

            var response = _clientAccountRepo.CreateClientAccount(clientForm);

            var expectedClientAccCount3 = _context.ClientAccounts.ToList().Count == 3;

            EndConnection();

            Assert.IsTrue(expectedClientAccCount3);
            Assert.IsTrue(response.Contains("success"));
        }
    }
}
