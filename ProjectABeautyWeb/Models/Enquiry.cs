using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectABeautyWeb.Models
{
    public sealed class Enquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnquiryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Note { get; set; }

        public Enquiry(string name, string email, string contactNumber, string note)
        {
            Name = name;
            Email = email;
            ContactNumber = contactNumber;
            Note = note;
        }

        public Enquiry()
        {

        }
    }
}
