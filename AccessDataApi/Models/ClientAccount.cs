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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string ContactNumber { get; set; }

        public virtual ICollection<BookApp> Appointments { get; set; }

        public ClientAccount()
        {
            Appointments = new List<BookApp>();
        }
    }
}
