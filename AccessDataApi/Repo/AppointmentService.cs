using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public class AppointmentService : IAppointmentService
    {
        private ApplicationContext _context;

        public AppointmentService(ApplicationContext context)
        {
            _context = context;
        }

        public bool isOperating(DateTime date)
        {
            return _context.OperatingTimes.Any(x => x.Id == (int)date.DayOfWeek && x.isOpen == true);
        }

        public TimeRange GetTimeRange(DateTime dateTime)
        {
            var operatingHours = _context.OperatingTimes.Find((int)dateTime.DayOfWeek);
            var range = new TimeRange(
                dateTime.AddMinutes(operatingHours.StartTime.TotalMinutes), dateTime.AddMinutes(operatingHours.EndTime.TotalMinutes));

            return range;
        }

        public TimeRange CastBookingToTimeRange(AppointmentDetails booking)
        {
            return new TimeRange(booking.Reservation.StartTime, booking.Reservation.EndTime);
        }

        public ITimePeriodCollection GetGapsInBooking(DateTime date, ICollection<AppointmentDetails> bookings)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            var operatingHours = GetTimeRange(date);

            ITimePeriodCollection bookingPeriods = new TimePeriodCollection();

            foreach (var booking in bookings)
            {
                var timeRangeCast = CastBookingToTimeRange(booking);

                bookingPeriods.Add(timeRangeCast);
            }

            return timegapcalculator.GetGaps(bookingPeriods, operatingHours);
        }

        public List<Employee> GetWorkingEmployeesByDateAndTreatment(DateTime date, int treatmentId)
        {
            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == ((int)date.DayOfWeek)).Select(x => x.EmployeeId);
            return _context.EmployeeTreatment.Where(x => x.TreatmentId == treatmentId && employees.Contains(x.EmployeeId)).Select(x => x.Employee).ToList();
        }

        public ITimePeriodCollection GetAvailbilityByEmployee(DateTime date, Employee employee)
        {
            var bookApp = _context.AppointmentDetails.Where(x => x.Employee == employee && x.DateTimeKeyId == date.ToShortDateString()).ToList();
            return GetGapsInBooking(date, bookApp);
        }
    }
}
