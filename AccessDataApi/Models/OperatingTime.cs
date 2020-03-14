using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessDataApi.Models
{
    public class OperatingTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1,7, ErrorMessage = "Day Id must be between 1 - 7")]
        public int Id { get; set; }
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
