using System;
using Moq;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;
using System.Linq;
using AccessDataApi.HTTPModels;
using AccessDataApi.Functions;

namespace AcessDataAPITest.RepoTest
{
    public class BookAppTest : BaseTest
    {
        private BookAppointment _bookAppRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.Employees.AddRange(GetEmployees());
            _context.Treatments.AddRange(GetTreatments());
            _context.SaveChanges();
            _context.EmployeeTreatment.AddRange(GetEmployeeTreatments());
            _context.SaveChanges();
            _context.OperatingTimes.AddRange(GetOpeningTimes());
            _context.SaveChanges();
            _context.workSchedules.AddRange(GetOperatingTimeEmployees());
            _context.SaveChanges();
        }

        // test with multiple bookings 

        [Test]
        public void BookTheFirstAppointmentWithValidModel()
        {
            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 9))).Returns(false);
            _bookAppRepo = new BookAppointment(_context, mockDoes.Object);


            var requestedBookApp = new BookAppointmentForm()
            {
                Client = new ClientForm()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    ContactNumber = "123",
                    Email = "Hong@mail.com"
                },
                DateTimeFormatt = "2020-12-9",
                StartTime = "10:00:00",
                TreatmentIds = new List<int>() { 1 }  
            };

           var response = _bookAppRepo.MakeAppointment(requestedBookApp);

            var appointmentwasbooked = _context.AppointmentDetails.Any();
            var wasClientAdded = _context.ClientAccounts.Any();
            EndConnection();

            Console.WriteLine(response);

            Assert.IsTrue(response.Contains("Booking has been successful, Order App 1"));
            Assert.IsTrue(appointmentwasbooked);
            Assert.IsTrue(wasClientAdded);
        // check if the appointment was made in reservation && booked App
        }

        [Test]
        public void ReturnCorrectResponseWhenDateIsInvalidOperatingTime()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 20))).Returns(false);

            _bookAppRepo = new BookAppointment(_context, mockDoes.Object);
            var invalidDateApp = new BookAppointmentForm()
            {
                Client = new ClientForm()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    ContactNumber = "123",
                    Email = "Hong@mail.com"
                },
                DateTimeFormatt = "2020-12-20",
                StartTime = "10:00:00",
                TreatmentIds = new List<int>() { 1 }
            };

            var response = _bookAppRepo.MakeAppointment(invalidDateApp);
            var shouldBeFalseNotAddedToDB = _context.DateTimeKeys.Any() && _context.AppointmentDetails.Any() ? true : false;


            EndConnection();

            Console.WriteLine(response);
            Assert.IsFalse(shouldBeFalseNotAddedToDB);
            Assert.IsTrue(response.Contains("closed"));
        }

        [Test]
        public void CanAllocateCorrectEmployeeIfChoosenEmployeeIs0IsAny()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 20))).Returns(false);

            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 9))).Returns(false);

            _bookAppRepo = new BookAppointment(_context, mockDoes.Object);
            var requestedBooking = new BookAppointmentForm()
            {
                Client = new ClientForm()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    ContactNumber = "123",
                    Email = "Hong@mail.com"
                },
                DateTimeFormatt = "2020-12-9",
                StartTime = "10:00:00",
                TreatmentIds = new List<int>() { 1 }
            };

            var response = _bookAppRepo.MakeAppointment(requestedBooking);
            Console.WriteLine(response);

            var employeeBookings = _context.Employees.First(x => x.EmployeeId == 1).Appointments.ToList();
            EndConnection();

            Assert.IsTrue(response.Contains("Booking has been successful, Order App 1"));
            Assert.IsTrue(employeeBookings.Count() == 1);
            Assert.IsTrue(employeeBookings[0].DateTimeKey.date == new DateTime(2020,12,9));
            Assert.IsTrue(employeeBookings[0].TreatmentId == 1);
        }

        [Test]
        public void CanReturnInvalidResponseIfEmployeeNotAvaliableAndNotSaveAnyData()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 9))).Returns(false);

            _bookAppRepo = new BookAppointment(_context, mockDoes.Object);

            var requestedBooking = new BookAppointmentForm()
            {
                Client = new ClientForm()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    ContactNumber = "123",
                    Email = "Hong@mail.com"
                },
                DateTimeFormatt = "2020-12-9",
                StartTime = "11:00:00",
                TreatmentIds = new List<int>() { 1 }
            };

            FullyBookedAllDay(2);

            var response = _bookAppRepo.MakeAppointment(requestedBooking);
            var employeeBookings = _context.AppointmentDetails.Where(x => x.EmployeeId != 3).ToList();
            var madeNewClient = _context.ClientAccounts.Any(x => x.FirstName == "Hong");

            EndConnection();

            Console.WriteLine(response);
            Assert.IsTrue(response.Contains("Time Slots for choosen treatment is not avaliable"));
            Assert.IsTrue(employeeBookings.Count() == 2);
            Assert.IsFalse(madeNewClient);
        }

        [Test]
        public void CanBookAppointmentWithExistingDateTimeKey()
        {
            var mockDoes = new Mock<IDoes>();

            mockDoes.Setup(x => x.DateTimeKeyExist(new DateTime(2020, 12, 9))).Returns(true);

            _bookAppRepo = new BookAppointment(_context, mockDoes.Object);

            var requestedBooking = new BookAppointmentForm()
            {
                Client = new ClientForm()
                {
                    FirstName = "Hong",
                    LastName = "Le",
                    ContactNumber = "123",
                    Email = "Hong@mail.com"
                },
                DateTimeFormatt = "2020-12-9",
                StartTime = "11:00:00",
                TreatmentIds = new List<int>() { 1 }
            };

            FullyBookedAllDay(1);

            var response = _bookAppRepo.MakeAppointment(requestedBooking);

            var employeeBookings = _context.Employees.First(x => x.EmployeeId == 2).Appointments.ToList();
            var bookings = _context.AppointmentDetails.First(x => x.EmployeeId == 2);

            EndConnection();

            Console.WriteLine(response);
            Assert.IsTrue(response.Contains("Booking has been successful, Order App 2"));
            Assert.IsNotNull(bookings);
            Assert.IsTrue(employeeBookings.Count() == 1);
            Assert.IsTrue(employeeBookings[0].DateTimeKey.date == new DateTime(2020, 12, 9));
            Assert.IsTrue(employeeBookings[0].Reservation.StartTime == new DateTime(2020,12,9,11,0,0));
            Assert.IsTrue(employeeBookings[0].Reservation.EndTime == new DateTime(2020,3,12,11,45,0));
        }

        private void FullyBookedAllDay(int count)
        {
            var date = new DateTime(2020, 12, 9);

            for (int i = 1; i <= count; i++)
            {
                var requestedBookApp = new AppointmentDetails()
                {
                    ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                    TreatmentId = 1,
                    Reservation = new Reservation()
                    {
                        StartTime = date.AddHours(10), EndTime = date.AddHours(19)
                    }
                };

                _bookAppRepo.CreateAppointment(date,requestedBookApp);
            }
        }
    }
}
