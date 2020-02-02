using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class AddEmployeeTreatment
    {
        public int EmployeeId { get; set; }
        public List<int> TreatmentIds { get; set; }

        public AddEmployeeTreatment()
        {
            TreatmentIds = new List<int>();
        }
    }
}
