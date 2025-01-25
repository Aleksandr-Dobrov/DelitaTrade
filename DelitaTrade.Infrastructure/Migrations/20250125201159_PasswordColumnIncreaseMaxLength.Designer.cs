﻿// <auto-generated />
using System;
using DelitaTrade.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    [DbContext(typeof(DelitaDbContext))]
    [Migration("20250125201159_PasswordColumnIncreaseMaxLength")]
    partial class PasswordColumnIncreaseMaxLength
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("GpsCoordinates")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Number")
                        .HasMaxLength(10)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("StreetName")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Town")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bulstad")
                        .HasMaxLength(15)
                        .HasColumnType("NVARCHAR");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Type")
                        .HasMaxLength(10)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.CompanyObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBankPay")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<int>("TraderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name");

                    b.HasIndex("TraderId");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyObjectId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CompanyObjectId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Product", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Unit")
                        .HasMaxLength(15)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Name", "Unit");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyObjectId")
                        .HasColumnType("int");

                    b.Property<string>("PayMethod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("ReturnedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TraderId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CompanyObjectId");

                    b.HasIndex("TraderId");

                    b.HasIndex("UserId");

                    b.ToTable("ReturnProtocols");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnedProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Batch")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("BestBefore")
                        .HasColumnType("datetime2");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)");

                    b.Property<string>("ProductUnit")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(15)");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<int>("ReturnProtocolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("ReturnProtocolId");

                    b.HasIndex("ProductName", "ProductUnit");

                    b.ToTable("ReturnedProduct");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnedProductDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("ReturnedProductDescriptions");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Trader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Traders");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.CompanyObject", b =>
                {
                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Address", "Address")
                        .WithMany("CompanyObjects")
                        .HasForeignKey("AddressId");

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Company", "Company")
                        .WithMany("Objects")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Trader", "Trader")
                        .WithMany("Objects")
                        .HasForeignKey("TraderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Company");

                    b.Navigation("Trader");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Invoice", b =>
                {
                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Company", "Company")
                        .WithMany("Invoices")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.CompanyObject", "CompanyObject")
                        .WithMany("Invoices")
                        .HasForeignKey("CompanyObjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("CompanyObject");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.CompanyObject", "Object")
                        .WithMany()
                        .HasForeignKey("CompanyObjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Trader", "Trader")
                        .WithMany()
                        .HasForeignKey("TraderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.User", "User")
                        .WithMany("ReturnProtocols")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Object");

                    b.Navigation("Trader");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnedProduct", b =>
                {
                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.ReturnedProductDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.ReturnProtocol", "ReturnProtocol")
                        .WithMany("ReturnedProducts")
                        .HasForeignKey("ReturnProtocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DelitaTrade.Infrastructure.Data.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductName", "ProductUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Product");

                    b.Navigation("ReturnProtocol");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Address", b =>
                {
                    b.Navigation("CompanyObjects");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Company", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Objects");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.CompanyObject", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.Navigation("ReturnedProducts");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.Trader", b =>
                {
                    b.Navigation("Objects");
                });

            modelBuilder.Entity("DelitaTrade.Infrastructure.Data.Models.User", b =>
                {
                    b.Navigation("ReturnProtocols");
                });
#pragma warning restore 612, 618
        }
    }
}
