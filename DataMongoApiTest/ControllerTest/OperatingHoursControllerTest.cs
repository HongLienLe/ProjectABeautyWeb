using System;
using DataMongoApi.Controllers.AdminController;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApiTest.ControllerTest
{
    public class OperatingHoursControllerTest
    {
        private Mock<IOperatingHoursService> _operatingHoursService;
        private OperatingHoursController _operatingHoursController;

        [SetUp]
        public void SetUp()
        {
            _operatingHoursService = new Mock<IOperatingHoursService>();
        }

        [Test]
        public void Get_Return_List_200()
        {
            _operatingHoursService.Setup(x => x.Get()).Returns(new List<OperatingHours>() { new OperatingHours() });
            _operatingHoursController = new OperatingHoursController(_operatingHoursService.Object);

            var actual = _operatingHoursController.Get() as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        [Test]
        public void Get_Valid_Day_200()
        {
            _operatingHoursService.Setup(x => x.Get(It.IsAny<string>())).Returns(new OperatingHours());
            _operatingHoursController = new OperatingHoursController(_operatingHoursService.Object);

            var actual = _operatingHoursController.Get("Id") as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        [Test]
        public void Update_Existing_Day_200()
        {
            _operatingHoursService.Setup(x => x.Get(It.IsAny<string>())).Returns(new OperatingHours());
            _operatingHoursService.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<OperatingHoursDetails>()));

            _operatingHoursController = new OperatingHoursController(_operatingHoursService.Object);

            var actual = _operatingHoursController.Update("id", new OperatingHoursDetails()) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);

        }
    }
}
