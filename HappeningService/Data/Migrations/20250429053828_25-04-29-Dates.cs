using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappeningService.Data.Migrations
{
    /// <inheritdoc />
    public partial class _250429Dates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Happenings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Happenings");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Happenings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Happenings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Happenings_EndDate",
                table: "Happenings",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Happenings_StartDate",
                table: "Happenings",
                column: "StartDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Happenings_EndDate",
                table: "Happenings");

            migrationBuilder.DropIndex(
                name: "IX_Happenings_StartDate",
                table: "Happenings");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Happenings");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Happenings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Happenings",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Happenings",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
