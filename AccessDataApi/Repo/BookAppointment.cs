using System;
using AccessDataApi.Data;
using AccessDataApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Itenso.TimePeriod;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using System.Text;

namespace AccessDataApi.Repo
{
    public class BookAppointment : AppointmentService, IBookAppointment
    {
        private ApplicationContext _context;

        public BookAppointment(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public string MakeAppointment(BookAppointmentForm bookAppointmentForm)
        {
            var apps = CastToAppointmentDetails(bookAppointmentForm);
            var choosenDate = DateTime.Parse(bookAppointmentForm.DateTimeFormatt);

            StringBuilder returnResponse = new StringBuilder();

            foreach (var app in apps) {
               returnResponse.AppendLine( CreateAppointment(choosenDate, app));
            };

            return returnResponse.ToString();
        }

        public string DeleteAppointment(DateTime date, int bookAppId)
        {
            if (!_context.AppointmentDetails.Any(x => x.AppointmentDetailsId == bookAppId))
                return null;

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

            //var isAvailable = get

            //return null

            //update booking

            return GetAppointment(bookAppId);
        }

        public List<BookedAppointmentDetails> GetBookedAppointmentByDay(DateTime date)
        {
            if (_context.DateTimeKeys.Any(x => x.DateTimeKeyId == date.ToShortDateString()))
                return null;

            var appForTheDay = _context.DateTimeKeys.Single(x => x.DateTimeKeyId == date.ToShortDateString()).Appointments.ToList();

            var castedApp = new List<BookedAppointmentDetails>();
            foreach(var app in appForTheDay)
            {
                castedApp.Add(new BookedAppointmentDetails()
                {
                    Id = app.AppointmentDetailsId,
                    StartTime = app.Reservation.StartTime,
                    EndTime = app.Reservation.EndTime,
                    EmployeeId = app.EmployeeId,
                    TreatmentName = app.Treatment.TreatmentName,
                    FirstName = app.ClientAccount.FirstName,
                    LastName = app.ClientAccount.LastName,
                    Email = app.ClientAccount.Email,
                    ContactNumber = app.ClientAccount.ContactNumber
                });
            }

            return castedApp;
        }

        public List<AppointmentDetails> GetBookAppByDateAndEmployee(DateTime date, int employeeId)
        {
            if (!_context.AppointmentDetails.Any(x => x.DateTimeKeyId == date.ToShortDateString() && x.EmployeeId == employeeId))
                return null;

            return _context.AppointmentDetails
                .Where(x => x.EmployeeId == employeeId
                && x.DateTimeKeyId == date.ToShortDateString()).ToList();
        }


        public string CreateAppointment(DateTime date, AppointmentDetails book)
        {
            //is operating?
            if (!isOperating(date))
                return "Business is closed on requested date";

            var requestedBookingDetails = book;
            var reservationTimePeriod = CastBookingToTimeRange(requestedBookingDetails);


            if (requestedBookingDetails.EmployeeId == 0)
            {
                var employees = GetWorkingEmployeesByDateAndTreatment(date, requestedBookingDetails.TreatmentId);

                foreach (var employee in employees)
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

            return $"Booking has been successful, Order App {requestedBookingDetails.AppointmentDetailsId}";
        }

        private bool bookAppValidation(AppointmentDetails bookApp)
        {
            var treatmentExist = _context.Treatments.Any(x => x.TreatmentId == bookApp.TreatmentId);

            var doesEmployeeExist = _context.Employees.Any(x => x.EmployeeId == bookApp.EmployeeId);

            var doesEmployeeProvideTreatment = _context.EmployeeTreatment.Any(x => x.EmployeeId == bookApp.EmployeeId && x.TreatmentId == bookApp.TreatmentId);

            var isEmployeeAvaliable = GetAvailbilityByEmployee(bookApp.DateTimeKey.date, bookApp.Employee);

            return true;
        }

        private List<AppointmentDetails> CastToAppointmentDetails(BookAppointmentForm bkappForm)
        {
            var appointments = new List<AppointmentDetails>();

            ClientAccount client = GetClient(bkappForm.ContactNumber);

            if (client == null)
            {
                client = new ClientAccount()
                {
                    FirstName = bkappForm.FirstName,
                    LastName = bkappForm.LastName,
                    Email = bkappForm.Email,
                    ContactNumber = bkappForm.ContactNumber
                };
            }

            foreach(var treatmentId in bkappForm.TreatmentIds)
            {
                appointments.Add(new AppointmentDetails()
                {
                    ClientAccount = client,
                    DateTimeKeyId = bkappForm.DateTimeFormatt,
                    EmployeeId = 0,
                    TreatmentId = treatmentId,
                    Reservation = CastReservation(bkappForm.DateTimeFormatt, bkappForm.StartTime, treatmentId),
                });
            }


            return appointments;

        }

        private Reservation CastReservation(string date, string StartTime, int treatmentId)
        {
            var treatmentDuration = _context.Treatments.Single(x => x.TreatmentId == treatmentId).Duration;
            var toDateTimeStartTime = DateTime.Parse(date).Add(TimeSpan.Parse(StartTime));
            var reservation = new Reservation()
            {
                StartTime = toDateTimeStartTime,
                EndTime = toDateTimeStartTime.AddMinutes(treatmentDuration),
            };

            return reservation;
        }

        private ClientAccount GetClient(string contactNo)
        {
            if (_context.ClientAccounts.Any(x => x.ContactNumber == contactNo))
                return _context.ClientAccounts.Single(x => x.ContactNumber == contactNo);

            return null;
        }
    }
}
