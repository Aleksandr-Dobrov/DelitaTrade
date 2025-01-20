﻿// <auto-generated />
using DelitaTrade.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    [DbContext(typeof(DelitaDbContext))]
    [Migration("20241222201320_addReturnProtocolIdToReturnedProduct")]
    partial class addReturnProtocolIdToReturnedProduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bulstad")
                        .HasMaxLength(15)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Type")
                        .HasMaxLength(10)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Companys");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.CompanyObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsBankPay")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<int?>("TraiderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("TraiderId");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Product", b =>
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

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyObjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TraderId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CompanyObjectId");

                    b.HasIndex("TraderId");

                    b.HasIndex("UserId");

                    b.ToTable("ReturnProtocol");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnedProduct", b =>
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

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnedProductDescription", b =>
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

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Trader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(5)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Traders");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.CompanyObject", b =>
                {
                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.Company", "Company")
                        .WithMany("Objects")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.Trader", "Trader")
                        .WithMany("Objects")
                        .HasForeignKey("TraiderId");

                    b.Navigation("Company");

                    b.Navigation("Trader");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.CompanyObject", "Object")
                        .WithMany()
                        .HasForeignKey("CompanyObjectId");

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.Trader", "Trader")
                        .WithMany()
                        .HasForeignKey("TraderId");

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.User", "User")
                        .WithMany("Protocols")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Object");

                    b.Navigation("Trader");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnedProduct", b =>
                {
                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.ReturnedProductDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.ReturnProtocol", "ReturnProtocol")
                        .WithMany("ReturnedProducts")
                        .HasForeignKey("ReturnProtocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBDelitaTrade.Infrastructure.Data.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductName", "ProductUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Product");

                    b.Navigation("ReturnProtocol");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Company", b =>
                {
                    b.Navigation("Objects");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.ReturnProtocol", b =>
                {
                    b.Navigation("ReturnedProducts");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Trader", b =>
                {
                    b.Navigation("Objects");
                });

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.User", b =>
                {
                    b.Navigation("Protocols");
                });
#pragma warning restore 612, 618
        }
    }
}
