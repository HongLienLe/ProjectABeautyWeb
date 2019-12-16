using System;
using Microsoft.EntityFrameworkCore;
using ProjectABeautyWeb.Models;

namespace ProjectABeautyWeb.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
          //  Database.EnsureCreated();
        }

        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enquiry>().HasKey(x => x.EnquiryId);
            modelBuilder.Entity<Treatment>().HasKey(x => x.Id);
        }

    }
}

