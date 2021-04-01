using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Extend_PUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkId",
                table: "PUDs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "PUDs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "PUDs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationStage",
                table: "PUDs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "PUDs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "PUDs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PUDs_CampusId",
                table: "PUDs",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_PUDs_FacultyId",
                table: "PUDs",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PUDs_Campuses_CampusId",
                table: "PUDs",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PUDs_Faculties_FacultyId",
                table: "PUDs",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PUDs_Campuses_CampusId",
                table: "PUDs");

            migrationBuilder.DropForeignKey(
                name: "FK_PUDs_Faculties_FacultyId",
                table: "PUDs");

            migrationBuilder.DropIndex(
                name: "IX_PUDs_CampusId",
                table: "PUDs");

            migrationBuilder.DropIndex(
                name: "IX_PUDs_FacultyId",
                table: "PUDs");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "PUDs");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "PUDs");

            migrationBuilder.DropColumn(
                name: "EducationStage",
                table: "PUDs");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "PUDs");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "PUDs");

            migrationBuilder.AlterColumn<int>(
                name: "LinkId",
                table: "PUDs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
