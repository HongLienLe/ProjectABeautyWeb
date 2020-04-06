using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using NUnit.Framework;

namespace AcessDataAPITest.RepoTest
{
    public class WorkScheduleTest : BaseTest
    {
        private WorkScheduleRepo _workScheduleRepo;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = new ConnectionFactory();
            _context = _connectionFactory.CreateContextForSQLite();
            _context.Employees.AddRange(GetEmployees());
            _context.OperatingTimes.AddRange(GetOpeningTimes());
            _context.SaveChanges();
            _context.workSchedules.AddRange(GetOperatingTimeEmployees());
            _context.SaveChanges();
        }

        [Test]
        public void GetEmployee1WorkDays()
        {
            _workScheduleRepo = new WorkScheduleRepo(_context);

            var resultEmployeeWorkDays = _workScheduleRepo.GetEmployeeWorkSchedule(1);
            var expectedDayId = new int[] { 1,2,3,4,5,6};

            EndConnection();

            Assert.IsTrue(resultEmployeeWorkDays.Count == expectedDayId.Length);
            Assert.IsTrue(resultEmployeeWorkDays.All(x => expectedDayId.Contains(x.Id)));
        }

        [Test]
        public void GetEmployeeByWorkDay6()
        {
            var resultEmployeeWorkingOnDay6 = _workScheduleRepo.GetEmployeeByWorkDay(6);
            var expectedEmployeeId = new int[] { 1, 2, 3 };

            EndConnection();

            Assert.IsTrue(resultEmployeeWorkingOnDay6.Count == expectedEmployeeId.Length);
            Assert.IsTrue(resultEmployeeWorkingOnDay6.All(x => expectedEmployeeId.Contains(x.EmployeeId)));

        }

        [Test]
        public void UpdatedEmployeeWorkDays()
        {
            WorkScheduleModel wsm = new WorkScheduleModel()
            {
                Id = 3,
                Ids = new List<int>() {1, 2, 3, 4, 5,6 },
                isEmployee = true
            };

           _workScheduleRepo.addWorkSchedule(wsm);

            var updatedEmployee3WorkDays = _workScheduleRepo.GetEmployeeWorkSchedule(3);

            var expectedDayId = new int[] { 1, 2, 3, 4, 5, 6 };

            EndConnection();

            Assert.IsTrue(updatedEmployee3WorkDays.Count == expectedDayId.Length);
            Assert.IsTrue(updatedEmployee3WorkDays.All(x => expectedDayId.Contains(x.Id)));

        }

        [Test]
        public void UpdatedWorkDayAddEmployees()
        {
            _workScheduleRepo = new WorkScheduleRepo(_context);

            WorkScheduleModel wsm = new WorkScheduleModel()
            {
                Id = 1,
                Ids = new List<int>() { 1, 2, 3 },
                isEmployee = false
            };

            _workScheduleRepo.addWorkSchedule(wsm);

            var updatedEmployee3ForWorkDay1 = _workScheduleRepo.GetEmployeeByWorkDay(1);

            var expectedEmployeeId = new int[] { 1, 2, 3 };

            EndConnection();

            Assert.IsTrue(updatedEmployee3ForWorkDay1.Count == expectedEmployeeId.Length);
            Assert.IsTrue(updatedEmployee3ForWorkDay1.All(x => expectedEmployeeId.Contains(x.EmployeeId)));

        }

    }
}
