﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Itenso.TimePeriod;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataMongoApi.Service
{
    public class AvailableAppointmentService
    {
        private IMongoCollection<Appointment> _appointments { get; set; }
        private IMongoCollection<Employee> _employees { get; set; }

        private IOperatingHoursService _operatingHoursService;
        private ITreatmentService _treatmentService;

        public AvailableAppointmentService(ISalonDatabaseSettings settings, IOperatingHoursService operatingHoursService, ITreatmentService treatmentService, IClientConfiguration clientConfiguration)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId);
            _appointments = database.GetCollection<Appointment>("Appointments");
            _employees = database.GetCollection<Employee>("Employees");
            _operatingHoursService = operatingHoursService;
            _treatmentService = treatmentService;
        }

        public List<DateTime> GetAvailableTimeSlot(DateTime date, string treatmentId)
        {
            var employees = EmployeeWorkingIds(treatmentId, date);
            var treatment = _treatmentService.Get(treatmentId);
            var freeslots = GetEmployeesFreeTimeSlot(employees, date);

            return AppFunction.GetFreeSlots(freeslots, treatment.About.Duration);
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
                var appointments = _appointments.Find(x => x.EmployeeId == employee && x.Info.Date == date.ToShortDateString()).ToList();

                bookedAppTimePeriods.AddAll(appointments.Select(x => AppFunction.CastBookingToTimeRange(x)));

                if (bookedAppTimePeriods == null)
                    bookedAppTimePeriods.AddAll(TimeGapWithNoAppointments(date));

                allFreeTimeSlotForTreatment.Add(timegapcalculator.GetGaps(bookedAppTimePeriods, openingHours));
            }

            return periodCombiner.CombinePeriods(allFreeTimeSlotForTreatment);
        }

        private List<string> EmployeeWorkingIds(string treatmentId, DateTime date)
        {
            var dayId = _operatingHoursService.Get(date.DayOfWeek.ToString()).ID;
            return _employees.AsQueryable<Employee>()
                .Where(x => x.WorkDays.Contains(dayId) && x.Treatments.Contains(treatmentId))
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
        return new TimeRange(booking.Info.StartTime, booking.Info.EndTime);
    }


}
