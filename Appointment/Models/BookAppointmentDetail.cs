using System;
using System.Text.Json;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class BookAppointmentDetail
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int EmployeeId { get; set; }
        public int TreatmentId { get; set; }
        public DateTime DateTimeId { get; set; }
        public TimeRange Reservation { get; set; }

    }

}
