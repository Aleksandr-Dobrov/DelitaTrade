using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBanknotesAsJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Banknotes",
                table: "DayReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCash",
                table: "DayReports",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banknotes",
                table: "DayReports");

            migrationBuilder.DropColumn(
                name: "TotalCash",
                table: "DayReports");
        }
    }
}
