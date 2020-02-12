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
        public string EmployeName { get; set; }

        public ICollection<BookApp> BookApp { get; set; }
        public ICollection<EmployeeTreatment> EmployeeTreatments { get; set; }
        public ICollection<OperatingTimeEmployee> workschedule { get; set; }

        public Employee()
        {
            EmployeeTreatments = new List<EmployeeTreatment>();
            BookApp = new List<BookApp>();
            workschedule = new List<OperatingTimeEmployee>();
        }

    }
}
