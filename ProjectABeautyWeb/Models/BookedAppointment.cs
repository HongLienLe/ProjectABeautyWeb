using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectABeautyWeb.Models
{
    public class BookedAppointment
    {

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public Treatment Treatments { get; set; }

    }
}
