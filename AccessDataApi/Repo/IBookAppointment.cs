using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IBookAppointment
    {
        public string MakeAppointment(BookAppointmentForm bookAppointmentForm);
        public AppointmentDetails UpdateApppointment(int bookAppId, AppointmentDetails updatedBooking);
        public AppointmentDetails GetAppointment(int bookAppId);
        public string DeleteAppointment(DateTime date, int bookAppId);
        public List<BookedAppointmentDetails> GetBookedAppointmentByDay(DateTime date);
    }
}
