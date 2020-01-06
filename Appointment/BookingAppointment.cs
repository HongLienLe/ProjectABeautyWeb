using System;
using System.Collections.Generic;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class BookingAppointment
    {

        internal OpeningTimes OpeningTimes;
        internal ValidationCheck ValidationCheck;
        public Dictionary<DateTime, TimePeriodCollection> Reservations = new Dictionary<DateTime, TimePeriodCollection>();

        public BookingAppointment()
        {
            OpeningTimes = new OpeningTimes();
            ValidationCheck = new ValidationCheck(OpeningTimes);
            Reservations = new Dictionary<DateTime, TimePeriodCollection>();
        }

        public TimePeriodCollection ConfirmedBookedAppointment(DateTime dateTime)
        {
            if (!Reservations.ContainsKey(dateTime.Date))
            {
                return null;
            }
            return Reservations[dateTime.Date];
        }

        public ITimePeriod BookAppointment(TimeRange app)
        {
            int indexOfBooking;
            if (!BookingValidation(app))
            {
                return null;
            }
            Reservations[app.Start.Date].Add(app);
            indexOfBooking = Reservations[app.Start.Date].IndexOf(app);
            return Reservations[app.Start.Date][indexOfBooking];
        }

        //public bool ConfirmAppExist(TimeRange app)
        //{
        //    if (!Reservations.ContainsKey(app.Start.Date))
        //    {
        //        return false;
        //    } if (!Reservations[app.Start.Date].IsSamePeriod(app)){
        //        return false;
        //    }

        //    return true;
        //}

        public bool isAvaliable(TimeRange app)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());
            var timeRange = getOpeningHoursRange(app.Start.Date);
            var avaliability = timegapcalculator.GetGaps(Reservations[app.Start.Date], timeRange);
            foreach (var gap in avaliability)
            {
                if (!gap.IntersectsWith(app))
                {
                    return true;
                }
            }
            return false;
        }

        public TimeRange getOpeningHoursRange(DateTime date)
        {
            var openingTimesForDate = OpeningTimes.OpeningHours[date.Date.DayOfWeek.ToString()];
            var firstAvaliableApp = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                openingTimesForDate.Start.Hour,
                openingTimesForDate.Start.Minute,
                openingTimesForDate.Start.Second);

            var lastAvaliableApp = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                openingTimesForDate.End.Hour,
                openingTimesForDate.End.Minute,
                openingTimesForDate.End.Second);

            return new TimeRange(firstAvaliableApp, lastAvaliableApp);
        }

        public bool BookingValidation(TimeRange app)
        {
            if (!ValidationCheck.IsDateAvaliable(app.Start.Date))
            {
                return false; //"Appointment Requested is during Closing Hours";
            }


            if (!Reservations.ContainsKey(app.Start.Date))
            {
                Reservations.Add(app.Start.Date, new TimePeriodCollection());
                return true; //"Appointment Requested is Avaliable";
            }

            if (!isAvaliable(app))
            {
                return false;
            }
            return true; //"Booking Request is Avaliable";
        }

    }
}

