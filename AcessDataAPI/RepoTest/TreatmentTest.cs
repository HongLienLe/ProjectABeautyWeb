using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
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
            _treatmentRepo = new TreatmentRepo(_context);
        }

        //"Sucessfully Added Treatment" test the add 

        [Test]
        public void ReturnAllTreatmentsList()
        {
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
            var resultTreatment = _treatmentRepo.GetTreatment(1);

            EndConnection();

            Assert.IsTrue(resultTreatment.TreatmentName == "Infill");
            Assert.IsTrue(resultTreatment.TreatmentType == TreatmentType.Acrylic);

        }

        [Test]
        public void ReturnUpdatedTreatmentById1()
        {
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
            _treatmentRepo.DeleteTreatment(1);

            var resultTreatmentShouldBeNull = _treatmentRepo.GetTreatment(1);

            EndConnection();

            Assert.IsTrue(resultTreatmentShouldBeNull == null);
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
