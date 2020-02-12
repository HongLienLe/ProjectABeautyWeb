﻿// <auto-generated />
using System;
using AccessDataApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccessDataApi.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200209001836_addemployeebookappremovedemployeeindatetimekeyId")]
    partial class addemployeebookappremovedemployeeindatetimekeyId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccessDataApi.Models.BookApp", b =>
                {
                    b.Property<int>("BookAppId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientAccountId")
                        .HasColumnName("Client_Id")
                        .HasColumnType("int");

                    b.Property<string>("DateTimeKeyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EmployeeId")
                        .HasColumnName("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("TreatmentId")
                        .HasColumnName("Treatment_Id")
                        .HasColumnType("int");

                    b.HasKey("BookAppId");

                    b.HasIndex("ClientAccountId");

                    b.HasIndex("DateTimeKeyId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TreatmentId");

                    b.ToTable("BookApps");
                });

            modelBuilder.Entity("AccessDataApi.Models.ClientAccount", b =>
                {
                    b.Property<int>("ClientAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientAccountId");

                    b.ToTable("ClientAccounts");
                });

            modelBuilder.Entity("AccessDataApi.Models.DateTimeKey", b =>
                {
                    b.Property<string>("DateTimeKeyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("DateTimeKeyId");

                    b.ToTable("DateTimeKeys");
                });

            modelBuilder.Entity("AccessDataApi.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmployeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AccessDataApi.Models.EmployeeTreatment", b =>
                {
                    b.Property<int>("TreatmentId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("TreatmentId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeTreatment");
                });

            modelBuilder.Entity("AccessDataApi.Models.OperatingTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<bool>("isOpen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("OperatingTimes");
                });

            modelBuilder.Entity("AccessDataApi.Models.OperatingTimeEmployee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("OperatingTimeId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "OperatingTimeId");

                    b.HasIndex("OperatingTimeId");

                    b.ToTable("workSchedules");
                });

            modelBuilder.Entity("AccessDataApi.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookAppId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ReservationId");

                    b.HasIndex("BookAppId")
                        .IsUnique();

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("AccessDataApi.Models.Treatment", b =>
                {
                    b.Property<int>("TreatmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("TreatmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TreatmentType")
                        .HasColumnType("int");

                    b.HasKey("TreatmentId");

                    b.ToTable("Treatments");
                });

            modelBuilder.Entity("AccessDataApi.Models.BookApp", b =>
                {
                    b.HasOne("AccessDataApi.Models.ClientAccount", "ClientAccount")
                        .WithMany("Appointments")
                        .HasForeignKey("ClientAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.DateTimeKey", "DateTimeKey")
                        .WithMany("bookApps")
                        .HasForeignKey("DateTimeKeyId");

                    b.HasOne("AccessDataApi.Models.Employee", "Employee")
                        .WithMany("BookApp")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.Treatment", "Treatment")
                        .WithMany("bookApps")
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
                    b.HasOne("AccessDataApi.Models.OperatingTime", "OperatingTime")
                        .WithMany("Employees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessDataApi.Models.Employee", "Employee")
                        .WithMany("workschedule")
                        .HasForeignKey("OperatingTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccessDataApi.Models.Reservation", b =>
                {
                    b.HasOne("AccessDataApi.Models.BookApp", "BookApp")
                        .WithOne("Reservation")
                        .HasForeignKey("AccessDataApi.Models.Reservation", "BookAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
