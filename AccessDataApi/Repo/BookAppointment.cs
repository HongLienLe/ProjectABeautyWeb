using System;
using AccessDataApi.Data;
using AccessDataApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Itenso.TimePeriod;
using System.Collections.Generic;

namespace AccessDataApi.Repo
{
    public class BookAppointment : AppointmentService, IBookAppointment
    {
        private ApplicationContext _context;

        public BookAppointment(ApplicationContext context) : base(context)
        {
            _context = context;

        }

        public string CreateAppointment(DateTime date,AppointmentDetails book)
        {
            //is operating?
            if (!isOperating(date))
                return "Business is closed on requested date";

            var requestedBookingDetails = book;
            var reservationTimePeriod = CastBookingToTimeRange(requestedBookingDetails);


            if (requestedBookingDetails.EmployeeId == 0)
            {
                var employees = GetWorkingEmployeesByDateAndTreatment(date, requestedBookingDetails.TreatmentId);

                foreach(var employee in employees)
                {
                    var employeeAvailableTime = GetAvailbilityByEmployee(date, employee);

                    if (employeeAvailableTime.IntersectsWith(reservationTimePeriod) && employeeAvailableTime.Count != 0)
                    {
                        requestedBookingDetails.Employee = employee;
                        break;
                    }
                }
            }

            if (requestedBookingDetails.Employee == null)
                return "Time Slots for choosen treatment is not avaliable";

            //If Booking is not there yet then create it
            if (!_context.DateTimeKeys.Any(x => x.DateTimeKeyId == date.ToShortDateString()))
            {
                _context.DateTimeKeys.Add(new DateTimeKey() { DateTimeKeyId = date.ToShortDateString(), date = date });
                _context.SaveChanges();
            }

            requestedBookingDetails.DateTimeKey = _context.DateTimeKeys.Single(x => x.DateTimeKeyId == date.ToShortDateString());

            _context.AppointmentDetails.Add(requestedBookingDetails);

            _context.SaveChanges();

            return "Booking has been successful";
        }

        public string DeleteAppointment(DateTime date, int bookAppId)
        {
            if (!_context.AppointmentDetails.Any(x => x.AppointmentDetailsId == bookAppId))
                return "Does not exist";

            _context.Remove(_context.AppointmentDetails.Single(x => x.AppointmentDetailsId == bookAppId && x.DateTimeKeyId == date.ToShortDateString()));

            return "Successfully Deleted";
        }

        public AppointmentDetails GetAppointment(int bookAppId)
        {
            if (!_context.AppointmentDetails.Any(x => x.AppointmentDetailsId == bookAppId))
                return null;

            return _context.AppointmentDetails.Single(x => x.AppointmentDetailsId == bookAppId);
        }

        public AppointmentDetails UpdateApppointment(int bookAppId, AppointmentDetails updatedBooking)
        {
            if (!_context.AppointmentDetails.Any(x => x.AppointmentDetailsId == bookAppId))
                return null;

            //get availability

          //  var isAvailable = get

            //return null

            //update booking

            return GetAppointment(bookAppId);
        }

        public bool bookAppValidation(AppointmentDetails bookApp)
        {
            var treatmentExist = _context.Treatments.Any(x => x.TreatmentId == bookApp.TreatmentId);

            var doesEmployeeExist = _context.Employees.Any(x => x.EmployeeId == bookApp.EmployeeId);

            var doesEmployeeProvideTreatment = _context.EmployeeTreatment.Any(x => x.EmployeeId == bookApp.EmployeeId && x.TreatmentId == bookApp.TreatmentId);

            var isEmployeeAvaliable = GetAvailbilityByEmployee(bookApp.DateTimeKey.date, bookApp.Employee);

            return true;
        }
    }
}
