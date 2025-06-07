using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetDescriptionToNullableAndCategoryToNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct");

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionId",
                table: "ReturnedProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionCategoryId",
                table: "ReturnedProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct",
                column: "DescriptionCategoryId",
                principalTable: "DescriptionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct",
                column: "DescriptionId",
                principalTable: "ReturnedProductDescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct");

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionId",
                table: "ReturnedProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionCategoryId",
                table: "ReturnedProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct",
                column: "DescriptionCategoryId",
                principalTable: "DescriptionCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnedProductDescriptions_DescriptionId",
                table: "ReturnedProduct",
                column: "DescriptionId",
                principalTable: "ReturnedProductDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
