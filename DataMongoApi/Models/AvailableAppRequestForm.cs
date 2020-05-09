using System;
using System.Collections.Generic;

namespace DataMongoApi.Models
{
    public class AvailableAppRequestForm
    {
        public DateTime DateTime { get; set; }
        public List<string> TreatmentIds { get; set; }
    }
}
