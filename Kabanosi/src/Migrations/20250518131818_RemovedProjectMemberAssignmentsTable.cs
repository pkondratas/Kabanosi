using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kabanosi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProjectMemberAssignmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMemberAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeId",
                table: "Assignments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ReporterId",
                table: "Assignments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssigneeId",
                table: "Assignments",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ReporterId",
                table: "Assignments",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ProjectMembers_AssigneeId",
                table: "Assignments",
                column: "AssigneeId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ProjectMembers_ReporterId",
                table: "Assignments",
                column: "ReporterId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ProjectMembers_AssigneeId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ProjectMembers_ReporterId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssigneeId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ReporterId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Assignments");

            migrationBuilder.CreateTable(
                name: "ProjectMemberAssignments",
                columns: table => new
                {
                    ProjectMemberId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsReporter = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Version = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberAssignments", x => new { x.ProjectMemberId, x.AssignmentId, x.IsReporter });
                    table.ForeignKey(
                        name: "FK_ProjectMemberAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberAssignments_ProjectMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberAssignments_AssignmentId",
                table: "ProjectMemberAssignments",
                column: "AssignmentId");
        }
    }
}
