using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Appointment.Models
{
    public class DateTimeBookedAppointment
    {
        public DateTimeBookedAppointment()
        {
            BookedAppointments = new List<BookAppointmentDetail>();
        }
            [Key]
            public string DateTimeId { get; set; }
            public ICollection<BookAppointmentDetail> BookedAppointments { get; set; }
        
    }
}
