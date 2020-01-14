using System;
using System.Collections.Generic;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class ValidationCheck
    {
        Data workHours;

        public ValidationCheck()
        {
            workHours = Data.Instance;
        }

        public bool IsDateAvaliable(DateTime date)
        {
            var choosenDay = date.DayOfWeek.ToString();
            if (workHours.getWorkHours()[choosenDay].End.IsZero && workHours.getWorkHours()[choosenDay].Start.IsZero)
            {
                return false;
            }

            return true;
        }

        public bool IsDateAvaliable(Dictionary<string, HourRange> employeeWorkHours,DateTime date)
        {
            var choosenDay = date.DayOfWeek.ToString();
            if (employeeWorkHours[choosenDay].End.IsZero && employeeWorkHours[choosenDay].Start.IsZero)
            {
                return false;
            }

            return true;
        }

        public bool isTimeAvaliable(ITimePeriodCollection reservationKey, ITimePeriod reservation)
        {
            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());
            var avaliability = timegapcalculator.GetGaps(reservationKey, GetTimeRange(reservation.Start.Date));
            foreach (var gap in avaliability)
            {
                if (!gap.IntersectsWith(reservation))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isTimeAvaliableByEmployee(ITimePeriodCollection employeeAvaliableTimeSlot, ITimePeriod reservation)
        {
            foreach(var freeSlot in employeeAvaliableTimeSlot)
            {
                if (freeSlot.HasInside(reservation))
                {
                    return true;
                }
            }
            return false;
        }

        public TimeRange GetTimeRange(DateTime dateTime)
        {
            var hourRange = workHours.getWorkHours()[dateTime.DayOfWeek.ToString()];
            var range = new TimeSpan(
                hourRange.End.Hour - hourRange.Start.Hour,
                hourRange.End.Minute - hourRange.Start.Minute,
                hourRange.End.Second - hourRange.Start.Second);

            return new TimeRange(hourRange.Start.ToDateTime(dateTime.Date), range);
        }
    }
}
