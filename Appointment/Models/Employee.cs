using System;
using System.Collections.Generic;

namespace Appointment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmployeName { get; set; }
        public Dictionary<int,Treatment> Treatments { get; set; }
        public List<string> OffDays { get; set; }

    }
}
