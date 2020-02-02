using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessDataApi.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("BookApp")]
        public int BookAppId { get; set; }
        public virtual BookApp BookApp { get; set; }

    }
}
