using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kabanosi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProjectMemberFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProjectMember");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProjectMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProjectMembers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectMembers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectMembers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProjectMembers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectProjectMember",
                columns: table => new
                {
                    ProjectMembersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProjectsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProjectMember", x => new { x.ProjectMembersId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectProjectMember_ProjectMembers_ProjectMembersId",
                        column: x => x.ProjectMembersId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProjectMember_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProjectMember_ProjectsId",
                table: "ProjectProjectMember",
                column: "ProjectsId");
        }
    }
}
