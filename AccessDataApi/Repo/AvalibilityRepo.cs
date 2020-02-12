using System;
using System.Linq;
using System.Collections.Generic;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public class AvalibilityRepo : IAvalibilityRepo
    {
        private readonly ApplicationContext _context;

        public AvalibilityRepo(ApplicationContext context)
        {
            _context = context;
        }

        public List<Employee> GetEmployeesAvaliableByDateTreatment (DateTime date, int treatmentId)
        {

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == (date.Day)).Select(x => x.Employee);
            return _context.EmployeeTreatment.Where(x => x.TreatmentId == treatmentId && employees.Contains(x.Employee)).Select(x => x.Employee).ToList();
        }

        public ITimePeriodCollection GetAvalibilityByEmployee (DateTime date, Employee employee)
        {
            var bookApp = _context.BookApps.Where(x => x.Employee == employee && x.DateTimeKeyId == date.ToShortDateString()).ToList();

            return GetGapsInBooking(date, bookApp);
        }

        public ITimePeriodCollection GetAvaliableTime(DateTime date)
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

                TimePeriodCollection allAvalibleTimeForDate = new TimePeriodCollection();

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == date.Day).Select(x => x.Employee);

            foreach(var employee in employees)
            {
                var avaliableTimePeriods = GetAvalibilityByEmployee(date, employee);

                allAvalibleTimeForDate.Add(avaliableTimePeriods);
            }

            return allAvalibleTimeForDate;
        }

        public bool isOperating(DateTime date)
        {
            var day = date.Day;

                OperatingTime operatingTime = _context.OperatingTimes.Find(day);

                if (operatingTime.isOpen)
                {
                    return true;
                }

                return false;
        }
        
        public TimeRange GetTimeRange(DateTime dateTime)
        {
                var operatingHours = _context.OperatingTimes.Find(dateTime.Day);
                var range = new TimeRange(
                    dateTime.AddMinutes(operatingHours.StartTime.TotalMinutes), dateTime.AddMinutes(operatingHours.EndTime.TotalMinutes));

                return range;
        }

        public TimeRange CastBookingToTimeRange(BookApp booking)
        {
            return new TimeRange(booking.Reservation.StartTime, booking.Reservation.EndTime);
        }

        public ITimePeriodCollection GetGapsInBooking(DateTime date, ICollection<BookApp> bookings)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            var operatingHours = GetTimeRange(date);

            ITimePeriodCollection bookingPeriods = new TimePeriodCollection();

            foreach(var booking in bookings)
            {
               var timeRangeCast = CastBookingToTimeRange(booking);

                bookingPeriods.Add(timeRangeCast);
            }

            return timegapcalculator.GetGaps(bookingPeriods, operatingHours);
        }
    }
}
