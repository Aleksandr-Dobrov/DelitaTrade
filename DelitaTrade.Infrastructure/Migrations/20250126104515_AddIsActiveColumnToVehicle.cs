using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumnToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayReports_Vehicle_VehicleId",
                table: "DayReports");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceInDayReport_DayReports_DayReportId",
                table: "InvoiceInDayReport");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceInDayReport_Invoices_InvoiceId",
                table: "InvoiceInDayReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceInDayReport",
                table: "InvoiceInDayReport");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "InvoiceInDayReport",
                newName: "InvoicesInDayReports");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceInDayReport_InvoiceId",
                table: "InvoicesInDayReports",
                newName: "IX_InvoicesInDayReports_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceInDayReport_DayReportId",
                table: "InvoicesInDayReports",
                newName: "IX_InvoicesInDayReports_DayReportId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoicesInDayReports",
                table: "InvoicesInDayReports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayReports_Vehicles_VehicleId",
                table: "DayReports",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesInDayReports_DayReports_DayReportId",
                table: "InvoicesInDayReports",
                column: "DayReportId",
                principalTable: "DayReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesInDayReports_Invoices_InvoiceId",
                table: "InvoicesInDayReports",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayReports_Vehicles_VehicleId",
                table: "DayReports");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesInDayReports_DayReports_DayReportId",
                table: "InvoicesInDayReports");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesInDayReports_Invoices_InvoiceId",
                table: "InvoicesInDayReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoicesInDayReports",
                table: "InvoicesInDayReports");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vehicle");

            migrationBuilder.RenameTable(
                name: "InvoicesInDayReports",
                newName: "InvoiceInDayReport");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesInDayReports_InvoiceId",
                table: "InvoiceInDayReport",
                newName: "IX_InvoiceInDayReport_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesInDayReports_DayReportId",
                table: "InvoiceInDayReport",
                newName: "IX_InvoiceInDayReport_DayReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceInDayReport",
                table: "InvoiceInDayReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayReports_Vehicle_VehicleId",
                table: "DayReports",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceInDayReport_DayReports_DayReportId",
                table: "InvoiceInDayReport",
                column: "DayReportId",
                principalTable: "DayReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceInDayReport_Invoices_InvoiceId",
                table: "InvoiceInDayReport",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
