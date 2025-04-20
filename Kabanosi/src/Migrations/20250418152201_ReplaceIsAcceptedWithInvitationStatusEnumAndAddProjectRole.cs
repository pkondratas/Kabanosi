using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kabanosi.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIsAcceptedWithInvitationStatusEnumAndAddProjectRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectRole",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectRole",
                table: "Invitations");
        }
    }
}
