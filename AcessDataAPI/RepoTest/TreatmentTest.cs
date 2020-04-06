using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Moq;
using NUnit.Framework;

namespace AcessDataAPITest.RepoTest
{
    public class TreatmentTest : BaseTest
    {
        private TreatmentRepo _treatmentRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.Treatments.AddRange(SeedTreatmentData());
            _context.SaveChanges();
        }

        //"Sucessfully Added Treatment" test the add 

        [Test]
        public void ReturnAllTreatmentsList()
        {
            var mockDoes = new Mock<IDoes>();

            _treatmentRepo = new TreatmentRepo(_context, mockDoes.Object);

            var resultTreatments = _treatmentRepo.GetTreatments();
            var expectedTreatments = SeedTreatmentData();

            EndConnection();

            for(int i = 0; i < expectedTreatments.Count; i++)
            {
                Assert.IsTrue(resultTreatments[i].TreatmentName == expectedTreatments[i].TreatmentName);
            }
        }

        [Test]
        public void ReturnCorrectTreatmentById()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.TreatmentExist(1)).Returns(true);

            _treatmentRepo = new TreatmentRepo(_context, mockDoes.Object);

            var resultTreatment = _treatmentRepo.GetTreatment(1);

            EndConnection();

            Assert.IsTrue(resultTreatment.TreatmentName == "Infill");
            Assert.IsTrue(resultTreatment.TreatmentType == TreatmentType.Acrylic);

        }

        [Test]
        public void ReturnUpdatedTreatmentById1()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.TreatmentExist(1)).Returns(true);

            _treatmentRepo = new TreatmentRepo(_context, mockDoes.Object);

            var updatedTreatment = new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 99, Price = 99 };

            _treatmentRepo.UpdateTreatment(1, updatedTreatment);

            var getTreatment1 = _treatmentRepo.GetTreatment(1);

            var expectedPrice = 99;
            var expectedDuration = 99;

            EndConnection();

            Assert.IsTrue(getTreatment1.Price == expectedPrice);
            Assert.IsTrue(getTreatment1.Duration == expectedDuration);
        }

        [Test]
        public void ReturnTrueIfSuccessfullyDeleteTreatmentId1()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.TreatmentExist(1)).Returns(true);

            _treatmentRepo = new TreatmentRepo(_context, mockDoes.Object);

            _treatmentRepo.DeleteTreatment(1);

            var resultTreatmentShouldBeNull = _context.Treatments.Any( x=> x.TreatmentId == 1);

            EndConnection();

            Assert.IsFalse(resultTreatmentShouldBeNull);
        }

        private List<Treatment> SeedTreatmentData()
        {
            List<Treatment> treatments = new List<Treatment>()
            {
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 },
              new Treatment() { TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 },
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 },
              new Treatment() { TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 }
            };

            return treatments;
        }
    }
}
