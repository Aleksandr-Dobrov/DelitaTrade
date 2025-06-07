using Microsoft.EntityFrameworkCore;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Infrastructure.Data.Models.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DelitaTrade.Infrastructure.Data
{
    public class DelitaDbContext : IdentityDbContext<DelitaUser, IdentityRole<Guid>, Guid>
    {
        //Remove comment on code below before applying migrations
        //public DelitaDbContext() { }

        public DelitaDbContext(DbContextOptions<DelitaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CompanyObject>().HasOne(a => a.Address);
            modelBuilder.Entity<Address>().HasMany(a => a.CompanyObjects);
            modelBuilder.ApplyConfiguration(new DayReportConfiguration());            
        }

        //Remove comment on code below and add connection string before applying migrations
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured == false)
        //    {
        //        optionsBuilder.UseSqlServer(); //Add your connection string here manually 
        //    }
        //}

        public DbSet<Product> Products { get; set; }
        public DbSet<ReturnedProductDescription> ReturnedProductDescriptions { get; set; }
        public DbSet<DescriptionCategory> DescriptionCategories { get; set; }  
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyObject> Objects { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<ReturnProtocol> ReturnProtocols { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<DayReport> DayReports { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<InvoiceInDayReport> InvoicesInDayReports { get; set; }
    }
}
