﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectABeautyWeb.Data;

namespace ProjectABeautyWeb.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20191215215758_AddTreatments")]
    partial class AddTreatments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("ProjectABeautyWeb.Models.Enquiry", b =>
                {
                    b.Property<int>("EnquiryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.HasKey("EnquiryId");

                    b.ToTable("Enquiries");
                });

            modelBuilder.Entity("ProjectABeautyWeb.Models.Treatment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("TreatmentName")
                        .HasColumnType("TEXT");

                    b.Property<int>("TreatmentType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Treatments");
                });
#pragma warning restore 612, 618
        }
    }
}
