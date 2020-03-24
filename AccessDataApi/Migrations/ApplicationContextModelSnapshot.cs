﻿// <auto-generated />
using System;
using AccessDataApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccessDataApi.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("AccessDataApi.Authentication.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccessDataApi.Models.AppointmentDetails", b =>
                {
                    b.Property<int>("AppointmentDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientAccountId")
                        .HasColumnName("Client_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DateTimeKeyId")
                        .HasColumnName("DateTimeAppointment_Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("EmployeeId")
                        .HasColumnName("Employee_Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TreatmentId")
                        .HasColumnName("Treatment_Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("AppointmentDetailsId");

                    b.HasIndex("ClientAccountId");

                    b.HasIndex("DateTimeKeyId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TreatmentId");

                    b.ToTable("AppointmentDetails");
                });

            modelBuilder.Entity("AccessDataApi.Models.ClientAccount", b =>
                {
                    b.Property<int>("ClientAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("ClientAccountId");

                    b.ToTable("ClientAccounts");
                });

            modelBuilder.Entity("AccessDataApi.Models.DateTimeKey", b =>
                {
                    b.Property<string>("DateTimeKeyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.HasKey("DateTimeKeyId");

                    b.ToTable("DateTimeKeys");
                });

            modelBuilder.Entity("AccessDataApi.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AccessDataApi.Models.EmployeeTreatment", b =>
                {
                    b.Property<int>("TreatmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TreatmentId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeTreatment");
                });

            modelBuilder.Entity("AccessDataApi.Models.OperatingTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Day")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isOpen")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("OperatingTimes");
                });

            modelBuilder.Entity("AccessDataApi.Models.OperatingTimeEmployee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OperatingTimeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EmployeeId", "OperatingTimeId");

                    b.HasIndex("OperatingTimeId");

                    b.ToTable("workSchedules");
                });

            modelBuilder.Entity("AccessDataApi.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppointmentDetailsId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("ReservationId");

                    b.HasIndex("AppointmentDetailsId")
                        .IsUnique();

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("AccessDataApi.Models.Treatment", b =>
                {
                    b.Property<int>("TreatmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("TreatmentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TreatmentType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isAddOn")
                        .HasColumnType("INTEGER");

                    b.HasKey("TreatmentId");

                    b.ToTable("Treatments");
                });

            modelBuilder.Entity("AccessDataApi.Models.AppointmentDetails", b =>
                {
                    b.HasOne("AccessDataApi.Models.ClientAccount", "ClientAccount")
                        .WithMany("Appointments")
                        .HasForeignKey("ClientAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.DateTimeKey", "DateTimeKey")
                        .WithMany("Appointments")
                        .HasForeignKey("DateTimeKeyId");

                    b.HasOne("AccessDataApi.Models.Employee", "Employee")
                        .WithMany("Appointments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.Treatment", "Treatment")
                        .WithMany("Appointments")
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccessDataApi.Models.EmployeeTreatment", b =>
                {
                    b.HasOne("AccessDataApi.Models.Employee", "Employee")
                        .WithMany("EmployeeTreatments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.Treatment", "Treatment")
                        .WithMany("EmployeeTreatments")
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccessDataApi.Models.OperatingTimeEmployee", b =>
                {
                    b.HasOne("AccessDataApi.Models.Employee", "Employee")
                        .WithMany("workschedule")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.OperatingTime", "OperatingTime")
                        .WithMany("Employees")
                        .HasForeignKey("OperatingTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccessDataApi.Models.Reservation", b =>
                {
                    b.HasOne("AccessDataApi.Models.AppointmentDetails", "AppointmentDetails")
                        .WithOne("Reservation")
                        .HasForeignKey("AccessDataApi.Models.Reservation", "AppointmentDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
