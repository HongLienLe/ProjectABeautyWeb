using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Itenso.TimePeriod;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataMongoApi.Service
{
    public class AvailableAppointmentService : IAvailableAppointmentService
    {
        private IMongoCollection<Appointment> _appointments { get; set; }
        private IMongoCollection<Employee> _employees { get; set; }
        private readonly IMongoDbContext _context;
        private IOperatingHoursService _operatingHoursService;
        private ITreatmentService _treatmentService;

        public AvailableAppointmentService(IMongoDbContext context, IOperatingHoursService operatingHoursService, ITreatmentService treatmentService)
        {
            _context = context;
            _appointments = _context.GetCollection<Appointment>("Appointments");
            _employees = _context.GetCollection<Employee>("Employees");
            _operatingHoursService = operatingHoursService;
            _treatmentService = treatmentService;
        }

        public List<DateTime> GetAvailableTimeSlot(DateTime date, List<string> treatmentIds)
        {
            var day = _operatingHoursService.Get(date.DayOfWeek.ToString());
            var employees = new List<string>();
            int duration = 0;

            foreach (var id in treatmentIds)
            {
                var treatment = _treatmentService.Get(id);
                if (treatment == null)
                    return null;
                employees.AddRange(EmployeeWorkingIds(treatment, day));
                duration = duration + treatment.About.Duration;
            }

            var freeslots = GetEmployeesFreeTimeSlot(employees, date);
            return AppFunction.GetFreeSlots(freeslots, duration);
        }

        private ITimePeriodCollection GetEmployeesFreeTimeSlot(List<string> employees, DateTime date)
        {
            TimePeriodCollection allFreeTimeSlotForTreatment = new TimePeriodCollection();
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());
            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();

            var openingHours = DateTimeRange(date);

            foreach (var employee in employees)
            {
                var bookedAppTimePeriods = new TimePeriodCollection();
                var dateId = date.ToString("u").Substring(0, 10);
                var appointments = _appointments.AsQueryable<Appointment>().Where(x => x.EmployeeId == employee && x.Info.Date.Contains(dateId)).ToList();
                var bookingPeriods = appointments.Select(x => AppFunction.CastBookingToTimeRange(x));

                bookedAppTimePeriods.AddAll(bookingPeriods);
                 
                if (bookedAppTimePeriods == null)
                    bookedAppTimePeriods.AddAll(TimeGapWithNoAppointments(date));

                var freeperiods = timegapcalculator.GetGaps(bookedAppTimePeriods, openingHours);
                allFreeTimeSlotForTreatment.AddAll(freeperiods);
            }

            return periodCombiner.CombinePeriods(allFreeTimeSlotForTreatment);
        }

        private List<string> EmployeeWorkingIds(Treatment treatment, OperatingHours day)
        {
            var treatmentName = $"{treatment.About.TreatmentType} {treatment.About.TreatmentName}"; 
            var skill = new TreatmentSkills(){ TreatmentId = treatment.ID, TreatmentName = treatmentName };
            var workday = new WorkDay() { OperatingHoursId = day.ID, Day = day.About.Day };

            return _employees.AsQueryable<Employee>()
                .Where(x => x.WorkDays.Contains(workday) && x.Treatments.Contains(skill))
                .Select(x => x.ID)
                .ToList();
        }

        private TimeRange DateTimeRange(DateTime date)
        {
            var openingHours = _operatingHoursService.Get(date.DayOfWeek.ToString());
            return new TimeRange(
                date.Add(TimeSpan.Parse(openingHours.About.OpeningHr)),
                date.Add(TimeSpan.Parse(openingHours.About.ClosingHr)));
        }

        private ITimePeriodCollection TimeGapWithNoAppointments(DateTime date)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());
            var day = _operatingHoursService.Get(date.DayOfWeek.ToString());
            var openingTimeSpan = TimeSpan.Parse(day.About.OpeningHr);
            var closingTimeSpan = TimeSpan.Parse(day.About.ClosingHr);
            var range = new TimeRange(date.Add(openingTimeSpan), date.Add(closingTimeSpan));

            return timegapcalculator.GetGaps(new TimePeriodCollection(), range);
        }
    }
}

public static class AppFunction
{
    public static List<DateTime> GetFreeSlots(ITimePeriodCollection timePeriods, int Duration)
    {
        List<DateTime> avaliableTimeSlots = new List<DateTime>();

        foreach (var period in timePeriods)
        {
            var startTimePeriod = period.Start;
            var endTimePeriod = period.End.Subtract(new TimeSpan(0, Duration, 0));

            while (startTimePeriod < endTimePeriod)
            {
                avaliableTimeSlots.Add(startTimePeriod);
                startTimePeriod = startTimePeriod.AddMinutes(15);
            }
        }
        return avaliableTimeSlots;
    }

    public static TimeRange CastBookingToTimeRange(Appointment booking)
    {
        return new TimeRange(DateTime.Parse(booking.Info.Date).Add(TimeSpan.Parse(booking.Info.StartTime)), DateTime.Parse(booking.Info.Date).Add(TimeSpan.Parse(booking.Info.EndTime)));
    }
}

