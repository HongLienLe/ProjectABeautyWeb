using System;
using DataMongoApi.Controllers.AdminController;
using DataMongoApi.Service.InterfaceService;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using DataMongoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApiTest.ControllerTest
{
    public class TreatmentControllerTest
    {
        private Mock<ITreatmentService> _treatmentService;
        private TreatmentController _treatmentController;

        [SetUp]
        public void SetUp()
        {
            _treatmentService = new Mock<ITreatmentService>();

        }

        [Test]
        public void Get_List_200()
        {
            _treatmentService.Setup(x => x.Get()).Returns(new List<Treatment>());

            _treatmentController = new TreatmentController(_treatmentService.Object);

            var actual = _treatmentController.Get() as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        [Test]
        public void Get_Treatment_Valid_200()
        {
            _treatmentService.Setup(x => x.Get(It.IsAny<string>()));
            _treatmentController = new TreatmentController(_treatmentService.Object);

            var actual = _treatmentController.Get("TreatmentId") as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);

        }

        [Test]
        public void Create_Treatment_Valid_200()
        {
            _treatmentService.Setup(x => x.Create(It.IsAny<Treatment>()));
            _treatmentController = new TreatmentController(_treatmentService.Object);

            var actual = _treatmentController.Create(new TreatmentDetails()) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        [Test]
        public void Update_Treatment_Valid_200()
        {
            _treatmentService.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<TreatmentDetails>()));

            _treatmentController = new TreatmentController(_treatmentService.Object);

            var actual = _treatmentController.Update("id", new TreatmentDetails()) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        [Test]
        public void Delete_Treatment_Valid_200()
        {
            _treatmentService.Setup(x => x.Get(It.IsAny<string>())).Returns(new Treatment());
            _treatmentService.Setup(x => x.Remove(It.IsAny<string>()));
            var actual = _treatmentController.Delete("IDIDIDIDIDIDIDID") as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }
    }
}
