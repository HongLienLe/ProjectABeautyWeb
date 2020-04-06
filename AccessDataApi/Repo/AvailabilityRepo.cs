using System;
using System.Linq;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Itenso.TimePeriod;
using AccessDataApi.Functions;

namespace AccessDataApi.Repo
{
    public class AvailabilityRepo : AppointmentService, IAvailabilityRepo
    {
        private readonly ApplicationContext _context;
        private IDoes _does;

        public AvailabilityRepo(ApplicationContext context, IDoes does) : base(context)
        {
            _context = context;
            _does = does;
        }

        public List<DateTime> GetAvailableTimeWithTreatment(DateTime date, List<int> treatmentIds)
        {
            var freeTimeperiods = GetFreeTimePeriodsByDateAndTreatment(date, treatmentIds);
            var treatmentDuration = _context.Treatments.Where(x => treatmentIds.Contains(x.TreatmentId)).Select(x => x.Duration).Sum();

            return GetFreeSlots(freeTimeperiods, treatmentDuration);
        }

        private List<DateTime> GetFreeSlots(ITimePeriodCollection timePeriods, int Duration)
        {          
            List<DateTime> avaliableTimeSlots = new List<DateTime>();

            foreach(var period in timePeriods)
            {
                var startTimePeriod = period.Start;
                var endTimePeriod = period.End.Subtract(new TimeSpan(0, Duration, 0));

                while(startTimePeriod < endTimePeriod)
                {
                    avaliableTimeSlots.Add(startTimePeriod);

                    startTimePeriod = startTimePeriod.AddMinutes(15);
                }
            }

            return avaliableTimeSlots;
        }

        public List<DateTime> GetAvailableTime(DateTime date)
        {
            if (!isOperating(date))
            {
                return null;
            }

            if (!_does.DateTimeKeyExist(date))
            {
                var timeRange = GetTimeRange(date);

                return GetFreeSlots(new TimePeriodCollection() { timeRange },30);
            }

            TimePeriodCollection availableTimeForEmployees = new TimePeriodCollection();

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == (int)date.DayOfWeek).Select(x => x.Employee);

            foreach (var employee in employees)
            {
                var employeeFreeTimeSlots = GetAvailbilityByEmployee(date, employee);

                availableTimeForEmployees.Add(employeeFreeTimeSlots);
            }

            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(availableTimeForEmployees);

            return GetFreeSlots(combinedPeriods,30);
        }
    }
}
