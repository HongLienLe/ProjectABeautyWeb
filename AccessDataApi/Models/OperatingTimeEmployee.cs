using System;
namespace AccessDataApi.Models
{
    public class OperatingTimeEmployee
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int OperatingTimeId { get; set; }
        public OperatingTime OperatingTime { get; set; }
    }
}
