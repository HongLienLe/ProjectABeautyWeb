using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessDataApi.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string EmployeName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<AppointmentDetails> Appointments { get; set; }
        public ICollection<EmployeeTreatment> EmployeeTreatments { get; set; }
        public ICollection<OperatingTimeEmployee> workschedule { get; set; }

        public Employee()
        {
            EmployeeTreatments = new List<EmployeeTreatment>();
            Appointments = new List<AppointmentDetails>();
            workschedule = new List<OperatingTimeEmployee>();
        }

    }
}
