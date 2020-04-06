using System;
using System.Collections.Generic;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public interface IAppointmentService
    {
        public bool isOperating(DateTime date);
        public TimeRange GetTimeRange(DateTime dateTime);
        public TimeRange CastBookingToTimeRange(AppointmentDetails booking);
        public ITimePeriodCollection GetGapsInBooking(DateTime date, ICollection<AppointmentDetails> bookings);
        public List<Employee> GetWorkingEmployeesByDateAndTreatment(DateTime date, int treatmentIds);
        public ITimePeriodCollection GetAvailbilityByEmployee(DateTime date, Employee employee);
        public ITimePeriodCollection GetFreeTimePeriodsByDateAndTreatment(DateTime date, List<int> treatmentIds);



    }
}
