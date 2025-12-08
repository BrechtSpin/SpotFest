using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoggerService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntityType = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Operation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedBy = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CorrelationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChangeLogId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyChanges_ChangeLogs_ChangeLogId",
                        column: x => x.ChangeLogId,
                        principalTable: "ChangeLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_ChangedAt",
                table: "ChangeLogs",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_ChangedBy",
                table: "ChangeLogs",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityId",
                table: "ChangeLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityType",
                table: "ChangeLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChanges_ChangeLogId",
                table: "PropertyChanges",
                column: "ChangeLogId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChanges_PropertyName",
                table: "PropertyChanges",
                column: "PropertyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyChanges");

            migrationBuilder.DropTable(
                name: "ChangeLogs");
        }
    }
}
