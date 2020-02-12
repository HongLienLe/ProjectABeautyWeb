using System;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IBookAppRepo
    {
        public string CreateAppointment(DateTime date, BookApp book);
        public string UpdateApppointment(int bookAppId, BookApp updatedBooking);
        public BookApp GetAppointment(int bookAppId);
        public void DeleteAppointment(DateTime date, int bookAppId);
    }
}
