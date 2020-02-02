﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Itenso.TimePeriod;

namespace AccessDataApi.Models
{
    public class BookApp
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookAppId { get; set; }

        [Column("Client_Id")]
        public int ClientAccountId { get; set; }
        public virtual ClientAccount ClientAccount { get; set; }

        public string DateTimeKeyId { get; set; }
        public virtual DateTimeKey DateTimeKey { get; set; }

        [Column("Employee_Id")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Column("Treatment_Id")]
        public int TreatmentId { get; set; }
        public virtual Treatment Treatment { get; set; }

        public virtual Reservation Reservation { get; set; }


    }

}
