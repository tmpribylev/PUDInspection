using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Update_PUDCheckPUDResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResults_PUDs_PUDId",
                table: "CheckResults");

            migrationBuilder.DropIndex(
                name: "IX_CheckResults_PUDId",
                table: "CheckResults");

            migrationBuilder.DropColumn(
                name: "PUDId",
                table: "CheckResults");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PUDId",
                table: "CheckResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_PUDId",
                table: "CheckResults",
                column: "PUDId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResults_PUDs_PUDId",
                table: "CheckResults",
                column: "PUDId",
                principalTable: "PUDs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
