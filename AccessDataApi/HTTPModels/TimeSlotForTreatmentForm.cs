using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class TimeSlotForTreatmentForm
    {
        public string Date { get; set; }
        public List<int> Treatments { get; set; }
    }
}
