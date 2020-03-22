using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class BookAppointmentForm : ClientForm
    {
        public string DateTimeFormatt { get; set; }
        public List<int> TreatmentIds { get; set; }
        public string StartTime { get; set; }

    }

    public class BookedAppointmentDetails : ClientForm
    {
        public int Id { get; set; }
        public string TreatmentName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class ClientForm {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

    }
}
