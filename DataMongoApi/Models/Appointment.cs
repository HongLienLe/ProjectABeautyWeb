using System;
using System.Collections.Generic;

namespace DataMongoApi.Models
{
    public class Appointment
    {
        public Client Client { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Treatment Treatment { get; set; }
        public Employee Employee { get; set; }

    }
}
