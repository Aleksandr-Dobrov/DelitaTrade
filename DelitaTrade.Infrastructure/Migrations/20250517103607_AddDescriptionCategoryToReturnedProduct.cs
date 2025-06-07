using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionCategoryToReturnedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescriptionCategoryId",
                table: "ReturnedProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DescriptionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProduct_DescriptionCategoryId",
                table: "ReturnedProduct",
                column: "DescriptionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct",
                column: "DescriptionCategoryId",
                principalTable: "DescriptionCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_DescriptionCategories_DescriptionCategoryId",
                table: "ReturnedProduct");

            migrationBuilder.DropTable(
                name: "DescriptionCategories");

            migrationBuilder.DropIndex(
                name: "IX_ReturnedProduct_DescriptionCategoryId",
                table: "ReturnedProduct");

            migrationBuilder.DropColumn(
                name: "DescriptionCategoryId",
                table: "ReturnedProduct");
        }
    }
}
