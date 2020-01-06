using System;
using System.Collections.Generic;
using Itenso.TimePeriod;

namespace Appointment.Models
{
    public class BookingPageAvalibility
    {
        
        public DateTime Date { get; set; }
        public OpeningTimes OpeningTimes { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        public DateTime StartTime { get; set; }

    }

    public class OpeningTimes
    {
        public Dictionary<string, HourRange> OpeningHours = new Dictionary<string, HourRange>()
        {
            {"Sunday", new HourRange(0,0) },
            {"Monday", new HourRange(10,19)},
            {"Tuesday", new HourRange(10,19) },
            {"Wednesday", new HourRange(10,19) },
            {"Thursday",  new HourRange(10,19)},
            {"Friday", new HourRange(10, 19)},
            {"Saturday", new HourRange(10,19)}
        };
    }
}
