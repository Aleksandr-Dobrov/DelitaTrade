using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultDescriptionCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DescriptionCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Грешна заявка" },
                    { 2, "Отказана поръчка" },
                    { 3, "Ненатоварена стока" },
                    { 4, "Къс срок на годност" },
                    { 5, "Изтегляне на стока" },
                    { 6, "Скъсана опаковка" },
                    { 7, "Развакуумирано" },
                    { 8, "Грешно количество" },
                    { 9, "Лошо качество" },
                    { 10, "Без Етикетировка" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DescriptionCategories",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
