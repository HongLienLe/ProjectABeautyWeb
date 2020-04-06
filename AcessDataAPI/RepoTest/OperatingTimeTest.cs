using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using NUnit.Framework;
using System;
using AccessDataApi.Functions;
using Moq;

namespace AcessDataAPITest.RepoTest
{
    public class OperatingTimeTest : BaseTest
    {
        private OperatingTimeRepo _operatingTimeRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.OperatingTimes.AddRange(GetOpeningTimes());
            _context.SaveChanges();
        }

        [Test]
        public void ReturnListOfOpeningHours()
        {
            var mockDoes = new Mock<IDoes>();
            _operatingTimeRepo = new OperatingTimeRepo(_context, mockDoes.Object);

            var resultOpeningHoursList = _operatingTimeRepo.GetOperatingTimes();
            var expectedOpeningHoursList = GetOpeningTimes();

            EndConnection();

            for (int i = 0; i < expectedOpeningHoursList.Count; i++)
            {
                Assert.IsTrue(resultOpeningHoursList[i].Day == expectedOpeningHoursList[i].Day);
            }
        }

        [Test]
        public void ReturnOpeningDayById()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.OpeningIdExist(1)).Returns(true);
            _operatingTimeRepo = new OperatingTimeRepo(_context, mockDoes.Object);

            var resultOpeningDay = _operatingTimeRepo.GetOperatingTime(1);
            var expectedOpeningDay = GetOpeningTimes()[0];

            EndConnection();

            Assert.IsTrue(resultOpeningDay.Day == expectedOpeningDay.Day);
            Assert.IsTrue(resultOpeningDay.isOpen == expectedOpeningDay.isOpen);

        }
    }
}
