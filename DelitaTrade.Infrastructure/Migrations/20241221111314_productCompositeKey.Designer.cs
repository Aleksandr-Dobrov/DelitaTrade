using DelitaTrade.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    [DbContext(typeof(DelitaDbContext))]
    [Migration("20241221111314_productCompositeKey")]
    partial class productCompositeKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DBDelitaTrade.Infrastructure.Data.Models.Product", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("ProductName");

                    b.Property<string>("Unit")
                        .HasMaxLength(15)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Name", "Unit");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
