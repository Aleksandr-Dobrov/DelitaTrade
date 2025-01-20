using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTraredIdColumnInCompanyObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Traders_TraiderId",
                table: "Objects");

            migrationBuilder.RenameColumn(
                name: "TraiderId",
                table: "Objects",
                newName: "TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_Objects_TraiderId",
                table: "Objects",
                newName: "IX_Objects_TraderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects");

            migrationBuilder.RenameColumn(
                name: "TraderId",
                table: "Objects",
                newName: "TraiderId");

            migrationBuilder.RenameIndex(
                name: "IX_Objects_TraderId",
                table: "Objects",
                newName: "IX_Objects_TraiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Traders_TraiderId",
                table: "Objects",
                column: "TraiderId",
                principalTable: "Traders",
                principalColumn: "Id");
        }
    }
}
