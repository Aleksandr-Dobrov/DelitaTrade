using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayReports_DelitaUsers_UserId",
                table: "DayReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_DelitaUsers_UserId",
                table: "ReturnProtocols");

            migrationBuilder.DropTable(
                name: "DelitaUsers");

            migrationBuilder.DropIndex(
                name: "IX_ReturnProtocols_UserId",
                table: "ReturnProtocols");

            migrationBuilder.DropIndex(
                name: "IX_DayReports_UserId",
                table: "DayReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReturnProtocols");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DayReports");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentityUserId",
                table: "ReturnProtocols",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductUnit",
                table: "ReturnedProduct",
                type: "NVARCHAR(15)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ReturnedProduct",
                type: "NVARCHAR(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentityUserId",
                table: "DayReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct",
                columns: new[] { "ProductName", "ProductUnit" },
                principalTable: "Products",
                principalColumns: new[] { "Name", "Unit" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentityUserId",
                table: "ReturnProtocols",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ReturnProtocols",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ProductUnit",
                table: "ReturnedProduct",
                type: "NVARCHAR(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(15)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ReturnedProduct",
                type: "NVARCHAR(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(150)");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentityUserId",
                table: "DayReports",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DayReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DelitaUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HashedPassword = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelitaUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_UserId",
                table: "ReturnProtocols",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DayReports_UserId",
                table: "DayReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DelitaUsers_Name",
                table: "DelitaUsers",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayReports_DelitaUsers_UserId",
                table: "DayReports",
                column: "UserId",
                principalTable: "DelitaUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_Products_ProductName_ProductUnit",
                table: "ReturnedProduct",
                columns: new[] { "ProductName", "ProductUnit" },
                principalTable: "Products",
                principalColumns: new[] { "Name", "Unit" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_DelitaUsers_UserId",
                table: "ReturnProtocols",
                column: "UserId",
                principalTable: "DelitaUsers",
                principalColumn: "Id");
        }
    }
}
