using System;
using AccessDataApi.Data;
using AccessDataApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Itenso.TimePeriod;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using System.Text;
using AccessDataApi.Functions;

namespace AccessDataApi.Repo
{
    public class BookAppointment : AppointmentService, IBookAppointment
    {
        private ApplicationContext _context;
        private IDoes _does;

        public BookAppointment(ApplicationContext context, IDoes does) : base(context)
        {
            _context = context;
            _does = does;
        }

        public string MakeAppointment(BookAppointmentForm bookAppForm)
        {
            var apps = CastToAppointmentDetails(bookAppForm);

            StringBuilder returnResponse = new StringBuilder();

            foreach (var app in apps) {
               returnResponse.AppendLine(CreateAppointment(CastTo.ChoosenDate(bookAppForm.DateTimeFormatt), app));
            };

            return returnResponse.ToString();
        }

        public string DeleteAppointment(DateTime date, int bookAppId)
        {
            if (!_does.AppIdExist(bookAppId))
                return "Booking does not exist";

            _context.Remove(_context.AppointmentDetails.Single(x => x.AppointmentDetailsId == bookAppId));
              return "Successfully Deleted";
        }

        public AppointmentDetails GetAppointment(int bookAppId)
        {
            if (!_does.AppIdExist(bookAppId))
                return null;

            return _context.AppointmentDetails.Single(x => x.AppointmentDetailsId == bookAppId);
        }

        public string UpdateApppointment(int bookAppId, BookAppointmentForm updatedBooking)
        {
            if (!_does.AppIdExist(bookAppId))
                return "Booking does not exist";

            var oldApp = _context.AppointmentDetails.Single(x => x.AppointmentDetailsId == bookAppId);
            var requestedNewApp = CastToAppointmentDetails(updatedBooking);

            var availableTime = GetFreeTimePeriodsByDateAndTreatment(
                DateTime.Parse(updatedBooking.DateTimeFormatt), updatedBooking.TreatmentIds);

            if (oldApp.DateTimeKey.date == DateTime.Parse(updatedBooking.DateTimeFormatt))
                availableTime.Remove(CastBookingToTimeRange(oldApp));
            
            foreach(var requestedapp in requestedNewApp)
            {
                if (!availableTime.IntersectsWith(CastBookingToTimeRange(requestedapp)))
                    return "Requested Time is not avaliable for one of the treatments";
            }

            _context.AppointmentDetails.Remove(oldApp);

            return MakeAppointment(updatedBooking);
        }

        public List<BookedAppointmentDetails> GetBookedAppointmentByDay(DateTime date)
        {
            if (!_does.DateTimeKeyExist(date))
                return null;

            var appForTheDay = _context.DateTimeKeys
                .Single(x => x.date == date)
                .Appointments
                .Select(x => CastTo.BookAppDetails(x)).ToList();

            return appForTheDay;
        }

        public List<BookedAppointmentDetails> GetBookAppByDateAndEmployee(DateTime date, int employeeId)
        {
            if (!_context.AppointmentDetails.Any(x => x.DateTimeKeyId == date.ToShortDateString() && x.EmployeeId == employeeId))
                return null;

            var appForTheDay = _context.AppointmentDetails
                .Where(x => x.EmployeeId == employeeId
                && x.DateTimeKeyId == date.ToShortDateString())
                .Select(x => CastTo.BookAppDetails(x)).ToList();

            return appForTheDay;
        }

        public string CreateAppointment(DateTime date, AppointmentDetails book)
        {
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
            if (!_does.DateTimeKeyExist(date))
            {
                _context.DateTimeKeys.Add(new DateTimeKey() { DateTimeKeyId = date.ToShortDateString(), date = date });
                _context.SaveChanges();
            }

            requestedBookingDetails.DateTimeKey = _context.DateTimeKeys.Single(x => x.DateTimeKeyId == date.ToShortDateString());

            _context.AppointmentDetails.Add(requestedBookingDetails);
            _context.DateTimeKeys.Single(x => x.DateTimeKeyId == requestedBookingDetails.DateTimeKeyId).Appointments.Add(requestedBookingDetails);
            _context.SaveChanges();

            return $"Booking has been successful, Order App {requestedBookingDetails.AppointmentDetailsId}";
        }

        private List<AppointmentDetails> CastToAppointmentDetails(BookAppointmentForm bkappForm)
        {
            var appointments = new List<AppointmentDetails>();

            ClientAccount client = GetClient(bkappForm.Client.ContactNumber);

            if (client == null)
            {
                client = CastTo.ClientAccount(bkappForm.Client);
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

        public Reservation CastReservation(string date, string StartTime, int treatmentId)
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
