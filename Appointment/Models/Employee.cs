using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmployeName { get; set; }
        //public ICollection<Treatment> Treatments { get; set; }
        //public List<string> OffDays { get; set; }

        public Employee()
        {

            //Treatments = new List<Treatment>();
            //OffDays = new List<string>();
        }

    }
}
