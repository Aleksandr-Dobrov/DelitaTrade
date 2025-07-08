using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovingFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "ReturnProtocols",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsScrapped",
                table: "ReturnedProduct",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseDescription",
                table: "ReturnedProduct",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_ApproverId",
                table: "ReturnProtocols",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_AspNetUsers_ApproverId",
                table: "ReturnProtocols",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_AspNetUsers_ApproverId",
                table: "ReturnProtocols");

            migrationBuilder.DropIndex(
                name: "IX_ReturnProtocols_ApproverId",
                table: "ReturnProtocols");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "ReturnProtocols");

            migrationBuilder.DropColumn(
                name: "IsScrapped",
                table: "ReturnedProduct");

            migrationBuilder.DropColumn(
                name: "WarehouseDescription",
                table: "ReturnedProduct");
        }
    }
}
