using System;
using System.Collections.Generic;
using AccessDataApi.Models;

namespace PaymentAPI.Models
{
    public class ClientAccountPayment
    {
        public ClientAccount ClientAccount { get; set; }
        public ICollection<ClientAccountPayment> HistoryPayments { get; set; }

        public ClientAccountPayment()
        {
            HistoryPayments = new List<ClientAccountPayment>();
        }
    }
}
