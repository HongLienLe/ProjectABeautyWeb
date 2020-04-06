using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AccessDataApi.Models;

namespace PaymentAPI.Models
{
    public class ClientPaymentHistory
    {
        public int ClientAccountId { get; set; }
        public ClientAccount ClientAccount { get; set; }

        public ICollection<CustomerPaymentReciept> PaymentHistory { get; set; }

        public ClientPaymentHistory()
        {
            PaymentHistory = new List<CustomerPaymentReciept>();
        }
    }
}
