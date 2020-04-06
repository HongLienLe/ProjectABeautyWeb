using System;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;

namespace PaymentAPI.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<ClientPaymentHistory> ClientPaymentHistorys { get; set; }
        public DbSet<CustomerPaymentReciept> PaymentHistorys { get; set; }
    }
}
