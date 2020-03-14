using System;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;

namespace AcessDataAPITest.RepoTest
{
    public class AvalibilityTest : BaseTest
    {
        private AvailabilityRepo _availbilityRepo;

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
            _availbilityRepo = new AvailabilityRepo(_context);
        }

        [Test]
        public void GetAvaliableTimesForGivenDate()
        {
            var date = new DateTime(2020, 3, 9);
            var expectedDate = new TimeRange(new DateTime(2020, 3, 9, 10, 0, 0), new DateTime(2020, 3, 9, 19, 0, 0));
            var freeTimePeriods = _availbilityRepo.GetAvailableTime(date);

            EndConnection();

            foreach(var timeperiod in freeTimePeriods)
            {
                Console.WriteLine($"Avaliable Time : {timeperiod}");
                Assert.IsTrue(expectedDate.Equals(timeperiod));
            }
        }

        [Test]
        public void GetAvaliableTimesViaTreatmentDateShouldBeTrue()
        {
            var date = new DateTime(2020, 3, 9);
            var expectedDate = new TimeRange(new DateTime(2020, 3, 9, 10, 0, 0), new DateTime(2020, 3, 9, 19, 0, 0));
            var freeTimePeriodsForInFillAcrylic = _availbilityRepo.GetAvailableTimeWithTreatment(date, 1);

            EndConnection();

            foreach(var timeperiod in freeTimePeriodsForInFillAcrylic)
            {
                Console.WriteLine($"Avaliable Time : {timeperiod}");
                Assert.IsTrue(timeperiod.Start.Equals(expectedDate.Start));
                Assert.IsTrue(timeperiod.End.Equals(expectedDate.End));
            }
        }

        [Test]
        public void GetAvaliableTimesViaTreatmentDateShouldBeNull()
        {
            var date = new DateTime(2020, 3, 9);
            var freeTimePeriodsForPedicure = _availbilityRepo.GetAvailableTimeWithTreatment(date, 6);

            EndConnection();

            foreach (var timeperiod in freeTimePeriodsForPedicure)
            {
                Console.WriteLine($"Available Time : {timeperiod}");
                Assert.IsTrue(timeperiod.Equals(null));
            }
        }

        //test if they are trying to request bookings from the past
    }
}
