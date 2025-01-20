using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NulableTraderCompanyObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Companies_CompanyId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Traders_TraderId",
                table: "ReturnProtocols");

            migrationBuilder.AlterColumn<int>(
                name: "TraderId",
                table: "ReturnProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyObjectId",
                table: "ReturnProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ReturnProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TraderId",
                table: "Objects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Companies_CompanyId",
                table: "ReturnProtocols",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                table: "ReturnProtocols",
                column: "CompanyObjectId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_Traders_TraderId",
                table: "ReturnProtocols",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Companies_CompanyId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Objects_CompanyObjectId",
                table: "ReturnProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_Traders_TraderId",
                table: "ReturnProtocols");

            migrationBuilder.AlterColumn<int>(
                name: "TraderId",
                table: "ReturnProtocols",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyObjectId",
                table: "ReturnProtocols",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ReturnProtocols",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TraderId",
                table: "Objects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Traders_TraderId",
                table: "Objects",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id");

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
        }
    }
}
