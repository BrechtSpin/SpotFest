using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappeningService.Data.Migrations
{
    /// <inheritdoc />
    public partial class _250501slugindexed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Happenings_Slug",
                table: "Happenings",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Happenings_Slug",
                table: "Happenings");
        }
    }
}
