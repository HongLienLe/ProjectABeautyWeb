using System;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;
using Moq;
using AccessDataApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AcessDataAPITest.ControllerTest
{
    public class AvailbilityControllerTest
    {
        [Test]
        public void ReturnNullWhenNonOperatingDateIsGiven()
        {
            Mock<IAvailabilityRepo> _mockAvalibilityRepo = new Mock<IAvailabilityRepo>();
            var sundayClosed = new DateTime(2020, 3, 8);
            _mockAvalibilityRepo
                .Setup(x => x.GetAvailableTime(sundayClosed))
                .Returns((TimePeriodCollection)null);

            var avalibilityController = new AvailabilityController(_mockAvalibilityRepo.Object);

            var response = avalibilityController.GetAvailableTime(2020, 3, 8) as ObjectResult;
            Console.WriteLine( response);
            Assert.IsTrue(400 == response.StatusCode);
        }

        [Test]
        public void ReturnAvailableAppointmentForTheDay()
        {
            Mock<IAvailabilityRepo> _mockAvalibilityRepo = new Mock<IAvailabilityRepo>();
            var openMonday = new DateTime(2020, 3, 9);
            _mockAvalibilityRepo
                .Setup(x => x.GetAvailableTime(openMonday))
                .Returns(new TimePeriodCollection() { new TimeRange(new DateTime(2020,3,9,10,0,0), new DateTime(2020,3,9,19,0,0)) });

            var availabilityController = new AvailabilityController(_mockAvalibilityRepo.Object);

            var response = availabilityController.GetAvailableTime(2020, 3, 9) as ObjectResult;

            Console.WriteLine(response);
            Assert.IsTrue(200 == response.StatusCode);
        }

        [Test]
        public void ReturnListTimeSlotsForGivenDateByTreatment()
        {
            var returnEmployees = new List<Employee>() { new Employee() { EmployeeId = 1 } };

            Mock<IAvailabilityRepo> _mockAvalibilityRepo = new Mock<IAvailabilityRepo>();
            _mockAvalibilityRepo
                .Setup(x => x.GetWorkingEmployeesByDateAndTreatment(new DateTime(2020, 3, 9), 1))
                .Returns(returnEmployees); 

            var availabilityController = new AvailabilityController(_mockAvalibilityRepo.Object);

            var response = availabilityController.GetWorkingEmployeesByDateAndTreatment(2020, 3, 9, 1) as ObjectResult;

            Console.WriteLine(response);
            Assert.IsTrue(200 == response.StatusCode);
        }

        [Test]
        public void Return400BadRequestIfRequestIsFullyBookedByDateAndTreatmentId()
        {

        }

    }
}
