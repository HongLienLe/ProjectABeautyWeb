using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class TimeSlotForTreatmentForm
    {
        public DateTime Date { get; set; }
        public List<int> Treatments { get; set; }
    }
}
