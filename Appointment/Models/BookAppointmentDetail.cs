using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Appointment.Models;
using Itenso.TimePeriod;

namespace Appointment
{
    public class BookAppointmentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClientId { get; set; }
        public ClientAccount Client { get; set; }


        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public string DateTimeBookedAppointmentId { get; set; }

        public TimeRange Reservation { get; set; }

    }

}
