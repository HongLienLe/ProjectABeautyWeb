using System;
using AccessDataApi.Data;
using AccessDataApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class BookAppRepo : IBookAppRepo
    {
        private ApplicationContext _context;
        private IAvalibilityRepo _avalibilityRepo;

        public BookAppRepo(ApplicationContext context, IAvalibilityRepo avalibility)
        {
            _context = context;
            _avalibilityRepo = avalibility;

        }

        public string CreateAppointment(DateTime date,BookApp book)
        {

            //check if time is avaliable via employee // if not that then check for all employees and assign

            var bookAppForDate = _context.BookApps.Where(x => x.DateTimeKeyId == book.DateTimeKeyId && x.EmployeeId == book.EmployeeId).ToList();

            var bookAppPeriods = _avalibilityRepo.GetGapsInBooking(date, bookAppForDate);

            var timeRangeApp = _avalibilityRepo.CastBookingToTimeRange(book);

            if (bookAppPeriods.IntersectsWith(timeRangeApp))
            {
                return "Time slot is not avaliable";
            }

            _context.BookApps.Add(book);

            _context.SaveChanges();

            return "Booking has been successfull";
        }

        public void DeleteAppointment(DateTime date, int bookAppId)
        {
            BookApp bookApp = new BookApp() { BookAppId = bookAppId };

            _context.Entry(bookApp).State = EntityState.Deleted;
        }

        public BookApp GetAppointment(int bookAppId)
        {
            return _context.BookApps.First(x => x.BookAppId == bookAppId);
        }

        public string UpdateApppointment(int bookAppId, BookApp updatedBooking)
        {
            return "nothing";
        }

        public bool bookAppValidation(BookApp bookApp)
        {
            var treatmentExist = _context.Treatments.Any(x => x.TreatmentId == bookApp.TreatmentId);

            var doesEmployeeExist = _context.Employees.Any(x => x.EmployeeId == bookApp.EmployeeId);

            var doesEmployeeProvideTreatment = _context.EmployeeTreatment.Any(x => x.EmployeeId == bookApp.EmployeeId && x.TreatmentId == bookApp.TreatmentId);

            var isEmployeeAvaliable = _avalibilityRepo.GetAvalibilityByEmployee(bookApp.DateTimeKey.date, bookApp.Employee);

            return true;
        }
    }
}
