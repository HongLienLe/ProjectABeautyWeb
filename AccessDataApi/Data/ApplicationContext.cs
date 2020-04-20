using System;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<EmployeeTreatment> EmployeeTreatment { get; set; }
        public DbSet<DateTimeKey> DateTimeKeys { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<AppointmentDetails> AppointmentDetails { get; set; }
        public DbSet<OperatingTime> OperatingTimes { get; set; }
        public DbSet<OperatingTimeEmployee> workSchedules { get; set; }
      //  public DbSet<Payment> Payments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
               Database.EnsureCreated();
              // Database.EnsureDeleted();
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EmployeeTreatment>(e =>
            {
                e.HasKey(et => new { et.TreatmentId, et.EmployeeId});

                e.HasOne<Employee>(e => e.Employee)
                    .WithMany(t => t.EmployeeTreatments)
                    .HasForeignKey(e => e.EmployeeId);

                e.HasOne<Treatment>(e => e.Treatment)
                .WithMany(t => t.EmployeeTreatments)
                .HasForeignKey(e => e.TreatmentId);

            });

            modelBuilder.Entity<OperatingTimeEmployee>(e =>
            {
                e.HasKey(k => new { k.EmployeeId, k.OperatingTimeId });

                e.HasOne(e => e.Employee)
                .WithMany(o => o.workschedule)
                .HasForeignKey(ofk => ofk.EmployeeId);

                e.HasOne(o => o.OperatingTime)
                .WithMany(e => e.Employees)
                .HasForeignKey(efk => efk.OperatingTimeId);

            });

           
            modelBuilder.Entity<AppointmentDetails>(entity =>
            {
                entity.HasKey(x => x.AppointmentDetailsId);

                entity.HasOne(x => x.ClientAccount)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.ClientAccountId);

                entity.HasOne(x => x.DateTimeKey)
                 .WithMany(x => x.Appointments)
                 .HasForeignKey(x => x.DateTimeKeyId);

                entity.HasOne(x => x.Reservation)
                 .WithOne(x => x.AppointmentDetails)
                 .HasForeignKey<Reservation>(x => x.AppointmentDetailsId);

                entity.HasOne(x => x.Treatment)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.TreatmentId);

            });

            modelBuilder.Entity<DateTimeKey>(entity =>
              {
                  entity.HasMany(x => x.Appointments)
                 .WithOne(x => x.DateTimeKey)
                 .HasForeignKey(x => x.DateTimeKeyId);

               });

        }
    }
}
