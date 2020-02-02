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
        public string EmployeName { get; set; }

        public virtual ICollection<BookApp> BookApps { get; set; }
        public virtual ICollection<EmployeeTreatment> EmployeeTreatments { get; set; }

        public Employee()
        {
            EmployeeTreatments = new List<EmployeeTreatment>();
            BookApps = new List<BookApp>();
        }

    }
}
