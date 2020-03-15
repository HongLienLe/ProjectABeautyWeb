using System;
using System.ComponentModel.DataAnnotations;
using AccessDataApi.Models;

namespace AccessDataApi.HTTPModels
{
    public class TreatmentForm
    {
        [Required]
        public string TreatmentName { get; set; }

        public TreatmentType TreatmentType { get; set; }

        public bool isAddOn { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Duration { get; set; }
    }

    public class TreatmentDetails : TreatmentForm
    {
        public int Id { get; set; }

    }
}
