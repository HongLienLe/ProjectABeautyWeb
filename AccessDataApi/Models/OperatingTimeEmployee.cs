using System;
namespace AccessDataApi.Models
{
    public class OperatingTimeEmployee
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int OperatingTimeId { get; set; }
        public virtual OperatingTime OperatingTime { get; set; }

    }
}
