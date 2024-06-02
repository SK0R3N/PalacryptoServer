using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalaCrypto.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogDifferenceAdmins",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nameItem = table.Column<string>(type: "TEXT", nullable: false),
                    logTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    newPrice = table.Column<double>(type: "REAL", nullable: false),
                    lastPrice = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDifferenceAdmins", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogDifferenceAdmins");
        }
    }
}
