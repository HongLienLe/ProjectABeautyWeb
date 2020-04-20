using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessDataApi.Models
{
    public class ClientAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientAccountId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max Length is 100")]
        public string ContactNumber { get; set; }

        public virtual ICollection<AppointmentDetails> Appointments { get; set; }
      //  public virtual ICollection<Payment> PaymentHistory { get; set; }
        

        public ClientAccount()
        {
            Appointments = new List<AppointmentDetails>();
      //      PaymentHistory = new List<Payment>();
        }
    }
}
