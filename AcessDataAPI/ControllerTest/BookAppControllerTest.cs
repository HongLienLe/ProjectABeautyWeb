using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;
using Moq;
using AccessDataApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AcessDataAPITest.ControllerTest
{
    public class BookAppControllerTest
    {
        //Valid APP

        [Test]
        public void ReturnSuccess200Status()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) }
            };

            var mockBookApp = new Mock<IBookAppointment>();

            mockBookApp.Setup(x => x.CreateAppointment(date, requestedBookApp))
                .Returns("Booking has been successful");

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.CreateAppointment(2020, 3, 9, requestedBookApp) as ObjectResult;

            Assert.IsTrue(200 == response.StatusCode);
        }

        //Invalid business opening hours

        [Test]
        public void ReturnIsClosedError404()
        {
            var mockBookApp = new Mock<IBookAppointment>();

            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) }
            };

            mockBookApp.Setup(x => x.CreateAppointment(date, requestedBookApp))
                .Returns("Business is closed on requested date");

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.CreateAppointment(2020, 3, 9, requestedBookApp) as ObjectResult;

            Assert.IsTrue(404 == response.StatusCode);
        }

        [Test]
        public void Return404WhenIdDoesNotExistGetApp()
        {
            var mockBookApp = new Mock<IBookAppointment>();

            mockBookApp.Setup(x => x.GetAppointment(0)).Returns((AppointmentDetails)null);

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.GetAppointmentById(0) as ObjectResult;

            Assert.IsTrue(404 == response.StatusCode);
        }

        [Test]
        public void Return200WhenIdExistGetApp()
        {
            var mockBookApp = new Mock<IBookAppointment>();

            mockBookApp.Setup(x => x.GetAppointment(0)).Returns(new AppointmentDetails());

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.GetAppointmentById(0) as ObjectResult;

            Assert.IsTrue(200 == response.StatusCode);
        }

        //Invalid Time slot not available for Treatment and Date

        [Test]
        public void Return404WhenTimeSlotForTreatmentIsNotAvaliableForChoosenDate()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) }
            };

            var mockBookApp = new Mock<IBookAppointment>();

            mockBookApp.Setup(x => x.CreateAppointment(date, requestedBookApp))
                .Returns("Time Slots for choosen treatment is not avaliable");

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.CreateAppointment(2020, 3, 9, requestedBookApp) as ObjectResult;

            Assert.IsTrue(404 == response.StatusCode);
        }

        //Invalid Cant look for past appointments

        [Test]
        public void Return404CantCreateAppForPastDates()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) }
            };

            var mockBookApp = new Mock<IBookAppointment>();

            mockBookApp.Setup(x => x.CreateAppointment(date, requestedBookApp))
                .Returns("Past date not available");

            var controller = new BookAppController(mockBookApp.Object);

            var response = controller.CreateAppointment(2020, 3, 9, requestedBookApp) as ObjectResult;

            Assert.IsTrue(404 == response.StatusCode);
        }
    }
}
