using System;
namespace AccessDataApi.Models
{
    public class EmployeeTreatment
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int TreatmentId { get; set; }
        public virtual Treatment Treatment { get; set; }


    }
}
