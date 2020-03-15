using System;
using Moq;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;
using System.Linq;

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
            _context.OperatingTimes.AddRange(GetOpeningTimes());
            _context.SaveChanges();
            _context.workSchedules.AddRange(GetOperatingTimeEmployees());
            _context.SaveChanges();
            _bookAppRepo = new BookAppointment(_context);
        }

        [Test]
        public void BookTheFirstAppointmentWithValidModel()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation()
                {
                    StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) 
                }
            };

           var response = _bookAppRepo.CreateAppointment(date, requestedBookApp);

            var appointmentwasbooked = _context.AppointmentDetails.Any();
            EndConnection();

            Console.WriteLine(response);

            Assert.IsTrue(response == "Booking has been successful");
            Assert.IsTrue(appointmentwasbooked);
        // check if the appointment was made in reservation && booked App
        }

        [Test]
        public void ReturnCorrectResponseWhenDateIsInvalidOperatingTime()
        {
            var invalidDate = new DateTime(2020, 3, 8);

            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                EmployeeId = 1,
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = invalidDate.AddHours(10), EndTime = invalidDate.AddHours(10).AddMinutes(45) } 
            };
            var response = _bookAppRepo.CreateAppointment(invalidDate, requestedBookApp);
            var shouldBeFalseNotAddedToDB = _context.DateTimeKeys.Any() && _context.AppointmentDetails.Any() ? true : false;


            EndConnection();

            Console.WriteLine(response);
            Assert.IsFalse(shouldBeFalseNotAddedToDB);
            Assert.IsTrue(response == "Business is closed on requested date");
        }

        [Test]
        public void CanAllocateCorrectEmployeeIfChoosenEmployeeIs0IsAny()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) } 
            };

            var response = _bookAppRepo.CreateAppointment(date, requestedBookApp);

            Console.WriteLine(response);

            var employeeBookings = _context.Employees.First(x => x.EmployeeId == 1).Appointments.ToList();
            EndConnection();

            Assert.IsTrue(response == "Booking has been successful");
            Assert.IsTrue(employeeBookings.Contains(requestedBookApp));
            Assert.IsTrue(employeeBookings[0].DateTimeKeyId == date.ToShortDateString());
            Assert.IsTrue(employeeBookings[0].Reservation.StartTime == requestedBookApp.Reservation.StartTime);
            Assert.IsTrue(employeeBookings[0].Reservation.EndTime == requestedBookApp.Reservation.EndTime);
            Assert.IsTrue(employeeBookings[0].TreatmentId == requestedBookApp.TreatmentId);
        }

        [Test]
        public void CanReturnInvalidResponseIfEmployeeNotAvaliableAndNotSaveAnyData()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation()
                {
                     StartTime = date.AddHours(11), EndTime = date.AddHours(11).AddMinutes(45) 
                }
            };

            FullyBookedAllDay(2);

            var response = _bookAppRepo.CreateAppointment(date, requestedBookApp);
            var employeeBookings = _context.Employees.First(x => x.EmployeeId == 1).Appointments.ToList();


            EndConnection();

            Console.WriteLine(response);
            Assert.IsTrue(response == "Time Slots for choosen treatment is not avaliable");
            Assert.IsFalse(employeeBookings.Contains(requestedBookApp));
            Assert.IsTrue(employeeBookings[0].DateTimeKeyId == date.ToShortDateString());
            Assert.IsFalse(employeeBookings[0].Reservation.StartTime == requestedBookApp.Reservation.StartTime);
            Assert.IsFalse(employeeBookings[0].Reservation.EndTime == requestedBookApp.Reservation.EndTime);
        }

        [Test]
        public void CanBookAppointmentWithExistingDateTimeKey()
        {
            var date = new DateTime(2020, 3, 9);
            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                Reservation = new Reservation()
                {
                    StartTime = date.AddHours(11), EndTime = date.AddHours(11).AddMinutes(45) 
                }
            };

            FullyBookedAllDay(1);

            var response = _bookAppRepo.CreateAppointment(date, requestedBookApp);

            var employeeBookings = _context.Employees.First(x => x.EmployeeId == 2).Appointments.ToList();
            var bookings = _context.AppointmentDetails.First(x => x.EmployeeId == 2);

            EndConnection();

            Console.WriteLine(response);
            Assert.IsTrue(response == "Booking has been successful");
            Assert.IsNotNull(bookings);
            Assert.IsTrue(employeeBookings.Contains(requestedBookApp));
            Assert.IsTrue(employeeBookings[0].DateTimeKeyId == date.ToShortDateString());
            Assert.IsTrue(employeeBookings[0].Reservation.StartTime == requestedBookApp.Reservation.StartTime);
            Assert.IsTrue(employeeBookings[0].Reservation.EndTime == requestedBookApp.Reservation.EndTime);
        }

        private void FullyBookedAllDay(int count)
        {
            var date = new DateTime(2020, 3, 9);

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
