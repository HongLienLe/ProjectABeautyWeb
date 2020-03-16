using System;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IBookAppointment
    {
        public string MakeAppointment(BookAppointmentForm bookAppointmentForm);
        public string CreateAppointment(DateTime date, AppointmentDetails book);
        public AppointmentDetails UpdateApppointment(int bookAppId, AppointmentDetails updatedBooking);
        public AppointmentDetails GetAppointment(int bookAppId);
        public string DeleteAppointment(DateTime date, int bookAppId);
    }
}
