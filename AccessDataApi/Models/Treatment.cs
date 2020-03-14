using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AccessDataApi.Models
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentId { get; set; }
        //[Range(0, int.MaxValue, ErrorMessage = "Select a correct Treatment Type")]
        public TreatmentType TreatmentType { get; set; }

        [Required]
        public string TreatmentName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Duration { get; set; }

        public virtual ICollection<EmployeeTreatment> EmployeeTreatments { get; set; }
        public virtual ICollection<AppointmentDetails> Appointments { get; set; }

        public Treatment()
        {
            EmployeeTreatments = new List<EmployeeTreatment>();
            Appointments = new List<AppointmentDetails>();
        }
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
