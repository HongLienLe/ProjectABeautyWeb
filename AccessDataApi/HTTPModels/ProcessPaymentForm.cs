using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class ProcessPaymentForm
    {
        public List<int> bookedAppIds { get; set; }
        public List<int> MiscAmount { get; set; }

    }

    public class PaymentDetails
    {
        public int Id { get; set; }
        public DateTime PaymentTime { get; set; }
        public int ClientId { get; set; }
        public bool HadMISC { get; set; }
        public int Total { get; set; }
    }
}
