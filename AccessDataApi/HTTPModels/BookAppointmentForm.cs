using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessDataApi.HTTPModels
{
    public class BookAppointmentForm
    {
        public string DateTimeFormatt { get; set; }
        public ClientForm Client { get; set; }
        public List<int> TreatmentIds { get; set; }
        public string StartTime { get; set; }

    }

    public class BookedAppointmentDetails 
    {
        public int Id { get; set; }
        public ClientForm Client { get; set; }
        public string TreatmentType { get; set; }
        public string TreatmentName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class ClientForm {

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string Email { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "Max Length is 11")]
        public string ContactNumber { get; set; }

    }

    public class ClientDetails : ClientForm
    {
        public int Id { get; set; }

    }
}
