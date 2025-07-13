using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryId",
                table: "InvoicesInDayReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: true),
                    DayReportId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_DayReports_DayReportId",
                        column: x => x.DayReportId,
                        principalTable: "DayReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Objects_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesInDayReports_DeliveryId",
                table: "InvoicesInDayReports",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DayReportId",
                table: "Deliveries",
                column: "DayReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryAddressId",
                table: "Deliveries",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_EmployeeId",
                table: "Deliveries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_VehicleId",
                table: "Deliveries",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesInDayReports_Deliveries_DeliveryId",
                table: "InvoicesInDayReports",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesInDayReports_Deliveries_DeliveryId",
                table: "InvoicesInDayReports");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesInDayReports_DeliveryId",
                table: "InvoicesInDayReports");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                table: "InvoicesInDayReports");
        }
    }
}
