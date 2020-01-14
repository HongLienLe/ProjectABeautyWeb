using System;
using System.Collections.Generic;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class AvaliableAppointment
    {
        internal ValidationCheck ValidationCheck;
        public Data Reservation;
        

        public AvaliableAppointment( ValidationCheck validationCheck)
        {   
            ValidationCheck = validationCheck;
            Reservation = Data.Instance;
        }

        public TimePeriodCollection AvaliableDatesForMonth(int year, int month)
        {
            var openingDates = new TimePeriodCollection( new Month(year, (YearMonth)month).GetDays());

            for (int i = 0; i < openingDates.Count; i++)
            {
                openingDates[i] = GetTimeRange(openingDates[i].Start);

                if (!ValidationCheck.IsDateAvaliable(openingDates[i].Start))
                {
                    openingDates.RemoveAt(i);
                }
            }

            return openingDates;
        }

        public ITimePeriodCollection AvaliableAppDate(DateTime date)
        {
            if (!ValidationCheck.IsDateAvaliable(date))
            {
                return null;
            }

            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            var workingHoursForDay = GetTimeRange(date);

            if (!Reservation.getAllReservations().ContainsKey(date.Date))
            {
                return timegapcalculator.GetGaps(new TimePeriodCollection(), workingHoursForDay);
            }

            return timegapcalculator.GetGaps(Reservation.getAllReservations()[date.Date], workingHoursForDay);
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