using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReturnProtocolIdToReturnedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ReturnProtocolId",
                table: "ReturnedProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId",
                principalTable: "ReturnProtocol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ReturnProtocolId",
                table: "ReturnedProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId",
                principalTable: "ReturnProtocol",
                principalColumn: "Id");
        }
    }
}
