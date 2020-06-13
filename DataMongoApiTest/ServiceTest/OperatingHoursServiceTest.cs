using System;
using System.Collections.Generic;
using System.Threading;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace DataMongoApiTest.ServiceTest
{
    public class OperatingHoursServiceTest
    {
        private Mock<IMongoCollection<OperatingHours>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private OperatingHoursService _ophService;
        private List<OperatingHours> _list;
        private OperatingHours _day;

        [SetUp]
        public void SetUp()
        {

            _day = new OperatingHours()
            {
                About = new OperatingHoursDetails()
                {
                    Day = "Monday",
                    OpeningHr = "10:00:00",
                    ClosingHr = "19:00:00",
                    isOpen = true
                }
            };

            _mockCollection = new Mock<IMongoCollection<OperatingHours>>();
            _mockCollection.Object.InsertOne(_day);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<OperatingHours>();
            _list.Add(_day);

            Mock<IAsyncCursor<OperatingHours>> _ophCursor = new Mock<IAsyncCursor<OperatingHours>>();
            _ophCursor.Setup(_ => _.Current).Returns(_list);
            _ophCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<OperatingHours>>(),
            It.IsAny<FindOptions<OperatingHours, OperatingHours>>(),
             It.IsAny<CancellationToken>())).Returns(_ophCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<OperatingHours>("OperatingHours")).Returns(_mockCollection.Object);

            _ophService = new OperatingHoursService(_mockContext.Object);

        }

        [Test]
        public void Get_Valid_Success()
        {
            var result = _ophService.Get();

            foreach (var day in result)
            {
                Assert.NotNull(day);
                Assert.AreEqual(day.About, _day.About);
                break;
            }

            _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<OperatingHours>>(),
                It.IsAny<FindOptions<OperatingHours>>(),
                 It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Create_Valid_Success()
        {
            var day = new OperatingHours()
            {
                About = new OperatingHoursDetails()
                {
                    Day = "Tuesday",
                    OpeningHr = "10:00:00",
                    ClosingHr = "19:00:00",
                    isOpen = true
                }
            };

            var result = _ophService.Create(day);

            Assert.NotNull(result);
            Assert.AreEqual(result.About, day.About);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Read_day_Valid_Success()
        {
            var result = _ophService.Get("2020/4/4");

            Assert.NotNull(result);
            Assert.AreEqual(result.About, _day.About);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Remove_Treatment_Valid_Success()
        {
            var day = new OperatingHours()
            {
                About = new OperatingHoursDetails()
                {
                    Day = "Tuesday",
                    OpeningHr = "10:00:00",
                    ClosingHr = "19:00:00",
                    isOpen = true
                }
            };

            var result = _ophService.Create(day);

            _ophService.Remove(result.ID);

            Assert.IsTrue(_ophService.Get().Count == 1);
        }

        //[Test]
        //public void Invalid_Id_Return_Null()
        //{
        //    var result = _ophService.Get("Invalid");
        //    Console.WriteLine(result.About.Day);
        //    Assert.IsNull(result);
        //}
    }
}
