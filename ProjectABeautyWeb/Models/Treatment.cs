using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectABeautyWeb.Models
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TreatmentType TreatmentType { get; set; }
        public string TreatmentName { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; }

    }

    public enum TreatmentType{

        Enchancements,
        NaturalNail,
        GelPolish,
        AddOn
        }
}
