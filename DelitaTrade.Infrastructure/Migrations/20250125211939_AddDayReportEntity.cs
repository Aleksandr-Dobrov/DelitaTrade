using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDayReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicensePlate = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "money", nullable: false),
                    TotalExpense = table.Column<decimal>(type: "money", nullable: false),
                    TotalNotPay = table.Column<decimal>(type: "money", nullable: false),
                    TotalOldInvoice = table.Column<decimal>(type: "money", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayReports_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceInDayReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Income = table.Column<decimal>(type: "money", nullable: false),
                    PayMethod = table.Column<int>(type: "int", nullable: false),
                    DayReportId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceInDayReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceInDayReport_DayReports_DayReportId",
                        column: x => x.DayReportId,
                        principalTable: "DayReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceInDayReport_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayReports_UserId",
                table: "DayReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DayReports_VehicleId",
                table: "DayReports",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceInDayReport_DayReportId",
                table: "InvoiceInDayReport",
                column: "DayReportId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceInDayReport_InvoiceId",
                table: "InvoiceInDayReport",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceInDayReport");

            migrationBuilder.DropTable(
                name: "DayReports");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
