using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JobVolunteerActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobVolunterActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolunteerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobVolunterActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobVolunterActivities_EventJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "EventJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobVolunterActivities_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobVolunterActivities_JobId",
                table: "JobVolunterActivities",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobVolunterActivities_VolunteerId",
                table: "JobVolunterActivities",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobVolunterActivities");
        }
    }
}
