using Microsoft.EntityFrameworkCore.Migrations;

namespace PUDInspection.Data.Migrations
{
    public partial class User_InspectionValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCheck");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Check",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Validation_ApplicationUserId",
                table: "Check",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Check_ApplicationUserId",
                table: "Check",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_Validation_ApplicationUserId",
                table: "Check",
                column: "Validation_ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CheckId",
                table: "AspNetUsers",
                column: "CheckId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Check_CheckId",
                table: "AspNetUsers",
                column: "CheckId",
                principalTable: "Check",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Check_AspNetUsers_ApplicationUserId",
                table: "Check",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Check_AspNetUsers_Validation_ApplicationUserId",
                table: "Check",
                column: "Validation_ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Check_CheckId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Check_AspNetUsers_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropForeignKey(
                name: "FK_Check_AspNetUsers_Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_Check_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_Check_Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CheckId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Check");

            migrationBuilder.DropColumn(
                name: "Validation_ApplicationUserId",
                table: "Check");

            migrationBuilder.DropColumn(
                name: "CheckId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserCheck",
                columns: table => new
                {
                    ChecksId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCheck", x => new { x.ChecksId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCheck_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCheck_Check_ChecksId",
                        column: x => x.ChecksId,
                        principalTable: "Check",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCheck_UserListId",
                table: "ApplicationUserCheck",
                column: "UserListId");
        }
    }
}
