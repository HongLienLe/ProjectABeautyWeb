using System;
using System.ComponentModel.DataAnnotations;

namespace AccessDataApi.Models
{
    public class OperatingTime
    {
        [Key]
        public int Id { get;}
        public string Day { get; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
