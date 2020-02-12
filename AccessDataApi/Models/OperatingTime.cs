using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessDataApi.Models
{
    public class OperatingTime
    {
        [Key]
        [Range(1,7, ErrorMessage = "Day Id must be between 1 - 7")]
        public int Id { get;}
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [Required]
        public bool isOpen { get; set; }

        public ICollection<OperatingTimeEmployee> Employees { get; set; }

        public OperatingTime()
        {
            Employees = new List<OperatingTimeEmployee>();
        }

    }
}
