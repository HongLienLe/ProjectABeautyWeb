using System;
using System.Collections.Generic;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class AvaliableAppointment
    {
        internal OpeningTimes OpeningTimes;
        internal ValidationCheck ValidationCheck;
        public Dictionary<DateTime, TimePeriodCollection> Reservations;


        public AvaliableAppointment()
        {
            OpeningTimes = new OpeningTimes();
            ValidationCheck = new ValidationCheck(OpeningTimes);
            Reservations = new Dictionary<DateTime, TimePeriodCollection>()
            { {new DateTime(2020, 1, 1), new TimePeriodCollection() } };
        }

        //Get All Avaliable Dates for the month
        public TimePeriodCollection AvaliableDatesForMonth(int month)
        {
            var openingDates = new TimePeriodCollection( new Month(2020, (YearMonth)month).GetDays());

            for (int i = 0; i < openingDates.Count; i++)
            {
                if (!ValidationCheck.IsDateAvaliable(openingDates[i].Start))
                {
                    openingDates.RemoveAt(i);
                }
            }

            return openingDates;
        }

        //Get All Avaliable pointments for the given date
        public TimePeriodCollection AvaliableAppDate(DateTime app)
        {

            var appDate = app.Date;

            if (!ValidationCheck.IsDateAvaliable(appDate))
            {
                return null;
            }

            if (!Reservations.ContainsKey(appDate))
            {
                Reservations.Add(appDate, new TimePeriodCollection());
            }

            var timeRange = getOpeningHoursRange(app);

            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            var avaliability = timegapcalculator.GetGaps(Reservations[appDate], timeRange);

            var allTimeGapsAvaliable = new TimePeriodCollection();

            foreach (var time in avaliability)
            {
                var compareTime = time.Start;
                var endTime = time.End;

                while (compareTime <= endTime)
                {
                    allTimeGapsAvaliable.Add(new TimeRange(compareTime));

                    compareTime = compareTime.Add(new TimeSpan(0, 15, 0));
                }
            }

            return allTimeGapsAvaliable;
        }

        //Get OpeningHours
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
    }
}