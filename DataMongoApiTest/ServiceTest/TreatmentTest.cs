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
    public class TreatmentTest
    {
        private Mock<IMongoCollection<Treatment>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private TreatmentService _treatmentService;
        private List<Treatment> _list;
        private Treatment _treatment;

        [SetUp]
        public void SetUp()
        {

            _treatment = new Treatment()
            {
                About = new TreatmentDetails()
                {
                    TreatmentName = "Full Set",
                    TreatmentType = "SNS",
                    Duration = 45,
                    Price = 28
                }
            };

            _mockCollection = new Mock<IMongoCollection<Treatment>>();
            _mockCollection.Object.InsertOne(_treatment);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<Treatment>();
            _list.Add(_treatment);

            Mock<IAsyncCursor<Treatment>> _treatmentCursor = new Mock<IAsyncCursor<Treatment>>();
            _treatmentCursor.Setup(_ => _.Current).Returns(_list);
            _treatmentCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Treatment>>(),
            It.IsAny<FindOptions<Treatment, Treatment>>(),
             It.IsAny<CancellationToken>())).Returns(_treatmentCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Treatment>("Treatments")).Returns(_mockCollection.Object);

            _treatmentService = new TreatmentService(_mockContext.Object);

        }

        [Test]
        public void Get_Valid_Success()
        {
            var result = _treatmentService.Get();

            foreach (var treatment in result)
            {
                Assert.NotNull(treatment);
                Assert.AreEqual(treatment.Name, _treatment.Name);
                break;
            }

            _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<Treatment>>(),
                It.IsAny<FindOptions<Treatment>>(),
                 It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Create_Valid_Success()
        {
            var treatment = new Treatment()
            {
                About = new TreatmentDetails()
                {
                    TreatmentName = "Test",
                    TreatmentType = "SNS",
                    Price = 10,
                    Duration = 45,
                }
            };

            var result = _treatmentService.Create(treatment);

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, treatment.Name);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Read_Treatment_Valid_Success()
        {
            var result = _treatmentService.Get(_treatment.ID);

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, _treatment.Name);
            Assert.IsNotEmpty(result.ID);

        }

        [Test]
        public void Remove_Treatment_Valid_Success()
        {
            var treatment = new Treatment()
            {
                About = new TreatmentDetails()
                {
                    TreatmentName = "Test",
                    TreatmentType = "SNS",
                    Price = 10,
                    Duration = 45,
                }
            };

            var result = _treatmentService.Create(treatment);

            _treatmentService.Remove(result.ID);

            Assert.IsTrue(_treatmentService.Get().Count == 1);
        }

        [Test]
        public void Invalid_Id_Return_Null()
        {
            var result = _treatmentService.Get("Invalid");
            Console.WriteLine(result.About.TreatmentName);
            Assert.IsNull(result);
        }
    }
}
