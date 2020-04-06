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
            if(!_context.Reservations.Any(x => x.AppointmentDetailsId == booking.AppointmentDetailsId))
                return new TimeRange(booking.Reservation.StartTime, booking.Reservation.EndTime);

            var timeSlot = _context.Reservations.Single(x => x.AppointmentDetailsId == booking.AppointmentDetailsId);
                return new TimeRange(timeSlot.StartTime, timeSlot.EndTime);
        }

        public ITimePeriodCollection GetGapsInBooking(DateTime date, ICollection<AppointmentDetails> bookings)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            var operatingHours = GetTimeRange(date);

            ITimePeriodCollection bookingPeriods = new TimePeriodCollection();

            if(bookings.Count == 0)
                return timegapcalculator.GetGaps(bookingPeriods, operatingHours);

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
            if (!_context.AppointmentDetails.Any(x => x.DateTimeKeyId == date.ToShortDateString() && x.EmployeeId == employee.EmployeeId))
                return GetGapsInBooking(date, new List<AppointmentDetails>());

            var bookApp = _context.AppointmentDetails.Where(x => x.Employee == employee && x.DateTimeKeyId == date.ToShortDateString()).ToList();
            return GetGapsInBooking(date, bookApp);
        }

        public ITimePeriodCollection GetFreeTimePeriodsByDateAndTreatment(DateTime date, List<int> treatmentIds)
        {
            TimePeriodCollection availableTimeForTreatment = new TimePeriodCollection();

            var employees = new List<Employee>();

            foreach (var treatmentId in treatmentIds)
            {
                employees.AddRange(GetWorkingEmployeesByDateAndTreatment(date, treatmentId));
            }

            foreach (var employee in employees)
            {
                var employeeFreeReservations = GetAvailbilityByEmployee(date, employee);
                availableTimeForTreatment.Add(employeeFreeReservations);
            }

            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            return periodCombiner.CombinePeriods(availableTimeForTreatment);
        }
    }
}
