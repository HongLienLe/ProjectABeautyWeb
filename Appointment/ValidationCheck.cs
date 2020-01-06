using System;
using Appointment.Models;

namespace Appointment
{
    public class ValidationCheck
    {
        public OpeningTimes OpeningTimes;

        public ValidationCheck(OpeningTimes openingTimes)
        {
            OpeningTimes = openingTimes;
        }


        public bool IsDateAvaliable(DateTime date)
        {
            var choosenDay = date.DayOfWeek.ToString();
            if ((OpeningTimes.OpeningHours[choosenDay].Start - OpeningTimes.OpeningHours[choosenDay].End).TotalHours == 0)
            {
                return false;
            }

            return true;
        }
    }
}
