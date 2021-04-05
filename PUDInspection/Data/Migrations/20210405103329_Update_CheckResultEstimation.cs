using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class Update_CheckResultEstimation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_AspNetUsers_UserId",
                table: "CheckResultEvaluations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CheckResultEvaluations",
                newName: "ValidatorId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResultEvaluations_UserId",
                table: "CheckResultEvaluations",
                newName: "IX_CheckResultEvaluations_ValidatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResultEvaluations_AspNetUsers_ValidatorId",
                table: "CheckResultEvaluations",
                column: "ValidatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckResultEvaluations_AspNetUsers_ValidatorId",
                table: "CheckResultEvaluations");

            migrationBuilder.RenameColumn(
                name: "ValidatorId",
                table: "CheckResultEvaluations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckResultEvaluations_ValidatorId",
                table: "CheckResultEvaluations",
                newName: "IX_CheckResultEvaluations_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckResultEvaluations_AspNetUsers_UserId",
                table: "CheckResultEvaluations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
