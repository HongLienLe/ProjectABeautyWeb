using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Models;
using PaymentAPI.Models;

namespace PaymentAPI.Data
{
    public static class PaymentContextExtension
    {
        public static void SeedData(this PaymentContext context)
        {
            if (context.ClientPaymentHistorys.Any())
                return;

            var client = context.ClientAccounts.First(x => x.ClientAccountId == 1);
            var payment = new ClientPaymentHistory()
            {
                PaymentHistory = new List<CustomerPaymentReciept>()
                {
                    new CustomerPaymentReciept(){
                    ClientAccount = client,
                    Treatments = new List<String>() {"Infill Acrylic"},
                    PurchaseDateTime = new DateTime(2020, 3, 9, 10, 0, 0),
                    Amount = 20
                    }
                },

                ClientAccount = client
            };

            context.ClientPaymentHistorys.Add(payment);
            context.SaveChanges();
        }
    }
}
