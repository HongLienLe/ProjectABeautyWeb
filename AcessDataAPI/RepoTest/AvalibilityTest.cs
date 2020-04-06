using System;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using Moq;
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
        }

        [Test]
        public void GetAvaliableTimesViaTreatmentDateShouldBeTrue()
        {
            var date = new DateTime(2020, 12, 9);

            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.DateTimeKeyExist(date)).Returns(false);

            _availbilityRepo = new AvailabilityRepo(_context, mockDoes.Object);


            var expectedDate = GetFreeSlots();
            var freeTimePeriodsForInFillAcrylic = _availbilityRepo.GetAvailableTimeWithTreatment(date, new List<int>() { 1 });

            EndConnection();

            for (int i = 0; i < expectedDate.Count; i++)
            {
                Console.WriteLine($" Result Avaliable Time : {freeTimePeriodsForInFillAcrylic[i].ToShortTimeString()}");

                Assert.IsTrue(freeTimePeriodsForInFillAcrylic[i] == expectedDate[i]);
            }
        }

        [Test]
        public void GetAvaliableTimesViaTreatmentDateShouldBeNull()
        {
            var date = new DateTime(2020, 12, 9);

            var mockDoes = new Mock<IDoes>();
            mockDoes.Setup(x => x.DateTimeKeyExist(date)).Returns(false);

            _availbilityRepo = new AvailabilityRepo(_context, mockDoes.Object);

            var freeTimePeriodsForPedicure = _availbilityRepo.GetAvailableTimeWithTreatment(date, new List<int>() { 6 });

            EndConnection();

            foreach (var timeperiod in freeTimePeriodsForPedicure)
            {
                Console.WriteLine($"Available Time : {timeperiod}");
                Assert.IsTrue(timeperiod.Equals(null));
            }
        }


        //test that it can compress and give the time with the added booking applications

        //test if they are trying to request bookings from the past

        private List<DateTime> GetFreeSlots()
        {
            List<DateTime> avaliableTimeSlots = new List<DateTime>();

            TimePeriodCollection timePeriods = new TimePeriodCollection()
            {
                new TimeRange(new DateTime(2020,12,9,10,00,00), new DateTime(2020,12,9,19,00,00))
            };

            foreach (var period in timePeriods)
            {
                var startTimePeriod = period.Start;
                var endTimePeriod = period.End.Subtract(new TimeSpan(0, 60, 0));

                while (startTimePeriod < endTimePeriod)
                {
                    avaliableTimeSlots.Add(startTimePeriod);

                    startTimePeriod = startTimePeriod.AddMinutes(15);
                }
            }

            return avaliableTimeSlots;
        }

        //[Test]
        //public void GetAvaliableTimesForGivenDate()
        //{
        //    var date = new DateTime(2020, 3, 9);
        //    var expectedDate = GetFreeSlots();
        //    var freeTimePeriods = _availbilityRepo.GetAvailableTime(date);

        //    EndConnection();

        //    for(int i = 0; i < expectedDate.Count; i++)
        //    {
        //        Console.WriteLine($" Result Avaliable Time : {freeTimePeriods[i].ToShortTimeString()}");

        //        Assert.IsTrue(freeTimePeriods[i] == expectedDate[i]);
        //    }
        //}
    }
}
