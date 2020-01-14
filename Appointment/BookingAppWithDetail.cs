using System;
using System.Collections.Generic;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class BookingAppWithDetail
    {
        ValidationCheck ValidationCheck;
        EmployeeAvalibility employeeAvaliableApp;
        AvaliableAppointment avaliableApp;
        Data Reservation;

        public BookingAppWithDetail()
        {
            Reservation = Data.Instance;
            ValidationCheck = new ValidationCheck();
            avaliableApp = new AvaliableAppointment(ValidationCheck);
            employeeAvaliableApp = new EmployeeAvalibility(avaliableApp);
        }

        public BookAppointmentDetail BookTreatment(BookAppointmentDetail bookingDetails)
        {
            var requestedBooking = bookingDetails;
            //Employee Avaliable Working that Day? && provide that treatment 
            if (!employeeAvaliableApp.isEmployeeWorking(requestedBooking.EmployeeId, requestedBooking.DateTimeId) || !employeeAvaliableApp.doesEmployeeProvideTreatment(requestedBooking.EmployeeId, requestedBooking.TreatmentId))
            {
                return null;
            }

            //Any booking for the day?
            if (!Reservation.getAllReservations().ContainsKey(requestedBooking.DateTimeId))
            {
                Reservation.getAllReservations().Add(requestedBooking.DateTimeId, new TimePeriodCollection() { requestedBooking.Reservation });
                Reservation.getAllConfirmedAppointments().Add(requestedBooking.DateTimeId, new List<BookAppointmentDetail>() { requestedBooking });

                return GetBookAppointmentDetail(requestedBooking.DateTimeId, requestedBooking.Id);
            }

            var employeeAvaliableTimeSlot = employeeAvaliableApp.EmployeeAvalibilityTimeSlots(requestedBooking.EmployeeId, requestedBooking.Reservation);

            //Checking Employee Diary
            if (!ValidationCheck.isTimeAvaliableByEmployee(employeeAvaliableTimeSlot, requestedBooking.Reservation))
            {
                return null;
            }


            //BookingApp
            Reservation.getAllReservations()[requestedBooking.DateTimeId].Add(requestedBooking.Reservation);
            Reservation.getAllConfirmedAppointments()[requestedBooking.DateTimeId].Add(requestedBooking);

            var detailsOfBookedReservation = GetBookAppointmentDetail(requestedBooking.DateTimeId, requestedBooking.Id);

            return detailsOfBookedReservation;
        }

        public BookAppointmentDetail GetBookAppointmentDetail(DateTime date, int appId)
        {
            if (!Reservation.getAllConfirmedAppointments().ContainsKey(date))
            {
                return null;
            }

            foreach (var booking in Reservation.getAllConfirmedAppointments()[date])
            {
                if (booking.Id == appId)
                {
                    return booking;
                }

            }
            return null;
        }

        public BookAppointmentDetail UpdateExistingBooking(DateTime date, int appId, BookAppointmentDetail bookAppointmentDetail)
        {
            var requestedApp = GetBookAppointmentDetail(date.Date, appId);

            if (requestedApp == null)
            {
                return null;
            }

            var requestedTimeSlot = GetTimeSlotDetails(requestedApp.DateTimeId, requestedApp.Id);

            var newBooking = BookTreatment(bookAppointmentDetail);

            if (newBooking == null)
            {
                return null;
            }

            Reservation.getAllReservations()[bookAppointmentDetail.DateTimeId].Remove(requestedTimeSlot);
            Reservation.getAllConfirmedAppointments()[bookAppointmentDetail.DateTimeId].Remove(requestedApp);

            return newBooking;
        }

        public void DeleteExistingBooking(DateTime date, int appId)
        {
            var requestedApp = GetBookAppointmentDetail(date, appId);
            if (requestedApp == null)
                return;
            var takenTimeSlot = Reservation.getAllReservations()[date].IndexOf(requestedApp.Reservation);
            Reservation.getAllReservations()[date].RemoveAt(takenTimeSlot);
            Reservation.getAllConfirmedAppointments()[date].Remove(requestedApp);
        }
        public ITimePeriod GetTimeSlotDetails(DateTime date, int appId)
        {
            var requestedTimeSlot = GetBookAppointmentDetail(date, appId);

            if (requestedTimeSlot == null)
            {
                return null;
            }

            var takenTimeSlot = Reservation.getAllReservations()[date].IndexOf(requestedTimeSlot.Reservation);

            return Reservation.getAllReservations()[requestedTimeSlot.DateTimeId][takenTimeSlot];

        }

        public TimeRange GetTimeRange(DateTime dateTime)
        {
            var hourRange = Reservation.getWorkHours()[dateTime.DayOfWeek.ToString()];
            var range = new TimeSpan(
                hourRange.End.Hour - hourRange.Start.Hour,
                hourRange.End.Minute - hourRange.Start.Minute,
                hourRange.End.Second - hourRange.Start.Second);

            return new TimeRange(hourRange.Start.ToDateTime(dateTime.Date), range);
        }
    }
}
