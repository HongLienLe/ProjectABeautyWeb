using System;
using Appointment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Appointment.Context 
{
    public class ApplicationContext : DbContext
    {
        public DbSet<DateTimeBookedAppointment> DateTimeBookedAppointments { get; set; }
        public DbSet<BookAppointmentDetail> BookAppointmentDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ProjectABeautyDb;User Id=sa;Password=Passw0rd!");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<>


        //    //modelBuilder.Entity<DateTimeBookedAppointment>().HasMany(b => b.BookedAppointments).WithOne();
        //    //modelBuilder.Entity<BookAppointmentDetail>().HasOne(e => e.Employee);
        //    //modelBuilder.Entity<BookAppointmentDetail>().HasOne(t => t.Treatment);
        //    //modelBuilder.Entity<BookAppointmentDetail>().HasOne(d => d.DateTimeId);
        //    //modelBuilder.Entity<Employee>().HasMany(t => t.Treatments).WithOne();

        //}
    }
}
