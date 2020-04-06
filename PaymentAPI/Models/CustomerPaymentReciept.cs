using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccessDataApi.Models;

namespace PaymentAPI.Models
{
    public class CustomerPaymentReciept
    {
        [Key]
        public int Id { get; set; }

        public ClientAccount ClientAccount { get; set; }

        [NotMapped]
        public ICollection<string> Treatments { get; set; }

        public DateTime PurchaseDateTime { get; set; }

        public int Amount { get; set; }

        public CustomerPaymentReciept()
        {
            Treatments = new List<string>();
        }
    }
}
