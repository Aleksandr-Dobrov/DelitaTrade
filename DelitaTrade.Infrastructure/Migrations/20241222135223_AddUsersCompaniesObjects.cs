using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersCompaniesObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProducts_Products_ProductName_ProductUnit",
                table: "ReturnedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProducts_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProducts");

            migrationBuilder.DropIndex(
                name: "IX_ReturnedProductDescriptions_Description",
                table: "ReturnedProductDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts");

            migrationBuilder.RenameTable(
                name: "ReturnedProducts",
                newName: "ReturnedProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnedProducts_ProductName_ProductUnit",
                table: "ReturnedProduct",
                newName: "IX_ReturnedProduct_ProductName_ProductUnit");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnedProducts_DescriptionId",
                table: "ReturnedProduct",
                newName: "IX_ReturnedProduct_DescriptionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ReturnedProductDescriptions",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "ReturnProtocolId",
                table: "ReturnedProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnedProduct",
                table: "ReturnedProduct",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: true),
                    Bulstad = table.Column<string>(type: "NVARCHAR(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: true),
                    IsBankPay = table.Column<bool>(type: "bit", nullable: false),
                    TraiderId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objects_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Objects_Traders_TraiderId",
                        column: x => x.TraiderId,
                        principalTable: "Traders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReturnProtocol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyObjectId = table.Column<int>(type: "int", nullable: true),
                    TraiderId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProtocol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnProtocol_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocol_Objects_CompanyObjectId",
                        column: x => x.CompanyObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocol_Traders_TraiderId",
                        column: x => x.TraiderId,
                        principalTable: "Traders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnProtocol_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductDescriptions_Description",
                table: "ReturnedProductDescriptions",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProduct_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_CompanyId",
                table: "Objects",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_Name",
                table: "Objects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_TraiderId",
                table: "Objects",
                column: "TraiderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocol_CompanyId",
                table: "ReturnProtocol",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocol_CompanyObjectId",
                table: "ReturnProtocol",
                column: "CompanyObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocol_TraiderId",
                table: "ReturnProtocol",
                column: "TraiderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocol_UserId",
                table: "ReturnProtocol",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct",
                columns: new[] { "ProductName", "ProductUnit" },
                principalTable: "Products",
                principalColumns: new[] { "Name", "Unit" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId",
                principalTable: "ReturnProtocol",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct",
                column: "DescriptionId",
                principalTable: "ReturnedProductDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct");

            migrationBuilder.DropTable(
                name: "ReturnProtocol");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropTable(
                name: "Traders");

            migrationBuilder.DropIndex(
                name: "IX_ReturnedProductDescriptions_Description",
                table: "ReturnedProductDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnedProduct",
                table: "ReturnedProduct");

            migrationBuilder.DropIndex(
                name: "IX_ReturnedProduct_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.DropColumn(
                name: "ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.RenameTable(
                name: "ReturnedProduct",
                newName: "ReturnedProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnedProduct_ProductName_ProductUnit",
                table: "ReturnedProducts",
                newName: "IX_ReturnedProducts_ProductName_ProductUnit");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnedProduct_DescriptionId",
                table: "ReturnedProducts",
                newName: "IX_ReturnedProducts_DescriptionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ReturnedProductDescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductDescriptions_Description",
                table: "ReturnedProductDescriptions",
                column: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProducts_Products_ProductName_ProductUnit",
                table: "ReturnedProducts",
                columns: new[] { "ProductName", "ProductUnit" },
                principalTable: "Products",
                principalColumns: new[] { "Name", "Unit" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProducts_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProducts",
                column: "DescriptionId",
                principalTable: "ReturnedProductDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
