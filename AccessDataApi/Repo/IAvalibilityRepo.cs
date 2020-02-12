using System;
using System.Collections.Generic;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public interface IAvalibilityRepo
    {
        public List<Employee> GetEmployeesAvaliableByDateTreatment(DateTime date, int treatmentId);
        public ITimePeriodCollection GetAvalibilityByEmployee(DateTime date, Employee employee);
        public ITimePeriodCollection GetAvaliableTime(DateTime date);
        public bool isOperating(DateTime date);
        public TimeRange CastBookingToTimeRange(BookApp booking);
        public ITimePeriodCollection GetGapsInBooking(DateTime date, ICollection<BookApp> bookings);
    }
}
