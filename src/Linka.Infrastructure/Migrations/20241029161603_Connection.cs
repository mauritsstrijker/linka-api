using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Volunteer1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Volunteer2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Volunteers_Volunteer1Id",
                        column: x => x.Volunteer1Id,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Connections_Volunteers_Volunteer2Id",
                        column: x => x.Volunteer2Id,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Volunteer1Id",
                table: "Connections",
                column: "Volunteer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Volunteer2Id",
                table: "Connections",
                column: "Volunteer2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");
        }
    }
}
