using System;

namespace AccessDataApi.HTTPModels
{
    public class OperatingTimeDetails : OperatingTimeForm
    {
        public int Id { get; set; }
        public string Day { get; set; }

    }

    public class OperatingTimeForm
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool isOpen { get; set; }
    }

}
