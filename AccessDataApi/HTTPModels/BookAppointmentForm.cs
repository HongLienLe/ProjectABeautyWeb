using System;
namespace AccessDataApi.HTTPModels
{
    public class BookAppointmentForm : ClientForm
    {
        public string DateTimeFormatt { get; set; }
        public int TreatmentId { get; set; }
        public string StartTime { get; set; }

    }

    public class BookedAppointmentDetails : BookAppointmentForm
    {
        public int Id { get; set; }
    }

    public class ClientForm {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

    }
}
