using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Traders_TraiderId",
                table: "ReturnProtocol");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Users_UserId",
                table: "ReturnProtocol");

            migrationBuilder.RenameColumn(
                name: "TraiderId",
                table: "ReturnProtocol",
                newName: "TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_TraiderId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_TraderId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ReturnProtocol",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Traders_TraderId",
                table: "ReturnProtocol",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Users_UserId",
                table: "ReturnProtocol",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Traders_TraderId",
                table: "ReturnProtocol");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Users_UserId",
                table: "ReturnProtocol");

            migrationBuilder.RenameColumn(
                name: "TraderId",
                table: "ReturnProtocol",
                newName: "TraiderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_TraderId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_TraiderId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ReturnProtocol",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Traders_TraiderId",
                table: "ReturnProtocol",
                column: "TraiderId",
                principalTable: "Traders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Users_UserId",
                table: "ReturnProtocol",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
