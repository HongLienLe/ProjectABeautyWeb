using System;
using System.Linq;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public class AvailabilityRepo : AppointmentService, IAvailabilityRepo
    {
        private readonly ApplicationContext _context;

        public AvailabilityRepo(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public ITimePeriodCollection GetAvailableTime(DateTime date)
        {
            if (!isOperating(date))
            {
                return null;
            }

            var hasBookings = _context.DateTimeKeys.Any(x => x.DateTimeKeyId == date.ToShortDateString());

                if (!hasBookings)
                {
                    var timeRange = GetTimeRange(date);

                    return new TimePeriodCollection() { timeRange };
                }

                TimePeriodCollection allAvailbleTimeForDate = new TimePeriodCollection();

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == date.Day).Select(x => x.Employee);

            foreach(var employee in employees)
            {
                var availableTimePeriods = GetAvailbilityByEmployee(date, employee);

                allAvailbleTimeForDate.Add(availableTimePeriods);
            }

            return allAvailbleTimeForDate;
        }

        public ITimePeriodCollection GetAvailableTimeWithTreatment(DateTime date, int treatmentId)
        {
            TimePeriodCollection availableTimeForTreatment = new TimePeriodCollection();

            var employees = GetWorkingEmployeesByDateAndTreatment(date, treatmentId);
            foreach(var employee in employees)
            {
                var employeeFreeReservations = GetAvailbilityByEmployee(date, employee);
                availableTimeForTreatment.Add(employeeFreeReservations);
            }

            return availableTimeForTreatment;
        }

    }
}
