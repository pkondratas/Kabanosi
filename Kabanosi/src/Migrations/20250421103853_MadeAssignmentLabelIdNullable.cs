using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kabanosi.Migrations
{
    /// <inheritdoc />
    public partial class MadeAssignmentLabelIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentLabels_AssignmentLabelId",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentLabelId",
                table: "Assignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssignmentLabels",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentLabels_AssignmentLabelId",
                table: "Assignments",
                column: "AssignmentLabelId",
                principalTable: "AssignmentLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentLabels_AssignmentLabelId",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentLabelId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AssignmentLabels",
                keyColumn: "Description",
                keyValue: null,
                column: "Description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssignmentLabels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentLabels_AssignmentLabelId",
                table: "Assignments",
                column: "AssignmentLabelId",
                principalTable: "AssignmentLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
