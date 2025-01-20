using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReturnProtocolsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Companies_CompanyId",
                table: "ReturnProtocol");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Objects_CompanyObjectId",
                table: "ReturnProtocol");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Traders_TraderId",
                table: "ReturnProtocol");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocol_Users_UserId",
                table: "ReturnProtocol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnProtocol",
                table: "ReturnProtocol");

            migrationBuilder.RenameTable(
                name: "ReturnProtocol",
                newName: "ReturnProtocols");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_UserId",
                table: "ReturnProtocols",
                newName: "IX_ReturnProtocols_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_TraderId",
                table: "ReturnProtocols",
                newName: "IX_ReturnProtocols_TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_CompanyObjectId",
                table: "ReturnProtocols",
                newName: "IX_ReturnProtocols_CompanyObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocol_CompanyId",
                table: "ReturnProtocols",
                newName: "IX_ReturnProtocols_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnProtocols",
                table: "ReturnProtocols",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocols_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId",
                principalTable: "ReturnProtocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Companies_CompanyId",
                table: "ReturnProtocols",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                table: "ReturnProtocols",
                column: "CompanyObjectId",
                principalTable: "Objects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Traders_TraderId",
                table: "ReturnProtocols",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Users_UserId",
                table: "ReturnProtocols",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocols_ReturnProtocolId",
                table: "ReturnedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Companies_CompanyId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Traders_TraderId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Users_UserId",
                table: "ReturnProtocols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnProtocols",
                table: "ReturnProtocols");

            migrationBuilder.RenameTable(
                name: "ReturnProtocols",
                newName: "ReturnProtocol");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocols_UserId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocols_TraderId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocols_CompanyObjectId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_CompanyObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnProtocols_CompanyId",
                table: "ReturnProtocol",
                newName: "IX_ReturnProtocol_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnProtocol",
                table: "ReturnProtocol",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProduct_ReturnProtocol_ReturnProtocolId",
                table: "ReturnedProduct",
                column: "ReturnProtocolId",
                principalTable: "ReturnProtocol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Companies_CompanyId",
                table: "ReturnProtocol",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocol_Objects_CompanyObjectId",
                table: "ReturnProtocol",
                column: "CompanyObjectId",
                principalTable: "Objects",
                principalColumn: "Id");

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
    }
}
