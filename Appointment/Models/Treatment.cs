﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Appointment.Models
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Range(0, int.MaxValue, ErrorMessage = "Select a correct Treatment Type")]
        public TreatmentType TreatmentType { get; set; }
        public string TreatmentName { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; }

    }

    public enum TreatmentType
    {
        [Display(Name = "Acrylic Powder")]
        Acrylic,

        [Display(Name = "Gel Powder")]
        GelPowder,

        [Display(Name = "Sns Dipping Powder")]
        Sns,

        [Display(Name = "Natural Nail Care")]
        NaturalNail,

        [Display(Name = "Gel Polish")]
        GelPolish,

        [Display(Name = "Add On")]
        AddOn

    }

}