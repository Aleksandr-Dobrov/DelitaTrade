using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnProtocols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyObjectId = table.Column<int>(type: "int", nullable: false),
                    TraderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProtocols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnProtocols_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                        column: x => x.CompanyObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocols_Traders_TraderId",
                        column: x => x.TraderId,
                        principalTable: "Traders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocols_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Batch = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    BestBefore = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    ProductUnit = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    ReturnProtocolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                        columns: x => new { x.ProductName, x.ProductUnit },
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "Unit" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProduct_ReturnProtocols_ReturnProtocolId",
                        column: x => x.ReturnProtocolId,
                        principalTable: "ReturnProtocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "ReturnedProductDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProduct_DescriptionId",
                table: "ReturnedProduct",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProduct_ProductName_ProductUnit",
                table: "ReturnedProduct",
                columns: new[] { "ProductName", "ProductUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProduct_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_CompanyId",
                table: "ReturnProtocols",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_CompanyObjectId",
                table: "ReturnProtocols",
                column: "CompanyObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_TraderId",
                table: "ReturnProtocols",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_UserId",
                table: "ReturnProtocols",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnedProduct");

            migrationBuilder.DropTable(
                name: "ReturnProtocols");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
