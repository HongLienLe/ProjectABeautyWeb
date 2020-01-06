using System;
using System.Collections.Generic;

namespace Appointment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmployeName { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<DayOfWeek> WorkingDays { get; set; }
        public TimeSpan WorkHours { get; set; }


    }
}
