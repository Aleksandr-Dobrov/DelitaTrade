using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Infrastructure.Data.Models;
using System.Reflection;

namespace DelitaTrade.Infrastructure.Data
{
    public class DelitaDbContext : DbContext
    {
        //Remove comment on code below before applying migrations
        public DelitaDbContext() { }

        public DelitaDbContext(DbContextOptions<DelitaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CompanyObject>().HasOne(a => a.Address);
            modelBuilder.Entity<Address>().HasMany(a => a.CompanyObjects);
        }

        //Remove comment on code below before applying migrations
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("delitaAppSetings", true)
                    .AddUserSecrets("100f3212-86c7-4ff4-ba58-d07f5ab41e50")
                    .Build();
                string connectionString = config.GetConnectionString("DelitaConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ReturnedProductDescription> ReturnedProductDescriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyObject> Objects { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<ReturnProtocol> ReturnProtocols { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
