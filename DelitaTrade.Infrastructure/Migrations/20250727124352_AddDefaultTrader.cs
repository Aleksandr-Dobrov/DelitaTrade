using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultTrader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Traders",
                columns: new[] { "Id", "IsActive", "Name", "PhoneNumber" },
                values: new object[] { 1, true, "няма запис", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Traders",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
