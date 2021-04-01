using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Check_CheckResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InspectionId",
                table: "CheckResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_InspectionId",
                table: "CheckResults",
                column: "InspectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_Check_InspectionId",
                table: "CheckResults",
                column: "InspectionId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_Check_InspectionId",
                table: "CheckResults");

            migrationBuilder.DropIndex(
                name: "IX_CheckResults_InspectionId",
                table: "CheckResults");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "CheckResults");
        }
    }
}
