using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReturnProtocol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "ReturnedProductDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProductDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Batch = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    BestBefore = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    ProductUnit = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProducts_Products_ProductName_ProductUnit",
                        columns: x => new { x.ProductName, x.ProductUnit },
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "Unit" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProducts_ReturnedProductDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "ReturnedProductDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductDescriptions_Description",
                table: "ReturnedProductDescriptions",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProducts_DescriptionId",
                table: "ReturnedProducts",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProducts_ProductName_ProductUnit",
                table: "ReturnedProducts",
                columns: new[] { "ProductName", "ProductUnit" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnedProducts");

            migrationBuilder.DropTable(
                name: "ReturnedProductDescriptions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductName");
        }
    }
}
