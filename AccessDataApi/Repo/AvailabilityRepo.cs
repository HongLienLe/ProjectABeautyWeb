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

        public List<DateTime> GetAvailableTime(DateTime date)
        {
            if (!isOperating(date))
            {
                return null;
            }

                if (!_context.DateTimeKeys.Any(x => x.DateTimeKeyId == date.ToShortDateString()))
                {
                    var timeRange = GetTimeRange(date);

                    return GetFreeSlots( new TimePeriodCollection() { timeRange });
                }

            TimePeriodCollection availableTimeForEmployees = new TimePeriodCollection();

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == (int)date.DayOfWeek).Select(x => x.Employee);

            foreach(var employee in employees)
            {
                var employeeFreeTimeSlots = GetAvailbilityByEmployee(date, employee);

                availableTimeForEmployees.Add(employeeFreeTimeSlots);
            }

            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(availableTimeForEmployees);

            return GetFreeSlots(combinedPeriods);
        }

        public List<DateTime> GetAvailableTimeWithTreatment(DateTime date, int treatmentId)
        {
            TimePeriodCollection availableTimeForTreatment = new TimePeriodCollection();

            var employees = GetWorkingEmployeesByDateAndTreatment(date, treatmentId);
            foreach(var employee in employees)
            {
                var employeeFreeReservations = GetAvailbilityByEmployee(date, employee);
                availableTimeForTreatment.Add(employeeFreeReservations);
            }

            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(availableTimeForTreatment);

            return GetFreeSlots(combinedPeriods);
        }

        private List<DateTime> GetFreeSlots(ITimePeriodCollection timePeriods)
        {          
            List<DateTime> avaliableTimeSlots = new List<DateTime>();

            foreach(var period in timePeriods)
            {
                var startTimePeriod = period.Start;
                var endTimePeriod = period.End.Subtract(new TimeSpan(0,30,0));

                while(startTimePeriod < endTimePeriod)
                {
                    avaliableTimeSlots.Add(startTimePeriod);

                    startTimePeriod = startTimePeriod.AddMinutes(15);
                }
            }

            return avaliableTimeSlots;
        }
    }
}
