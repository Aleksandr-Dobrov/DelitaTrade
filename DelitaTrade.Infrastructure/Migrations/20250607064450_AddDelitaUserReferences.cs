using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelitaTrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDelitaUserReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdentityUserId",
                table: "ReturnProtocols",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityUserId",
                table: "DayReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnProtocols_IdentityUserId",
                table: "ReturnProtocols",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DayReports_IdentityUserId",
                table: "DayReports",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayReports_AspNetUsers_IdentityUserId",
                table: "DayReports",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnProtocols_AspNetUsers_IdentityUserId",
                table: "ReturnProtocols",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayReports_AspNetUsers_IdentityUserId",
                table: "DayReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnProtocols_AspNetUsers_IdentityUserId",
                table: "ReturnProtocols");

            migrationBuilder.DropIndex(
                name: "IX_ReturnProtocols_IdentityUserId",
                table: "ReturnProtocols");

            migrationBuilder.DropIndex(
                name: "IX_DayReports_IdentityUserId",
                table: "DayReports");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "ReturnProtocols");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "DayReports");
        }
    }
}
