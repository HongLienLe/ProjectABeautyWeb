using System;
namespace Appointment.Models
{
    public class EditBookAppointmentDetails
    {
        public DateTime DateTimeId { get; set; }
        public int AppId { get; set; }
        public BookAppointmentDetail updatedDetails { get; set; }
    }
}
