using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransmissionDateToDatReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransmissionDate",
                table: "DayReports",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransmissionDate",
                table: "DayReports");
        }
    }
}
